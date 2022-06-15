using Microsoft.EntityFrameworkCore.Migrations;

namespace UF.AssessmentProject.Migrations
{
    public partial class UpdateBDf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemdetails_orders_orderId",
                table: "itemdetails");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "itemdetails",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_itemdetails_orderId",
                table: "itemdetails",
                newName: "IX_itemdetails_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemdetails_orders_OrderId",
                table: "itemdetails",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemdetails_orders_OrderId",
                table: "itemdetails");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "itemdetails",
                newName: "orderId");

            migrationBuilder.RenameIndex(
                name: "IX_itemdetails_OrderId",
                table: "itemdetails",
                newName: "IX_itemdetails_orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemdetails_orders_orderId",
                table: "itemdetails",
                column: "orderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
