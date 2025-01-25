using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Increase_Data_Lenghth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Consumers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20
            );

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
                    18,
                    9,
                    24,
                    49,
                    695,
                    DateTimeKind.Utc
                ).AddTicks(4915)
            );

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Consumers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50
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
                    18,
                    9,
                    24,
                    49,
                    693,
                    DateTimeKind.Utc
                ).AddTicks(7523)
            );

            migrationBuilder.AlterColumn<string>(
                name: "Address_Street",
                table: "Consumers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Consumers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Consumers",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2025, 1, 18, 9, 24, 49, 695, DateTimeKind.Utc).AddTicks(
                    4915
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

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Consumers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Consumers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 18, 9, 24, 49, 693, DateTimeKind.Utc).AddTicks(
                    7523
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
                name: "Address_Street",
                table: "Consumers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true
            );
        }
    }
}
