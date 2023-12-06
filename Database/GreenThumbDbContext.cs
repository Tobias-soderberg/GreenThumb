using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using GreenThumb.Managers;
using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenThumb.Database
{
    internal class GreenThumbDbContext : DbContext
    {
        private readonly IEncryptionProvider _provider;
        public GreenThumbDbContext()
        {
            _provider = new GenerateEncryptionProvider(KeyManager.getEncryptionKey());
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<GardenModel> Gardens { get; set; }
        public DbSet<PlantModel> Plants { get; set; }
        public DbSet<InstructionModel> Instructions { get; set; }
        public DbSet<GardenPlant> GardenPlant { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = GreenThumbDb; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Set Encryption provider
            modelBuilder.UseEncryption(_provider);

            //Set Relations
            modelBuilder.Entity<GardenModel>()
                .HasMany(g => g.Plants)
                .WithMany(p => p.Gardens)
                .UsingEntity<GardenPlant>(
                gp => gp.HasOne(g => g.Plant).WithMany().HasForeignKey(gp => gp.PlantId),
                gp => gp.HasOne(g => g.Garden).WithMany().HasForeignKey(gp => gp.GardenId),
                gp =>
                {
                    gp.HasKey(gp => new { gp.GardenId, gp.PlantId });
                    gp.Property(gp => gp.Quanity).IsRequired();
                }

                );

            modelBuilder.Entity<UserModel>()
                .HasOne(g => g.Garden)
                .WithOne(u => u.User)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PlantModel>()
                .HasMany(plant => plant.Instructions)
                .WithOne(ins => ins.Plant)
                .HasForeignKey(ins => ins.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserModel>()
                .HasOne(e => e.Garden)
                .WithOne(e => e.User)
                .HasForeignKey<GardenModel>(g => g.UserId);


            //SEED DATA

            modelBuilder.Entity<PlantModel>()
                .HasData(
                new PlantModel()
                {
                    PlantId = 1,
                    Name = "Rose",
                    Description = "The rose is a perennial flowering plant known for its exquisite and diverse beauty. With its lush, fragrant blossoms and thorny stems, the rose has become a symbol of love, passion, and elegance. The flower's intricate layers of petals form a captivating array of colors, ranging from classic reds and pinks to whites, yellows, and even blues. Roses are cultivated in various varieties, each showcasing unique shapes and sizes. Beyond its aesthetic appeal, the rose has cultural and symbolic significance in art, literature, and traditions worldwide.",
                    ImgUrl = "https://images.photowall.com/products/59359/red-rose.jpg?h=699&q=85",
                },
                new PlantModel()
                {
                    PlantId = 2,
                    Name = "Dandelion",
                    Description = "The dandelion (Taraxacum officinale) is a common flowering plant known for its distinctive yellow flowers that transform into fluffy seed heads resembling small parachutes. Despite being considered a weed in many lawns, dandelions have notable health benefits and culinary uses. The plant's leaves, flowers, and roots are all utilized in various traditional and alternative medicines.",
                    ImgUrl = "https://wildflower-favours.co.uk/wp-content/uploads/2022/04/dandelion.jpg",
                },
                new PlantModel()
                {
                    PlantId = 3,
                    Name = "Cactus",
                    Description = "Cacti are a unique group of succulent plants known for their distinctive appearance, typically characterized by thick, fleshy stems and spines. Adapted to arid environments, cacti are renowned for their water storage capabilities, enabling them to survive in dry and challenging conditions. They come in various shapes and sizes, ranging from the iconic saguaro cactus with arms to small, globular species.",
                    ImgUrl = "https://www.lovethegarden.com/sites/default/files/styles/large/public/content/articles/UK_plant-finder-indoor-plants-saguaro-cactus_main.jpg?itok=qCFCzXFH",
                },
                new PlantModel()
                {
                    PlantId = 4,
                    Name = "Pear Tree",
                    Description = "The pear tree (Pyrus) is a deciduous fruit tree known for producing the sweet and succulent pear fruit. With a graceful and spreading canopy, the pear tree typically features simple, ovate leaves that provide a vibrant green backdrop during the growing season. In spring, the tree showcases clusters of white to pinkish blossoms, contributing to its ornamental appeal. Pears, the tree's fruit, vary in shape, size, and color, with popular varieties including Bartlett, Bosc, and Anjou. The fruit ripens in late summer to early fall, offering a juicy and flavorful treat enjoyed fresh or used in various culinary applications, from desserts to preserves. Pear trees are valued not only for their delicious fruit but also for their aesthetic contributions to gardens and orchards, providing shade and visual interest throughout the seasons.",
                    ImgUrl = "https://th.bing.com/th/id/OIG.gpxPEeeHCxn5.h_rIBId?pid=ImgGn",
                },
                new PlantModel()
                {
                    PlantId = 5,
                    Name = "Peony",
                    Description = "The peony (Paeonia) is a prized perennial known for its large, fragrant blooms with layered petals in various colors. Blooming in late spring to early summer, these luxurious flowers, set against glossy green foliage, add elegance to gardens and floral arrangements. Symbolizing love and prosperity, peonies are cherished for their enduring beauty and adaptability to well-drained soils.",
                    ImgUrl = "https://th.bing.com/th/id/OIG.j3RcQJt8GSbQNJWIj3en?pid=ImgGn",
                }
                    );


            modelBuilder.Entity<InstructionModel>()
                .HasData(
                new InstructionModel()
                {
                    InstructionId = 1,
                    InstructionText = "6 hours of sunlight per day",
                    PlantId = 1,
                },
                new InstructionModel()
                {
                    InstructionId = 2,
                    InstructionText = "Water in the morning",
                    PlantId = 1,
                },
                new InstructionModel()
                {
                    InstructionId = 3,
                    InstructionText = "Remove dead leaves",
                    PlantId = 1,
                },
                new InstructionModel()
                {
                    InstructionId = 4,
                    InstructionText = "Fertilize during spring or early summer",
                    PlantId = 1,
                },
                new InstructionModel()
                {
                    InstructionId = 5,
                    InstructionText = "Inspect regularly for pests",
                    PlantId = 1,
                },
                new InstructionModel()
                {
                    InstructionId = 6,
                    InstructionText = "6 hours of sunlight per day",
                    PlantId = 2,
                },
                new InstructionModel()
                {
                    InstructionId = 7,
                    InstructionText = "Good drainage to prevent root rot",
                    PlantId = 2,
                },
                new InstructionModel()
                {
                    InstructionId = 8,
                    InstructionText = "Water when soil is dry on touch",
                    PlantId = 2,
                },
                new InstructionModel()
                {
                    InstructionId = 9,
                    InstructionText = "Remove bad blooms",
                    PlantId = 2,
                },
                new InstructionModel()
                {
                    InstructionId = 10,
                    InstructionText = "Harvest leaves when young for culinary use",
                    PlantId = 2,
                },
                new InstructionModel()
                {
                    InstructionId = 11,
                    InstructionText = "At least 6 hours of sunlight per day",
                    PlantId = 3,
                },
                new InstructionModel()
                {
                    InstructionId = 12,
                    InstructionText = "Mix sand into soil for better drainage",
                    PlantId = 3,
                },
                new InstructionModel()
                {
                    InstructionId = 13,
                    InstructionText = "Dry soil completely between watering",
                    PlantId = 3,
                },
                new InstructionModel()
                {
                    InstructionId = 14,
                    InstructionText = "Protect from colder than 10C",
                    PlantId = 3,
                },
                new InstructionModel()
                {
                    InstructionId = 15,
                    InstructionText = "Repot every 2-3 years if you want growth",
                    PlantId = 3,
                },
                new InstructionModel()
                {
                    InstructionId = 16,
                    InstructionText = "Plant with full sunlight and slightly acidic to neutral soil",
                    PlantId = 4,
                },
                new InstructionModel()
                {
                    InstructionId = 17,
                    InstructionText = "Consistent and deep watering",
                    PlantId = 4,
                },
                new InstructionModel()
                {
                    InstructionId = 18,
                    InstructionText = "Regular pruning, especially in late winter / early spring",
                    PlantId = 4,
                },
                new InstructionModel()
                {
                    InstructionId = 19,
                    InstructionText = "Fertilize annually in early spring",
                    PlantId = 4,
                },
                new InstructionModel()
                {
                    InstructionId = 20,
                    InstructionText = "Monitor regularly for signs of pests",
                    PlantId = 4,
                },
                new InstructionModel()
                {
                    InstructionId = 21,
                    InstructionText = "Plant in a well-drained and sunny spot",
                    PlantId = 5,
                },
                new InstructionModel()
                {
                    InstructionId = 22,
                    InstructionText = "Consistent watering, water at the base",
                    PlantId = 5,
                },
                new InstructionModel()
                {
                    InstructionId = 23,
                    InstructionText = "Install stakes to provide support while growing",
                    PlantId = 5,
                },
                new InstructionModel()
                {
                    InstructionId = 24,
                    InstructionText = "Remove bad blooms directly",
                    PlantId = 5,
                },
                new InstructionModel()
                {
                    InstructionId = 25,
                    InstructionText = "Use a slow fertilizer in early spring",
                    PlantId = 5,
                });

        }
    }
}
