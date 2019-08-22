using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMIC.SDIM;
namespace SMIC.EntityFrameworkCore
{
    // 2. DbContext
    public class SDIMDbContext : AbpDbContext
    {
        public virtual DbSet<VW_SJMX> VW_SJMX { get; set; }

        public virtual DbSet<WTD> WTD { get; set; }

        public virtual DbSet<SJMX> SJMX { get; set; }

        public virtual DbSet<STATS> STATS { get; set; }
        public virtual DbSet<JBCS> JBCSS { get; set; }
        public virtual DbSet<JDRQ> JDRQS { get; set; }

        public SDIMDbContext(DbContextOptions<SDIMDbContext> options)
            : base(options)
        {

        }
    }
}