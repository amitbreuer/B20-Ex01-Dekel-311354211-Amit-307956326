﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public static class TopThreeLikedPhotosGenerator
    {
        public static List<PhotoData> Generate(User i_LoggedInUser)
        {
            List<PhotoData> allPhotosData = new List<PhotoData>();
            FacebookObjectCollection<Album> albums;

            try
            {
                albums = i_LoggedInUser.Albums;
            }
            catch
            {
                albums = null;
            }

            if (albums != null)
            {
                foreach (Album album in albums)
                {
                    PhotosWithLikesCollection photos = new PhotosWithLikesCollection(album.Photos);

                    foreach (Photo photo in photos)
                    {
                        allPhotosData.Add(new PhotoData(photo));
                    }
                }

                allPhotosData.OrderBy(PictureData => PictureData.NumOfLikes);
            }

            return allPhotosData.Count < 4 ? allPhotosData : allPhotosData.Take(3) as List<PhotoData>;
        }
    }
}
