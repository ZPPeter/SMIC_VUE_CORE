using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMIC.SDIM;
namespace SMIC.EntityFrameworkCore
{
    // 2. DbContext
    public class SDIMDbContext : AbpDbContext
    {
        public virtual DbSet<VW_SJMX> Courses { get; set; }

        public SDIMDbContext(DbContextOptions<SDIMDbContext> options)
            : base(options)
        {

        }
    }
}