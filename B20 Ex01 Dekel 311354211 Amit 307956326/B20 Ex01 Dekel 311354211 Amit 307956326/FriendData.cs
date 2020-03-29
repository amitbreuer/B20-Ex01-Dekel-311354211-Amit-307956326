using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class FriendData : IComparable
    {
        public string Name { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int SharedCheckins { get; set; }
        public int SharedPages { get; set; }
        public int SharedGroups { get; set; }

        public FriendData(User i_User, User i_Friend)
        {
            Name = i_Friend.Name;

            foreach (Post post in i_User.Posts)
            {
                if (post.LikedBy.Contains(i_Friend))
                {
                    Likes++;
                }

                foreach (Comment comment in post.Comments)
                {
                    if (comment.From.Equals(i_Friend))
                    {
                        Comments++;
                    }
                }
            }

            foreach (Checkin checkin in i_User.Checkins)
            {
                if (i_Friend.Checkins.Contains(checkin))
                {
                    SharedCheckins++;
                }
            }

            foreach (Page page in i_User.LikedPages)
            {
                if (i_Friend.LikedPages.Contains(page))
                {
                    SharedPages++;
                }
            }

            foreach (Group group in i_User.Groups)
            {
                if (i_Friend.Groups.Contains(group))
                {
                    SharedGroups++;
                }
            }
        }

        public int CompareTo(object obj)
        {
            int friend1Rating = this.GetFriendRating();
            int friend2Rating = (obj as FriendData).GetFriendRating();

            return friend2Rating - friend1Rating;
        }

        public int GetFriendRating()
        {
            int o_Rank = 0;
            int intValue;
            Type thisType = this.GetType();

            foreach (PropertyInfo propertyInfo in thisType.GetProperties())
            {
                object propertyValue = propertyInfo.GetValue(this, null);
                intValue = int.Parse(propertyValue.ToString());
                o_Rank += 3 * intValue;
            }

            return o_Rank;
        }
    }
}
