using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api.Migrations
{
    /// <inheritdoc />
    public partial class RenameSenhaToPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Vehicles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Marca",
                table: "Vehicles",
                newName: "Mark");

            migrationBuilder.RenameColumn(
                name: "Ano",
                table: "Vehicles",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Admins",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Perfil",
                table: "Admins",
                newName: "Profile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Vehicles",
                newName: "Ano");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vehicles",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Mark",
                table: "Vehicles",
                newName: "Marca");

            migrationBuilder.RenameColumn(
                name: "Profile",
                table: "Admins",
                newName: "Perfil");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Admins",
                newName: "Senha");
        }
    }
}
