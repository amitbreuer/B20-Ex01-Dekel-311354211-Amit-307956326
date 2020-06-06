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
        //private static LoginResult m_LoginResult;

        public static User Login(out string resultAccessToken)
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

            resultAccessToken = !string.IsNullOrEmpty(loginResult.AccessToken) ?
                loginResult.AccessToken : string.Empty;

            return loginResult.LoggedInUser;
        }

        public static User Connect(string i_AccessToken,out bool o_Connected)
        {
            LoginResult loginResult = FacebookService.Connect(i_AccessToken);

            o_Connected = !string.IsNullOrEmpty(loginResult.AccessToken);
            
            return loginResult.LoggedInUser;
        }

        public static void CacheLoggedInUserData(LoggedInUserData i_LoggedInUserData)
        {
            using (Stream stream = new FileStream("User Cache.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LoggedInUserData));
                serializer.Serialize(stream, i_LoggedInUserData);
            }
        }

        public static void Logout()
        {
            FacebookWrapper.FacebookService.Logout(() => { });
        }
    }
}
