using System.IO;

using Microsoft.EntityFrameworkCore;

using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;
using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.DataAccess
{
    public class SyntaxErrorContext: DbContext
    {
        readonly string path = @"..\DonauInfo.txt";

        public DbSet<ChallengeBase> Challenges { get; set; }
        public DbSet<GameChallenge> GameChallenges { get; set; }
        public DbSet<QuestionChallenge> QuestionChallenges { get; set; }
        public DbSet<ImageChallenge> ImageChallenges { get; set; }
        public DbSet<UsingBase> UsingPanes { get; set; }
        public DbSet<UsingChallenge> UsingChallenges { get; set; }
        public DbSet<MediaObject> Objects { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<CrewMember> CrewMembers { get; set; }
        public DbSet<GameProfile> GameProfiles { get; set; }

        /// <summary>
        ///   <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        ///   <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions">DbContextOptions</see> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured">IsConfigured</see> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)">OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)</see>.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = File.ReadAllText(path);

            optionsBuilder.UseSqlServer(connection, x => x.MigrationsAssembly("SyntaxError.V2.DatabaseConfig.ConsoleApp"));
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1">DbSet</see> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.
        /// </param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)">UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)</see>)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallengeBase>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<AudienceChallenge>("AudienceChallenge")
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

            modelBuilder.Entity<MediaObject>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Game>("Game")
                .HasValue<Image>("Image")
                .HasValue<Music>("Music");
        }
    }
}
