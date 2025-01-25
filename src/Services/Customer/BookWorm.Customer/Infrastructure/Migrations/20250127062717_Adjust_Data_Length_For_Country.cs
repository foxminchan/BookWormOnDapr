using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adjust_Data_Length_For_Country : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Consumers",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2025, 1, 27, 6, 27, 15, 639, DateTimeKind.Utc).AddTicks(
                    871
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    18,
                    9,
                    32,
                    36,
                    519,
                    DateTimeKind.Utc
                ).AddTicks(567)
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Consumers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 27, 6, 27, 15, 637, DateTimeKind.Utc).AddTicks(
                    3572
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    18,
                    9,
                    32,
                    36,
                    517,
                    DateTimeKind.Utc
                ).AddTicks(4302)
            );

            migrationBuilder.AlterColumn<string>(
                name: "Address_Country",
                table: "Consumers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Consumers",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2025, 1, 18, 9, 32, 36, 519, DateTimeKind.Utc).AddTicks(
                    567
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    27,
                    6,
                    27,
                    15,
                    639,
                    DateTimeKind.Utc
                ).AddTicks(871)
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Consumers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 18, 9, 32, 36, 517, DateTimeKind.Utc).AddTicks(
                    4302
                ),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(
                    2025,
                    1,
                    27,
                    6,
                    27,
                    15,
                    637,
                    DateTimeKind.Utc
                ).AddTicks(3572)
            );

            migrationBuilder.AlterColumn<string>(
                name: "Address_Country",
                table: "Consumers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true
            );
        }
    }
}
