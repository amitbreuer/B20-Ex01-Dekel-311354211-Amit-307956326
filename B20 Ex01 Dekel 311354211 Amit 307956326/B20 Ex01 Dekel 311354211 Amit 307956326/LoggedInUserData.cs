using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class LoggedInUserData
    {
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        public List<string> FriendsList { get; set; }
        public List<string> CheckinsList { get; set; }
        public List<PostData> PostsDataList { get; set; }
    }
}
