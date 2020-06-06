using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    class FacebookCacheProxy
    {
        public static User LoggedInUser { get; set; }

        private static List<PhotoData> s_TopThreeLikedPhotos;

        public static List<FriendData> FriendsDataList { get; set; }

        public static User Connect(string i_AccessToken, out bool o_Connected)
        {
            LoginResult loginResult = FacebookService.Connect(i_AccessToken);

            o_Connected = !string.IsNullOrEmpty(loginResult.AccessToken);

            return loginResult.LoggedInUser;
        }

        public static List<PhotoData> GetTopThreeLikedPhotos(User i_LoggedInUser)
        {
            if (s_TopThreeLikedPhotos == null)
            {
                s_TopThreeLikedPhotos = TopThreeLikedPhotosGenerator.Generate(i_LoggedInUser);
            }

            return s_TopThreeLikedPhotos;
        }

        public static FriendData GetFriendDataByName(string i_FriendName)
        {
            FriendData o_FriendData = null;

            if (FriendsDataList == null)
            {
                FetchSortedFriendsDataList();
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

        public static void FetchSortedFriendsDataList()
        {
            if (FriendsDataList == null)
            {
                FriendsDataList = new List<FriendData>();

                foreach (User friend in LoggedInUser.Friends)
                {
                    FriendData friendData = new FriendData(LoggedInUser, friend);
                    FriendsDataList.Add(friendData);
                }

                FriendsDataList.Sort();
            }
        }
    }
}
