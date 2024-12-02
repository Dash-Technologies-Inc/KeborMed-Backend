﻿namespace kebormed.grpcservice.Models
{
    public class Organization
    {
        public int Id { get; set; }  // This should be auto-generated by the database
        public required string Name { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; } // Nullable for soft delete

        public bool? IsDeleted { get; set; }

    }

}
