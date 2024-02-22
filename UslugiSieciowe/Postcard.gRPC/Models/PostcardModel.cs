using System.ComponentModel.DataAnnotations;

namespace Postcard.gRPC.Models
{
    public class PostcardModel
    {
        public int Id { get; set; }
        [Required]
        public string User { get; set; }
        [Required]
        public int Email { get; set; }
        [Required]
        public int Prompt { get; set; }
    }
}
