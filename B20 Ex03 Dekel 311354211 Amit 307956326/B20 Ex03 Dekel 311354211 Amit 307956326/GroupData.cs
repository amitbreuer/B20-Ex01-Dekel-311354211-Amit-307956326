using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class GroupData
    {
        public string Name { get; set; }

        public string OwnerName { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string IconUrl { get; set; }

        public List<string> KnownMembers { get; }

        public GroupData(GroupAndKnownMembers i_GroupAndKnownMembers)
        {
            this.Name = i_GroupAndKnownMembers.Group.Name;
            this.OwnerName = i_GroupAndKnownMembers.Group.Owner.Name;
            this.Link = i_GroupAndKnownMembers.Group.Link;
            this.Description = i_GroupAndKnownMembers.Group.Description;
            this.IconUrl = i_GroupAndKnownMembers.Group.IconUrl;
            this.KnownMembers = getKnownMembersNames(i_GroupAndKnownMembers.KnownMembers);
        }

        private List<string> getKnownMembersNames(FacebookObjectCollection<User> knownMembers)
        {
            List<string> knownMembersNames = new List<string>();

            foreach(User user in knownMembers)
            {
                knownMembersNames.Add(user.Name);
            }

            return knownMembersNames;
        }
    }
}
