using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class LoggedInUserData
    {
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        public List<string> FriendsNames { get; set; }
        public List<string> Checkins { get; set; } 
        public List<PostData> PostsData { get; set; }

        public LoggedInUserData(User i_LoggedInUser)
        {
            this.Name = i_LoggedInUser.Name;
            this.ProfilePictureUrl = i_LoggedInUser.PictureNormalURL;
            this.CoverPhotoUrl = i_LoggedInUser.Cover != null ? i_LoggedInUser.Cover.SourceURL : null;
            createFriendsNamesList(i_LoggedInUser.Friends);
            createCheckinsList(i_LoggedInUser.Checkins);
            createPostsDataList(i_LoggedInUser.Posts);
        }

        private void createPostsDataList(FacebookObjectCollection<Post> i_Posts)
        {
            this.PostsData = new List<PostData>();

            foreach(Post post in i_Posts)
            {
                this.PostsData.Add(new PostData(post));
            }
        }

        private void createCheckinsList(FacebookObjectCollection<Checkin> i_Checkins)
        {
            this.Checkins = new List<string>();

            foreach (Checkin checkin in i_Checkins)
            {
                this.Checkins.Add(string.Format("{0},{1} At {2}", checkin.Place.Location.City, checkin.Place.Location.Country, checkin.CreatedTime.Value.ToShortDateString()));
            }
        }

        private void createFriendsNamesList(FacebookObjectCollection<User> i_Friends)
        {
            this.FriendsNames = new List<string>();

            foreach (User friend in i_Friends)
            {
                this.FriendsNames.Add(friend.Name);
            }
        }
    }
}
