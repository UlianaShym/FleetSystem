package md5047dd960a6a9ab360bb62735d4b65bf1;


public class LruBitmapCache
	extends android.support.v4.util.LruCache
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_sizeOf:(Ljava/lang/Object;Ljava/lang/Object;)I:GetSizeOf_Ljava_lang_Object_Ljava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("XamarinFleetApp.LruBitmapCache, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LruBitmapCache.class, __md_methods);
	}


	public LruBitmapCache (int p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == LruBitmapCache.class)
			mono.android.TypeManager.Activate ("XamarinFleetApp.LruBitmapCache, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public int sizeOf (java.lang.Object p0, java.lang.Object p1)
	{
		return n_sizeOf (p0, p1);
	}

	private native int n_sizeOf (java.lang.Object p0, java.lang.Object p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
