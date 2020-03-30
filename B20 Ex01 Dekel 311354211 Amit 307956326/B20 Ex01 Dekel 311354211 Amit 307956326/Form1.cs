using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;
using System.IO;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public partial class form : Form
    {
        User m_LoggedInUser;
        FacebookApp m_FacebookApp;
        AppSettings m_AppSettings;
        LoginResult m_LoginResult;

        public form()
        {
            InitializeComponent();
            m_AppSettings = AppSettings.LoadFromFile();

            this.StartPosition = FormStartPosition.Manual;

            this.Location = m_AppSettings.LastWindowLocation;
            this.Size = m_AppSettings.LastWindowSize;
            this.checkBoxRememberMe.Checked = m_AppSettings.RememberUser;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if(m_AppSettings.RememberUser && !string.IsNullOrEmpty(m_AppSettings.LastAccessToken))
            {
                m_LoginResult = FacebookService.Connect(m_AppSettings.LastAccessToken);
                updateDisplay(m_LoginResult);
                buttonLogin.Text = "Logout";
            } 
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            m_AppSettings.LastWindowLocation = this.Location;
            m_AppSettings.LastWindowSize = this.Size;
            m_AppSettings.RememberUser = this.checkBoxRememberMe.Checked;

            if(m_AppSettings.RememberUser)
            {
                m_AppSettings.LastAccessToken = m_LoginResult.AccessToken;
            }
            else
            {
                m_AppSettings.LastAccessToken = null;
            }

            m_AppSettings.SaveToFile();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (buttonLogin.Text == "Logout")
            {
                FacebookWrapper.FacebookService.Logout(() => { });
                buttonLogin.Text = "Login";
                m_FacebookApp = null;
                form NewForm = new form();
                NewForm.Show();
                this.Dispose(false);
                return;
            }

            buttonLogin.Text = "Logout";
            m_LoginResult = FacebookWrapper.FacebookService.Login("202490531010346",
                "public_profile",
                "email",
                "publish_to_groups",
                "user_age_range",
                "user_gender",
                "user_link",
                "user_tagged_places",
                "user_videos",
                "groups_access_member_info",
                "user_friends",
                "user_likes",
                "user_photos",
                "user_posts",
                "user_hometown"
                );
            
            updateDisplay(m_LoginResult);
        }

        private void updateDisplay(LoginResult i_LoggedInResult)
        {
            m_LoggedInUser = i_LoggedInResult.LoggedInUser;
            m_FacebookApp = new FacebookApp(m_LoggedInUser);
            updateUserInfo(m_LoggedInUser);
            updateGeneralInfoTab(m_LoggedInUser);
        }

        private void updateMostLikedPhotosTab(User i_LoggedInUser)
        {
            IList<Photo> allPhotos;
            try
            {
                allPhotos = m_FacebookApp.GetUsersPhotosSortedByLikes(i_LoggedInUser);
            }
            catch
            {
                MessageBox.Show("Failed to access your photos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (allPhotos.Count > 0)
            {
                pictureBoxFirstMostLikedPicture.Load(allPhotos[0].PictureNormalURL);
                labelFirstMostLikedPicture.Text = allPhotos[0].LikedBy.Count.ToString();
            }
            if (allPhotos.Count > 1)
            {
                pictureBoxSecondMostLikedPicture.Load(allPhotos[1].PictureNormalURL);
                labelSecondMostLikedPicture.Text = allPhotos[1].LikedBy.Count.ToString();
            }
            if (allPhotos.Count > 2)
            {
                pictureBoxThirdMostLikedPicture.Load(allPhotos[2].PictureNormalURL);
                labelThirdMostLikedPicture.Text = allPhotos[2].LikedBy.Count.ToString();
            }
        }

        private void updateUserInfo(User i_LoggedInUser)
        {
            labelName.Text = i_LoggedInUser.Name;
            pictureBoxProfilePicture.Load(i_LoggedInUser.PictureNormalURL);

            try
            {    
                pictureBoxCoverPhoto.Load(i_LoggedInUser.Cover.SourceURL);
            }
            catch
            {
                MessageBox.Show("Failed to load your cover photo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateGeneralInfoTab(User i_LoggedInUser)
        {
            updateUserFeed(i_LoggedInUser.Posts);
            addFriendsToFriendsLists(i_LoggedInUser.Friends);
            addCheckinsToCheckinsList(i_LoggedInUser.Checkins);
        }

        private void addCheckinsToCheckinsList(FacebookObjectCollection<Checkin> i_Checkins)
        {
            foreach (Checkin checkin in i_Checkins)
            {
                listBoxCheckins.Items.Add(string.Format("{0},{1} At {2}", checkin.Place.Location.City, checkin.Place.Location.Country, checkin.CreatedTime.Value.ToShortDateString()));
            }
        }

        private void addFriendsToFriendsLists(FacebookObjectCollection<User> i_Friends)
        {
            foreach (User friend in i_Friends)
            {
                listBoxFriends.Items.Add(friend.Name);
                listBoxRatingFriendsList.Items.Add(friend.Name);
            }
        }

        private void updateUserFeed(FacebookObjectCollection<Post> i_Posts)
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(200, 128);
            listViewFeed.LargeImageList = imageList;

            foreach (Post post in i_Posts)
            {
                ListViewItem postViewItem = new ListViewItem();

                if (post.Message != null)
                {
                    postViewItem.Text = post.Message;
                }
                if (post.PictureURL != null && post.PictureURL.Length > 0)
                {
                    Image postImage = m_FacebookApp.CreateImageFromUrl(post.PictureURL);
                    imageList.Images.Add(post.Id, postImage);
                    postViewItem.ImageKey = post.Id;
                }
                if (!string.IsNullOrEmpty(postViewItem.Text) || !string.IsNullOrEmpty(postViewItem.ImageKey))//if current post contains avaiable data such as Message or photo
                {
                    postViewItem.Text = postViewItem.Text + string.Format("\n{0}", post.CreatedTime.Value.ToShortDateString());
                    listViewFeed.Items.Add(postViewItem);
                }
            }
        }

        private void listBoxRatingFriendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            getSelectedFriendData(listBoxRatingFriendsList.SelectedItem.ToString());
        }

        private void getSelectedFriendData(string i_FriendName)
        {
            User friendToRate = m_FacebookApp.GetFriendUser(i_FriendName);

            if (friendToRate != null)
            {
                pictureBoxRatingFriendProfilePic.Load(friendToRate.PictureLargeURL);
                try
                {
                    updateFriendDataLabels(m_FacebookApp.GetFriendDataByName(i_FriendName));
                }
                catch
                {
                    MessageBox.Show("Failed to access your friend's data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void updateFriendDataLabels(FriendData friendData)
        {
            int friendRank;

            labelRatingTabLikesCount.Text = friendData.Likes.ToString();
            labelRatingTabCommentsCount.Text = friendData.Comments.ToString();
            labelRatingTabCheckinsCount.Text = friendData.SharedCheckins.ToString();
            labelRatinTabPagesCount.Text = friendData.SharedPages.ToString();
            labelRatingTabGroupsCount.Text = friendData.SharedGroups.ToString();
            friendRank = m_FacebookApp.GetFriendRankInFriendsList(friendData.Name);

            labelRatingTabRankMessage.Text = string.Format("is ranked {1}# in your friends!", friendData.Name, friendRank);

        }

        private void buttonShowTop3Photos_Click(object sender, EventArgs e)
        {
            updateMostLikedPhotosTab(m_LoggedInUser);
            buttonShowTop3Photos.Visible = false;
        }
    }
}
