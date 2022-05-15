using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CertificateAPI.Migrations
{
    public partial class cert_auth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Certificates",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Certificates");
        }
    }
}
