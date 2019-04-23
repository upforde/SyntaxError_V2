using Microsoft.EntityFrameworkCore;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;
using System.Data.SqlClient;
using System.IO;

namespace SyntaxError.V2.DataAccess
{
    public class SyntaxErrorContext: DbContext
    {
        readonly string path = @"..\LocalInfo.txt";

        public DbSet<ChallengeBase> Challenges { get; set; }
        public DbSet<GameChallenge> GameChallenges { get; set; }
        public DbSet<QuestionChallenge> QuestionChallenges { get; set; }
        public DbSet<ImageChallenge> ImageChallenges { get; set; }
        public DbSet<UsingBase> UsingPanes { get; set; }
        public DbSet<UsingChallenge> UsingChallenges { get; set; }
        public DbSet<OuterSourceObject> Objects { get; set; }
        public DbSet<Answers> Answers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = File.ReadAllText(path);

            optionsBuilder.UseSqlServer(connection, x => x.MigrationsAssembly("SyntaxError.V2.DatabaseConfig.ConsoleApp"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallengeBase>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<AudienceChallenge>("AuddienceChallenge")
                .HasValue<CrewChallenge>("CrewChallenge")
                .HasValue<MultipleChoiceChallenge>("MultipleChoiceChallenge")
                .HasValue<MusicChallenge>("MusicChallenge")
                .HasValue<QuizChallenge>("QuizChallenge")
                .HasValue<ScreenshotChallenge>("ScreenshotChallenge")
                .HasValue<SilhouetteChallenge>("SilhouetteChallenge")
                .HasValue<SologameChallenge>("SologameChallenge");

            modelBuilder.Entity<UsingBase>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<SaveGame>("SaveGame")
                .HasValue<Profile>("Profile");

            modelBuilder.Entity<UsingChallenge>()
                .HasKey(uc => new { uc.ChallengeID, uc.UsingID });
            modelBuilder.Entity<UsingChallenge>()
                .HasOne(uc => uc.Challenge)
                .WithMany(c => c.UsedIn)
                .HasForeignKey(uc => uc.ChallengeID);
            modelBuilder.Entity<UsingChallenge>()
                .HasOne(uc => uc.UsingPane)
                .WithMany(u => u.Challenges)
                .HasForeignKey(uc => uc.UsingID);

            modelBuilder.Entity<OuterSourceObject>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Game>("Game")
                .HasValue<Image>("Image")
                .HasValue<Music>("Music");
        }
    }
}
