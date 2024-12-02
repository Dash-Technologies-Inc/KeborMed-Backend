namespace kebormed.grpcservice.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
