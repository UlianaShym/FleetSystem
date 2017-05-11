using System;

using Android.Content;

namespace XamarinFleetApp
{
    class SessionManager
    {
        ISharedPreferences pref;
        ISharedPreferencesEditor editor;
        Context _context;        

        // Shared preferences file name
        private static String PREF_NAME = "AndroidHiveLogin";

        private static String KEY_IS_LOGGEDIN = "isLoggedIn";

        public SessionManager(Context context)
        {
            this._context = context;
            pref = _context.GetSharedPreferences(PREF_NAME, FileCreationMode.Private);
            editor = pref.Edit();
        }

        public void setLogin(Boolean isLoggedIn)
        {

            editor.PutBoolean(KEY_IS_LOGGEDIN, isLoggedIn);

            // Commit changes
            editor.Commit();            
        }

        public Boolean isLoggedIn()
        {
            return pref.GetBoolean(KEY_IS_LOGGEDIN, false);
        }
    }
}