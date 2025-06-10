using System.ComponentModel.DataAnnotations;
using WpfApp1.Model;

public class Meeting
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    public DateTime? StartTime { get; set; }    // dodaj
    public DateTime? EndTime { get; set; }      // dodaj

    public ICollection<MeetingParticipant> Participants { get; set; } = new List<MeetingParticipant>();
}
