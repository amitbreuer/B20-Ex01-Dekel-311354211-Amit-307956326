﻿using System;
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

        private LoginResult m_LoginResult;

        public FacebookAppFacade()
        {
            AppSettings = AppSettings.LoadSettingsFromFile();
        }

        public void Connect()
        {
            if (AppSettings.RememberUser && !string.IsNullOrEmpty(AppSettings.LastAccessToken))
            {
                m_LoginResult = FacebookService.Connect(AppSettings.LastAccessToken);
                LoggedInUserData loggedInUserData = new LoggedInUserData(m_LoginResult.LoggedInUser);
                m_FacebookConnectionNotifier.Invoke(loggedInUserData);
            }
        }

        public void Login()
        {
            m_LoginResult = FacebookWrapper.FacebookService.Login(
                "202490531010346",
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
                "user_hometown");

            AppSettings.LastAccessToken = m_LoginResult.AccessToken;
            LoggedInUserData loggedInUserData = new LoggedInUserData(m_LoginResult.LoggedInUser);
            m_FacebookConnectionNotifier.Invoke(loggedInUserData);
        }

        public void Logout()
        {
            FacebookWrapper.FacebookService.Logout(() => {});
        }

        public void SaveSettingsToFile()
        {
            AppSettings.SaveSettingsToFile();
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

        public void Get3MostLikedPhotos()
        {
               
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
            List<PhotoData> allPicturesData = new List<PhotoData>();
            FacebookObjectCollection<Album> albums = LoggedInUser.Albums;

            foreach (Album album in albums)
            {
                foreach (Photo photo in album.Photos)
                {
                    allPicturesData.Add(new PhotoData(photo));
                }
            }

            allPicturesData.OrderBy(PictureData => PictureData.NumOfLikes);
            return allPicturesData;
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