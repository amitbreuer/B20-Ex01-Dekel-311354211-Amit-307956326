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
            FacebookWrapper.LoginResult loginResult = FacebookWrapper.FacebookService.Login("202490531010346",
                "user_friends",
                "public_profile",
                "user_friends",
                "user_likes",
                "user_photos",
                "user_posts",
                "user_status"
                );
            m_LoggedInUser = loginResult.LoggedInUser;
            updateDisplay(m_LoggedInUser);



        }

        private void updateDisplay(User i_LoggedInUser)
        {
            updateUserInfo(i_LoggedInUser);
            updateGeneralInfoTab(i_LoggedInUser);
            // updateMostLikedPhotosTab(i_LoggedInUser);

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
            updateLastStatusDisplay(i_LoggedInUser);
            addFriendsToFriendsList(i_LoggedInUser.Friends);
            //addPagesToPagesList(i_LoggedInUser.LikedPages);
            addCheckinsToCheckinsList(i_LoggedInUser.Checkins);
        }

        private void addCheckinsToCheckinsList(FacebookObjectCollection<Checkin> i_Checkins)
        {
            foreach (Checkin checkin in i_Checkins)
            {
                listBoxCheckins.Items.Add(string.Format("{0},{1} At {2}", checkin.Place.Location.Country, checkin.Place.Location.City, checkin.CreatedTime.ToString()));
            }

        }

        private void addPagesToPagesList(FacebookObjectCollection<Page> i_likedPages)
        {
            foreach (Page likedPage in i_likedPages)
            {
                listBoxPages.Items.Add(likedPage.Name);
            }
        }

        private void addFriendsToFriendsList(FacebookObjectCollection<User> i_Friends)
        {
            foreach (User friend in i_Friends)
            {
                listBoxFriends.Items.Add(friend.Name);
            }
        }

        private void updateLastStatusDisplay(User i_LoggedInUser)
        {
            if (i_LoggedInUser.Statuses.Count > 0)
            {
                listBoxLastStatus.Items.Add(i_LoggedInUser.Posts[0].Type);
            }
            else
            {
                listBoxLastStatus.Items.Add("You never posted a status on FaceBook");
            }
        }

    }
}
