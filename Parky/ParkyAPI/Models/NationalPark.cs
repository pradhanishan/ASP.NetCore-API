using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models
{

    public class NationalPark
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        public byte[] Picture { get; set; }
        public DateTime Created { get; set; }

        public DateTime Established { get; set; }
    }
}
