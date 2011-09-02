using System;
using DevExpress.XtraReports.Serialization;
using System.Collections.Generic;
using System.Collections;

namespace XtraSubreportEngine.Support
{
    // http://www.devexpress.com/Support/Center/p/Q232725.aspx
    public class XRSerializableCollection<T> : List<T>, IXRSerializable, IXRSerializableCollection
            where T : class, IXRSerializable, new()
    {
        void IXRSerializable.DeserializeProperties(XRSerializer serializer)
        {
            Clear();
            int count = serializer.DeserializeInteger("ItemCount", 0);
            for (int i = 0; i < count; i++)
            {
                var item = new T();
                serializer.Deserialize("Item" + i, item);
                Add(item);
            }
        }
        IList IXRSerializable.SerializableObjects
        {
            get
            {
                return new Object[] { };
            }
        }
        void IXRSerializable.SerializeProperties(XRSerializer serializer)
        {
            serializer.SerializeInteger("ItemCount", this.Count);
            for (int i = 0; i < Count; i++)
                serializer.Serialize("Item" + i, this[i]);
        }
    }

    public interface IXRSerializableCollection : IList, IXRSerializable
    {

    }

}
