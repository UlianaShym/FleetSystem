using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace XamarinFleetApp.Fragments
{
    public class SettingsFragment : Android.Support.V4.App.Fragment
    {
        private SQLiteHandler db;
        EditText distance_textbox;
        CheckBox distance_checkBox;
        EditText time_textbox;
        CheckBox time_checkBox;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            db = new SQLiteHandler(Activity.ApplicationContext);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.fragment_settings, container, false);

            distance_textbox = (EditText)view.FindViewById(Resource.Id.distance_texbox);
            distance_checkBox = (CheckBox)view.FindViewById(Resource.Id.distance_checkBox);
            time_textbox = (EditText)view.FindViewById(Resource.Id.time_texbox);
            time_checkBox = (CheckBox)view.FindViewById(Resource.Id.time_checkBox);

            //Settings.RefreshSettings(db);

            distance_textbox.Text = Settings.Distance_value.ToString();
            distance_checkBox.Checked = Settings.Distance_enabled;
            time_textbox.Text = Settings.Time_value.ToString();
            time_checkBox.Checked = Settings.Time_enabled;

            Button save_button = (Button)view.FindViewById(Resource.Id.button_save_settings);
            save_button.Click += Save_Click;

            return view;
        }

        private void Save_Click(object sender, System.EventArgs e)
        {
            int distance_val = int.Parse(distance_textbox.Text);
            bool distance_enabled = distance_checkBox.Checked;
            int time_val = int.Parse(time_textbox.Text);
            bool time_enabled = time_checkBox.Checked;

            db.UpdateSettings(int.Parse(distance_textbox.Text), distance_checkBox.Checked,
                int.Parse(time_textbox.Text), time_checkBox.Checked);
            Toast.MakeText(Activity.ApplicationContext,
                               "SETTINGS SAVED!",
                               ToastLength.Long).Show();
            Settings.RefreshSettings(db);
        }
    }
}