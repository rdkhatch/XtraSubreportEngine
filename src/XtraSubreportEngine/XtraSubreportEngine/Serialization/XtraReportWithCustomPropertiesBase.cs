using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DevExpress.XtraReports.Serialization;
using DevExpress.XtraReports.UI;
using GeniusCode.Framework.Support.Refection;

namespace XtraSubreport.Engine.Support
{
    public abstract class XtraReportWithCustomPropertiesBase : XtraReport
    {
        private List<PropertyInfo> _CustomProperties = new List<PropertyInfo>();

        public XtraReportWithCustomPropertiesBase()
            : base()
        {
            DeclareCustomProperties();
        }

        #region Declare Custom Properties

        protected abstract void DeclareCustomProperties();

        protected void DeclareCustomObjectProperty<TProperty>(Expression<Func<TProperty>> property)
            where TProperty : IXRSerializable, new()
        {
            _DeclareCustomProperty(property);
        }

        protected void DeclareCustomValueProperty<T>(Expression<Func<T, string>> property)
            where T : XtraReportWithCustomPropertiesBase
        {
            _DeclareCustomProperty(property);
        }

        protected void DeclareCustomValueProperty<TProperty>(Expression<Func<TProperty>> property)
            where TProperty : struct
        {
            _DeclareCustomProperty(property);
        }

        private void _DeclareCustomProperty(LambdaExpression property)
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
