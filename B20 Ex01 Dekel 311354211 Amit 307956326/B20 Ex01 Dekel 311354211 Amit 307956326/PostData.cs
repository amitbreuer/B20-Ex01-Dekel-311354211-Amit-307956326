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

        public PostData(Post i_Post)
        {
            this.Id = i_Post.Id;
            this.Text = i_Post.Message;
            this.PictureUrl = i_Post.PictureURL;
            this.CreatedTime = i_Post.CreatedTime.Value.ToShortDateString();
        }
    }
}
