using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UF.AssessmentProject.Migrations
{
    public partial class UpdateBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    totalamount = table.Column<long>(type: "bigint", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "partners",
                columns: table => new
                {
                    partnerrefno = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    partnerkey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    partnerpassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partners", x => x.partnerrefno);
                });

            migrationBuilder.CreateTable(
                name: "itemdetails",
                columns: table => new
                {
                    partneritemref = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    orderId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    qty = table.Column<int>(type: "int", nullable: false),
                    unitprice = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemdetails", x => x.partneritemref);
                    table.ForeignKey(
                        name: "FK_itemdetails_orders_orderId",
                        column: x => x.orderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_itemdetails_orderId",
                table: "itemdetails",
                column: "orderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itemdetails");

            migrationBuilder.DropTable(
                name: "partners");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
