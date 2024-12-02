﻿namespace kerbormed_httpservice.Model
{
    public class UserOrganizations
    {
        public int Id { get; set; }  // This should be auto-generated by the database
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsAssociate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
