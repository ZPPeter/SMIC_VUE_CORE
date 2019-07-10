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

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        //public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }
        
        //[NotMapped]
        [ForeignKey("UserId")]        
        public virtual ICollection<UserRole> Roles { get; set; }
        

        public bool IsTransient()
        {
            throw new NotImplementedException();
        }
    }

}
