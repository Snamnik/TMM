using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMM.Infrastructure.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TMM");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "TMM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Forename = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(75)", unicode: false, maxLength: 75, nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "TMM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    AddressLine2 = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Town = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    County = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Postcode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "TMM",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                schema: "TMM",
                table: "Addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_EmailAddress",
                schema: "TMM",
                table: "Customers",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_MobileNo",
                schema: "TMM",
                table: "Customers",
                column: "MobileNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "TMM");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "TMM");
        }
    }
}
