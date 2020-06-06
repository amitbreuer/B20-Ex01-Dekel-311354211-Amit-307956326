using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class PostData
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string PictureUrl { get; set; }
        public string CreatedTime { get; set; }

        public PostData(Post post)
        {
            this.Id = post.Id;
            this.Text = post.Message;
            this.PictureUrl = post.PictureURL;
            this.CreatedTime = post.CreatedTime.Value.ToShortDateString();
        }
    }
}
