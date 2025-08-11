using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooks2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("20a6ec6e-d4d8-4ff1-8e19-3ff0c67dc8ec"),
                column: "StockQuantity",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("3f84bb7a-2eb5-4a21-bb16-74af4f16fbbf"),
                column: "StockQuantity",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("4453c2c5-f2df-46c0-b3d4-482f7b37602e"),
                column: "StockQuantity",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("5a3df3e4-bcc4-4b17-a0b6-4c1c98c3f4f7"),
                column: "StockQuantity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6b1894a2-71cb-4a85-815b-c245b53c7923"),
                column: "StockQuantity",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"),
                column: "StockQuantity",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("8c276e4b-b1a0-4e72-b15f-f8b9b2e6fd72"),
                column: "StockQuantity",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9df23a68-8b7e-43b3-a7f2-fb32cb0f89f3"),
                column: "StockQuantity",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9e5d8d6b-3e1a-4d82-84f1-f5b86e7a95c4"),
                column: "StockQuantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b0af9fd1-3a8e-4ef4-90aa-2f86b1c34f66"),
                column: "StockQuantity",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b0c85cb0-6041-40d4-a0d0-3df4d9e0c733"),
                column: "StockQuantity",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b31f1f26-1420-4c54-a25f-d344bb8a7e23"),
                column: "StockQuantity",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c771cf23-9a6d-4e77-8f8f-1f58692eb3a7"),
                column: "StockQuantity",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c7d0f9c6-9c71-4370-bf54-93b47c987edf"),
                column: "StockQuantity",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d6b2df6f-52d4-4098-9957-6cb8ce00c9a5"),
                column: "StockQuantity",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d8b42b6a-5bc5-4e9b-ae0c-bdcd02c6c8b2"),
                column: "StockQuantity",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e23e9eaf-4cb5-43a6-a7a4-08ffb6432781"),
                column: "StockQuantity",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e78a8c29-9382-4d53-b5cb-8265f8b2a27f"),
                column: "StockQuantity",
                value: 70);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f0cb6c6d-bb65-4a88-9d3c-54f3a58dd8bb"),
                column: "StockQuantity",
                value: 13);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f6332d83-5ed3-41f2-bc4b-5f93c97a3e07"),
                column: "StockQuantity",
                value: 70);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("20a6ec6e-d4d8-4ff1-8e19-3ff0c67dc8ec"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("3f84bb7a-2eb5-4a21-bb16-74af4f16fbbf"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("4453c2c5-f2df-46c0-b3d4-482f7b37602e"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("5a3df3e4-bcc4-4b17-a0b6-4c1c98c3f4f7"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6b1894a2-71cb-4a85-815b-c245b53c7923"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("73ff9fd9-4f2e-4d7c-a67e-56a13f3b5876"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("8c276e4b-b1a0-4e72-b15f-f8b9b2e6fd72"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9df23a68-8b7e-43b3-a7f2-fb32cb0f89f3"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9e5d8d6b-3e1a-4d82-84f1-f5b86e7a95c4"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b0af9fd1-3a8e-4ef4-90aa-2f86b1c34f66"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b0c85cb0-6041-40d4-a0d0-3df4d9e0c733"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b31f1f26-1420-4c54-a25f-d344bb8a7e23"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c771cf23-9a6d-4e77-8f8f-1f58692eb3a7"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("c7d0f9c6-9c71-4370-bf54-93b47c987edf"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d6b2df6f-52d4-4098-9957-6cb8ce00c9a5"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d8b42b6a-5bc5-4e9b-ae0c-bdcd02c6c8b2"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e23e9eaf-4cb5-43a6-a7a4-08ffb6432781"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e78a8c29-9382-4d53-b5cb-8265f8b2a27f"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f0cb6c6d-bb65-4a88-9d3c-54f3a58dd8bb"),
                column: "StockQuantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f6332d83-5ed3-41f2-bc4b-5f93c97a3e07"),
                column: "StockQuantity",
                value: 0);
        }
    }
}
