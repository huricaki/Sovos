using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SovosDocApi.Migrations
{
    /// <inheritdoc />
    public partial class updateInvoiceLineTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_InvoiceHeaders_InvoiceId",
                table: "InvoiceLines");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "InvoiceLines",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "InvoiceLines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SenderTitle",
                table: "InvoiceHeaders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverTitle",
                table: "InvoiceHeaders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "InvoiceHeaders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_InvoiceHeaders_InvoiceId",
                table: "InvoiceLines",
                column: "InvoiceId",
                principalTable: "InvoiceHeaders",
                principalColumn: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceLines_InvoiceHeaders_InvoiceId",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "InvoiceLines");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "InvoiceLines",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderTitle",
                table: "InvoiceHeaders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverTitle",
                table: "InvoiceHeaders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "InvoiceHeaders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceLines_InvoiceHeaders_InvoiceId",
                table: "InvoiceLines",
                column: "InvoiceId",
                principalTable: "InvoiceHeaders",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
