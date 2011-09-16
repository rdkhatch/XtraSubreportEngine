using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ColorReplaceAction;
using ColorReplaceAction.Configuration;
using DevExpress.XtraReports.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XtraSubreport.Contracts.RuntimeActions;
using XtraSubreport.Engine.RuntimeActions;

namespace XtraSubReport.Tests.RuntimeActions
{
    [TestClass]
    public class ColorReplacerTest
    {
        private XRLabel beforeLabel;
        private XRLabel afterLabel;
        private ColorReplaceActionConfiguration config;

        [TestInitialize]
        public void Init()
        {
            beforeLabel = new XRLabel() { ForeColor = Color.Red, BackColor = Color.White, BorderColor = Color.Blue };
            afterLabel = new XRLabel() { ForeColor = Color.Green, BackColor = Color.Gold, BorderColor = Color.Yellow };

            config = new ColorReplaceActionConfiguration();
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.ForeColor, beforeLabel.ForeColor, afterLabel.ForeColor));
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.BackColor, beforeLabel.BackColor, afterLabel.BackColor));
            config.ColorReplaceDefinitions.Add(new ColorReplaceDefinition(ColorLocation.BorderColor, beforeLabel.BorderColor, afterLabel.BorderColor));
        }

        [TestMethod]
        public void should_replace_colors_through_report_exporting_itself()
        {
            XtraReport myreport = null;

            var action = new ColorReplacerAction(config);

            var actions = new List<IReportRuntimeAction>();
            actions.Add(action);

            var visitor = new XRRuntimeVisitor(actions);
            visitor.AttachToAggregator();

            var stream = new MemoryStream();
            myreport.ExportToHtml(stream);
            var html = new StringReader(stream.ToString()).ReadToEnd();

            // Color Asserts!
            Assert.Fail();
        }

        [TestMethod]
        public void should_replace_colors_directly()
        {
            var action = new ColorReplacerAction(config);
            action.ActionToApply.Invoke(beforeLabel);

            Assert.AreEqual(afterLabel.ForeColor, beforeLabel.ForeColor);
            Assert.AreEqual(afterLabel.BackColor, beforeLabel.BackColor);
            Assert.AreEqual(afterLabel.BorderColor, beforeLabel.BorderColor);
        }

        [TestMethod]
        public void configuration_serialization_test()
        {
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
