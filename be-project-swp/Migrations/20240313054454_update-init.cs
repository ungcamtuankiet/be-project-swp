using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class updateinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "requestorders",
                newName: "FullName_Sender");

            migrationBuilder.AddColumn<string>(
                name: "FullName_Receivier",
                table: "requestorders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName_Receivier",
                table: "requestorders");

            migrationBuilder.RenameColumn(
                name: "FullName_Sender",
                table: "requestorders",
                newName: "FullName");
        }
    }
}
