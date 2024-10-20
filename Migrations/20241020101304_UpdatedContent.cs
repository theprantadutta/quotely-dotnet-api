using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quotely_dotnet_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Quotes",
                type: "text",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Quotes",
                type: "character varying(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 400);
        }
    }
}
