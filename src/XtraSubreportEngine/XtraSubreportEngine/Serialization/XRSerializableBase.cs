using System;
using DevExpress.XtraReports.Serialization;
using System.Collections.Generic;
using System.Reflection;
using GeniusCode.Framework.Extensions;
using GeniusCode.Framework.Support.Refection;
using XtraSubreport.Engine.Support;

namespace XtraSubreportEngine.Support
{
    public abstract class XRSerializableBase : IXRSerializable
    {
        #region Helper Methods
        
        private IEnumerable<MemberInfo> GetMemberInfos()
        {
            var list = new List<MemberInfo>();
            list.AddRange(ReflectionHelper.GetProperties(this));
            list.AddRange(ReflectionHelper.GetFields(this));
            return list;
        }


        #endregion


        #region XR Serialization

        void IXRSerializable.SerializeProperties(XRSerializer serializer)
        {
            var members = this.GetMemberInfos();

            foreach (var member in members)
                XtraReportsSerializationHelper.SerializeMember(this, member, serializer);
        }

        void IXRSerializable.DeserializeProperties(XRSerializer serializer)
        {
            var members = this.GetMemberInfos();

            foreach(var member in members)
                XtraReportsSerializationHelper.DeserializeMember(this, member, serializer);
        }

        System.Collections.IList IXRSerializable.SerializableObjects
        {
            get { return null; }
        }

        #endregion

    }
}
