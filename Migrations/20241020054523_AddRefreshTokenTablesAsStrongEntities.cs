using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenTablesAsStrongEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminId",
                table: "AdminRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorId",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberId",
                table: "MemberRefreshTokens");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "MemberRefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberEmail",
                table: "MemberRefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "AuthorRefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorEmail",
                table: "AuthorRefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "AdminRefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminName",
                table: "AdminRefreshTokens",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_MemberRefreshTokens_MemberEmail",
                table: "MemberRefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Authors_Email",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_AuthorRefreshTokens_AuthorEmail",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Admins_Name",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_AdminRefreshTokens_AdminName",
                table: "AdminRefreshTokens");

            migrationBuilder.DropColumn(
                name: "MemberEmail",
                table: "MemberRefreshTokens");

            migrationBuilder.DropColumn(
                name: "AuthorEmail",
                table: "AuthorRefreshTokens");

            migrationBuilder.DropColumn(
                name: "AdminName",
                table: "AdminRefreshTokens");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "MemberRefreshTokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "AuthorRefreshTokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AdminId",
                table: "AdminRefreshTokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminRefreshTokens_Admins_AdminId",
                table: "AdminRefreshTokens",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRefreshTokens_Authors_AuthorId",
                table: "AuthorRefreshTokens",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberRefreshTokens_Members_MemberId",
                table: "MemberRefreshTokens",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
