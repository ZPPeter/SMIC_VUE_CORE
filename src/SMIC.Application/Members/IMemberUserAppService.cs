using Abp.Application.Services;
using Abplus.ZeroDemo.Members.Dto;

namespace Abplus.ZeroDemo.Members
{
    public interface IMemberUserAppService : IAsyncCrudAppService<MemberUserDto, long, PagedMemberUserResultRequestDto, CreateMemberUserDto, MemberUserDto>
    {
    }
}
