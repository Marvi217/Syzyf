using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class ParticipantItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public object Entity { get; set; }
    }
}
