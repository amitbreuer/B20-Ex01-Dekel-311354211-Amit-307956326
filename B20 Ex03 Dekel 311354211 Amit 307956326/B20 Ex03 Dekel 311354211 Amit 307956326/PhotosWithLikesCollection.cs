using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class PhotosWithLikesCollection : FacebookObjectCollection<Photo>
    {
        private FacebookObjectCollection<Photo> m_Photos = null;

        public PhotosWithLikesCollection(FacebookObjectCollection<Photo> i_Photos)
        {
            this.m_Photos = i_Photos;
        }

        public PhotosWithLikesIterator GetIterator()
        {
            return new PhotosWithLikesIterator(m_Photos);
        }
    }
}
