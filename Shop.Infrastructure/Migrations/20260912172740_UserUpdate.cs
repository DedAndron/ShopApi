using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_ParentId",
                table: "categories");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "categories",
                newName: "parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_categories_ParentId",
                table: "categories",
                newName: "IX_categories_parent_id");

            migrationBuilder.AddColumn<bool>(
                name: "is_verified",
                table: "users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_parent_id",
                table: "categories",
                column: "parent_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_parent_id",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "is_verified",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "categories",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_categories_parent_id",
                table: "categories",
                newName: "IX_categories_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_ParentId",
                table: "categories",
                column: "ParentId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
