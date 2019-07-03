

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMIC.MyTasks;

namespace SMIC.MyTasks.Dtos
{
    public class CreateOrUpdateTaskInput
    {
        [Required]
        public TaskEditDto Task { get; set; }

    }
}