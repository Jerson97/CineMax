using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemax.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImageMovieSerie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Movies");
        }
    }
}
