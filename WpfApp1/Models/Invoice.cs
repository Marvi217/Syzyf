using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string Company { get; set; }
        public string InvoiceNumber { get; set; }
        public string Price { get; set; }
        public string RangeValue { get; set; }
        public string ServiceRange { get; set; }

        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}
