using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class fixed_fk_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentCategoryId",
                table: "trainingContents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "contentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contentCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_trainingContents_ContentCategoryId",
                table: "trainingContents",
                column: "ContentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_trainingContents_contentCategories_ContentCategoryId",
                table: "trainingContents",
                column: "ContentCategoryId",
                principalTable: "contentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trainingContents_contentCategories_ContentCategoryId",
                table: "trainingContents");

            migrationBuilder.DropTable(
                name: "contentCategories");

            migrationBuilder.DropIndex(
                name: "IX_trainingContents_ContentCategoryId",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "ContentCategoryId",
                table: "trainingContents");
        }
    }
}
