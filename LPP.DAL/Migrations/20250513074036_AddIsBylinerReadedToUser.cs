using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LPP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBylinerReadedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBylinerReaded",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MessageReactions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBylinerReaded",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MessageReactions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
