using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimCare.Migrations
{
    /// <inheritdoc />
    public partial class FixInvoiceRequestRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceRequests",
                columns: table => new
                {
                    InvoiceRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceRequests", x => x.InvoiceRequestId);
                    table.ForeignKey(
                        name: "FK_InvoiceRequests_HealthcareProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "HealthcareProviders",
                        principalColumn: "ProviderId");
                    table.ForeignKey(
                        name: "FK_InvoiceRequests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_PatientId",
                table: "InvoiceRequests",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceRequests_ProviderId",
                table: "InvoiceRequests",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceRequests");
        }
    }
}
