using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP5.API.Migrations
{
    /// <inheritdoc />
    public partial class AjoutTechnicien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Technicien",
                table: "Reparations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Technicien",
                table: "Reparations");
        }
    }
}
