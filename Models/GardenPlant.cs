namespace GreenThumb.Models
{
    internal class GardenPlant
    {
        public int GardenId { get; set; }
        public GardenModel Garden { get; set; } = null!;
        public int PlantId { get; set; }
        public PlantModel Plant { get; set; } = null!;
        public int Quanity { get; set; }
    }
}
