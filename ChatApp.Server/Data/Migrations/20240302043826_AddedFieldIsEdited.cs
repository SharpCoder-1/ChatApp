﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldIsEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Messages");
        }
    }
}
