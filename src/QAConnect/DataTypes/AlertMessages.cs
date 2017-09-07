using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class AlertMessages
    {
        public string Title { get; set; }
        public bool IsDismissible { get; set; }
        public DataTypes.Enums.MessageType Type { get; set; }
    }
}
