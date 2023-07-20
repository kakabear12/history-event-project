using Microsoft.EntityFrameworkCore.Migrations;

namespace BusinessObjectsLayer.Migrations
{
    public partial class HistoryDbnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImagePost",
                columns: table => new
                {
                    ImagesId = table.Column<long>(type: "bigint", nullable: false),
                    PostsPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePost", x => new { x.ImagesId, x.PostsPostId });
                    table.ForeignKey(
                        name: "FK_ImagePost_Image_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagePost_Posts_PostsPostId",
                        column: x => x.PostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagePost_PostsPostId",
                table: "ImagePost",
                column: "PostsPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagePost");
        }
    }
}
