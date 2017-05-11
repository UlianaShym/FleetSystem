using System.Collections.Generic;

namespace XamarinFleetApp
{
    class Settings
    {
        public static bool Distance_enabled { get; private set; }
        public static int Distance_value { get; private set; }
        public static bool Time_enabled { get; private set; }
        public static int Time_value { get; private set; }
        public static int Last_id { get; private set; }

        /// <summary>
        /// Create Settings object, geting data from Data Bafe by db Handler
        /// </summary>
        /// <param name="db">SQL connection handler</param>
        static Settings()
        {
            Distance_enabled = true;
            Time_enabled = false;
            Distance_value = 5000;
            Time_value = 60;
            Last_id = 0;
        }

        public static void RefreshSettings(SQLiteHandler db)
        {
            List<string[]> settings = db.GetSetting();

            foreach (string[] item in settings)
            {
                switch (item[0])
                {
                    case "distance":
                        {
                            Distance_value = int.Parse(item[1]);
                            Distance_enabled = item[2] == "1";                            
                            break;
                        }
                    case "time":
                        {
                            Time_value = int.Parse(item[1]);
                            Time_enabled = item[2] == "1";
                            
                            break;
                        }
                    case "last_id":
                        {
                            Last_id = int.Parse(item[1]);
                            break;
                        }
                    default: break;
                }
            }            
        }
        public static void IncLastId()
        {
            Last_id++;
        }
    }
}