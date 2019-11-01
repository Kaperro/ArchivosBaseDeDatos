using Microsoft.EntityFrameworkCore.Migrations;

namespace ArchivosBaseDeDatos.Migrations
{
    public partial class NullableFieldsOnDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Destinatario",
                table: "Documento",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Departamento",
                table: "Documento",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Destinatario",
                table: "Documento",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Departamento",
                table: "Documento",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
