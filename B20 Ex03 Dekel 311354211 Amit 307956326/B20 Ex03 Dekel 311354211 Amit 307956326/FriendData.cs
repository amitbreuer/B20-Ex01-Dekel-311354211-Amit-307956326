using System;
using System.Collections.Generic;
using System.Reflection;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class FriendData : IComparable
    {
        public string Name { get; set; }

        public int NumberOfLikesToFriend { get; set; }

        public int NumberOfCommentsToFriend { get; set; }

        public int NumberOfLikesFromFriend { get; set; }

        public int NumberOfCommentsFromFriend { get; set; }

        public int NumberOfSharedCheckins { get; set; }

        public int NumberOfSharedPages { get; set; }

        public int NumberOfSharedGroups { get; set; }

        public string ProfilePictureUrl { get; set; }

        public int Score { get; set; }

        public FriendData(User i_User, User i_Friend)
        {
            this.Name = i_Friend.Name;
            this.ProfilePictureUrl = i_Friend.PictureLargeURL;
            this.NumberOfCommentsFromFriend = -1;
            this.NumberOfCommentsToFriend = -1;
            this.NumberOfLikesFromFriend = -1;
            this.NumberOfLikesToFriend = -1;
            this.NumberOfSharedCheckins = -1;
            this.NumberOfSharedPages = -1;
            this.NumberOfSharedGroups = -1;

            updateFriendData(i_User, i_Friend);
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

        private void updateFriendData(User i_User, User i_Friend)
        {
            getNumberOfLikesAndCommentsFromFriend(i_User, i_Friend);
            getNumberOfLikesAndCommentsToFriend(i_User, i_Friend);
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
                foreach (Group group in groups)
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
                foreach (Page page in likedPages)
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
                foreach (Checkin checkin in checkins)
                {
                    if (i_Friend.Checkins != null && i_Friend.Checkins.Contains(checkin))
                    {
                        NumberOfSharedCheckins++;
                    }
                }
            }
        }

        private void getNumberOfLikesAndCommentsFromFriend(User i_User, User i_Friend)
        {
            int[] likesAndComments = getNumberOfLikesAndCommentsBetweenFriends(i_User, i_Friend);

            NumberOfLikesFromFriend = likesAndComments[0];
            NumberOfCommentsFromFriend = likesAndComments[1];
        }

        private void getNumberOfLikesAndCommentsToFriend(User i_User, User i_Friend)
        {
            int[] likesAndComments = getNumberOfLikesAndCommentsBetweenFriends(i_Friend, i_User);

            NumberOfLikesToFriend = likesAndComments[0];
            NumberOfCommentsToFriend = likesAndComments[1];
        }

        private int[] getNumberOfLikesAndCommentsBetweenFriends(User i_User, User i_Friend)
        {
            int[] o_NumberOfLikesAndComments = { 0, 0 };
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
                foreach (Post post in posts)
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
                        o_NumberOfLikesAndComments[0]++;
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
                        foreach (Comment comment in comments)
                        {
                            if (comment.From.Name.Equals(i_Friend.Name))
                            {
                                o_NumberOfLikesAndComments[1]++;
                            }
                        }
                    }
                }
            }

            return o_NumberOfLikesAndComments;
        }

        public int CompareTo(object obj)
        {
            int friend1Rating = this.Score;
            int friend2Rating = (obj as FriendData).Score;

            return friend2Rating - friend1Rating;
        }
    }
}