using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using SMIC.Authorization.Roles;
namespace SMIC
{
    public class Person : FullAuditedEntity
    {
        public virtual string Name { get; set; }
        public virtual string EmailAddress { get; set; }
        //public virtual ICollection<Role> Roles{ get; set; }
    }
}
