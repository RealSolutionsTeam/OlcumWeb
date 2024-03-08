using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OlcumWeb.Recipe
{
    public partial class RecipeContext : DbContext
    {
        public RecipeContext()
            : base("name=RecipeContext")
        {
        }

        public virtual DbSet<F_WASHING_TYPE> F_WASHING_TYPE { get; set; }
        public virtual DbSet<RECIPE> RECIPE { get; set; }
        public virtual DbSet<RECIPE_STEPS> RECIPE_STEPS { get; set; }
        public virtual DbSet<RECIPE_TYPES> RECIPE_TYPES { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RECIPE>()
                .Property(e => e.SERIAL_NO)
                .IsUnicode(false);

            modelBuilder.Entity<RECIPE_TYPES>()
                .HasMany(e => e.RECIPE)
                .WithOptional(e => e.RECIPE_TYPES)
                .HasForeignKey(e => e.RECIPE_TYPE);

        }
    }
}
