using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models
{
    internal class UserModel
    {
        [Key]
        [Column("id")]
        public int UserId { get; set; }
        [Column("username")]
        public string Username { get; set; } = null!;
        [EncryptColumn]
        [Column("password")]
        public string Password { get; set; } = null!;
        [Column("garden_id")]
        public int GardenId { get; set; }
        public GardenModel Garden { get; set; } = null!;
    }
}
