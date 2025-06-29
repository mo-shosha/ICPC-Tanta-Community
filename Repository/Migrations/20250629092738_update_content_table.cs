using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class update_content_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auther",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "ContentUrl",
                table: "trainingContents");

            migrationBuilder.AddColumn<string>(
                name: "ExplanationBy",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExplanationLink",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SheetLink",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpsolveBy",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpsolveLink",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekNumber",
                table: "trainingContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "anotherLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingContentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anotherLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_anotherLinks_trainingContents_TrainingContentId",
                        column: x => x.TrainingContentId,
                        principalTable: "trainingContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_anotherLinks_TrainingContentId",
                table: "anotherLinks",
                column: "TrainingContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anotherLinks");

            migrationBuilder.DropColumn(
                name: "ExplanationBy",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "ExplanationLink",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "SheetLink",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "UpsolveBy",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "UpsolveLink",
                table: "trainingContents");

            migrationBuilder.DropColumn(
                name: "WeekNumber",
                table: "trainingContents");

            migrationBuilder.AddColumn<string>(
                name: "Auther",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentUrl",
                table: "trainingContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
