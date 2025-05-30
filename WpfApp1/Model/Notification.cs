namespace WpfApp1.Model
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int fromId { get; set; }
        public List<int> toId { get; set; }
        public bool IsRead { get; set; }
    }
}
