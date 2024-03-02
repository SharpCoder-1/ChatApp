namespace ChatApp.Server.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string SenderId{ get; set; }
        public User Sender { get; set; }
        public string ReceiverId { get; set; }
        public bool IsEdited { get; set; } = false;
        public User Receiver { get; set; }
        public DateTime DateTime { get; set; }
    }
}
