using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineVault.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Persons_ActorId",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Persons_DirectorId",
                table: "MovieDirectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Directors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Directors",
                table: "Directors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Actors_ActorId",
                table: "MovieActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorId",
                table: "MovieDirectors",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Actors_ActorId",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorId",
                table: "MovieDirectors");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Directors",
                table: "Directors");

            migrationBuilder.RenameTable(
                name: "Directors",
                newName: "Persons");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Persons",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Persons_ActorId",
                table: "MovieActors",
                column: "ActorId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Persons_DirectorId",
                table: "MovieDirectors",
                column: "DirectorId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
