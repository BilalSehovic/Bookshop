using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Books_BookId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("20a6ec6e-d4d8-4ff1-8e19-3ff0c67dc8ec"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("3f84bb7a-2eb5-4a21-bb16-74af4f16fbbf"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("4453c2c5-f2df-46c0-b3d4-482f7b37602e"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("5a3df3e4-bcc4-4b17-a0b6-4c1c98c3f4f7"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6b1894a2-71cb-4a85-815b-c245b53c7923"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("8c276e4b-b1a0-4e72-b15f-f8b9b2e6fd72"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9df23a68-8b7e-43b3-a7f2-fb32cb0f89f3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9e5d8d6b-3e1a-4d82-84f1-f5b86e7a95c4"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b0af9fd1-3a8e-4ef4-90aa-2f86b1c34f66"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b0c85cb0-6041-40d4-a0d0-3df4d9e0c733"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b31f1f26-1420-4c54-a25f-d344bb8a7e23"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c771cf23-9a6d-4e77-8f8f-1f58692eb3a7"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c7d0f9c6-9c71-4370-bf54-93b47c987edf"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d6b2df6f-52d4-4098-9957-6cb8ce00c9a5"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d8b42b6a-5bc5-4e9b-ae0c-bdcd02c6c8b2"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e23e9eaf-4cb5-43a6-a7a4-08ffb6432781"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e78a8c29-9382-4d53-b5cb-8265f8b2a27f"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f0cb6c6d-bb65-4a88-9d3c-54f3a58dd8bb"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f6332d83-5ed3-41f2-bc4b-5f93c97a3e07"));

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "Sales",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Sales",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<string>(
                name: "BookDescription",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sales",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Books",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Books",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn",
                table: "Books",
                column: "Isbn",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Books_BookId",
                table: "Sales",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Books_BookId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Books_Isbn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookDescription",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Books");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Books",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Books",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Isbn", "Price", "StockQuantity", "Title" },
                values: new object[,]
                {
                    { new Guid("20a6ec6e-d4d8-4ff1-8e19-3ff0c67dc8ec"), "C.S. Lewis", "978-0066238500", 8.7899999999999991, 30, "The Chronicles of Narnia" },
                    { new Guid("3f84bb7a-2eb5-4a21-bb16-74af4f16fbbf"), "Victor Hugo", "978-0451419439", 12.49, 10, "Les Misérables" },
                    { new Guid("4453c2c5-f2df-46c0-b3d4-482f7b37602e"), "Aldous Huxley", "978-0060850524", 7.8899999999999997, 100, "Brave New World" },
                    { new Guid("5a3df3e4-bcc4-4b17-a0b6-4c1c98c3f4f7"), "George Orwell", "978-0451524935", 7.9900000000000002, 2, "1984" },
                    { new Guid("6b1894a2-71cb-4a85-815b-c245b53c7923"), "Fyodor Dostoevsky", "978-0486415871", 11.49, 50, "Crime and Punishment" },
                    { new Guid("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"), "Jane Austen", "978-1503290563", 6.9900000000000002, 14, "Pride and Prejudice" },
                    { new Guid("8c276e4b-b1a0-4e72-b15f-f8b9b2e6fd72"), "J.K. Rowling", "978-0590353427", 9.4900000000000002, 50, "Harry Potter and the Sorcerer's Stone" },
                    { new Guid("9df23a68-8b7e-43b3-a7f2-fb32cb0f89f3"), "Mary Shelley", "978-0486282114", 6.8899999999999997, 50, "Frankenstein" },
                    { new Guid("9e5d8d6b-3e1a-4d82-84f1-f5b86e7a95c4"), "J.R.R. Tolkien", "978-0547928227", 9.9900000000000002, 1, "The Hobbit" },
                    { new Guid("b0af9fd1-3a8e-4ef4-90aa-2f86b1c34f66"), "Khaled Hosseini", "978-1594631931", 9.1899999999999995, 30, "The Kite Runner" },
                    { new Guid("b0c85cb0-6041-40d4-a0d0-3df4d9e0c733"), "Paulo Coelho", "978-0061122415", 8.2899999999999991, 14, "The Alchemist" },
                    { new Guid("b31f1f26-1420-4c54-a25f-d344bb8a7e23"), "Harper Lee", "978-0061120084", 8.4900000000000002, 15, "To Kill a Mockingbird" },
                    { new Guid("c771cf23-9a6d-4e77-8f8f-1f58692eb3a7"), "Leo Tolstoy", "978-0143035008", 11.99, 30, "Anna Karenina" },
                    { new Guid("c7d0f9c6-9c71-4370-bf54-93b47c987edf"), "Leo Tolstoy", "978-1400079988", 12.99, 20, "War and Peace" },
                    { new Guid("d6b2df6f-52d4-4098-9957-6cb8ce00c9a5"), "Charles Dickens", "978-1503219700", 8.5899999999999999, 20, "A Tale of Two Cities" },
                    { new Guid("d8b42b6a-5bc5-4e9b-ae0c-bdcd02c6c8b2"), "J.D. Salinger", "978-0316769488", 8.9900000000000002, 5, "The Catcher in the Rye" },
                    { new Guid("e23e9eaf-4cb5-43a6-a7a4-08ffb6432781"), "Herman Melville", "978-1503280786", 10.99, 12, "Moby-Dick" },
                    { new Guid("e78a8c29-9382-4d53-b5cb-8265f8b2a27f"), "Bram Stoker", "978-0486411095", 7.1900000000000004, 70, "Dracula" },
                    { new Guid("f0cb6c6d-bb65-4a88-9d3c-54f3a58dd8bb"), "F. Scott Fitzgerald", "978-0743273565", 7.4900000000000002, 13, "The Great Gatsby" },
                    { new Guid("f6332d83-5ed3-41f2-bc4b-5f93c97a3e07"), "J.R.R. Tolkien", "978-0618640157", 14.99, 70, "The Lord of the Rings" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Books_BookId",
                table: "Sales",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
