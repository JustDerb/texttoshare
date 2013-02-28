using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace t2sBackend
{
    public enum PhoneCarrier
    {
        [Description("vtext.com")]
        Verizon,
        [Description("messaging.sprint.com")]
        Sprint,
        [Description("email.uscc.net")]
        USCellular,
        [Description("txt.att.com")]
        ATT
    }
}
