using System;
using System.Collections.Generic;

using Android.Content;
using Android.Database.Sqlite;
using Android.Database;
using Android.Locations;

namespace XamarinFleetApp
{
    class SQLiteHandler : SQLiteOpenHelper
    {
        private static int DATABASE_VERSION = 1;

        // Database Name
        private static String DATABASE_NAME = "android_api";

        // User table name
        private static String TABLE_USER = "user";

        // User table column name
        private static String KEY_USER_LOGIN = "login";
        private static String KEY_USER_ID = "user_id";

        // Points table name
        private static String TABLE_POINTS = "points";

        // Points table column name
        private static String KEY_POINTS_ID = "id";
        private static String KEY_POINTS_LATITUDE = "lalitude";
        private static String KEY_POINTS_LONGITUDE = "longitude";
        private static String KEY_POINTS_TIME = "time";
        private static String KEY_POINTS_SENDED = "sended";

        // Settings table name
        private static String TABLE_SETTINGS = "settings";

        // Settings table column name
        private static String KEY_SETTINGS_TYPE = "type";
        private static String KEY_SETTINGS_VALUE = "_value";
        private static String KEY_SETTINGS_ENABLED = "enabled";

        // Home table name
        private static String TABLE_HOME = "home";

        // Home table column name
        private static String KEY_HOME_RACE = "type";
        private static String KEY_HOME_POINTS = "_value";
        private static String KEY_HOME_CAR = "enabled";

        public SQLiteHandler(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION) { }

        public override void OnCreate(SQLiteDatabase db)
        {
            String CREATE_USER_TABLE = "CREATE TABLE " + TABLE_USER + "( "
                + KEY_USER_LOGIN + " TEXT PRIMARY KEY, "
                + KEY_USER_ID + " TEXT "
                + ")";
            String CREATE_POINTS_TABLE = "CREATE TABLE " + TABLE_POINTS + "( "
                + KEY_POINTS_ID + " INTEGER PRIMARY KEY, "
                + KEY_POINTS_LATITUDE + " REAL, "
                + KEY_POINTS_LONGITUDE + " REAL, "
                + KEY_POINTS_SENDED + " INTEGER, "
                + KEY_POINTS_TIME + " DATETIME DEFAULT CURRENT_TIMESTAMP "
                + ")";
            String CREATE_SETTINGS_TABLE = "CREATE TABLE " + TABLE_SETTINGS + "( "
                + KEY_SETTINGS_TYPE + " TEXT PRIMARY KEY, "
                + KEY_SETTINGS_VALUE + " INTEGER, "
                + KEY_SETTINGS_ENABLED + " INTEGER "
                + ")";
            String CREATE_HOME_TABLE = "CREATE TABLE " + TABLE_HOME + "( "
                + KEY_HOME_RACE + " TEXT PRIMARY KEY, "
                + KEY_HOME_CAR + " TEXT, "
                + KEY_HOME_POINTS + " INTEGER "
                + ")";

            db.ExecSQL(CREATE_USER_TABLE);
            db.ExecSQL(CREATE_POINTS_TABLE);
            db.ExecSQL(CREATE_SETTINGS_TABLE);
            db.ExecSQL(CREATE_HOME_TABLE);

            // Default setings
            db.ExecSQL("INSERT INTO " + TABLE_SETTINGS + " VALUES ( 'distance' , 5000 , 1 )");
            db.ExecSQL("INSERT INTO " + TABLE_SETTINGS + " VALUES ( 'time' , 60 , 0 )");
            db.ExecSQL("INSERT INTO " + TABLE_SETTINGS + " VALUES ( 'last_id' , 3 , 0 )");

            // Default Home
            db.ExecSQL("INSERT INTO " + TABLE_HOME + " VALUES ( 'none' , 'none', 0 )");

            // Test Points
            db.ExecSQL("INSERT INTO " + TABLE_POINTS + "( "
                + KEY_POINTS_ID + ", " + KEY_POINTS_LATITUDE + ", " + KEY_POINTS_LONGITUDE + ", " + KEY_POINTS_SENDED
                + " ) VALUES ( 0, 12.3456, 12.3456, 0 )");
            db.ExecSQL("INSERT INTO " + TABLE_POINTS + "( "
                + KEY_POINTS_ID + ", " + KEY_POINTS_LATITUDE + ", " + KEY_POINTS_LONGITUDE + ", " + KEY_POINTS_SENDED
                + " ) VALUES ( 1, 12.3457, 12.3456, 0 )");
            db.ExecSQL("INSERT INTO " + TABLE_POINTS + "( "
                + KEY_POINTS_ID + ", " + KEY_POINTS_LATITUDE + ", " + KEY_POINTS_LONGITUDE + ", " + KEY_POINTS_SENDED
                + " ) VALUES ( 2, 12.3458, 12.3456, 0 )");
            db.ExecSQL("INSERT INTO " + TABLE_POINTS + "( "
                + KEY_POINTS_ID + ", " + KEY_POINTS_LATITUDE + ", " + KEY_POINTS_LONGITUDE + ", " + KEY_POINTS_SENDED
                + " ) VALUES ( 3, 12.3459, 12.3456, 0 )");
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            // Drop older table if existed
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_USER);
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_HOME);
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_POINTS);
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_SETTINGS);

            // Create tables again
            OnCreate(db);
        }


        // Storing user details in database                   
        public void AddUser(String login, String userid)
        {
            SQLiteDatabase db = this.WritableDatabase;

            ContentValues values = new ContentValues();

            // Login
            values.Put(KEY_USER_LOGIN, login);
            values.Put(KEY_USER_ID, userid);

            // Inserting Row
            long id = db.Insert(TABLE_USER, null, values);

            // Closing database connection            
            db.Close();
        }

        // Getting user data from database
        public String GetUserName()
        {
            String selectQuery = "SELECT  * FROM " + TABLE_USER;
            return GetOneValue(selectQuery, 0);
        }
        public String GetUserID()
        {
            String selectQuery = "SELECT  * FROM " + TABLE_USER;
            return GetOneValue(selectQuery, 1);
        }

        // Delete all users from User tabel
        public void DeleteUsers()
        {
            ClearTable(TABLE_USER);
        }

        //--------------------------------------------------------------------------------------
        //------------------------------    Settings functions    ------------------------------
        //--------------------------------------------------------------------------------------
        private void SetDeffaulSettings()
        {
            ClearTable(TABLE_SETTINGS);
            ContentValues values = new ContentValues();

            values.Put(KEY_SETTINGS_TYPE, "distance");
            values.Put(KEY_SETTINGS_VALUE, 5000);
            values.Put(KEY_SETTINGS_ENABLED, 1);
            InsertData(TABLE_SETTINGS, values);

            values = new ContentValues();

            values.Put(KEY_SETTINGS_TYPE, "time");
            values.Put(KEY_SETTINGS_VALUE, 60);
            values.Put(KEY_SETTINGS_ENABLED, 0);

            InsertData(TABLE_SETTINGS, values);

        }
        public int UpdateSettings(int distance_value, bool distance_enabled, int time_value, bool time_enabled)
        {
            ContentValues values = new ContentValues();

            values.Put(KEY_SETTINGS_VALUE, distance_value);
            values.Put(KEY_SETTINGS_ENABLED, distance_enabled ? 1 : 0);
            UpdateData(TABLE_SETTINGS, values, KEY_SETTINGS_TYPE + " = 'distance' ");

            values = new ContentValues();

            values.Put(KEY_SETTINGS_VALUE, time_value);
            values.Put(KEY_SETTINGS_ENABLED, time_enabled ? 1 : 0);

            UpdateData(TABLE_SETTINGS, values, KEY_SETTINGS_TYPE + " = 'time' ");
            return 1;
        }
        public List<string[]> GetSetting()
        {
            String request = "SELECT  * FROM " + TABLE_SETTINGS;

            return GetAllRows(request);
        }
        public int UpdateLastPointId(int k)
        {
            ContentValues values = new ContentValues();
            values.Put(KEY_SETTINGS_VALUE, k);
            UpdateData(TABLE_SETTINGS, values, KEY_SETTINGS_TYPE + " = 'last_id'");
            return 1;
        }
        /*public int GetLastPointId()
        {
            String selectQuery = "SELECT  * FROM " + TABLE_SETTINGS + " WHERE " + KEY_SETTINGS_TYPE + " = 'last_id'";
            return int.Parse(GetOneValue(selectQuery, 1));
        }*/

        //--------------------------------------------------------------------------------------
        //-----------------------------------    Home info   -----------------------------------
        //--------------------------------------------------------------------------------------
        /// <summary>
        /// 0 - race | 1 - car | 2 - points cout
        /// </summary>
        /// <returns></returns>
        public HomeInfo GetHomeInfo()
        {
            String request = "SELECT  * FROM " + TABLE_HOME;

            return new HomeInfo(GetAllRows(request));
        }
        public int UpdatePointsCount(int k)
        {
            ContentValues values = new ContentValues();
            values.Put(KEY_HOME_POINTS, k);
            UpdateData(TABLE_SETTINGS, values, null);
            return 1;
        }

        //--------------------------------------------------------------------------------------
        //------------------------------    Location Points List    ----------------------------
        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Select 10 points per page (desc)
        /// </summary>
        /// <param name="page">Number of page to get</param>
        /// <returns></returns>
        public List<WayPoint> GetLocationPoints(int page)
        {
            String request = "SELECT * FROM " + TABLE_POINTS
                + " ORDER BY " + KEY_POINTS_ID + " DESC"
                + " LIMIT " + 10 + (page == 0 ? "" : " OFFSET " + (page * 10));

            List<string[]> resp = GetAllRows(request);
            List<WayPoint> WPL;
            if (resp != null)
            {
                WPL = new List<WayPoint>();

                foreach (string[] item in resp)
                {
                    WPL.Add(new WayPoint(item[0], item[1], item[2]));
                }
                return WPL;
            }

            return null;
        }
        public string[] GetLocationPointById(int id)
        {
            return null;
        }
        public int RemoveLocationPoitById(string id)
        {
            return RemoveData(TABLE_POINTS, KEY_POINTS_ID + "=" + id);
        }
        public long AddLocationPoint(int id, Location location, int sended)
        {
            ContentValues values = new ContentValues();

            values.Put(KEY_POINTS_ID, id);
            values.Put(KEY_POINTS_LATITUDE, location.Latitude);
            values.Put(KEY_POINTS_LONGITUDE, location.Longitude);
            values.Put(KEY_POINTS_SENDED, sended);

            return InsertData(TABLE_POINTS, values);
        }
        /// <summary>
        /// 0 - ID  | 1 - LATITUDE | 2 - LONGITUDE | 3 - SENDED(1/0)    | 4 - TIME
        /// </summary>
        /// <returns></returns>
        public string[] GetLastLocationPoint()
        {
            String request = "SELECT  *, MAX("+KEY_POINTS_ID+") FROM " + TABLE_POINTS;

            return GetOneRow(request);            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateSendedStatus(int id)
        {
            ContentValues values = new ContentValues();
            values.Put(KEY_POINTS_SENDED, "true");
            UpdateData(TABLE_POINTS, values, KEY_POINTS_ID + " = '" + id + "'");
            return 1;
        }
        //--------------------------------------------------------------------------------------
        //---------------------------------    SQL Requests    ---------------------------------
        //--------------------------------------------------------------------------------------

        private String GetOneValue(String request, int index)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.RawQuery(request, null);

            String res;

            if (cursor.MoveToFirst())
                res = cursor.GetString(index);
            else
                res = null;

            cursor.Close();
            db.Close();

            return res;
        }
        private String[] GetOneRow(String request)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.RawQuery(request, null);

            String[] res;

            if (cursor.MoveToFirst())
            {
                string[] text = cursor.GetColumnNames();
                int columnsCount = cursor.GetColumnNames().Length;
                res = new String[columnsCount];
                for (int i = 0; i < columnsCount; i++)
                {
                    res[i] = cursor.GetString(i);
                }

            }
            else
                res = null;

            cursor.Close();
            db.Close();

            return res;
        }
        private List<String[]> GetAllRows(String request)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor cursor = db.RawQuery(request, null);
            List<String[]> res;

            int columnsCount;

            if (cursor.MoveToFirst())
            {
                res = new List<string[]>();
                columnsCount = cursor.GetColumnNames().Length;

                do
                {
                    String[] row = new String[columnsCount];
                    for (int i = 0; i < columnsCount; i++)
                    {
                        row[i] = cursor.GetString(i);
                    }
                    res.Add(row);
                } while (cursor.MoveToNext());
            }
            else
                res = null;

            cursor.Close();
            db.Close();

            return res;
        }
        private long InsertData(String table, ContentValues values)
        {
            SQLiteDatabase db = this.WritableDatabase;
            long id = db.Insert(table, null, values);
            db.Close();
            return id;
        }
        private int UpdateData(String table, ContentValues values, String condition)
        {
            SQLiteDatabase db = this.WritableDatabase;

            int count = db.Update(table, values, condition, null);
            db.Close();
            return count;
        }
        private int RemoveData(String table, String condition)
        {
            SQLiteDatabase db = this.WritableDatabase;

            int count = db.Delete(table, condition, null);
            db.Close();
            return count;
        }
        private int ClearTable(String table)
        {
            SQLiteDatabase db = this.WritableDatabase;

            // Delete all rows
            int ans = db.Delete(table, null, null);
            db.Close();
            return ans;
        }
    }
}