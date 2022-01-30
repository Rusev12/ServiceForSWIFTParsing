using Microsoft.EntityFrameworkCore;
using WorkerServiceApp1.Models.DBModels;

namespace WorkerServiceApp1.Services
{
    public class SWIFTParserDBContext : DbContext
    {
        public DbSet<BodyMessage20> BodyMessages20 { get; set; }

        public DbSet<BodyMessage21> BodyMessages21 { get; set; }

        public DbSet<BodyMessage79> BodyMessages79 { get; set; }

        public SWIFTParserDBContext(DbContextOptions<SWIFTParserDBContext> options):base(options)
        {

        }
    }
}
