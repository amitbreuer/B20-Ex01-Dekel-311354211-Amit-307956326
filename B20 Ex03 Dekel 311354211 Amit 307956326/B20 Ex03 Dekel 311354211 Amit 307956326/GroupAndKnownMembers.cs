using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class GroupAndKnownMembers
    {
        private FacebookObjectCollection<User> m_KnownMembers;

        public Group Group { get; set; }

        public FacebookObjectCollection<User> KnownMembers { get; }

        public GroupAndKnownMembers(User i_User, Group i_Group)
        {
            this.Group = i_Group;
            m_KnownMembers = new FacebookObjectCollection<User>();

            foreach (User member in this.Group.Members)
            {
                if (memberIsAFriend(i_User, member.Name))
                {
                    m_KnownMembers.Add(member);
                }
            }
        }

        private bool memberIsAFriend(User i_User, string i_MemberName)
        {
            bool memberIsAfriend = false;

            foreach (User friend in i_User.Friends)
            {
                if (friend.Name.Equals(i_MemberName))
                {
                    memberIsAfriend = true;
                    break;
                }
            }

            return memberIsAfriend;
        }
    }
}
