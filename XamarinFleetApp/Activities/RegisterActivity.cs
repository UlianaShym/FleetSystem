using System;
using System.Net.Http;
using System.Json;
using System.Threading.Tasks;
using System.Text;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;

namespace XamarinFleetApp.Activities
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    class RegisterActivity : AppCompatActivity
    {
        private Button btnRegister;
        private Button btnLinkToLogin;
        private EditText inputLogin;
        private EditText inputPassword;
        private ProgressDialog pDialog;
        private SessionManager session;
        private SQLiteHandler db;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_register);

            inputLogin = (EditText)FindViewById(Resource.Id.login);
            inputPassword = (EditText)FindViewById(Resource.Id.password);
            btnRegister = (Button)FindViewById(Resource.Id.btnRegister);
            btnLinkToLogin = (Button)FindViewById(Resource.Id.btnLinkToLoginScreen);

            // Progress dialog
            pDialog = new ProgressDialog(this);
            pDialog.SetCancelable(false);

            // Session manager
            session = new SessionManager(ApplicationContext);

            // SQLite database handler
            db = new SQLiteHandler(ApplicationContext);

            // Check if user is already logged in or not
            if (session.isLoggedIn())
            {
                // User is already logged in. Take him to main activity
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }

            btnRegister.Click += btnRegister_Click;
            btnLinkToLogin.Click += btnLinkToLogin_Click;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            String name = inputLogin.Text.Trim();
            String password = inputPassword.Text.Trim();

            if (name != String.Empty && password != String.Empty)
            {

                registerUser(name, password);
            }
            else
            {
                Toast.MakeText(ApplicationContext,
                               Resource.String.register_message,
                               ToastLength.Long).Show();
            }
        }

        private void btnLinkToLogin_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(ApplicationContext, typeof(LoginActivity));
            StartActivity(i);
            Finish();
        }

        private async void registerUser(String login, String password)
        {
            pDialog.SetMessage("Реєстрація ...");
            showDialog();

            string response = await SendRequests(login, password);
            hideDialog();

            JsonValue jsonResponse = JsonObject.Parse(response);
            bool error = jsonResponse["error"];

            if (!error)
            {
                // Now store the user in SQLite
                string userLogin = jsonResponse["login"];

                // Inserting row in users table
                //db.addUser(userLogin);

                Toast.MakeText(ApplicationContext,
                               "Користувача успішно зареєстровано. Спробуйте ввійти тепер!",
                               ToastLength.Long).Show();

                Intent intent = new Intent(this, typeof(LoginActivity));

                StartActivity(intent);
                Finish();
            }
            else
            {
                Toast.MakeText(ApplicationContext,
                               (string)jsonResponse["message"],
                               ToastLength.Long).Show();
            }
        }

        private async Task<string> SendRequests(string login, string password)
        {
            var client = new HttpClient();

            string json = String.Format("\"login\":\"{0}\", \"password\":\"{1}\"", login, password);
            json = "{" + json + "}";

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(AppConfig.URL_REGISTER, content);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        private void showDialog()
        {
            if (!pDialog.IsShowing)
                pDialog.Show();
        }

        private void hideDialog()
        {
            if (pDialog.IsShowing)
                pDialog.Dismiss();
        }
    }
}