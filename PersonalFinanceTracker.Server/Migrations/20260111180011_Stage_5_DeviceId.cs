#nullable disable

namespace PersonalFinanceTracker.Server.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    /// <inheritdoc />
    public partial class Stage_5_DeviceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "RefreshTokens");
    }
}
