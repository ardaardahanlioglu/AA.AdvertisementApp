﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AA.AdvertisementApp.DataAccess.Migrations
{
    public partial class DataAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "Definition" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "Definition" },
                values: new object[] { 2, "Memeber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
