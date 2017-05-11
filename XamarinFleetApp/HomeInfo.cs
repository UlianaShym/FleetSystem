using System;
using System.Collections.Generic;

namespace XamarinFleetApp
{
    class HomeInfo
    {
        public String Race { get; private set; }        
        public String Car { get; private set; }
        public String Points { get; private set; }

        public HomeInfo(List<string[]> info)
        {
            if (info != null)
            {
                Race = info[0][0];
                Car = info[0][1];
                Points = info[0][2];
            }
            else
            {
                Race = "None";
                Car = "None";
                Points = "-1";
            }
        }
    }
}