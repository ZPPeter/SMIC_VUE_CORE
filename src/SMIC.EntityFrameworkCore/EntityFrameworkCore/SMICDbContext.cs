using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using SMIC.Authorization.Roles;
using SMIC.Authorization.Users;
using SMIC.MultiTenancy;

using SMIC.Members;
using SMIC.MyTasks;

namespace SMIC.EntityFrameworkCore
{
    public class SMICDbContext : AbpZeroDbContext<Tenant, Role, User, SMICDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<MemberUser> MemberUsers { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public SMICDbContext(DbContextOptions<SMICDbContext> options)
            : base(options)
        {
        }
        /*
         继承于 User 基类在此配置
         扩充 User 表
         继承于User，并为之配置TPH关系
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
