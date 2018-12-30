using Microsoft.EntityFrameworkCore;

namespace Realstate.Method
{
    public class RealstateDb : DbContext
    {
        public RealstateDb(DbContextOptions<RealstateDb> options) : base(options)
        {

        }

        public DbSet<Users> User { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<SeriesCombination> SeriesCombinations { get; set; }
        public DbSet<Series> series { get; set; }
        public DbSet<PrefrenceKey> prefrenceKeys { get; set; }

    }
}
