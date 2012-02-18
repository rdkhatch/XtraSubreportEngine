using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XtraSubReport.Winforms.Support;
using XtraSubreport.Engine.Eventing;

namespace XtraSubReport.Winforms.Popups
{
    public partial class TraceOutput : Form, IHandle<NLogMessage>
    {
        private readonly IEventAggregator _eventAggregator;

        public TraceOutput()
        {
            InitializeComponent();
        }

        public TraceOutput(IEventAggregator eventAggregator): this()
        {
            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        public void Handle(NLogMessage message)
        {
            memoEdit1.Text = message.LogMessage.FormattedMessage + "\r\n" + memoEdit1.Text;
            this.Refresh();
        }

        private void TraceOutput_Load(object sender, EventArgs e)
        {
        }

        private void TraceOutput_FormClosed(object sender, FormClosedEventArgs e)
        {
            _eventAggregator.Unsubscribe(this);
        }

    }
}
