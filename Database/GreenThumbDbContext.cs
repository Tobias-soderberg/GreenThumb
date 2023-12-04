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

            //modelBuilder.Entity<PlantModel>()
            //    .HasMany(p => p.Gardens)
            //    .WithMany(g => g.Plants)
            //    .UsingEntity<GardenPlant>(
            //    gp => gp.HasOne(g => g.Garden).WithMany().HasForeignKey(gp => gp.GardenId),
            //    gp => gp.HasOne(g => g.Plant).WithMany().HasForeignKey(gp => gp.PlantId),
            //    gp =>
            //    {
            //        gp.HasKey(gp => new { gp.GardenId, gp.PlantId });
            //        gp.Property(gp => gp.Quanity).IsRequired();
            //    }

            //);

            modelBuilder.Entity<PlantModel>()
                .HasMany(plant => plant.Instructions)
                .WithOne(ins => ins.Plant)
                .HasForeignKey(ins => ins.PlantId)
                .OnDelete(DeleteBehavior.Cascade);


            //SEED DATA
            //modelBuilder.Entity<PlantModel>()
            //    .HasData(
            //    new PlantModel()
            //    {
            //        PlantId = 1,
            //        Name = "Red Rose",
            //        Description = "A red flower",
            //        Instructions = new List<InstructionModel>()
            //        {
            //            new InstructionModel()
            //            {
            //                InstructionId = 1,
            //                InstructionText = "Water Rose everyday",
            //                PlantId = 1
            //            },
            //            new InstructionModel()
            //            {
            //                InstructionId = 2,
            //                InstructionText = "Be careful of the thorns",
            //                PlantId = 1
            //            },
            //            new InstructionModel()
            //            {
            //                InstructionId = 3,
            //                InstructionText = "Give to someone special",
            //                PlantId = 1
            //            },
            //        }
            //    }
            //    );
        }
    }
}
