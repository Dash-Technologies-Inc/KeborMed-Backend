namespace kerbormed_httpservice.Model
{
    public class Pagination
    {
        public int Page { get; set; }  
        public int PageSize { get; set; }  
        public string? OrderBy { get; set; }  
        public string? Direction { get; set; } 
        public string? QueryString { get; set; } 

        public int OrganizationId { get; set; }
    }
}
