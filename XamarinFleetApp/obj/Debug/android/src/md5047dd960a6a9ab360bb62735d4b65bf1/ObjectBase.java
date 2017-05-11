package md5047dd960a6a9ab360bb62735d4b65bf1;


public abstract class ObjectBase
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.io.Serializable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_ReadObject:(Ljava/io/ObjectInputStream;)V:__export__\n" +
			"n_WriteObject:(Ljava/io/ObjectOutputStream;)V:__export__\n" +
			"";
		mono.android.Runtime.register ("XamarinFleetApp.ObjectBase, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ObjectBase.class, __md_methods);
	}


	public ObjectBase () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ObjectBase.class)
			mono.android.TypeManager.Activate ("XamarinFleetApp.ObjectBase, XamarinFleetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	private void readObject (java.io.ObjectInputStream p0) throws java.io.IOException, java.lang.ClassNotFoundException
	{
		n_ReadObject (p0);
	}

	private native void n_ReadObject (java.io.ObjectInputStream p0);


	private void writeObject (java.io.ObjectOutputStream p0) throws java.io.IOException, java.lang.ClassNotFoundException
	{
		n_WriteObject (p0);
	}

	private native void n_WriteObject (java.io.ObjectOutputStream p0);

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
