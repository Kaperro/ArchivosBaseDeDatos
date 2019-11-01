using Microsoft.EntityFrameworkCore.Migrations;

namespace ArchivosBaseDeDatos.Migrations
{
    public partial class AddedAddressee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destinatario",
                table: "Documento",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destinatario",
                table: "Documento");
        }
    }
}
