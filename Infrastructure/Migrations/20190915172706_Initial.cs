using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseRateCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseRateCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agreements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    BaseRateCodeId = table.Column<int>(nullable: false),
                    Margin = table.Column<decimal>(nullable: false),
                    AgreementDuration = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreements_BaseRateCodes_BaseRateCodeId",
                        column: x => x.BaseRateCodeId,
                        principalTable: "BaseRateCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agreements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BaseRateCodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "VILIBOR1m" },
                    { 2, "VILIBOR3m" },
                    { 3, "VILIBOR6m" },
                    { 4, "VILIBOR1y" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 67812203006L, "Goras Trusevičius" },
                    { 78706151287L, "Dange Kulkavičiutė" }
                });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "AgreementDuration", "Amount", "BaseRateCodeId", "Margin", "UserId" },
                values: new object[] { 1L, 60, 12000m, 2, 1.6m, 67812203006L });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "AgreementDuration", "Amount", "BaseRateCodeId", "Margin", "UserId" },
                values: new object[] { 2L, 36, 8000m, 4, 2.2m, 78706151287L });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "Id", "AgreementDuration", "Amount", "BaseRateCodeId", "Margin", "UserId" },
                values: new object[] { 3L, 24, 1000m, 3, 1.85m, 78706151287L });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_BaseRateCodeId",
                table: "Agreements",
                column: "BaseRateCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_UserId",
                table: "Agreements",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agreements");

            migrationBuilder.DropTable(
                name: "BaseRateCodes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
