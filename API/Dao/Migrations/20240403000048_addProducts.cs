using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dao.Migrations
{
    /// <inheritdoc />
    public partial class addProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO product(Id, Name, Description, Price, CreatedDate, CategoryId) VALUES('a0dfb675-924d-4f38-92eb-95b05693db29', 'Coca-Cola', '350 ml', 5.55, now(), '28b33a7d-b4d9-4f0c-9b72-af4087404c47')");
            migrationBuilder.Sql("INSERT INTO product(Id, Name, Description, Price, CreatedDate, CategoryId) VALUES('a1dfb675-924d-4f38-92eb-95b05693db29', 'Cheese burguer', 'Cheese, bread, tomato and burguer 150mg', 15.55, now(), 'c014c0f8-6a1d-49f3-b071-9a789b5e1442')");
            migrationBuilder.Sql("INSERT INTO product(Id, Name, Description, Price, CreatedDate, CategoryId) VALUES('a2dfb675-924d-4f38-92eb-95b05693db29', 'Pudim', 'Pudim with lemons', 10.55, now(), 'e25d14d9-1d03-4d8e-bf69-9a7f817c58f7')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from product");

        }
    }
}
