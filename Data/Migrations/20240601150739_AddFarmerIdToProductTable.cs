using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriEnergy_ST10204001_POE_Part_2.Data.Migrations
{
    public partial class AddFarmerIdToProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<Guid>(
                name: "FarmerId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FarmerDetails_FarmerId",
                table: "Products",
                column: "FarmerId",
                principalTable: "FarmerDetails",
                principalColumn: "FarmerDetailId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "FarmerDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FarmerDetails_ProductId",
                table: "FarmerDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmerDetails_Products_ProductId",
                table: "FarmerDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
