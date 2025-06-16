using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class ProjectCardVersion
    {
        public long Id { get; set; }
        public long ProjectCardId { get; set; }
        public string SerializedData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
