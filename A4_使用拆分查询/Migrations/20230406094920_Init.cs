using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace A4_使用拆分查询.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contributors_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "tom's blog" });

            migrationBuilder.InsertData(
                table: "Contributors",
                columns: new[] { "Id", "BlogId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Contributor # 1" },
                    { 2, 1, "Contributor # 2" },
                    { 3, 1, "Contributor # 3" },
                    { 4, 1, "Contributor # 4" },
                    { 5, 1, "Contributor # 5" },
                    { 6, 1, "Contributor # 6" },
                    { 7, 1, "Contributor # 7" },
                    { 8, 1, "Contributor # 8" },
                    { 9, 1, "Contributor # 9" },
                    { 10, 1, "Contributor # 10" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "BlogId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "post # 1" },
                    { 2, 1, "post # 2" },
                    { 3, 1, "post # 3" },
                    { 4, 1, "post # 4" },
                    { 5, 1, "post # 5" },
                    { 6, 1, "post # 6" },
                    { 7, 1, "post # 7" },
                    { 8, 1, "post # 8" },
                    { 9, 1, "post # 9" },
                    { 10, 1, "post # 10" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contributors_BlogId",
                table: "Contributors",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributors");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
