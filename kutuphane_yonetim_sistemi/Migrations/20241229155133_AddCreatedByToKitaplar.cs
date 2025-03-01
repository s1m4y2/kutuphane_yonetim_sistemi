﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kutuphane_yonetim_sistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByToKitaplar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Kitaplar");
        }
    }
}
