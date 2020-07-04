using FacebookWrapper.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class PhotosWithLikesIterator : IEnumerator<Photo>
    {
        //private IEnumerator<Photo> m_Current;
        //private IEnumerator<Photo> m_CollectionCurrent;

        private readonly FacebookObjectCollection<Photo> r_Photos;
        private int m_Index = -1;
        private int m_Count = -1;

        public PhotosWithLikesIterator(FacebookObjectCollection<Photo> i_Photos)
        {
            //m_CollectionCurrent = i_Photos.GetEnumerator();
            r_Photos = i_Photos;
            m_Count = i_Photos.Count;
        }

        public Photo Current
        {
            get
            {
                return r_Photos[m_Index];
            }
        }

        object IEnumerator.Current => Current;

        
        public bool MoveNext()
        {
            return nextPhotoWithLikes();
        }

        private bool nextPhotoWithLikes()
        {
            m_Index++;

            while(r_Photos[m_Index].LikedBy.Count <= 0)
            {
                m_Index++;
            }

            return m_Index < m_Count;
        }

        public void Reset()
        {
            m_Index = 0;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
