using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookWorm.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Databas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    Location = table.Column<string>(
                        type: "character varying(255)",
                        maxLength: 255,
                        nullable: false
                    ),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    Description = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    Website = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValue: new DateTime(
                            2025,
                            1,
                            22,
                            17,
                            40,
                            47,
                            820,
                            DateTimeKind.Utc
                        ).AddTicks(1442)
                    ),
                    LastModifiedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true,
                        defaultValue: new DateTime(
                            2025,
                            1,
                            22,
                            17,
                            40,
                            47,
                            821,
                            DateTimeKind.Utc
                        ).AddTicks(8292)
                    ),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId_WarehouseId",
                table: "Stocks",
                columns: new[] { "ProductId", "WarehouseId" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_WarehouseId",
                table: "Stocks",
                column: "WarehouseId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_IsDeleted",
                table: "Warehouses",
                column: "IsDeleted"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Stocks");

            migrationBuilder.DropTable(name: "Warehouses");
        }
    }
}
