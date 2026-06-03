using System.ComponentModel.DataAnnotations.Schema;

namespace TeddySmith.API.Models
{
    [Table("Portfollos")]
    public class Portfollo
    {
        
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
