using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPGW.Infra.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerIbans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Iban = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShareType = table.Column<byte>(type: "tinyint", nullable: false),
                    SharedAmount = table.Column<float>(type: "real", nullable: false),
                    ShareAmountMax = table.Column<long>(type: "bigint", nullable: false),
                    ShareAmountMin = table.Column<long>(type: "bigint", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIbans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebSiteAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Psps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RSAPrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RSAPublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedirectAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    RedirectAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TodayOrderNumber = table.Column<long>(type: "bigint", nullable: true),
                    DirectPSPGW = table.Column<bool>(type: "bit", nullable: false),
                    PSPCode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    PSPId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TerminalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    uid = table.Column<int>(type: "int", nullable: false),
                    PassPhrase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CmsPreservationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RsaPublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Merchants_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Merchants_Psps_PSPId",
                        column: x => x.PSPId,
                        principalTable: "Psps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSettlements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IbanOrderNo = table.Column<byte>(type: "tinyint", nullable: true),
                    amountPercent = table.Column<double>(type: "float", nullable: true),
                    OrderID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSettlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSettlements_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PspRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    PSPId = table.Column<long>(type: "bigint", nullable: true),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PspRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PspRequests_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PspRequests_Psps_PSPId",
                        column: x => x.PSPId,
                        principalTable: "Psps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    PSPId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    TransactionRefrenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDateString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RRN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaskedCarNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PPaymentDate = table.Column<int>(type: "int", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    PayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayerMobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReserveNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemTraceAuditNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TerminalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transactions_Psps_PSPId",
                        column: x => x.PSPId,
                        principalTable: "Psps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_CustomerId",
                table: "Merchants",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_PSPId",
                table: "Merchants",
                column: "PSPId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSettlements_OrderID",
                table: "OrderSettlements",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PspRequests_OrderId",
                table: "PspRequests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PspRequests_PSPId",
                table: "PspRequests",
                column: "PSPId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CustomerId",
                table: "Transactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PSPId",
                table: "Transactions",
                column: "PSPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerIbans");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "OrderSettlements");

            migrationBuilder.DropTable(
                name: "PspRequests");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Psps");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
