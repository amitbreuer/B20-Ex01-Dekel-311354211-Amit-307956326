using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public partial class FacebookForm : Form
    {
        private FacebookAppFacade m_FacebookAppFacade;

        public FacebookForm()
        {
            InitializeComponent();

            m_FacebookAppFacade = new FacebookAppFacade();
            updateFormSettings();
            m_FacebookAppFacade.m_FacebookConnectionNotifier += updateDisplay;
        }

        private void updateFormSettings()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = m_FacebookAppFacade.AppSettings.LastWindowLocation;
            this.Size = m_FacebookAppFacade.AppSettings.LastWindowSize;
            this.checkBoxRememberMe.Checked = m_FacebookAppFacade.AppSettings.RememberUser;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            m_FacebookAppFacade.Connect();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            m_FacebookAppFacade.AppSettings.LastWindowLocation = this.Location;
            m_FacebookAppFacade.AppSettings.LastWindowSize = this.Size;
            m_FacebookAppFacade.AppSettings.RememberUser = this.checkBoxRememberMe.Checked;

            if (!this.checkBoxRememberMe.Checked)
            {
                m_FacebookAppFacade.AppSettings.LastAccessToken = null;
            }

            m_FacebookAppFacade.CloseApplication();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (buttonLogin.Text == "Logout")
            {
                m_FacebookAppFacade.Logout();
                m_FacebookAppFacade = null;
                FacebookForm NewForm = new FacebookForm();
                NewForm.Show();
                this.Dispose(false);
                return;
            }

            m_FacebookAppFacade.Login();
        }

        private void updateDisplay(LoggedInUserData i_LoggedInUserData)
        {
            updateUserInfo(i_LoggedInUserData);
            updateGeneralInfoTab(i_LoggedInUserData);
            buttonLogin.Text = "Logout";
        }

        private void updateUserInfo(LoggedInUserData i_LoggedInUserData)
        {
            this.Text = string.Format("Welcome {0}!", i_LoggedInUserData.Name);
            labelName.Text = i_LoggedInUserData.Name;
            pictureBoxProfilePicture.Load(i_LoggedInUserData.ProfilePictureUrl);

            if (i_LoggedInUserData.CoverPhotoUrl != null)
            {
                pictureBoxCoverPhoto.Load(i_LoggedInUserData.CoverPhotoUrl);
            }
        }

        private void updateGeneralInfoTab(LoggedInUserData i_LoggedInUserData)
        {
            updateUserFeed(i_LoggedInUserData.PostsData);
            addFriendsToFriendsLists(i_LoggedInUserData.FriendsNames);
            addCheckinsToCheckinsList(i_LoggedInUserData.Checkins);
            addGroupsToGroupsList(i_LoggedInUserData.GroupsData);
        }

        private void addGroupsToGroupsList(List<GroupData> i_GroupsData)
        {
            groupDataBindingSource.DataSource = i_GroupsData;
        }

        private void addCheckinsToCheckinsList(List<string> i_Checkins)
        {
            foreach (string checkin in i_Checkins)
            {
                listBoxCheckins.Items.Add(checkin);
            }
        }

        private void addFriendsToFriendsLists(List<string> i_FriendsNames)
        {
            foreach (string friendName in i_FriendsNames)
            {
                listBoxFriends.Items.Add(friendName);
                listBoxRatingFriendsList.Items.Add(friendName);
            }
        }

        private void updateUserFeed(List<PostData> i_PostData)
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(200, 128);
            listViewFeed.LargeImageList = imageList;

            foreach (PostData postData in i_PostData)
            {
                ListViewItem postViewItem = new ListViewItem();

                if (postData.Text != null)
                {
                    postViewItem.Text = postData.Text;
                }

                if (postData.PictureUrl != null && postData.PictureUrl.Length > 0)
                {
                    Image postImage = m_FacebookAppFacade.CreateImageFromUrl(postData.PictureUrl);
                    imageList.Images.Add(postData.Id, postImage);
                    postViewItem.ImageKey = postData.Id;
                }

                if (!string.IsNullOrEmpty(postViewItem.Text) || !string.IsNullOrEmpty(postViewItem.ImageKey))
                {
                    postViewItem.Text = string.Format("{0}\n{1}", postViewItem.Text, postData.CreatedTime);
                    listViewFeed.Items.Add(postViewItem);
                }
            }
        }

        private void buttonShowTop3Photos_Click(object sender, EventArgs e)
        {
            updateThreeMostLikedPhotosTab();
        }

        private void updateThreeMostLikedPhotosTab()
        {
            List<PhotoData> TopThreeLikedPhotos = m_FacebookAppFacade.GetTopThreeLikedPhotos();

            if (TopThreeLikedPhotos != null && TopThreeLikedPhotos.Count != 0)
            {
                if (TopThreeLikedPhotos.Count > 0)
                {
                    pictureBoxFirstMostLikedPicture.Load(TopThreeLikedPhotos[0].PhotoURL);
                    labelFirstMostLikedPicture.Text = TopThreeLikedPhotos[0].NumOfLikes.ToString();
                }

                if (TopThreeLikedPhotos.Count > 1)
                {
                    pictureBoxSecondMostLikedPicture.Load(TopThreeLikedPhotos[1].PhotoURL);
                    labelSecondMostLikedPicture.Text = TopThreeLikedPhotos[1].NumOfLikes.ToString();
                }

                if (TopThreeLikedPhotos.Count > 2)
                {
                    pictureBoxThirdMostLikedPicture.Load(TopThreeLikedPhotos[2].PhotoURL);
                    labelThirdMostLikedPicture.Text = TopThreeLikedPhotos[2].NumOfLikes.ToString();
                }
            }
            else
            {
                MessageBox.Show("Failed to access your photos", "Access Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBoxRatingFriendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchAndUpdateSelectedFriendData(listBoxRatingFriendsList.SelectedItem.ToString());
        }

        private void fetchAndUpdateSelectedFriendData(string i_FriendName)
        {
            FriendData friendData = m_FacebookAppFacade.GetFriendDataByName(i_FriendName);
            
            if(friendData != null)
            {
                updateFriendDataDisplay(friendData);
            }
            else
            {
                MessageBox.Show("Failed to access your friend's data", "Access Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateFriendDataDisplay(FriendData i_FriendData)
        {
            int friendRank;

            if(!string.IsNullOrEmpty(i_FriendData.ProfilePictureUrl))
            {
                pictureBoxRatingFriendProfilePic.Load(i_FriendData.ProfilePictureUrl);
            }

            labelRatingTabLikesCount.Text = i_FriendData.NumberOfLikes != -1 ? i_FriendData.NumberOfLikes.ToString() : "-";
            labelRatingTabCommentsCount.Text = i_FriendData.NumberOfComments != -1 ? i_FriendData.NumberOfComments.ToString() : "-";
            labelRatingTabCheckinsCount.Text = i_FriendData.NumberOfSharedCheckins != -1 ? i_FriendData.NumberOfSharedCheckins.ToString() : "-";
            labelRatinTabPagesCount.Text = i_FriendData.NumberOfSharedPages != -1 ? i_FriendData.NumberOfSharedPages.ToString() : "-";
            labelRatingTabGroupsCount.Text = i_FriendData.NumberOfSharedGroups != -1 ? i_FriendData.NumberOfSharedGroups.ToString() : "-";
            friendRank = m_FacebookAppFacade.GetFriendRankInFriendsList(i_FriendData.Name);
            labelRatingTabRankMessage.Text = string.Format("is ranked {1}# in your friends!", i_FriendData.Name, friendRank);
        }
    }
}