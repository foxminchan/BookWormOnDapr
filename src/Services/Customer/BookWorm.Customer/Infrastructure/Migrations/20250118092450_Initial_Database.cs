using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    LastName = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    Email = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    PhoneNumber = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    AccountId = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    Address_Street = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    Address_City = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    Address_State = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: true
                    ),
                    Address_Country = table.Column<string>(type: "text", nullable: true),
                    Address_ZipCode = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValue: new DateTime(
                            2025,
                            1,
                            18,
                            9,
                            24,
                            49,
                            693,
                            DateTimeKind.Utc
                        ).AddTicks(7523)
                    ),
                    LastModifiedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true,
                        defaultValue: new DateTime(
                            2025,
                            1,
                            18,
                            9,
                            24,
                            49,
                            695,
                            DateTimeKind.Utc
                        ).AddTicks(4915)
                    ),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Consumers_IsDeleted",
                table: "Consumers",
                column: "IsDeleted"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Consumers");
        }
    }
}
