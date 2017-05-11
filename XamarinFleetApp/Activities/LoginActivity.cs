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
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        private Button btnLogin;
        private Button btnLinkToRegister;
        private EditText inputLogin;
        private EditText inputPassword;
        private ProgressDialog pDialog;
        private SessionManager session;
        private SQLiteHandler db;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_login);

            inputLogin = (EditText)FindViewById(Resource.Id.login);
            inputPassword = (EditText)FindViewById(Resource.Id.password);
            btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
            btnLinkToRegister = (Button)FindViewById(Resource.Id.btnLinkToRegisterScreen);

            // Progress dialog
            pDialog = new ProgressDialog(this);
            pDialog.SetCancelable(false);

            // SQLite database handler
            db = new SQLiteHandler(ApplicationContext);

            // Session manager
            session = new SessionManager(ApplicationContext);

            // Check if user is already logged in or not

            if (session.isLoggedIn())
            {
                // User is already logged in. Take him to main activity
                Intent intent = new Intent(this, typeof(MainActivity));
                this.StartActivity(intent);

                this.Finish();
            }

            // Login button Click Event
            btnLogin.Click += btnLogin_Click;

            // Link to Register Screen
            btnLinkToRegister.Click += btnLinkToRegister_Click;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String login = inputLogin.Text.ToString().Trim();
            String password = inputPassword.Text.ToString().Trim();

            // Check for empty data in the form
            if (login != String.Empty && password != String.Empty)
            {
                // Login user
                checkLogin(login, password);
            }
            else
            {
                // Prompt user to enter credentials
                Toast.MakeText(ApplicationContext,
                               Resource.String.enter_credentials,
                               ToastLength.Long).Show();
            }
        }

        private void btnLinkToRegister_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(ApplicationContext, typeof(RegisterActivity));
            StartActivity(i);
            Finish();
        }

        // Function to verify login details in mysql db
        private async void checkLogin(String login, String password)
        {
            string loginMessage = ApplicationContext.GetString(Resource.String.logging_message);

            pDialog.SetMessage(loginMessage);
            showDialog();

            string response = await SendRequests(login, password);
            hideDialog();

            JsonValue jsonResponse = JsonObject.Parse(response);
            bool error = jsonResponse["error"];

            if (!error)
            {
                session.setLogin(true);

                // Now store the user in SQLite
                string userLogin = jsonResponse["user_id"];

                // Inserting row in users table
                db.AddUser(login, userLogin);

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

            var response = await client.PostAsync(AppConfig.URL_LOGIN, new StringContent(json, Encoding.UTF8, "application/json"));

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