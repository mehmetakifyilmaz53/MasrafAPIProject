using System.ComponentModel.DataAnnotations;

namespace MasrafDeneme.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
