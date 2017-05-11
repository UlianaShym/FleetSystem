package md5eb9918f09d23af55f28f270a5890e931;


public class KeepAliveService
	extends android.app.Service
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onBind:(Landroid/content/Intent;)Landroid/os/IBinder;:GetOnBind_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("Android.Support.CustomTabs.KeepAliveService, Xamarin.Android.Support.CustomTabs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=71f3e3261ac778b5", KeepAliveService.class, __md_methods);
	}


	public KeepAliveService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == KeepAliveService.class)
			mono.android.TypeManager.Activate ("Android.Support.CustomTabs.KeepAliveService, Xamarin.Android.Support.CustomTabs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=71f3e3261ac778b5", "", this, new java.lang.Object[] {  });
	}


	public android.os.IBinder onBind (android.content.Intent p0)
	{
		return n_onBind (p0);
	}

	private native android.os.IBinder n_onBind (android.content.Intent p0);

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
