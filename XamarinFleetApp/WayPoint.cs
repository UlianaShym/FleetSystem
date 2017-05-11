using System;
using Android.Runtime;
using Java.IO;
using Java.Interop;

namespace XamarinFleetApp
{
    [Flags]
    public enum ObjectState
    {
        Normal = 0,
        New = 1,
        Modified = 2,
        Removed = 4
    }

    class WayPoint : ObjectBase
    {
        public string PointId { get; set; }
        public string PointLat { get; set; }
        public string PointLon { get; set; }

        public WayPoint(string pointId, string pointLat, string pointLon)
        {
            this.PointId = pointId;
            this.PointLat = pointLat;
            this.PointLon = pointLon;
        }

        protected override void Deserialize(ObjectInputStream stream)
        {
            base.Deserialize(stream);

            if (stream.ReadBoolean())
                this.PointId = stream.ReadUTF();
            if (stream.ReadBoolean())
                this.PointLat = stream.ReadUTF();
            if (stream.ReadBoolean())
                this.PointLon = stream.ReadUTF();

        }

        protected override void Serialize(ObjectOutputStream stream)
        {
            base.Serialize(stream);

            stream.WriteBoolean(this.PointId != null);
            if (this.PointId != null)
                stream.WriteUTF(this.PointId);

            stream.WriteBoolean(this.PointLat != null);
            if (this.PointLat != null)
                stream.WriteUTF(this.PointLat);

            stream.WriteBoolean(this.PointLon != null);
            if (this.PointLon != null)
                stream.WriteUTF(this.PointLon);
        }
    }
    public abstract class ObjectBase : Java.Lang.Object, ISerializable
    {
        public Guid Id { get; set; }

        public ObjectState State { get; set; }

        protected ObjectBase(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        protected ObjectBase()
        {
        }

        [Export("readObject", Throws = new[] { typeof(IOException), typeof(Java.Lang.ClassNotFoundException) })]
        private void ReadObject(ObjectInputStream stream)
        {
            this.Deserialize(stream);
        }

        [Export("writeObject", Throws = new[] { typeof(IOException), typeof(Java.Lang.ClassNotFoundException) })]
        private void WriteObject(ObjectOutputStream stream)
        {
            this.Serialize(stream);
        }

        protected virtual void Deserialize(ObjectInputStream stream)
        {
            this.Id = Guid.Parse(stream.ReadUTF());
            this.State = (ObjectState)stream.ReadInt();
        }

        protected virtual void Serialize(ObjectOutputStream stream)
        {
            stream.WriteUTF(this.Id.ToString());
            stream.WriteInt((int)this.State);
        }
    }

}