using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models
{
    internal class PlantModel
    {
        [Key]
        [Column("id")]
        public int PlantId { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("description")]
        public string? Description { get; set; }
        [Column("img_url")]
        public string? ImgUrl { get; set; }
        public List<InstructionModel> Instructions { get; set; } = new();
        public List<GardenModel> Gardens { get; set; } = new();
    }
}
