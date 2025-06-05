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

        [Column("title")] // dodaj kolumnę w tabeli lub usuń to pole, jeśli nie potrzebujesz
        public string Title { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("msg_from")]
        public long FromId { get; set; }

        [Column("msg_to")]
        public string ToIds { get; set; }  // w bazie jest int, a model string - trzeba ujednolicić

        [NotMapped]
        public List<long> ToId
        {
            get
            {
                if (string.IsNullOrEmpty(ToIds))
                    return new List<long>();
                return JsonSerializer.Deserialize<List<long>>(ToIds);
            }
            set
            {
                ToIds = JsonSerializer.Serialize(value);
            }
        }

        [Column("is_read")]
        public bool IsRead { get; set; }
    }
}
