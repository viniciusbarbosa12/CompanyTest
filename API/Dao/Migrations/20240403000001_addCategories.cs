using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dao.Migrations
{
    /// <inheritdoc />
    public partial class addCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("Insert into category(Id, Name, Codigo) Values('28b33a7d-b4d9-4f0c-9b72-af4087404c47', 'Drinks','12351')");
            migrationBuilder.Sql("Insert into category(Id, Name, Codigo) Values('c014c0f8-6a1d-49f3-b071-9a789b5e1442', 'Snacks','2435')");
            migrationBuilder.Sql("Insert into category(Id, Name, Codigo) Values('e25d14d9-1d03-4d8e-bf69-9a7f817c58f7', 'Desserts','321323')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from category");
        }
    }
}
