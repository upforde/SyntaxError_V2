using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SyntaxError.V2.DatabaseConfig.ConsoleApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswersID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Answer = table.Column<string>(nullable: true),
                    DummyAnswer1 = table.Column<string>(nullable: true),
                    DummyAnswer2 = table.Column<string>(nullable: true),
                    DummyAnswer3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswersID);
                });

            migrationBuilder.CreateTable(
                name: "CrewMembers",
                columns: table => new
                {
                    CrewMemberID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CrewTag = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMembers", x => x.CrewMemberID);
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    URI = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UsingProfiles",
                columns: table => new
                {
                    UsingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsingProfiles", x => x.UsingID);
                });

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    ChallengeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChallengeTask = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    GameID = table.Column<int>(nullable: true),
                    CrewMemberID = table.Column<int>(nullable: true),
                    ImageID = table.Column<int>(nullable: true),
                    SongID = table.Column<int>(nullable: true),
                    AnswersID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.ChallengeID);
                    table.ForeignKey(
                        name: "FK_Challenges_CrewMembers_CrewMemberID",
                        column: x => x.CrewMemberID,
                        principalTable: "CrewMembers",
                        principalColumn: "CrewMemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenges_Objects_GameID",
                        column: x => x.GameID,
                        principalTable: "Objects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenges_Objects_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Objects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenges_Objects_SongID",
                        column: x => x.SongID,
                        principalTable: "Objects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Challenges_Answers_AnswersID",
                        column: x => x.AnswersID,
                        principalTable: "Answers",
                        principalColumn: "AnswersID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsingProfileToChallenge",
                columns: table => new
                {
                    ChallengeID = table.Column<int>(nullable: false),
                    UsingID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsingProfileToChallenge", x => new { x.ChallengeID, x.UsingID });
                    table.ForeignKey(
                        name: "FK_UsingProfileToChallenge_Challenges_ChallengeID",
                        column: x => x.ChallengeID,
                        principalTable: "Challenges",
                        principalColumn: "ChallengeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsingProfileToChallenge_UsingProfiles_UsingID",
                        column: x => x.UsingID,
                        principalTable: "UsingProfiles",
                        principalColumn: "UsingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_CrewMemberID",
                table: "Challenges",
                column: "CrewMemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_GameID",
                table: "Challenges",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ImageID",
                table: "Challenges",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_SongID",
                table: "Challenges",
                column: "SongID");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_AnswersID",
                table: "Challenges",
                column: "AnswersID");

            migrationBuilder.CreateIndex(
                name: "IX_UsingProfileToChallenge_UsingID",
                table: "UsingProfileToChallenge",
                column: "UsingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsingProfileToChallenge");

            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.DropTable(
                name: "UsingProfiles");

            migrationBuilder.DropTable(
                name: "CrewMembers");

            migrationBuilder.DropTable(
                name: "Objects");

            migrationBuilder.DropTable(
                name: "Answers");
        }
    }
}
