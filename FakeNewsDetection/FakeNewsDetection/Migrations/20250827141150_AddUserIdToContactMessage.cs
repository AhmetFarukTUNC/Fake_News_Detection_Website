﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FakeNewsDetection.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToContactMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContactMessages");
        }
    }
}
