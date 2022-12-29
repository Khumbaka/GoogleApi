using Microsoft.EntityFrameworkCore.Migrations;

namespace GoogleApi.Migrations
{
    public partial class AddMessageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "GoogleEmails",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "GoogleEmails");
        }
    }
}
