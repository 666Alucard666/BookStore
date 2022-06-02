using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class FixedManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "OrdersBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrdersBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "OrdersBooks",
                type: "int",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "OrdersBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrdersBooks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);
        }
    }
}
