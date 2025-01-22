using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Ordering.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Adjust_Status_DataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2025, 1, 22, 15, 34, 51, 826, DateTimeKind.Utc).AddTicks(
                    5616
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    18,
                    16,
                    34,
                    42,
                    419,
                    DateTimeKind.Utc
                ).AddTicks(7207)
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 22, 15, 34, 51, 824, DateTimeKind.Utc).AddTicks(
                    9150
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    18,
                    16,
                    34,
                    42,
                    418,
                    DateTimeKind.Utc
                ).AddTicks(331)
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint"
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2025, 1, 18, 16, 34, 42, 419, DateTimeKind.Utc).AddTicks(
                    7207
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    22,
                    15,
                    34,
                    51,
                    826,
                    DateTimeKind.Utc
                ).AddTicks(5616)
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 18, 16, 34, 42, 418, DateTimeKind.Utc).AddTicks(
                    331
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    22,
                    15,
                    34,
                    51,
                    824,
                    DateTimeKind.Utc
                ).AddTicks(9150)
            );
        }
    }
}
