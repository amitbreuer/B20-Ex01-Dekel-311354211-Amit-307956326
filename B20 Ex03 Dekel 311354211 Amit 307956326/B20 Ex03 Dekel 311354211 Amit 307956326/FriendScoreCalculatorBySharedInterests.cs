using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex03_Dekel_311354211_Amit_307956326
{
    public class FriendScoreCalculatorBySharedInterests : IFriendScoreCalculator
    {
        public int CalculateFriendScore(FriendData i_FriendData)
        {
            int o_Score = 0;
            int intValue;
            Type thisType = this.GetType();

            foreach (PropertyInfo propertyInfo in thisType.GetProperties())
            {
                object propertyValue = propertyInfo.GetValue(this, null);

                if (propertyInfo.Name.Contains("Shared"))
                {
                    intValue = int.Parse(propertyValue.ToString());
                    o_Score += 3 * intValue;
                }
            }

            return o_Score;
        }
    }
}
