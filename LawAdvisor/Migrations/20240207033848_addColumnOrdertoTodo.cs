using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class addColumnOrdertoTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ToDos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ToDos");
        }
    }
}
