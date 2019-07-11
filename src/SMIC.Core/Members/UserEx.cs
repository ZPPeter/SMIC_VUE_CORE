using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Configuration;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Authorization.Users;
namespace SMIC.Members
{
    public class AbpUser : IEntity<long>
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        //[Column("Email")] //字段映射
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        //public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }
        
        [NotMapped]
        public string[] RoleNames { get; set; }

        /*
          // 没有作用
          // Role 继承于 AbpRole,User Role 是多对多，有中间表 UserRole        
          public ICollection<SMIC.Authorization.Roles.Role> Roles { get; set; } 
        */

        public bool IsTransient()
        {
            throw new NotImplementedException();
        }
    }

}
