using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class DealersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DealerId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Dealer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DealerId",
                table: "Cars",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealer_UserId",
                table: "Dealer",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Dealer_DealerId",
                table: "Cars",
                column: "DealerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Dealer_DealerId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Dealer");

            migrationBuilder.DropIndex(
                name: "IX_Cars_DealerId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DealerId",
                table: "Cars");
        }
    }
}
