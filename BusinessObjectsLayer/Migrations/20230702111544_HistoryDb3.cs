using Microsoft.EntityFrameworkCore.Migrations;

namespace BusinessObjectsLayer.Migrations
{
    public partial class HistoryDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Events_EventId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_PostMeta_PostMetaId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Tags_TagId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_EventId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_PostMetaId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_TagId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "PostMetaId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Image");

            migrationBuilder.CreateTable(
                name: "EventImage",
                columns: table => new
                {
                    EventsEventId = table.Column<int>(type: "int", nullable: false),
                    ImagesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventImage", x => new { x.EventsEventId, x.ImagesId });
                    table.ForeignKey(
                        name: "FK_EventImage_Events_EventsEventId",
                        column: x => x.EventsEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventImage_Image_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagePostMeta",
                columns: table => new
                {
                    ImagesId = table.Column<long>(type: "bigint", nullable: false),
                    PostMetasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePostMeta", x => new { x.ImagesId, x.PostMetasId });
                    table.ForeignKey(
                        name: "FK_ImagePostMeta_Image_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagePostMeta_PostMeta_PostMetasId",
                        column: x => x.PostMetasId,
                        principalTable: "PostMeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageTag",
                columns: table => new
                {
                    ImagesId = table.Column<long>(type: "bigint", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageTag", x => new { x.ImagesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ImageTag_Image_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventImage_ImagesId",
                table: "EventImage",
                column: "ImagesId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePostMeta_PostMetasId",
                table: "ImagePostMeta",
                column: "PostMetasId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageTag_TagsId",
                table: "ImageTag",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventImage");

            migrationBuilder.DropTable(
                name: "ImagePostMeta");

            migrationBuilder.DropTable(
                name: "ImageTag");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostMetaId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Image_EventId",
                table: "Image",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_PostMetaId",
                table: "Image",
                column: "PostMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_TagId",
                table: "Image",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Events_EventId",
                table: "Image",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_PostMeta_PostMetaId",
                table: "Image",
                column: "PostMetaId",
                principalTable: "PostMeta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Tags_TagId",
                table: "Image",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
