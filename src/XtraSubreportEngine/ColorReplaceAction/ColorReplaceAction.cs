using System.Drawing;
using System.IO;
using System.Linq;
using ColorReplaceAction.Configuration;
using DevExpress.XtraReports.UI;
using XtraSubreport.Contracts.RuntimeActions;

namespace ColorReplaceAction
{
    [ReportRuntimeActionExportAttribute("Color Replacer", "Replaces ForeColors, BackColors, and BorderColors of any report item using a config file.")]
    public class ColorReplacerAction : ReportRuntimeActionBase<XRControl>
    {
        ColorReplaceActionConfiguration _config;

        public ColorReplacerAction()
            : this(GetDefaultConfigFilePath())
        {
        }

        public ColorReplacerAction(string configFile)
            : this(ColorReplaceActionConfiguration.Deserialize(configFile))
        {
        }

        public ColorReplacerAction(ColorReplaceActionConfiguration config)
            : base(control => true, null)
        {
            _config = config;

            _action = control =>
            {
                control.ForeColor = GetReplacementColor(control.ForeColor, ColorLocation.ForeColor);
                control.BackColor = GetReplacementColor(control.BackColor, ColorLocation.BackColor);
                control.BorderColor = GetReplacementColor(control.BorderColor, ColorLocation.BorderColor);
            };
        }

        private Color GetReplacementColor(Color color, ColorLocation location)
        {
            var q = from definition in _config.ColorReplaceDefinitions
                    where location == definition.Location
                    where color.ToArgb() == definition.FromColor.ToArgb()
                    select definition;

            var colorChange = q.SingleOrDefault();

            if (colorChange != null)
                // Replace Color
                return colorChange.ToColor;
            else
                // Same Color
                return color;
        }

        private static string GetDefaultConfigFilePath()
        {
            // Default config file location
            var assemblyPath = Path.GetDirectoryName(typeof(ColorReplacerAction).Assembly.Location) + @"\";
            var fileName = @"ColorReplaceAction.config";
            var filePath = System.IO.Path.Combine(assemblyPath, fileName);

            return filePath;
        }

    }
}