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
using System.Web;
using System.Net;
using System.IO;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public partial class form : Form
    {
        FacebookWrapper.ObjectModel.User m_LoggedInUser;

        public form()
        {
            InitializeComponent();
        }

        private void checkBoxRememberMe_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (buttonLogin.Text == "Logout")
            {
                FacebookWrapper.FacebookService.Logout(() => { });
                buttonLogin.Text = "Login";
                form NewForm = new form();
                NewForm.Show();
                this.Dispose(false);
                return;
            }

            buttonLogin.Text = "Logout";
            FacebookWrapper.LoginResult loginResult = FacebookWrapper.FacebookService.Login("202490531010346",
                "public_profile",
                "email",
                "publish_to_groups",
                "user_age_range",
                "user_gender",
                "user_link",
                "user_tagged_places",
                "user_videos",
                "publish_to_groups",
                "groups_access_member_info",
                "user_friends",
                "user_likes",
                "user_photos",
                "user_posts",
                "user_hometown"
                );
            m_LoggedInUser = loginResult.LoggedInUser;
            updateDisplay(m_LoggedInUser);
        }

        private void resetDisplay()
        {
        }

        private void updateDisplay(User i_LoggedInUser)
        {
            updateUserInfo(i_LoggedInUser);
            updateGeneralInfoTab(i_LoggedInUser);
            //updateMostLikedPhotosTab(i_LoggedInUser);//the feature currently not working 

        }

        private void updateMostLikedPhotosTab(User i_LoggedInUser)
        {
            IList<Photo> allPhotos = getUsersPhotosSortedByLikes(i_LoggedInUser);
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

        private IList<Photo> getUsersPhotosSortedByLikes(User i_LoggedInUser)
        {
            IList<Photo> allPhotos = new FacebookObjectCollection<Photo>();
            FacebookObjectCollection<Album> albums = i_LoggedInUser.Albums;
            foreach (Album album in albums)
            {
                foreach (Photo photo in album.Photos)
                {
                    allPhotos.Add(photo);
                }
            }

            allPhotos.OrderBy(photo => photo.LikedBy.Count);
            return allPhotos;
        }

        private void updateUserInfo(User i_LoggedInUser)
        {
            pictureBoxProfilePicture.Load(i_LoggedInUser.PictureNormalURL);
            //pictureBoxCoverPhoto.Load(i_LoggedInUser.Cover.SourceURL);
            //this.pictureBoxCoverPhoto.BackgroundImage = m_LoggedInUser.Albums[0].Photos[0].ImageNormal;

            labelName.Text = i_LoggedInUser.Name;
        }

        private void updateGeneralInfoTab(User i_LoggedInUser)
        {
            updateUserFeed(i_LoggedInUser.Posts);
            addFriendsToFriendsList(i_LoggedInUser.Friends);
            addCheckinsToCheckinsList(i_LoggedInUser.Checkins);
        }

        private void addCheckinsToCheckinsList(FacebookObjectCollection<Checkin> i_Checkins)
        {
            foreach (Checkin checkin in i_Checkins)
            {
                listBoxCheckins.Items.Add(string.Format("{0},{1} At {2}", checkin.Place.Location.Country, checkin.Place.Location.City, checkin.CreatedTime.Value.ToShortDateString()));
            }

        }

        private void addFriendsToFriendsList(FacebookObjectCollection<User> i_Friends)
        {
            foreach (User friend in i_Friends)
            {
                listBoxFriends.Items.Add(friend.Name);
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
                    Image postImage = createImageFromUrl(post.PictureURL);
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

        private Image createImageFromUrl(string i_PictureURL)
        {
            WebClient webClient = new WebClient();
            byte[] _data = webClient.DownloadData(i_PictureURL);
            MemoryStream memoryStream = new MemoryStream(_data);
            return Image.FromStream(memoryStream);
        }
    }
}
