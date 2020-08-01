using System.ComponentModel.DataAnnotations;
using KmLog.Server.Domain;

namespace KmLog.Server.Model
{
    public class RefuelEntry : Entry
    {
        [Required]
        public long Distance { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public double PricePerLiter { get; set; }

        [Required]
        public TankStatus TankStatus { get; set; }
    }
}
