using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorageWebApi.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnusedCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "uploaded",
                table: "files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "uploaded",
                table: "files",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
