namespace BookStoreManagement_1.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? UserFullName { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? IPAddress { get; set; }
        public DateTime RequestAt { get; set; }
        public DateTime ResponseAt { get; set; }
    }

}
