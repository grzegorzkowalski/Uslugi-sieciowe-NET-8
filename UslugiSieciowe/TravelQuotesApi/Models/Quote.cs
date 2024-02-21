using System.ComponentModel.DataAnnotations;

namespace TravelQuotesApi.Models
{
    public class Quote
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Author { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
