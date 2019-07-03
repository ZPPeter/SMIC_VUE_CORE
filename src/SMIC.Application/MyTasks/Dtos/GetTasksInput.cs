
using Abp.Runtime.Validation;
using SMIC.Dtos;
using SMIC.MyTasks;

namespace SMIC.MyTasks.Dtos
{
    public class GetTasksInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }
}
