using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex02_Dekel_311354211_Amit_307956326
{
    public class GroupData
    {
        public string Name { get; set; }

        public string OwnerName { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string IconUrl { get; set; }

        public GroupData(Group i_Group)
        {
            this.Name = i_Group.Name;
            this.OwnerName = i_Group.Owner.Name;
            this.Link = i_Group.Link;
            this.Description = i_Group.Description;
            this.IconUrl = i_Group.IconUrl;
        }
    }
}
