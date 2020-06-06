using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public delegate void FacebookConnectionNotifier(LoggedInUserData i_UserData);

    public class FacebookAppFacade
    {
        public event FacebookConnectionNotifier m_FacebookConnectionNotifier;

        public User LoggedInUser { get; set; }

        public List<FriendData> FriendsDataList { get; set; }

        private static bool s_FriendsDataListWasFetched = false;

        public AppSettings AppSettings { get; set; }

        public FacebookAppFacade()
        {
            AppSettings = AppSettings.LoadSettingsFromFile();
        }

        public void Connect()
        {
            bool connected;

            if (AppSettings.RememberUser && !string.IsNullOrEmpty(AppSettings.LastAccessToken))
            {
                LoggedInUser = FacebookCacheProxy.Connect(AppSettings.LastAccessToken,out connected);

                if(!connected)
                {
                    FacebookCacheProxy.FetchUserDataFromCache();
                }

                LoggedInUserData loggedInUserData = new LoggedInUserData(LoggedInUser,AppSettings.LastAccessToken);
                m_FacebookConnectionNotifier.Invoke(loggedInUserData);
            }
        }

        public void Login()
        {
            string resultAccessToken;
            LoggedInUserData loggedInUserData;

            LoggedInUser = FacebookCacheProxy.Login(out resultAccessToken);
            AppSettings.LastAccessToken = resultAccessToken;
            loggedInUserData = new LoggedInUserData(LoggedInUser,resultAccessToken);
            m_FacebookConnectionNotifier.Invoke(loggedInUserData);
        }

        public void CloseApplication()
        {
            AppSettings.SaveSettingsToFile();

            if(AppSettings.RememberUser)
            {
                FacebookCacheProxy.CacheLoggedInUserData(new LoggedInUserData(LoggedInUser,AppSettings.LastAccessToken));
            }
        }

        public void Logout()
        {
            FacebookCacheProxy.Logout();
        }

        public void FetchSortedFriendsDataList()
        {
            if (!s_FriendsDataListWasFetched)
            {
                FriendsDataList = new List<FriendData>();

                foreach (User friend in LoggedInUser.Friends)
                {
                    FriendData friendData = new FriendData(LoggedInUser, friend);
                    FriendsDataList.Add(friendData);
                }

                FriendsDataList.Sort();
                s_FriendsDataListWasFetched = true;
            }
        }

        public int GetFriendRankInFriendsList(string i_FriendName)
        {
            int o_FriendRank = -1;

            foreach (FriendData friendData in FriendsDataList)
            {
                if (friendData.Name.Equals(i_FriendName))
                {
                    o_FriendRank = FriendsDataList.IndexOf(friendData) + 1;
                }
            }

            return o_FriendRank;
        }

        public List<PhotoData> GetUsersPhotosDataSortedByLikes()
        {
            List<PhotoData> allPhotosData = new List<PhotoData>();
            FacebookObjectCollection<Album> albums;

            try
            {
                albums = LoggedInUser.Albums;
            }
            catch
            {
                albums = null;
            }

            if(albums != null)
            {
                foreach (Album album in albums)
                {
                    foreach (Photo photo in album.Photos)
                    {
                        allPhotosData.Add(new PhotoData(photo));
                    }
                }

                allPhotosData.OrderBy(PictureData => PictureData.NumOfLikes);
            }

            return allPhotosData;
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

            foreach (User friend in LoggedInUser.Friends)
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

            if (!s_FriendsDataListWasFetched)
            {
                FetchSortedFriendsDataList();
                s_FriendsDataListWasFetched = true;
            }

            foreach (FriendData friendData in FriendsDataList)
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