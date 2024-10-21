using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReAddRefreshTokenTables__ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminId1",
                table: "AdminRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorId1",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberId1",
                table: "MemberRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_MemberRefreshTokens_MemberId1",
                table: "MemberRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AuthorRefreshTokens_AuthorId1",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AdminRefreshTokens_AdminId1",
                table: "AdminRefreshTokens");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "MemberRefreshTokens");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropColumn(
                name: "AdminId1",
                table: "AdminRefreshTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId1",
                table: "MemberRefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId1",
                table: "AuthorRefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdminId1",
                table: "AdminRefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MemberRefreshTokens_MemberId1",
                table: "MemberRefreshTokens",
                column: "MemberId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRefreshTokens_AuthorId1",
                table: "AuthorRefreshTokens",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_AdminRefreshTokens_AdminId1",
                table: "AdminRefreshTokens",
                column: "AdminId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminId1",
                table: "AdminRefreshTokens",
                column: "AdminId1",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorId1",
                table: "AuthorRefreshTokens",
                column: "AuthorId1",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberId1",
                table: "MemberRefreshTokens",
                column: "MemberId1",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
