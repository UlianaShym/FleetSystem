using Android.Support.V4.Util;
using Java.Lang;
using Android.Graphics;

namespace XamarinFleetApp
{
    class LruBitmapCache : LruCache
    {
        public LruBitmapCache(int sizeInKiloBytes) : base(sizeInKiloBytes) { }

        public LruBitmapCache() : base(getDefaultLruCacheSize()) { }

        public static int getDefaultLruCacheSize()
        {
            int maxMemory = (int)(Runtime.GetRuntime().MaxMemory() / 1024);
            int cacheSize = maxMemory / 8;

            return cacheSize;
        }

        protected override int SizeOf(Java.Lang.Object key, Java.Lang.Object value)
        {
            Bitmap valueTemp = (Bitmap)value;
            
            return valueTemp.RowBytes * valueTemp.Height / 1024;
        }
        
        public Bitmap getBitmap(string url)
        {
            return (Bitmap)Get(url);
        }

    
        public void putBitmap(string url, Bitmap bitmap)
        {
            Put(url, bitmap);
        }
    }
}