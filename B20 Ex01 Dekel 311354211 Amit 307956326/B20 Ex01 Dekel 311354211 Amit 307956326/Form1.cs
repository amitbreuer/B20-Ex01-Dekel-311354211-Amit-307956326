using System;
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
            FacebookWrapper.LoginResult loginResult = FacebookWrapper.FacebookService.Login("202490531010346", "user_friends");
            m_LoggedInUser = loginResult.LoggedInUser;
            updateDisplay(m_LoggedInUser);

            
            
        }

        private void updateDisplay(User i_LoggedInUser)
        {
            updateUserInfo(i_LoggedInUser);
            updateGeneralInfoTab(i_LoggedInUser);
        }

        private void updateUserInfo(User i_LoggedInUser)
        {
            pictureBoxProfilePicture.Load(i_LoggedInUser.PictureNormalURL);
            pictureBoxCoverPhoto.Load(i_LoggedInUser.Cover.SourceURL);
            labelName.Text = i_LoggedInUser.Name;
        }

        private void updateGeneralInfoTab(User i_LoggedInUser)
        {

        }

        private void initiateCompareFriendsTab(User i_LoggedInUser)
        {
            // listBoxCompareFriendsList1
            // listBoxCompareFriendsList2
        }

        private void listBoxCompareFriendsList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string friend1Name = listBoxCompareFriendsList1.SelectedItem.ToString();
            getFriendDataForComparison(friend1Name);
        }

        private void getFriendDataForComparison(string i_friendsName)
        {
            User friendInList = null;

            foreach (User friend in m_LoggedInUser.Friends)
            {
                if(friend.Name.Equals(i_friendsName))
                {
                    friendInList = friend;
                }
            }

            pictureBoxFriend1.Load(friendInList.PictureNormalURL);
        }
    }
}
