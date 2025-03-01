using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kutuphane_yonetim_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddYazarAndYayinevi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Yazarlar",
                columns: table => new
                {
                    YazarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yazarlar", x => x.YazarId);
                });

            migrationBuilder.CreateTable(
                name: "Yayinevleri",
                columns: table => new
                {
                    YayineviId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yayinevleri", x => x.YayineviId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Yazarlar");
            migrationBuilder.DropTable(name: "Yayinevleri");
        }

    }
}
