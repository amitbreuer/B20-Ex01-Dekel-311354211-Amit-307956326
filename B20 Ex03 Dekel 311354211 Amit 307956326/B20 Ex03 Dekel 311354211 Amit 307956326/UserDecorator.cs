using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class UserDecorator : User
    {
        private User m_WrappeeUser;

        public UserDecorator(User i_User)
        {
            m_WrappeeUser = i_User;
        }

        public new FacebookObjectCollection<GroupAndKnownMembers> Groups
        {
            get
            {
                FacebookObjectCollection<GroupAndKnownMembers> groupsAndKnownMembers = new FacebookObjectCollection<GroupAndKnownMembers>();
                GroupAndKnownMembers groupAndKnownMembers;
                FacebookObjectCollection<Group> groups;

                try
                {
                    groups = m_WrappeeUser.Groups;
                }
                catch
                {
                    groups = null;
                }

                if (groups != null)
                {
                    foreach (Group group in groups)
                    {
                        groupAndKnownMembers = new GroupAndKnownMembers(m_WrappeeUser, group);
                        groupsAndKnownMembers.Add(groupAndKnownMembers);
                    }
                }

                return groupsAndKnownMembers;
            }
        }
    }
}
