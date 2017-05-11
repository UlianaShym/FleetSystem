using System;
using System.Collections.Generic;
using System.Json;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinOMDB.Fragments;

namespace XamarinOMDB.Activities
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    public class SearchActivity : AppCompatActivity
    {
        public static String imdbToShow = "";

        // Movies json url
        private static String url = AppConfig.URL_SEARCH;
        private ProgressDialog pDialog;
        private List<Movie> movieList = new List<Movie>();
        private ListView listView;
        private CustomListAdapter adapter;
        private Button showMore;

        private int page = 1;
        private int pages_load = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_search);

            listView = (ListView)FindViewById(Resource.Id.list);
            adapter = new CustomListAdapter(this, movieList);
            listView.Adapter = adapter;

            pDialog = new ProgressDialog(this);
            pDialog.SetMessage("Завантаження...");
            pDialog.Show();             

            listView.ItemClick += listOnItemClick;
            listView.ScrollChange += ListView_ScrollChange;

            DisplaySearchResults(page, SearchFragment.searchText);

            showMore = (Button)FindViewById(Resource.Id.showMore);
            showMore.Click += ShowMore_Click;

        }

        private void ShowMore_Click(object sender, EventArgs e)
        {
            if (page == pages_load)
                DisplaySearchResults(++page, SearchFragment.searchText);
        }

        private void ListView_ScrollChange(object sender, View.ScrollChangeEventArgs e)
        {
            /*
            string down ="";

            if (e.OldScrollY - e.ScrollY > 0)
            {

                down = "down";
                
            }

            pDialog.Hide();
            pDialog.SetMessage(e.OldScrollY + " " + e.ScrollY + " " + e.OldScrollX + " " + e.V.ScrollY);
            pDialog.Show();
            */
            //Toast.MakeText(ApplicationContext, e.OldScrollY + " " + e.ScrollY, ToastLength.Short).Show();

        }

        private void listOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Movie listItem = (Movie)listView.GetItemAtPosition(e.Position);
            imdbToShow = listItem.getId();

            Intent movie = new Intent(this, typeof(MovieInfoActivity));
            movie.PutExtra("id", imdbToShow);

            StartActivity(movie);
        }

        private async void DisplaySearchResults(int page, string text)
        {
            string response = await SendRequests(page, text);

            JsonValue jObj = JsonObject.Parse(response);

            bool isResponse = jObj["Response"];

            if (isResponse)
            {
                JsonArray movies = (JsonArray)jObj["Search"];

                for (int i = 0; i < movies.Count; i++)
                {
                    JsonValue obj = movies[i];

                    Movie movie = new Movie();
                    movie.setTitle(obj["Title"]);

                    string poster = obj["Poster"];

                    if (poster.ToLower() != "N/A".ToLower())
                    {
                        movie.setThumbnailUrl(obj["Poster"]);
                    }
                    else
                    {
                        movie.setThumbnailUrl("http://www.clker.com/cliparts/y/7/8/u/H/D/n-a-md.png");
                    }

                    movie.setId(obj["imdbID"]);
                    movie.setYear(obj["Year"]);

                    movie.setType(obj["Type"]);

                    // adding movie to movies array
                    movieList.Add(movie);
                }

                adapter.NotifyDataSetChanged();
                /*
                while (listView.Count != movies.Count)
                {

                }*/
                pDialog.Hide();

                pages_load++;
            }
            else
            {
                String errorMsg = jObj["Error"];
                Toast.MakeText(ApplicationContext,
                               errorMsg, ToastLength.Long).Show();
            }            
        }

        private async Task<string> SendRequests(int page, string text)
        {
            var client = new HttpClient();

            string json = String.Format("\"page\":\"{0}\"", page);
            json = "{" + json + "}";

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(AppConfig.URL_SEARCH + text, content);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            hidePDialog();
        }

        private void hidePDialog()
        {
            if (pDialog != null)
            {
                pDialog.Dismiss();
                pDialog = null;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            //noinspection SimplifiableIfStatement
            if (id == Android.Resource.Id.Home)
            {
                // finish the activity
                OnBackPressed();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

    }
}