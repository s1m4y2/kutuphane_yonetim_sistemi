using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kutuphane_yonetim_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddKitapTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kitaplar",
                columns: table => new
                {
                    KitapId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(nullable: false),
                    KategoriId = table.Column<int>(nullable: false),
                    YazarId = table.Column<int>(nullable: false),
                    YayineviId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitaplar", x => x.KitapId);
                    table.ForeignKey(
                        name: "FK_Kitaplar_Kategoriler_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kitaplar_Yazarlar_YazarId",
                        column: x => x.YazarId,
                        principalTable: "Yazarlar",
                        principalColumn: "YazarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kitaplar_Yayinevleri_YayineviId",
                        column: x => x.YayineviId,
                        principalTable: "Yayinevleri",
                        principalColumn: "YayineviId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Index oluştur
            migrationBuilder.CreateIndex(
                name: "IX_Kitaplar_KategoriId",
                table: "Kitaplar",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitaplar_YazarId",
                table: "Kitaplar",
                column: "YazarId");

            migrationBuilder.CreateIndex(
                name: "IX_Kitaplar_YayineviId",
                table: "Kitaplar",
                column: "YayineviId");

            migrationBuilder.AddColumn<string>(
                name: "KullaniciId",
                table: "OduncIslemleri",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kitaplar");
        }

    }
}
