using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace A4_使用拆分查询.Migrations
{
    /// <inheritdoc />
    public partial class Add_Comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "Name", "PostId" },
                values: new object[,]
                {
                    { 1, "Contributor # 1", 1 },
                    { 2, "Contributor # 2", 2 },
                    { 3, "Contributor # 3", 3 },
                    { 4, "Contributor # 4", 4 },
                    { 5, "Contributor # 5", 5 },
                    { 6, "Contributor # 6", 6 },
                    { 7, "Contributor # 7", 7 },
                    { 8, "Contributor # 8", 8 },
                    { 9, "Contributor # 9", 9 },
                    { 10, "Contributor # 10", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");
        }
    }
}
