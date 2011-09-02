using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace ColorReplaceAction.Configuration
{
    public class ColorReplaceDefinition
    {
        public ColorReplaceDefinition()
        {
        }

        public ColorReplaceDefinition(ColorLocation location, Color fromColor, Color toColor)
            : this(location, fromColor, toColor, string.Empty)
        {
        }

        public ColorReplaceDefinition(ColorLocation location, Color fromColor, Color toColor, string name)
        {
            Location = location;
            FromColor = fromColor;
            ToColor = toColor;
            Name = name;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public ColorLocation Location { get; set; }

        [XmlIgnore]
        public Color FromColor { get; set; }

        [XmlIgnore]
        public Color ToColor { get; set; }




        #region Serialization

        [XmlAttribute]
        public string _ToColorHex { get; set; }

        [XmlAttribute]
        public string _FromColorHex { get; set; }

        internal void OnSerializing()
        {
            _FromColorHex = ColorTranslator.ToHtml(FromColor);
            _ToColorHex = ColorTranslator.ToHtml(ToColor);
        }

        internal void OnDeserialized()
        {
            FromColor = ColorTranslator.FromHtml(_FromColorHex);
            ToColor = ColorTranslator.FromHtml(_ToColorHex);

            // Require colors
            if (FromColor == Color.Empty || ToColor == Color.Empty)
                throw new SerializationException(string.Format("Cannot deserialize ColorReplaceDefinition {0} with color {1} or {2}. Valid color is required.", Name, _FromColorHex, _ToColorHex));
        }

        #endregion
    }
}
