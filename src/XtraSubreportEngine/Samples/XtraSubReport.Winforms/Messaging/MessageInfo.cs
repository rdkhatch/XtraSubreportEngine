using System;
using System.Linq;
using System.Text;

namespace XtraSubReport.Winforms.Prototypes
{
    public class MessageInfo
    {
        public MessageInfo(object message, int counter)
        {
            CreatedOn = DateTime.Now;
            Name = message.GetType().Name;
            Id = counter;
            Contents = message.ToString();
        }

        public string Name { get; private set; }
        public int Id { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string Contents { get; private set; }
    }
}
