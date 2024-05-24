using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class Wallet
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int PointBalance { get; set; }
        public decimal PersonalWallet { get; set; }
        public decimal OrganizationalWallet { get; set; }
        public virtual User User { get; set; }
    }
}
