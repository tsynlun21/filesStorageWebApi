using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorageWebApi.Migrations
{
    /// <inheritdoc />
    public partial class deletedfilesamount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "files_amount",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "files_amount",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
