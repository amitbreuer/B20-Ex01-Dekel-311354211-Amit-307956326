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
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        public List<string> FriendsNames { get; set; }
        public List<string> Checkins { get; set; }
        public List<PostData> PostsData { get; set; }
        public List<GroupData> GroupsData { get; set; }

        public LoggedInUserData(User i_LoggedInUser, string i_AccessToken)
        {
            this.AccessToken = i_AccessToken;
            this.Name = i_LoggedInUser.Name;
            this.ProfilePictureUrl = i_LoggedInUser.PictureNormalURL;
            this.CoverPhotoUrl = i_LoggedInUser.Cover != null ? i_LoggedInUser.Cover.SourceURL : null;
            createAllDataLists(i_LoggedInUser);
        }

        private void createAllDataLists(User i_LoggedInUser)
        {
            FacebookObjectCollection<User> friends;
            FacebookObjectCollection<Checkin> checkins;
            FacebookObjectCollection<Post> posts;
            FacebookObjectCollection<Group> groups;

            try
            {
                friends = i_LoggedInUser.Friends;
            }
            catch
            {
                friends = null;
            }

            createFriendsNamesList(friends);

            try
            {
                checkins = i_LoggedInUser.Checkins;
            }
            catch
            {
                checkins = null;
            }

            createCheckinsList(checkins);

            try
            {
                posts = i_LoggedInUser.Posts;
            }
            catch
            {
                posts = null;
            }

            createPostsDataList(posts);

            try
            {
                groups = i_LoggedInUser.Groups;
            }
            catch
            {
                groups = null;
            }

            createGroupsDataList(groups);
        }

        private void createGroupsDataList(FacebookObjectCollection<Group> i_Groups)
        {
            this.GroupsData = new List<GroupData>();

            if (i_Groups != null)
            {
                foreach (Group group in i_Groups)
                {
                    this.GroupsData.Add(new GroupData(group));
                }
            }
        }

        private void createPostsDataList(FacebookObjectCollection<Post> i_Posts)
        {
            this.PostsData = new List<PostData>();

            if (i_Posts != null)
            {
                foreach (Post post in i_Posts)
                {
                    this.PostsData.Add(new PostData(post));
                }
            }
        }

        private void createCheckinsList(FacebookObjectCollection<Checkin> i_Checkins)
        {
            this.Checkins = new List<string>();

            if (i_Checkins != null)
            {
                foreach (Checkin checkin in i_Checkins)
                {
                    this.Checkins.Add(string.Format("{0},{1} At {2}", checkin.Place.Location.City, checkin.Place.Location.Country, checkin.CreatedTime.Value.ToShortDateString()));
                }
            }
        }

        private void createFriendsNamesList(FacebookObjectCollection<User> i_Friends)
        {
            this.FriendsNames = new List<string>();

            if (i_Friends != null)
            {
                foreach (User friend in i_Friends)
                {
                    this.FriendsNames.Add(friend.Name);
                }
            }
        }
    }
}
