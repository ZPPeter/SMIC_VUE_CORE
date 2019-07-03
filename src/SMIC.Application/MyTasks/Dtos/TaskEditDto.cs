
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using SMIC.MyTasks;
using SMIC.Authorization.Users;
namespace  SMIC.MyTasks.Dtos
{
    public class TaskEditDto
    {

        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024;//64kb

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }         


        
		/// <summary>
		/// Id
		/// </summary>
		public long? AssignedPersonId { get; set; }



		/// <summary>
		/// 分配给
		/// </summary>
		public User AssignedPerson { get; set; }



		/// <summary>
		/// 标题
		/// </summary>
[MaxLength(MaxTitleLength)]
		[Required(ErrorMessage="标题不能为空")]
		public string Title { get; set; }



		/// <summary>
		/// 内容
		/// </summary>
[MaxLength(MaxDescriptionLength)]
		[Required(ErrorMessage="内容不能为空")]
		public string Description { get; set; }



		/// <summary>
		/// 状态
		/// </summary>
		public TaskState State { get; set; }



		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreationTime { get; set; }




    }
}