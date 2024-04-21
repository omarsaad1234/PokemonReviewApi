using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Models;

namespace PokemonReviewApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<Category> Categories {  get; set; }
        public DbSet<Pokemon> Pokemon {  get; set; }
        public DbSet<Country> Countries {  get; set; }
        public DbSet<Owner> Owners {  get; set; }
        public DbSet<PokemonOwner> PokemonOwners {  get; set; }
        public DbSet<PokemonCategory> PokemonCategories {  get; set; }
        public DbSet<Review> Reviews {  get; set; }
        public DbSet<Reviewer> Reviewers {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PokemonCategories)
                .HasForeignKey(c=>c.CategoryId);

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(pc => pc.Pokemon)
                .WithMany(p => p.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);


            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(po => po.Owner)
                .WithMany(o => o.PokemonOwners)
                .HasForeignKey(o => o.OwnerId);

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(po => po.Pokemon)
                .WithMany(p => p.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);


        }

    }
}
