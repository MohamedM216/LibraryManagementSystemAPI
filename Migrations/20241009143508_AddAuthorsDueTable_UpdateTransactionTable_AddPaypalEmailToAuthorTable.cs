using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorsDueTable_UpdateTransactionTable_AddPaypalEmailToAuthorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCards");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MemberCreditCardNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "StripeEmail",
                table: "Authors",
                newName: "PaypalEmail");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentIntentId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryCost",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "AuthorPayout",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,3)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Members",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LateFee",
                table: "Members",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AuthorsDues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PaypalEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueAuthorPayout = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    IsDuePaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorsDues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorsDues_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorsDues_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsDues_AuthorId",
                table: "AuthorsDues",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorsDues_TransactionId",
                table: "AuthorsDues",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorsDues");

            migrationBuilder.DropIndex(
                name: "IX_Members_Email",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LateFee",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "PaypalEmail",
                table: "Authors",
                newName: "StripeEmail");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentIntentId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DeliveryCost",
                table: "Transactions",
                type: "decimal(10,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AuthorPayout",
                table: "Transactions",
                type: "decimal(10,3)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MemberCreditCardNumber",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreditCardNumber",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Authors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CVC = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => new { x.MemberId, x.CardNumber });
                    table.ForeignKey(
                        name: "FK_CreditCards_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_MemberId",
                table: "CreditCards",
                column: "MemberId",
                unique: true);
        }
    }
}
