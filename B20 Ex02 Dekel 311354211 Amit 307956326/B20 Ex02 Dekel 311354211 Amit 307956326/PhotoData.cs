using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex02_Dekel_311354211_Amit_307956326
{
    public class PhotoData
    {
        public string PhotoURL { get; set; }

        public int NumOfLikes { get; set; }

        public PhotoData(Photo i_Photo)
        {
            this.PhotoURL = i_Photo.PictureNormalURL;
            this.NumOfLikes = i_Photo.LikedBy.Count;
        }
    }
}
