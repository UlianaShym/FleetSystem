using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace XamarinFleetApp.Fragments
{
    public class PointsFragment : Android.Support.V4.App.Fragment
    {
        private SQLiteHandler db;

        private List<WayPoint> pointsList;
        private ProgressDialog pDialog;
        private int page = 0;
        private int pages_load = 0;

        private ListView pointsListView;
        private CustomListAdapter adapter;

        private Button loadMore;


        public override void OnCreate(Bundle savedInstanceState)
        {
            db = new SQLiteHandler(Activity.ApplicationContext);

            pointsList = new List<WayPoint>();
            int i = 0;
            if (savedInstanceState != null)
            {
                while (savedInstanceState.ContainsKey("Point" + i))
                {
                    var p = (WayPoint)savedInstanceState.GetSerializable("Point" + i);
                    pointsList.Add(p);
                    i++;
                }
                page = pages_load = savedInstanceState.GetInt("page");
            }               


            base.OnCreate(savedInstanceState);
                        
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.fragment_points, container, false);

            pDialog = new ProgressDialog(this.Context);
            pDialog.SetMessage("Завантаження...");
            pDialog.Show();

            //Bind list to adapter
            pointsListView = (ListView)view.FindViewById(Resource.Id.points_list_view);
            adapter = new CustomListAdapter(this.Activity, pointsList);
            pointsListView.Adapter = adapter;
            //menu
            pointsListView.ItemLongClick += PointsListView_ItemLongClick;

            //load more event
            loadMore = (Button)view.FindViewById(Resource.Id.loadMore);
            loadMore.Click += ShowMore_Click;

            if (savedInstanceState == null)
                LoadPoints();
            else
                pDialog.Hide();
            
            return view;
        }

        private void PointsListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var menu = new PopupMenu(this.Activity, pointsListView.GetChildAt(e.Position));
            menu.Inflate(Resource.Menu.popup_menu);
            menu.MenuItemClick += (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.pop_menu_delete:
                        // update stuff
                        {
                            WayPoint wp = (WayPoint)pointsListView.GetItemAtPosition(e.Position);
                            pointsList.Remove(wp);
                            adapter.NotifyDataSetChanged();
                            // + Remove from DB
                            //db.RemoveLocationPoitById(wp.PointId);
                        }
                        break;
                    default:
                        break;
                }
            };
            menu.Show();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            for (int i = 0; i < pointsList.Count; i++)
            {
                var p = pointsList[i];
                outState.PutSerializable("Point" + i, p);
            }
            outState.PutInt("page",page);

            // always call the base implementation!
            base.OnSaveInstanceState(outState);
        }

        private void ShowMore_Click(object sender, EventArgs e)
        {
            if (page == pages_load)
                    LoadPoints();
        }

        private async void LoadPoints()
        {
            /*
            WayPoint wp = new WayPoint(pointsList.Count.ToString(), "11.1351", "15.54845");
            pointsList.Add(wp);
            adapter.NotifyDataSetChanged();

            pDialog.Hide();

            pages_load++;*/
            List<WayPoint> wpl = db.GetLocationPoints(page++);
            if (wpl != null)
            {
                pointsList.AddRange(wpl);
                pages_load++;
                adapter.NotifyDataSetChanged();
            }
            else
            {
                page--;
                Toast.MakeText(Activity.ApplicationContext,
                               "Sorry! No more Points",
                               ToastLength.Long).Show();
            }
            pDialog.Hide();
        }
    }
}