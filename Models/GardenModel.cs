using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models
{
    internal class GardenModel
    {
        [Column("id")]
        public int GardenId { get; set; }
        public List<PlantModel> Plants { get; set; } = new();
    }
}
