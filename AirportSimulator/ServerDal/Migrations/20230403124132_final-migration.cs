using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerDal.Migrations
{
    /// <inheritdoc />
    public partial class finalmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<int>(type: "int", nullable: false),
                    NumberOfPassengers = table.Column<int>(type: "int", nullable: false),
                    PlaneModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AirLine = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeparting = table.Column<bool>(type: "bit", nullable: false),
                    MadeContactAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegNumber = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    IsArrivingStart = table.Column<bool>(type: "bit", nullable: false),
                    IsDepartingStart = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    LegId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegLogs_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegLogs_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminalLegConnection",
                columns: table => new
                {
                    TerminalLegStartId = table.Column<int>(type: "int", nullable: false),
                    TerminalLegContinueId = table.Column<int>(type: "int", nullable: false),
                    IsDepartingConnection = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminalLegConnection", x => new { x.TerminalLegStartId, x.TerminalLegContinueId });
                    table.ForeignKey(
                        name: "FK_TerminalLegConnection_Legs_TerminalLegContinueId",
                        column: x => x.TerminalLegContinueId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TerminalLegConnection_Legs_TerminalLegStartId",
                        column: x => x.TerminalLegStartId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegLogs_FlightId",
                table: "LegLogs",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_LegLogs_LegId",
                table: "LegLogs",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminalLegConnection_TerminalLegContinueId",
                table: "TerminalLegConnection",
                column: "TerminalLegContinueId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminalLegConnection_TerminalLegStartId_TerminalLegContinueId_IsDepartingConnection",
                table: "TerminalLegConnection",
                columns: new[] { "TerminalLegStartId", "TerminalLegContinueId", "IsDepartingConnection" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegLogs");

            migrationBuilder.DropTable(
                name: "TerminalLegConnection");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Legs");
        }
    }
}
