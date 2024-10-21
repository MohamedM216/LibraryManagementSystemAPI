using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeleteRefreshTokenTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminId",
                table: "AdminRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminName",
                table: "AdminRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorEmail",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorId",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberEmail",
                table: "MemberRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberId",
                table: "MemberRefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Members_Email",
                table: "Members");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Authors_Email",
                table: "Authors");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Admins_Name",
                table: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRefreshTokens",
                table: "MemberRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_MemberRefreshTokens_MemberEmail",
                table: "MemberRefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorRefreshTokens",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AuthorRefreshTokens_AuthorEmail",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminRefreshTokens",
                table: "AdminRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AdminRefreshTokens_AdminName",
                table: "AdminRefreshTokens");

            migrationBuilder.RenameTable(
                name: "MemberRefreshTokens",
                newName: "MemberRefreshToken");

            migrationBuilder.RenameTable(
                name: "AuthorRefreshTokens",
                newName: "AuthorRefreshToken");

            migrationBuilder.RenameTable(
                name: "AdminRefreshTokens",
                newName: "AdminRefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_MemberRefreshTokens_MemberId",
                table: "MemberRefreshToken",
                newName: "IX_MemberRefreshToken_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorRefreshTokens_AuthorId",
                table: "AuthorRefreshToken",
                newName: "IX_AuthorRefreshToken_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminRefreshTokens_AdminId",
                table: "AdminRefreshToken",
                newName: "IX_AdminRefreshToken_AdminId");

            migrationBuilder.AlterColumn<string>(
                name: "MemberEmail",
                table: "MemberRefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEmail",
                table: "AuthorRefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AdminName",
                table: "AdminRefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRefreshToken",
                table: "MemberRefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorRefreshToken",
                table: "AuthorRefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminRefreshToken",
                table: "AdminRefreshToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminRefreshToken_Admins_AdminId",
                table: "AdminRefreshToken",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRefreshToken_Authors_AuthorId",
                table: "AuthorRefreshToken",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRefreshToken_Members_MemberId",
                table: "MemberRefreshToken",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminRefreshToken_Admins_AdminId",
                table: "AdminRefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRefreshToken_Authors_AuthorId",
                table: "AuthorRefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRefreshToken_Members_MemberId",
                table: "MemberRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRefreshToken",
                table: "MemberRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorRefreshToken",
                table: "AuthorRefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminRefreshToken",
                table: "AdminRefreshToken");

            migrationBuilder.RenameTable(
                name: "MemberRefreshToken",
                newName: "MemberRefreshTokens");

            migrationBuilder.RenameTable(
                name: "AuthorRefreshToken",
                newName: "AuthorRefreshTokens");

            migrationBuilder.RenameTable(
                name: "AdminRefreshToken",
                newName: "AdminRefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_MemberRefreshToken_MemberId",
                table: "MemberRefreshTokens",
                newName: "IX_MemberRefreshTokens_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorRefreshToken_AuthorId",
                table: "AuthorRefreshTokens",
                newName: "IX_AuthorRefreshTokens_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminRefreshToken_AdminId",
                table: "AdminRefreshTokens",
                newName: "IX_AdminRefreshTokens_AdminId");

            migrationBuilder.AlterColumn<string>(
                name: "MemberEmail",
                table: "MemberRefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEmail",
                table: "AuthorRefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AdminName",
                table: "AdminRefreshTokens",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Members_Email",
                table: "Members",
                column: "Email");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Authors_Email",
                table: "Authors",
                column: "Email");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Admins_Name",
                table: "Admins",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRefreshTokens",
                table: "MemberRefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorRefreshTokens",
                table: "AuthorRefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminRefreshTokens",
                table: "AdminRefreshTokens",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRefreshTokens_MemberEmail",
                table: "MemberRefreshTokens",
                column: "MemberEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRefreshTokens_AuthorEmail",
                table: "AuthorRefreshTokens",
                column: "AuthorEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AdminRefreshTokens_AdminName",
                table: "AdminRefreshTokens",
                column: "AdminName");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminId",
                table: "AdminRefreshTokens",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminName",
                table: "AdminRefreshTokens",
                column: "AdminName",
                principalTable: "Admins",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorEmail",
                table: "AuthorRefreshTokens",
                column: "AuthorEmail",
                principalTable: "Authors",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorId",
                table: "AuthorRefreshTokens",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberEmail",
                table: "MemberRefreshTokens",
                column: "MemberEmail",
                principalTable: "Members",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberId",
                table: "MemberRefreshTokens",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
