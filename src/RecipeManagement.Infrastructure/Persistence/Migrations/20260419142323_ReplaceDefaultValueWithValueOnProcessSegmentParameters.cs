using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceDefaultValueWithValueOnProcessSegmentParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "ProcessSegmentParameters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "ProcessSegmentParameters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
