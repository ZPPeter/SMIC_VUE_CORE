using Abp.Application.Services;
using SMIC.Members.Dto;

namespace SMIC.Members
{
    public interface IMemberUserAppService : IAsyncCrudAppService<MemberUserDto, long, PagedMemberUserResultRequestDto, CreateMemberUserDto, MemberUserDto>
    {
    }
}
