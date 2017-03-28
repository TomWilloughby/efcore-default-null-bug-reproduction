using Microsoft.EntityFrameworkCore;

namespace efcore_default_null_bug.Contexts
{
    public class WorkingDbContext : DbContext
    {
        public WorkingDbContext( DbContextOptions options ) : base( options ) { }
        
        DbSet<Models.ModelWithDefaultNullInt> theModels { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.Entity<Models.ModelWithDefaultNullInt>( entity => {
                entity.ToTable( "Test" );

                entity.Property( e => e.Id ).HasColumnName( "id" ).UseSqlServerIdentityColumn();
                entity.HasKey( e => e.Id );

                entity.Property( e => e.Bval ).HasColumnName( "bval" ).HasDefaultValue( null );
                entity.Property( e => e.Sval ).HasColumnName( "sval" ).HasDefaultValue( null );
                entity.Property( e => e.Ival ).HasColumnName( "ival" ).HasDefaultValueSql( "null" );
            } );
        }
    }
}
