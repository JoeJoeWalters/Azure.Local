using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Local.Domain.Timesheets
{
    public class TimesheetComponentItem
    {
        public double Units { get; set; } = 0.0D;
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MinValue;
    }
}
