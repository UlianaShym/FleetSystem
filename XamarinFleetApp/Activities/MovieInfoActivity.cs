using System;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using Android.Graphics;

namespace XamarinOMDB.Activities
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    public class MovieInfoActivity : AppCompatActivity
    {
        private ProgressDialog pDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_movie_info);
            // Create your application here

            pDialog = new ProgressDialog(this);
            pDialog.SetMessage("Завантаження...");
            pDialog.Show();

            load();
        }
        private async void load()
        {
            String id = Intent.Extras.GetString("id");

            string response = await SendRequests(id);

            pDialog.Hide();

            JsonValue jObj = JsonObject.Parse(response);

            bool isResponse = jObj["Response"];

            if (isResponse)
            {
                String s = jObj["Title"];
                s = jObj["Title"].ToString();

                TextView description = (TextView)FindViewById(Resource.Id.description);
                description.Text = jObj["Plot"].ToString();

                TextView title = (TextView)FindViewById(Resource.Id.title);
                title.Text = jObj["Title"];

                TextView genre = (TextView)FindViewById(Resource.Id.genre);
                genre.Text = "Жанр: " + jObj["Genre"].ToString();

                TextView duration = (TextView)FindViewById(Resource.Id.duration);
                duration.Text = "Тривалість: " + jObj["Runtime"];

                TextView rating = (TextView)FindViewById(Resource.Id.rating);
                rating.Text = "Рейтинг: " + jObj["imdbRating"];

                ImageView image = (ImageView)FindViewById(Resource.Id.poster);
                String poster = jObj["Poster"];
                if (!poster.Equals("N/A"))
                {
                    image.SetImageBitmap(GetImageBitmapFromUrl(poster));
                }
                else
                {
                    image.SetImageBitmap(GetImageBitmapFromUrl("http://www.clker.com/cliparts/y/7/8/u/H/D/n-a-md.png"));
                }

            }
            else
            {
                String errorMsg = jObj["Error"];
                Toast.MakeText(ApplicationContext,
                               errorMsg, ToastLength.Long).Show();
            }
        }

        private async Task<string> SendRequests(string id)
        {
            var client = new HttpClient();

            var response = await client.GetAsync(AppConfig.URL_SEARCH + id);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}