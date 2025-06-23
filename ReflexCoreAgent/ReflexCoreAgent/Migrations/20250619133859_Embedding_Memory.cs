using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReflexCoreAgent.Migrations
{
    /// <inheritdoc />
    public partial class Embedding_Memory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmbeddingVector",
                table: "KnowledgeEntries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbeddingVector",
                table: "KnowledgeEntries");
        }
    }
}
