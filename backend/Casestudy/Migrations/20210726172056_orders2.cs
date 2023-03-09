using Microsoft.EntityFrameworkCore.Migrations;

namespace Casestudy.Migrations
{
    public partial class orders2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItem_Orders_OrderId",
                table: "OrderLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItem_Products_ProductId",
                table: "OrderLineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderLineItem",
                table: "OrderLineItem");

            migrationBuilder.RenameTable(
                name: "OrderLineItem",
                newName: "OrderLineItems");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLineItem_ProductId",
                table: "OrderLineItems",
                newName: "IX_OrderLineItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLineItem_OrderId",
                table: "OrderLineItems",
                newName: "IX_OrderLineItems_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderLineItems",
                table: "OrderLineItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItems_Orders_OrderId",
                table: "OrderLineItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItems_Products_ProductId",
                table: "OrderLineItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItems_Orders_OrderId",
                table: "OrderLineItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItems_Products_ProductId",
                table: "OrderLineItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderLineItems",
                table: "OrderLineItems");

            migrationBuilder.RenameTable(
                name: "OrderLineItems",
                newName: "OrderLineItem");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLineItems_ProductId",
                table: "OrderLineItem",
                newName: "IX_OrderLineItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderLineItems_OrderId",
                table: "OrderLineItem",
                newName: "IX_OrderLineItem_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderLineItem",
                table: "OrderLineItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItem_Orders_OrderId",
                table: "OrderLineItem",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItem_Products_ProductId",
                table: "OrderLineItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
