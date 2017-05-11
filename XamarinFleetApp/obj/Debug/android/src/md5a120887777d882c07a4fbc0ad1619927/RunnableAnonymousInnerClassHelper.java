package md5a120887777d882c07a4fbc0ad1619927;


public class RunnableAnonymousInnerClassHelper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Runnable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler:Java.Lang.IRunnableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("XamarinFleetApp.Activities.RunnableAnonymousInnerClassHelper, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RunnableAnonymousInnerClassHelper.class, __md_methods);
	}


	public RunnableAnonymousInnerClassHelper () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RunnableAnonymousInnerClassHelper.class)
			mono.android.TypeManager.Activate ("XamarinFleetApp.Activities.RunnableAnonymousInnerClassHelper, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public RunnableAnonymousInnerClassHelper (md5a120887777d882c07a4fbc0ad1619927.MainActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == RunnableAnonymousInnerClassHelper.class)
			mono.android.TypeManager.Activate ("XamarinFleetApp.Activities.RunnableAnonymousInnerClassHelper, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "XamarinFleetApp.Activities.MainActivity, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
