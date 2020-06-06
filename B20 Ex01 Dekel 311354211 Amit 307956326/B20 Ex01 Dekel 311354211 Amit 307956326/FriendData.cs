using System;
using System.Collections.Generic;
using System.Reflection;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class FriendData : IComparable
    {
        public string Name { get; set; }

        public int NumberOfLikes { get; set; }

        public int NumberOfComments { get; set; }

        public int NumberOfSharedCheckins { get; set; }

        public int NumberOfSharedPages { get; set; }

        public int NumberOfSharedGroups { get; set; }

        public string ProfilePictureUrl { get; set; }

        public FriendData(User i_User, User i_Friend)
        {
            this.Name = i_Friend.Name;
            this.ProfilePictureUrl = i_Friend.PictureLargeURL;
            this.NumberOfComments = -1;
            this.NumberOfLikes = -1;
            this.NumberOfSharedCheckins = -1;
            this.NumberOfSharedPages = -1;
            this.NumberOfSharedGroups = -1;

            updateFriendData(i_User, i_Friend);
        }

        private void updateFriendData(User i_User, User i_Friend)
        {
            getNumberOfLikesAndComments(i_User, i_Friend);
            getNumberOfSharedCheckins(i_User, i_Friend);
            getNumberOfSharedPages(i_User, i_Friend);
            getNumberOfSharedGroups(i_User, i_Friend);
        }

        private void getNumberOfSharedGroups(User i_User, User i_Friend)
        {
            FacebookObjectCollection<Group> groups;

            try
            {
                groups = i_User.Groups;
            }
            catch
            {
                groups = null;
            }

            if (groups != null)
            {
                foreach (Group group in i_User.Groups)
                {
                    if (i_Friend.Groups != null && i_Friend.Groups.Contains(group))
                    {
                        NumberOfSharedGroups++;
                    }
                }
            }
        }

        private void getNumberOfSharedPages(User i_User, User i_Friend)
        {
            FacebookObjectCollection<Page> likedPages;

            try
            {
                likedPages = i_User.LikedPages;
            }
            catch
            {
                likedPages = null;
            }

            if (likedPages != null)
            {
                foreach (Page page in i_User.LikedPages)
                {
                    if (i_Friend.LikedPages != null && i_Friend.LikedPages.Contains(page))
                    {
                        NumberOfSharedPages++;
                    }
                }
            }
        }

        private void getNumberOfSharedCheckins(User i_User, User i_Friend)
        {
            FacebookObjectCollection<Checkin> checkins;

            try
            {
                checkins = i_User.Checkins;
            }
            catch
            {
                checkins = null;
            }

            if (checkins != null)
            {
                foreach (Checkin checkin in i_User.Checkins)
                {
                    if (i_Friend.Checkins != null && i_Friend.Checkins.Contains(checkin))
                    {
                        NumberOfSharedCheckins++;
                    }
                }
            }
        }

        private void getNumberOfLikesAndComments(User i_User, User i_Friend)
        {
            FacebookObjectCollection<Post> posts;

            try
            {
                posts = i_User.Posts;
            }
            catch
            {
                posts = null;
            }

            if (posts != null)
            {
                foreach (Post post in i_User.Posts)
                {
                    FacebookObjectCollection<User> likedBy;
                    FacebookObjectCollection<Comment> comments;

                    try
                    {
                        likedBy = post.LikedBy;
                    }
                    catch
                    {
                        likedBy = null;
                    }

                    if (likedBy != null && isUserInCollectionOfUser(i_Friend, likedBy))
                    {
                        NumberOfLikes++;
                    }

                    try
                    {
                        comments = post.Comments;
                    }
                    catch
                    {
                        comments = null;
                    }

                    if (comments != null)
                    {
                        foreach (Comment comment in post.Comments)
                        {
                            if (comment.From.Name.Equals(i_Friend.Name))
                            {
                                NumberOfComments++;
                            }
                        }
                    }
                }
            }

        }

        private static bool isUserInCollectionOfUser(User i_User, ICollection<User> i_CollectionOfUsers)
        {
            bool exists = false;

            foreach (User user in i_CollectionOfUsers)
            {
                if (user.Name.Equals(i_User.Name))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        public int CompareTo(object obj)
        {
            int friend1Rating = this.GetFriendScore();
            int friend2Rating = (obj as FriendData).GetFriendScore();

            return friend2Rating - friend1Rating;
        }

        public int GetFriendScore()
        {
            int o_Score = 0;
            int intValue;
            Type thisType = this.GetType();

            foreach (PropertyInfo propertyInfo in thisType.GetProperties())
            {
                object propertyValue = propertyInfo.GetValue(this, null);

                if (!propertyInfo.Name.Equals("Name"))
                {
                    intValue = int.Parse(propertyValue.ToString());
                    o_Score += 3 * intValue;
                }
            }

            return o_Score;
        }
    }
}