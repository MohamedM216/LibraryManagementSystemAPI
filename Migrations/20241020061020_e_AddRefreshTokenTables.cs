using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class e_AddRefreshTokenTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId1 = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminRefreshTokens_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminRefreshTokens_Admins_AdminId1",
                        column: x => x.AdminId1,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId1 = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorRefreshTokens_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorRefreshTokens_Authors_AuthorId1",
                        column: x => x.AuthorId1,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId1 = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberRefreshTokens_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRefreshTokens_Members_MemberId1",
                        column: x => x.MemberId1,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminRefreshTokens_AdminId",
                table: "AdminRefreshTokens",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminRefreshTokens_AdminId1",
                table: "AdminRefreshTokens",
                column: "AdminId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRefreshTokens_AuthorId",
                table: "AuthorRefreshTokens",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRefreshTokens_AuthorId1",
                table: "AuthorRefreshTokens",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRefreshTokens_MemberId",
                table: "MemberRefreshTokens",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRefreshTokens_MemberId1",
                table: "MemberRefreshTokens",
                column: "MemberId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminRefreshTokens");

            migrationBuilder.DropTable(
                name: "AuthorRefreshTokens");

            migrationBuilder.DropTable(
                name: "MemberRefreshTokens");
        }
    }
}
