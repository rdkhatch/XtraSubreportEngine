using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using System.Linq.Expressions;
using System.Collections;
using XtraSubreportEngine.Support;
using GeniusCode.Framework.Support.Refection;
using System.Reflection;
using DevExpress.XtraReports.Serialization;

namespace XtraSubreport.Engine.Support
{
    public abstract class XtraReportWithCustomPropertiesBase : XtraReport
    {
        private List<PropertyInfo> _CustomProperties = new List<PropertyInfo>();

        public XtraReportWithCustomPropertiesBase() : base()
        {
            DeclareCustomProperties();
        }

        #region Declare Custom Properties

        protected abstract void DeclareCustomProperties();

        protected void DeclareCustomObjectProperty<T>(Expression<Func<T>> property)
            where T : IXRSerializable, new()
        {
            _DeclareCustomProperty(property);
        }

        protected void DeclareCustomValueProperty(Expression<Func<string>> property)
        {
            _DeclareCustomProperty(property);
        }

        protected void DeclareCustomValueProperty<T>(Expression<Func<T>> property)
            where T : struct
        {
            _DeclareCustomProperty(property);
        }

        private void _DeclareCustomProperty<T>(Expression<Func<T>> property)
        {
            var propertyInfo = ReflectionHelper.ExtractPropertyInfo(property);
            _CustomProperties.Add(propertyInfo);
        }

        #endregion

        #region Serialization Overrides

        protected override void SerializeProperties(XRSerializer serializer)
        {
            base.SerializeProperties(serializer);
          
            foreach (var property in _CustomProperties)
                XtraReportsSerializationHelper.SerializeMember(this, property, serializer);
        }

        protected override void DeserializeProperties(DevExpress.XtraReports.Serialization.XRSerializer serializer)
        {
            base.DeserializeProperties(serializer);
            
            foreach (var property in _CustomProperties)
                XtraReportsSerializationHelper.DeserializeMember(this, property, serializer);
        }

        #endregion

    }
}
