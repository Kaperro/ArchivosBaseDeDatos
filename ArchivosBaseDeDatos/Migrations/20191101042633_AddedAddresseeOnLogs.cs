using Microsoft.EntityFrameworkCore.Migrations;

namespace ArchivosBaseDeDatos.Migrations
{
    public partial class AddedAddresseeOnLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destinatario",
                table: "DocumentoRegistro",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destinatario",
                table: "DocumentoRegistro");
        }
    }
}
