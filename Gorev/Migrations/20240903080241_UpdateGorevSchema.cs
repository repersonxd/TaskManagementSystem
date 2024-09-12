using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GorevY.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGorevSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "KullaniciAdi",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GorevAdi",
                table: "Gorevler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "GorevDurumuId",
                table: "Gorevler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KategoriId",
                table: "Gorevler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KullaniciId",
                table: "Gorevler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturulmaTarihi",
                table: "Gorevler",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TamamlanmaTarihi",
                table: "Gorevler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GorevDurumu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GorevDurumu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_GorevDurumuId",
                table: "Gorevler",
                column: "GorevDurumuId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_KategoriId",
                table: "Gorevler",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_KullaniciId",
                table: "Gorevler",
                column: "KullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gorevler_GorevDurumu_GorevDurumuId",
                table: "Gorevler",
                column: "GorevDurumuId",
                principalTable: "GorevDurumu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gorevler_Kategori_KategoriId",
                table: "Gorevler",
                column: "KategoriId",
                principalTable: "Kategori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gorevler_Kullanicilar_KullaniciId",
                table: "Gorevler",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gorevler_GorevDurumu_GorevDurumuId",
                table: "Gorevler");

            migrationBuilder.DropForeignKey(
                name: "FK_Gorevler_Kategori_KategoriId",
                table: "Gorevler");

            migrationBuilder.DropForeignKey(
                name: "FK_Gorevler_Kullanicilar_KullaniciId",
                table: "Gorevler");

            migrationBuilder.DropTable(
                name: "GorevDurumu");

            migrationBuilder.DropTable(
                name: "Kategori");

            migrationBuilder.DropIndex(
                name: "IX_Gorevler_GorevDurumuId",
                table: "Gorevler");

            migrationBuilder.DropIndex(
                name: "IX_Gorevler_KategoriId",
                table: "Gorevler");

            migrationBuilder.DropIndex(
                name: "IX_Gorevler_KullaniciId",
                table: "Gorevler");

            migrationBuilder.DropColumn(
                name: "GorevDurumuId",
                table: "Gorevler");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "Gorevler");

            migrationBuilder.DropColumn(
                name: "KullaniciId",
                table: "Gorevler");

            migrationBuilder.DropColumn(
                name: "OlusturulmaTarihi",
                table: "Gorevler");

            migrationBuilder.DropColumn(
                name: "TamamlanmaTarihi",
                table: "Gorevler");

            migrationBuilder.AlterColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "KullaniciAdi",
                table: "Kullanicilar",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GorevAdi",
                table: "Gorevler",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
