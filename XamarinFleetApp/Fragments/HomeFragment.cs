using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace XamarinFleetApp.Fragments
{
    public class HomeFragment : Android.Support.V4.App.Fragment
    {
        private SQLiteHandler db;

        private ProgressDialog pDialog;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            db = new SQLiteHandler(Activity.ApplicationContext);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Inflate the layout for this fragment
            View view = inflater.Inflate(Resource.Layout.fragment_home, container, false);


            pDialog = new ProgressDialog(Context);

            // Showing progress dialog before making http request
            pDialog.SetMessage("Завантаження...");
            pDialog.Show();




            TextView race_link = (TextView)view.FindViewById(Resource.Id.Race_link_textview);
            TextView points_count = (TextView)view.FindViewById(Resource.Id.Points_count_textview);
            TextView car_link = (TextView)view.FindViewById(Resource.Id.Car_link_textview);

            HomeInfo info = db.GetHomeInfo();

            race_link.Text = info.Race;
            car_link.Text = info.Car;
            points_count.Text = info.Points;

            race_link.Click += Link_Click;
            car_link.Click += Link_Click;

            pDialog.Hide();
            return view;
        }

        private void Link_Click(object sender, EventArgs e)
        {
            //var uri = Android.Net.Uri.Parse(((TextView)sender).Text);
            StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://www.google.com.ua")));
        }
        /*
private async Task<string> SendRequests(string id)
{
   var client = new HttpClient();

   var response = await client.GetAsync(AppConfig.URL_SEARCH + id);

   var responseString = await response.Content.ReadAsStringAsync();

   return responseString;
}*/


    }
}