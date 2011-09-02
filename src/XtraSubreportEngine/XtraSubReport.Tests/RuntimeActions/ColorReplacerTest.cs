using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevExpress.XtraReports.UI;
using ColorReplaceAction;
using ColorReplaceAction.Configuration;
using System.Drawing;
using System.IO;

namespace XtraSubReport.Tests.RuntimeActions
{
    [TestClass]
    public class ColorReplacerTest
    {
        [TestMethod]
        public void should_replace_colors()
        {
            var beforeLabel = new XRLabel() { ForeColor = Color.Red, BackColor = Color.White, BorderColor = Color.Blue };
            var afterLabel = new XRLabel() { ForeColor = Color.Green, BackColor = Color.Gold, BorderColor = Color.Yellow };

            var config = new ColorReplaceActionConfiguration();
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.ForeColor, beforeLabel.ForeColor, afterLabel.ForeColor));
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.BackColor, beforeLabel.BackColor, afterLabel.BackColor));
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.BorderColor, beforeLabel.BorderColor, afterLabel.BorderColor));

            var action = new ColorReplacerAction(config);
            action.ActionToApply.Invoke(beforeLabel);

            Assert.AreEqual(afterLabel.ForeColor, beforeLabel.ForeColor);
            Assert.AreEqual(afterLabel.BackColor, beforeLabel.BackColor);
            Assert.AreEqual(afterLabel.BorderColor, beforeLabel.BorderColor);
        }

        [TestMethod]
        public void configuration_serialization_test()
        {
            var beforeLabel = new XRLabel() { ForeColor = Color.Red, BackColor = Color.White, BorderColor = Color.Blue };
            var afterLabel = new XRLabel() { ForeColor = Color.Green, BackColor = Color.Gold, BorderColor = Color.Yellow };

            var config = new ColorReplaceActionConfiguration();
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.ForeColor, beforeLabel.ForeColor, afterLabel.ForeColor));
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.BackColor, beforeLabel.BackColor, afterLabel.BackColor));
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.BorderColor, beforeLabel.BorderColor, afterLabel.BorderColor));

            var stream = new MemoryStream();

            // Uncomment to create example config file
            //var stream = new FileStream(@"c:\temp\ColorReplaceAction.config", FileMode.Create);

            // Serialize
            config.Serialize(stream);
            stream.Position = 0;

            // Deserialize
            var config2 = ColorReplaceActionConfiguration.Deserialize(stream);

            Assert.AreEqual(config.ColorReplaceDefinitions.Count, config2.ColorReplaceDefinitions.Count);

            Action<ColorReplaceDefinition, ColorReplaceDefinition> assertAreEqual = (def1, def2) =>
                {
                    Assert.AreEqual(def1.Location, def2.Location);
                    Assert.AreEqual(def1.FromColor, def2.FromColor);
                    Assert.AreEqual(def1.ToColor, def2.ToColor);
                };

            assertAreEqual(config.ColorReplaceDefinitions[0], config2.ColorReplaceDefinitions[0]);
            assertAreEqual(config.ColorReplaceDefinitions[1], config2.ColorReplaceDefinitions[1]);
            assertAreEqual(config.ColorReplaceDefinitions[2], config2.ColorReplaceDefinitions[2]);
        }
    }
}
