using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace WpfApp1.Model
{
    public class Notification
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("msg_from")]
        public long FromId { get; set; }

        [Column("msg_to")]
        public List<long> ToId { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }
    }
}
