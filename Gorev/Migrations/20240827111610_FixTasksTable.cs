using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GorevY.Migrations
{
    /// <inheritdoc />
    public partial class FixTasksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gorevler_Kullanicilar_KullaniciId",
                table: "Gorevler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gorevler",
                table: "Gorevler");

            migrationBuilder.RenameTable(
                name: "Gorevler",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Gorevler_KullaniciId",
                table: "Tasks",
                newName: "IX_Tasks_KullaniciId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Kullanicilar_KullaniciId",
                table: "Tasks",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Kullanicilar_KullaniciId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Gorevler");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_KullaniciId",
                table: "Gorevler",
                newName: "IX_Gorevler_KullaniciId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gorevler",
                table: "Gorevler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gorevler_Kullanicilar_KullaniciId",
                table: "Gorevler",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
