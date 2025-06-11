using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class Notification
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        public long? ProjectCardId { get; set; }
        [ForeignKey("ProjectCardId")]
        public ProjectCard? ProjectCard { get; set; }

        public long? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project{ get; set; }


        [Column("title")]
        public string Title { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("tag")]
        public string Tag { get; set; }

        [Column("msg_from")]
        public long FromId { get; set; }

        [Column("msg_to")]
        public long ToId { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }
    }
}
