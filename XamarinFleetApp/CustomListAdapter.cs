using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace XamarinFleetApp
{
    class CustomListAdapter: BaseAdapter
    {
        private SQLiteHandler db;

        private Activity activity;
        private LayoutInflater inflater;
        private List<WayPoint> pointsList;

        public CustomListAdapter(Activity activity, List<WayPoint> pointsList)
        {
            db = new SQLiteHandler(activity.ApplicationContext);

            this.activity = activity;
            this.pointsList = pointsList;
        }

        public override int Count
        {
            get
            {
                return getCount();
            }
        }

        public int getCount()
        {
            return pointsList.Count;
        }

        public override Java.Lang.Object GetItem(int position)
        {            
            return getItem(position);
        }

        public Java.Lang.Object getItem(int location)
        {
            return pointsList[location];
        }

        public override long GetItemId(int position)
        {
            return getItemId(position);
        }

        public long getItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return getView(position, convertView, parent);
        }

        public View getView(int position, View convertView, ViewGroup parent)
        {

            if (inflater == null)
                inflater = (LayoutInflater)activity
                        .GetSystemService(Context.LayoutInflaterService);

            if (convertView == null)
            {

                convertView = inflater.Inflate(Resource.Layout.list_row, null);
            }                       

            TextView point_id = (TextView)convertView.FindViewById(Resource.Id.point_id);
            TextView point_x = (TextView)convertView.FindViewById(Resource.Id.point_x);
            TextView point_y = (TextView)convertView.FindViewById(Resource.Id.point_y);

            // getting Point data for the row
            WayPoint wp = pointsList[position];

            point_id.SetText(wp.PointId, TextView.BufferType.Normal);
            point_x.SetText(wp.PointLat, TextView.BufferType.Normal);
            point_y.SetText(wp.PointLon, TextView.BufferType.Normal);
            
            /*
            // image
            image.SetImageBitmap(GetImageBitmapFromUrl(wp.getThumbnailUrl()));*/

            return convertView;
        }

        //-------------------------------------------------------------------
        /*
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                try
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return imageBitmap;
        }*/
    }
}