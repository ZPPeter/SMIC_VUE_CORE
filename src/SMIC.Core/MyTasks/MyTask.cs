using System;

using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

using SMIC.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //[ForeignKey("AssignedPersonId")]

using Abp.Domain.Entities;

using Abp.Timing; // Clock.Now;

namespace SMIC.MyTasks
{
    public class MyTask : IEntity<int>, IHasCreationTime
    {
        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024;//64kb

        public long? AssignedPersonId { get; set; }

        [NotMapped]
        [ForeignKey("AssignedPersonId")]
        public User AssignedPerson { get; set; }

        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public TaskState State { get; set; }
        public DateTime CreationTime { get; set; }
        public int Id { get; set; }

        public MyTask()
        {
            CreationTime = Clock.Now;
            State = TaskState.Open; ;
        }

        public MyTask(string title, string description = null) : this()
        {
            Title = title;
            Description = description;
        }

        public bool IsTransient()
        {
            throw new NotImplementedException();
        }
    }

    public enum TaskState : byte
    {
        Open = 0,
        Completed = 1
    }
}
