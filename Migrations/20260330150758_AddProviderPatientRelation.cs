using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimCare.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderPatientRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderPatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderPatients_HealthcareProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "HealthcareProviders",
                        principalColumn: "ProviderId");
                    table.ForeignKey(
                        name: "FK_ProviderPatients_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderPatients_PatientId",
                table: "ProviderPatients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderPatients_ProviderId",
                table: "ProviderPatients",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderPatients");
        }
    }
}
