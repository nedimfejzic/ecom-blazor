using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                 new Product
                 {
                     Id = 7,
                     Name = "adidas LITE RACER 2.0",
                     Description = "adidas LITE RACER 2.0 I su dječije patike za dječake. U njima će vaši mališani da nauče da balansiraju između snage i odgovornosti.",
                     ImageUrl = "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/EH1/EH1425/images/thumbs_800/thumbs_w/EH1425_800_800px_w.jpg",
                     Price = 47.40m
                 },
             new Product
             {
                 Id = 8,
                 Name = "KRONOS Jakna Bee Jacket",
                 Description = "Jaketa",
                 ImageUrl = "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/KRA/KRA203M504-01/images/thumbs_800/thumbs_w/KRA203M504-01_800_800px_w.jpg",
                 Price = 69.50m
             },
             new Product
             {
                 Id = 9,
                 Name = "KRONOS LADIES CREWNECK",
                 Description = "dukserica",
                 ImageUrl = "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/KRA/KRA221F602-80/images/thumbs_800/thumbs_w/KRA221F602-80_800_800px_w.jpg",
                 Price = 52.20m
             }
                );
        }

        public DbSet<Product> Products { get; set; }
    }
}
