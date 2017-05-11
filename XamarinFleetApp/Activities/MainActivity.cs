using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Widget;
using XamarinFleetApp.Fragments;
using Android.Support.V4.View;
using Android.Locations;
using Android.Runtime;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Globalization;
using System.Json;

namespace XamarinFleetApp.Activities
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    class MainActivity : AppCompatActivity, ILocationListener
    {
        private SessionManager session;
        private SQLiteHandler db;
        private string userid;

        private NavigationView navigationView;
        private DrawerLayout drawer;
        private View navHeader;
        private TextView txtLogin;
        private Android.Support.V7.Widget.Toolbar toolbar;

        // index to identify current nav menu item
        public static int navItemIndex = 0;

        // tags used to attach the fragments
        private static String TAG_HOME = "home";
        private static String TAG_SETTINGS = "settings";
        private static String TAG_LOGOUT = "logout";
        private static String TAG_POINTS = "points";
        private static String TAG_MAP = "map";
        public static String CURRENT_TAG = TAG_HOME;

        // toolbar titles respected to selected nav menu item
        private String[] activityTitles;

        // flag to load home fragment when user presses back key
        private bool shouldLoadHomeFragOnBackPress = true;
        private Handler mHandler;

        // GPS objects
        LocationManager locMgr;
        Location lastLocation;
        string lastTime;


        public MainActivity() : base() { }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main_old);

            toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            mHandler = new Handler();

            drawer = (DrawerLayout)FindViewById(Resource.Id.drawer_layout);
            navigationView = (NavigationView)FindViewById(Resource.Id.nav_view);

            // Navigation view header
            navHeader = navigationView.GetHeaderView(0);
            txtLogin = (TextView)navHeader.FindViewById(Resource.Id.name);

            // load toolbar titles from string resources
            activityTitles = Resources.GetStringArray(Resource.Array.nav_item_activity_titles);

            // initializing navigation menu
            SetUpNavigationView();

            if (bundle == null)
            {
                navItemIndex = 0;
                CURRENT_TAG = TAG_HOME;
                LoadHomeFragment();
            }

            // SqLite database handler
            db = new SQLiteHandler(ApplicationContext);

            // session manager
            session = new SessionManager(ApplicationContext);

            if (!session.isLoggedIn())
            {
                LogoutUser();
            }

            // Fetching user details from sqlite
            string name = db.GetUserName();
            userid = db.GetUserID();
            //String name = "Max";

            // Displaying the user details on the screen
            txtLogin.SetText("Hello " + name + "!", TextView.BufferType.Normal);

            //Button btnLogout = (Button)FindViewById(Resource.Id.btnLogout);
            //btnLogout.Click += btnLogout_Click;

            //--------------------------------------------------------------------------------------------
            //-----------------------------  Geting settings params  -------------------------------------
            //--------------------------------------------------------------------------------------------

            Settings.RefreshSettings(db);

            //--------------------------------------------------------------------------------------------
            //-----------------------  Create GPS connection and variables -------------------------------
            //--------------------------------------------------------------------------------------------

            string[] llp = db.GetLastLocationPoint();
            if (llp != null)
            {
                lastLocation = new Location("gps")
                {
                    Latitude = double.Parse(llp[1]),
                    Longitude = double.Parse(llp[2])
                };
                lastTime = llp[4];
            }
            else
            {
                lastLocation = null;
                lastTime = null;
            }



            locMgr = GetSystemService(Context.LocationService) as LocationManager;
            Criteria locationCriteria = new Criteria()
            {
                Accuracy = Accuracy.Coarse,
                PowerRequirement = Power.Medium
            };
            String locationProvider = locMgr.GetBestProvider(locationCriteria, true);

            if (locationProvider != null)
            {
                locMgr.RequestLocationUpdates(locationProvider, 2000, 1, this);
            }
            else
            {
                string Provider = LocationManager.GpsProvider;

                if (locMgr.IsProviderEnabled(Provider))
                {
                    locMgr.RequestLocationUpdates(Provider, 2000, 1, this);
                }
                else
                {
                    Toast.MakeText(ApplicationContext,
                                  Provider + " is not available. Does the device have location services enabled?",
                                  ToastLength.Long).Show();
                }
            }
        }

        //------------------------------------------------------------------------------------------------
        //-----------------------------------   Activity actions  ----------------------------------------
        //------------------------------------------------------------------------------------------------

        private void LogoutUser()
        {
            session.setLogin(false);

            db.DeleteUsers();

            // Launching the login activity
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
        }

        public Android.Support.V4.App.Fragment GetHomeFragment()
        {
            switch (navItemIndex)
            {
                case 0:
                    // home
                    HomeFragment homeFragment = new HomeFragment();
                    this.OnAttachFragment(homeFragment);
                    return homeFragment;
                case 1:
                    // Map
                    MapFragment mapFragment = new MapFragment();
                    this.OnAttachFragment(mapFragment);
                    return mapFragment;
                case 2:
                    // Points
                    PointsFragment pointsFragment = new PointsFragment();
                    this.OnAttachFragment(pointsFragment);
                    return pointsFragment;
                case 3:
                    //Settings
                    SettingsFragment settingFragment = new SettingsFragment();
                    this.OnAttachFragment(settingFragment);
                    return settingFragment;
                case 4:
                    //Exit
                    LogoutUser();
                    return null;
                default:
                    HomeFragment _homeFragment = new HomeFragment();
                    this.OnAttachFragment(_homeFragment);
                    return _homeFragment;
            }
        }

        private void LoadHomeFragment()
        {
            // selecting appropriate nav menu item
            SelectNavMenu();

            // set toolbar title
            SetToolbarTitle();

            // if user select the current navigation menu again, don't do anything
            // just close the navigation drawer
            if (this.SupportFragmentManager.FindFragmentByTag(CURRENT_TAG) != null)
            {
                drawer.CloseDrawers();

                return;
            }

            // Sometimes, when fragment has huge data, screen seems hanging
            // when switching between navigation menus
            // So using runnable, the fragment is loaded with cross fade effect
            // This effect can be seen in GMail app

            Java.Lang.IRunnable mPendingRunnable = new RunnableAnonymousInnerClassHelper(this);

            // If mPendingRunnable is not null, then add to the message queue
            if (mPendingRunnable != null)
            {
                mHandler.Post(mPendingRunnable);
            }

            //Closing drawer on item click
            drawer.CloseDrawers();

            // refresh toolbar menu
            InvalidateOptionsMenu();
        }

        private void SetUpNavigationView()
        {
            //Setting Navigation View Item Selected Listener to handle the item click of the navigation menu
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            ActionBarDrawerToggle actionBarDrawerToggle = new ActionBarDrawerToggle(this, drawer, toolbar,
                Resource.String.openDrawer, Resource.String.closeDrawer);

            //Setting the actionbarToggle to drawer layout
            drawer.SetDrawerListener(actionBarDrawerToggle);

            //calling sync state is necessary or else your hamburger icon wont show up
            actionBarDrawerToggle.SyncState();
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            //Check to see which item was being clicked and perform appropriate action
            switch (e.MenuItem.ItemId)
            {
                //Replacing the main content with ContentFragment Which is our Inbox View;
                case Resource.Id.nav_home:
                    navItemIndex = 0;
                    CURRENT_TAG = TAG_HOME;
                    break;
                case Resource.Id.nav_map:
                    navItemIndex = 1;
                    CURRENT_TAG = TAG_MAP;
                    break;
                case Resource.Id.nav_points:
                    navItemIndex = 2;
                    CURRENT_TAG = TAG_POINTS;
                    break;
                case Resource.Id.nav_settings:
                    navItemIndex = 3;
                    CURRENT_TAG = TAG_SETTINGS;
                    break;
                case Resource.Id.nav_logout:
                    navItemIndex = 4;
                    CURRENT_TAG = TAG_LOGOUT;
                    break;
                default:
                    navItemIndex = 0;
                    break;
            }

            //Checking if the item is in checked state or not, if not make it in checked state
            if (e.MenuItem.IsChecked)
            {
                e.MenuItem.SetChecked(false);
            }
            else
            {
                e.MenuItem.SetChecked(true);
            }

            e.MenuItem.SetChecked(true);

            LoadHomeFragment();
        }

        private void SetToolbarTitle()
        {
            SupportActionBar.Title = activityTitles[navItemIndex];
        }

        private void SelectNavMenu()
        {
            navigationView.Menu.GetItem(navItemIndex).SetChecked(true);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            session.setLogin(false);

            db.DeleteUsers();

            // Launching the login activity
            Intent intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            Finish();
        }

        public override void OnBackPressed()
        {
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawers();
                return;
            }

            // This code loads home fragment when back key is pressed
            // when user is in other fragment than home
            if (shouldLoadHomeFragOnBackPress)
            {
                // checking if user is on other navigation menu
                // rather than home
                if (navItemIndex != 0)
                {
                    navItemIndex = 0;
                    CURRENT_TAG = TAG_HOME;
                    LoadHomeFragment();
                    return;
                }
            }

            base.OnBackPressed();
        }

        //------------------------------------------------------------------------------------------------
        //-----------------------------------   GPS Monitoring   -----------------------------------------
        //------------------------------------------------------------------------------------------------
        public async void OnLocationChanged(Location location)
        {
            float dist;
            int time;
            if (lastLocation == null)
            {
                dist = 0;
                time = 0;
            }
            else
            {
                dist = lastLocation.DistanceTo(location);

                DateTime startTime = DateTime.ParseExact(lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime endTime = DateTime.Now;
                TimeSpan span = endTime.Subtract(startTime);

                time = span.Minutes;//current time - last time
            }
            if (lastLocation == null || (!Settings.Distance_enabled || dist > Settings.Distance_value)
                && (Settings.Distance_enabled || Settings.Time_enabled)
                && (!Settings.Time_enabled || time > Settings.Time_value))
            {
                if (db.AddLocationPoint(Settings.Last_id+1, location, 0) > 0)
                {
                    Settings.IncLastId();

                    lastLocation = location;
                    lastTime = db.GetLastLocationPoint()[4];
                    db.UpdateLastPointId(Settings.Last_id);

                    string response = await SendLocation(location, lastTime);

                    JsonValue jsonResponse = JsonObject.Parse(response);
                    bool error = jsonResponse["error"];

                    if (!error)
                    {
                        Toast.MakeText(ApplicationContext,
                                       "Data successfully sended and received",
                                       ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(ApplicationContext,
                                       (string)jsonResponse["message"],
                                       ToastLength.Long).Show();
                    }
                }
                
            }
            //throw new NotImplementedException();
            
        }

        public void OnProviderDisabled(string provider)
        {
            Toast.MakeText(ApplicationContext,
                                  provider + " connection lost",
                                  ToastLength.Long).Show();
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            Toast.MakeText(ApplicationContext,
                                  provider + " connection restored",
                                  ToastLength.Long).Show();
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {/*
            Toast.MakeText(ApplicationContext,
                                  provider + " status changed",
                                  ToastLength.Long).Show();*/
            //throw new NotImplementedException();
        }

        private async Task<string> SendLocation(Location location, string datetime)
        {
            var client = new HttpClient();

            string json =   "{" + 
                String.Format("\"User_id\":\"{0}\", \"Latitude\":\"{1}\", \"Longitude\":\"{2}\", \"Time\":\"{3}\""
                ,userid, location.Latitude, location.Longitude, datetime) 
                            + "}";

            var response = await client.PostAsync(AppConfig.URL_PUSH_COORDS, new StringContent(json, Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }

    class RunnableAnonymousInnerClassHelper : Java.Lang.Object, Java.Lang.IRunnable
    {
        MainActivity m;

        public RunnableAnonymousInnerClassHelper(MainActivity m)
        {
            this.m = m;
        }

        public void Run()
        {
            // update the main content by replacing fragments
            Android.Support.V4.App.Fragment fragment = m.GetHomeFragment();

            if (fragment == null) { return; }

            Android.Support.V4.App.FragmentTransaction fragmentTransaction = m.SupportFragmentManager.BeginTransaction();
            fragmentTransaction.SetCustomAnimations(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeIn);
            fragmentTransaction.Replace(Resource.Id.frame, fragment, MainActivity.CURRENT_TAG);
            fragmentTransaction.CommitAllowingStateLoss();
        }
    }
}