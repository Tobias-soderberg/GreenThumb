using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenThumb.Migrations
{
    public partial class FinalVersion10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "id", "description", "img_url", "name" },
                values: new object[,]
                {
                    { 1, "The rose is a perennial flowering plant known for its exquisite and diverse beauty. With its lush, fragrant blossoms and thorny stems, the rose has become a symbol of love, passion, and elegance. The flower's intricate layers of petals form a captivating array of colors, ranging from classic reds and pinks to whites, yellows, and even blues. Roses are cultivated in various varieties, each showcasing unique shapes and sizes. Beyond its aesthetic appeal, the rose has cultural and symbolic significance in art, literature, and traditions worldwide.", "https://images.photowall.com/products/59359/red-rose.jpg?h=699&q=85", "Rose" },
                    { 2, "The dandelion (Taraxacum officinale) is a common flowering plant known for its distinctive yellow flowers that transform into fluffy seed heads resembling small parachutes. Despite being considered a weed in many lawns, dandelions have notable health benefits and culinary uses. The plant's leaves, flowers, and roots are all utilized in various traditional and alternative medicines.", "https://wildflower-favours.co.uk/wp-content/uploads/2022/04/dandelion.jpg", "Dandelion" },
                    { 3, "Cacti are a unique group of succulent plants known for their distinctive appearance, typically characterized by thick, fleshy stems and spines. Adapted to arid environments, cacti are renowned for their water storage capabilities, enabling them to survive in dry and challenging conditions. They come in various shapes and sizes, ranging from the iconic saguaro cactus with arms to small, globular species.", "https://www.lovethegarden.com/sites/default/files/styles/large/public/content/articles/UK_plant-finder-indoor-plants-saguaro-cactus_main.jpg?itok=qCFCzXFH", "Cactus" },
                    { 4, "The pear tree (Pyrus) is a deciduous fruit tree known for producing the sweet and succulent pear fruit. With a graceful and spreading canopy, the pear tree typically features simple, ovate leaves that provide a vibrant green backdrop during the growing season. In spring, the tree showcases clusters of white to pinkish blossoms, contributing to its ornamental appeal. Pears, the tree's fruit, vary in shape, size, and color, with popular varieties including Bartlett, Bosc, and Anjou. The fruit ripens in late summer to early fall, offering a juicy and flavorful treat enjoyed fresh or used in various culinary applications, from desserts to preserves. Pear trees are valued not only for their delicious fruit but also for their aesthetic contributions to gardens and orchards, providing shade and visual interest throughout the seasons.", "https://th.bing.com/th/id/OIG.gpxPEeeHCxn5.h_rIBId?pid=ImgGn", "Pear Tree" },
                    { 5, "The peony (Paeonia) is a prized perennial known for its large, fragrant blooms with layered petals in various colors. Blooming in late spring to early summer, these luxurious flowers, set against glossy green foliage, add elegance to gardens and floral arrangements. Symbolizing love and prosperity, peonies are cherished for their enduring beauty and adaptability to well-drained soils.", "https://th.bing.com/th/id/OIG.j3RcQJt8GSbQNJWIj3en?pid=ImgGn", "Peony" }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "id", "instruction_text", "plant_id" },
                values: new object[,]
                {
                    { 1, "6 hours of sunlight per day", 1 },
                    { 2, "Water in the morning", 1 },
                    { 3, "Remove dead leaves", 1 },
                    { 4, "Fertilize during spring or early summer", 1 },
                    { 5, "Inspect regularly for pests", 1 },
                    { 6, "6 hours of sunlight per day", 2 },
                    { 7, "Good drainage to prevent root rot", 2 },
                    { 8, "Water when soil is dry on touch", 2 },
                    { 9, "Remove bad blooms", 2 },
                    { 10, "Harvest leaves when young for culinary use", 2 },
                    { 11, "At least 6 hours of sunlight per day", 3 },
                    { 12, "Mix sand into soil for better drainage", 3 },
                    { 13, "Dry soil completely between watering", 3 },
                    { 14, "Protect from colder than 10C", 3 },
                    { 15, "Repot every 2-3 years if you want growth", 3 },
                    { 16, "Plant with full sunlight and slightly acidic to neutral soil", 4 },
                    { 17, "Consistent and deep watering", 4 },
                    { 18, "Regular pruning, especially in late winter / early spring", 4 },
                    { 19, "Fertilize annually in early spring", 4 },
                    { 20, "Monitor regularly for signs of pests", 4 },
                    { 21, "Plant in a well-drained and sunny spot", 5 },
                    { 22, "Consistent watering, water at the base", 5 },
                    { 23, "Install stakes to provide support while growing", 5 },
                    { 24, "Remove bad blooms directly", 5 },
                    { 25, "Use a slow fertilizer in early spring", 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
