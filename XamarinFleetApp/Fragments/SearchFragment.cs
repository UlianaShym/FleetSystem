using System;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace XamarinFleetApp.Fragments
{
    public class SearchFragment : Android.Support.V4.App.Fragment
    {
        public static String searchText = "";
        private EditText text;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_search, container, false);

            text = (EditText)view.FindViewById(Resource.Id.search);

            Button b = (Button)view.FindViewById(Resource.Id.btnSearch);

            b.Click += SearchClick;

            // Inflate the layout for this fragment
            return view;
        }

        private void SearchClick(object sender, EventArgs e)
        {
            searchText = text.Text;
            //StartActivity(new Intent(this.Activity, typeof(SearchActivity)));
        }
    }
}