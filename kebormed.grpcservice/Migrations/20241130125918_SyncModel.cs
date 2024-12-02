using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kebormed.grpcservice.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganization",
                table: "UserOrganization");

            migrationBuilder.RenameTable(
                name: "UserOrganization",
                newName: "UserOrganizations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizations_OrganizationId",
                table: "UserOrganizations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizations_UserId",
                table: "UserOrganizations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationId",
                table: "UserOrganizations",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrganizations_Users_UserId",
                table: "UserOrganizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Organizations_OrganizationId",
                table: "UserOrganizations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrganizations_Users_UserId",
                table: "UserOrganizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOrganizations",
                table: "UserOrganizations");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizations_OrganizationId",
                table: "UserOrganizations");

            migrationBuilder.DropIndex(
                name: "IX_UserOrganizations_UserId",
                table: "UserOrganizations");

            migrationBuilder.RenameTable(
                name: "UserOrganizations",
                newName: "UserOrganization");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOrganization",
                table: "UserOrganization",
                column: "Id");
        }
    }
}
