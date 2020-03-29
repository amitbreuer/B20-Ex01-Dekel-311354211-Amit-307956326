using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class FacebookApp
    {
        public User m_LoggedInUser { get; set; }
        public List<FriendData> m_FriendsDataList { get; set; }

        public FacebookApp(User m_LoggedInUser)
        {
            this.m_LoggedInUser = m_LoggedInUser;
            GetSortedFriendsDataList();
        }

        public void GetSortedFriendsDataList()
        {
            m_FriendsDataList = new List<FriendData>();

            foreach (User friend in m_LoggedInUser.Friends)
            {
                FriendData friendData = new FriendData(m_LoggedInUser, friend);
                m_FriendsDataList.Add(friendData);
            }

            m_FriendsDataList.Sort();
        }

        public int GetFriendRankInFriendsList(string i_FriendName)
        {
            int o_FriendRank = -1;

            foreach(FriendData friendData in m_FriendsDataList)
            {
                if (friendData.Name.Equals(i_FriendName))
                {
                    o_FriendRank = m_FriendsDataList.IndexOf(friendData) + 1;
                }
            }

            return o_FriendRank;
        }

        public IList<Photo> GetUsersPhotosSortedByLikes(User i_LoggedInUser)
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

        public Image CreateImageFromUrl(string i_PictureURL)
        {
            WebClient webClient = new WebClient();
            byte[] _data = webClient.DownloadData(i_PictureURL);
            MemoryStream memoryStream = new MemoryStream(_data);
            return Image.FromStream(memoryStream);
        }

        public User GetFriendUser(string i_FriendName)
        {
            User friendToRate = null;

            foreach (User friend in m_LoggedInUser.Friends)
            {
                if (friend.Name.Equals(i_FriendName))
                {
                    friendToRate = friend;
                }
            }

            return friendToRate;
        }

        public FriendData GetFriendDataByName(string i_FriendName)
        {
            FriendData o_FriendData = null;

            foreach (FriendData friendData in m_FriendsDataList)
            {
                if (friendData.Name.Equals(i_FriendName))
                {
                    o_FriendData = friendData;
                }
            }

            return o_FriendData;
        }
    }
}
