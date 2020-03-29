using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    public class FacebookApp
    {
        User m_LoggedInUser;
        List<FriendData> m_FriendsDataList;

        public void GetSortedFriendsDataList()
        {
            foreach(User friend in m_LoggedInUser.Friends)
            {
                FriendData friendData = new FriendData(m_LoggedInUser, friend);
            }

            m_FriendsDataList.Sort();
        }

        public int GetFriendRankByName(string i_FriendName)
        {
            int o_FriendRank = -1;

            foreach(FriendData friendData in m_FriendsDataList)
            {
                if (friendData.Name.Equals(i_FriendName))
                {
                    o_FriendRank = new FriendRank(friendData).Rank;
                }
            }

            return o_FriendRank + 1;
        }
    }
}
