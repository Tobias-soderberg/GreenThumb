﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GreenThumb.Models;

internal class GardenPlant
{
    [Column("garden_id")]
    public int GardenId { get; set; }
    public GardenModel Garden { get; set; } = null!;
    [Column("plant_id")]
    public int PlantId { get; set; }
    public PlantModel Plant { get; set; } = null!;
    [Column("quantity")]
    public int Quanity { get; set; }
}
