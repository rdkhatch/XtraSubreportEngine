using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace ColorReplaceAction.Configuration
{
    public class ColorReplaceActionConfiguration
    {
        public ColorReplaceActionConfiguration()
        {
            ColorReplaceDefinitions = new List<ColorReplaceDefinition>();
        }

        public List<ColorReplaceDefinition> ColorReplaceDefinitions { get; set; }

        public void Serialize(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Create);
            Serialize(stream);
            stream.Close();
        }

        public static ColorReplaceActionConfiguration Deserialize(string filePath)
        {
            if (File.Exists(filePath))
            {
                ColorReplaceActionConfiguration result;
                var stream = new FileStream(filePath, FileMode.Open);
                result = Deserialize(stream);
                stream.Close();
                return result;
            }
            else
                // If no file, return empty configuration
                return new ColorReplaceActionConfiguration();
        }

        public void Serialize(Stream stream)
        {
            // Serializing Event
            this.ColorReplaceDefinitions.ForEach(definition => definition.OnSerializing());

            // XmlSerializer
            var izer = CreateSerializer();
            izer.Serialize(stream, this);
            stream.Flush();
        }

        public static ColorReplaceActionConfiguration Deserialize(Stream stream)
        {
            // XmlSerializer
            var izer = CreateSerializer();
            var result = (ColorReplaceActionConfiguration)izer.Deserialize(stream);

            // Deserialized Event
            result.ColorReplaceDefinitions.ForEach(definition => definition.OnDeserialized());

            return result;
        }

        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(ColorReplaceActionConfiguration));
        }
    }
}
