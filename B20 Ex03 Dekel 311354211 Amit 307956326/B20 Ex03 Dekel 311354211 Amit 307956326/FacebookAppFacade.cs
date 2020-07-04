﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public delegate void FacebookConnectionNotifier(LoggedInUserData i_UserData);

    public class FacebookAppFacade
    {
        public event FacebookConnectionNotifier m_FacebookConnectionNotifier;

        public User LoggedInUser { get; set; }

        public AppSettings AppSettings { get; set; }

        public FacebookAppFacade()
        {
            AppSettings = AppSettings.LoadSettingsFromFile();
        }

        public void Connect()
        {
            Thread thread = new Thread(asyncConnect);
            thread.ApartmentState = System.Threading.ApartmentState.STA;
            thread.Start();
        }

        private void asyncConnect()
        {
            if (AppSettings.RememberUser && !string.IsNullOrEmpty(AppSettings.LastAccessToken))
            {
                LoginResult loginResult = FacebookService.Connect(AppSettings.LastAccessToken);
                LoggedInUser = loginResult.LoggedInUser;
                FacebookCacheProxy.Instace.LoggedInUser = LoggedInUser;
                m_FacebookConnectionNotifier.Invoke(new LoggedInUserData(LoggedInUser, AppSettings.LastAccessToken));
            }
        }

        public void Login()
        {
            Thread thread = new Thread(asyncLogin);
            thread.ApartmentState = System.Threading.ApartmentState.STA;
            thread.Start();
        }

        private void asyncLogin()
        {
            LoginResult loginResult = FacebookWrapper.FacebookService.Login(
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

            LoggedInUser = loginResult.LoggedInUser;
            AppSettings.LastAccessToken = loginResult.AccessToken;
            FacebookCacheProxy.Instace.LoggedInUser = LoggedInUser;
            m_FacebookConnectionNotifier.Invoke(new LoggedInUserData(LoggedInUser, AppSettings.LastAccessToken));
        }

        public void CloseApplication()
        {
            AppSettings.SaveSettingsToFile();
        }

        public void Logout()
        {
            FacebookWrapper.FacebookService.Logout(() => { });
            File.Delete("AppSettings.xml");
        }

        public int GetFriendRankInFriendsList(string i_FriendName)
        {
            int o_FriendRank = -1;
            List<FriendData> friendsData = FacebookCacheProxy.Instace.FriendsDataList;

            foreach (FriendData friendData in friendsData)
            {
                if (friendData.Name.Equals(i_FriendName))
                {
                    o_FriendRank = friendsData.IndexOf(friendData) + 1;
                }
            }

            return o_FriendRank;
        }

        public List<PhotoData> GetTopThreeLikedPhotos()
        {
            return FacebookCacheProxy.Instace.GetTopThreeLikedPhotos(LoggedInUser);
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
            return FacebookCacheProxy.Instace.GetFriendDataByName(i_FriendName);
        }
    }
}