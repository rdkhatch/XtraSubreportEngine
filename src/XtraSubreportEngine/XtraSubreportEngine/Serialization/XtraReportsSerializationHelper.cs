using System;
using System.Collections;
using System.Reflection;
using DevExpress.XtraReports.Serialization;
using GeniusCode.Framework.Extensions;
using GeniusCode.Framework.Support.Refection;

namespace XtraSubreport.Engine.Support
{
    public static class XtraReportsSerializationHelper
    {

        public static void SerializeMember(IXRSerializable containerInstance, MemberInfo memberInfo, XRSerializer serializer)
        {
            var member = GetTypeDetailsFromMember(memberInfo);
            var name = member.Item1;
            var type = member.Item2;
            // Get Value
            var value = ReflectionHelper.GetMemberValue(containerInstance, name);

            if (typeof(IXRSerializable).IsAssignableFrom(type))
                serializer.Serialize(name, value as IXRSerializable);

            else if (type == typeof(string))
                serializer.SerializeString(name, (string)value);

            else if (type == typeof(Type))
            {
                var typeValue = (Type)value;
                var typeString = (typeValue != null) ? typeValue.AssemblyQualifiedName : string.Empty;
                serializer.SerializeString(name, typeString);
            }

            // Other types here
        }

        public static void DeserializeMember(IXRSerializable containerInstance, MemberInfo memberInfo, XRSerializer serializer)
        {
            var member = GetTypeDetailsFromMember(memberInfo);
            var name = member.Item1;
            var type = member.Item2;
            object value = null;

            // If collection, Clear before deserializing
            var currentValue = ReflectionHelper.GetMemberValue(containerInstance, name);
            var collection = currentValue as IList;
            if (collection != null && collection as IXRSerializable != null)
            {
                collection.Clear();
                serializer.Deserialize(name, collection as IXRSerializable);
                // Items have now been added to the collection.  Finished.
                return;
            }

            else if (typeof(IXRSerializable).IsAssignableFrom(type))
            {
                // must create object using parameterless constructor
                if (type.HasDefaultConstructor() == false)
                    throw new MissingMethodException("A default constructor is required to deserialize IXRSerializable type: {0}".FormatString(type.ToString()));

                value = Activator.CreateInstance(type);
                serializer.Deserialize(name, value as IXRSerializable);
            }

            else if (type == typeof(string))
                value = serializer.DeserializeString(name, string.Empty);

            else if (type == typeof(Type))
                value = Type.GetType(serializer.DeserializeString(name, string.Empty), false);

            // Set Value
            ReflectionHelper.SetMemberValue(containerInstance, name, value);
        }

        private static Tuple<string, Type> GetTypeDetailsFromMember(MemberInfo memberInfo)
        {
            var name = memberInfo.Name;
            Type datatype = null;
            memberInfo.TryAs<PropertyInfo>(property => datatype = property.PropertyType);
            memberInfo.TryAs<FieldInfo>(field => datatype = field.FieldType);
            if (datatype == null)
                throw new ArgumentOutOfRangeException("memberInfo", "Only Fields and Properties can be serialized!");
            return new Tuple<string, Type>(name, datatype);
        }

    }
}
