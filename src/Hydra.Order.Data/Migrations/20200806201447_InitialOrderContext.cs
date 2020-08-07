using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hydra.Order.Data.Migrations
{
    public partial class InitialOrderContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "OrderSequence",
                startValue: 1000L);

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(type: "varchar(100)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(nullable: true),
                    DiscountAmount = table.Column<decimal>(nullable: true),
                    VoucherType = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UsedDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<int>(nullable: false, defaultValueSql: "NEXT VALUE FOR OrderSequence"),
                    Amount = table.Column<decimal>(nullable: false),
                    DiscountApplied = table.Column<decimal>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: false),
                    VourcherId = table.Column<Guid>(nullable: true),
                    HasVoucher = table.Column<bool>(nullable: false),
                    IsUsedVoucher = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Vouchers_VourcherId",
                        column: x => x.VourcherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(type: "varchar(250)", nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VourcherId",
                table: "Orders",
                column: "VourcherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropSequence(
                name: "OrderSequence");
        }
    }
}
