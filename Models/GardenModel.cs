using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models;

internal class GardenModel
{
    [Key]
    [Column("id")]
    public int GardenId { get; set; }
    public List<PlantModel> Plants { get; set; } = new();
    [Column("user_id")]
    public int UserId { get; set; }
    public UserModel User { get; set; } = null!;
}
