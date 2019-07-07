using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using SMIC.Authentication.External.Wechat.Events;
using SMIC.Members;

namespace SMIC.Authentication.External.Wechat.Handlers
{
    public class UpdateSessionKeyWhenWechatLoginSuccessEventHandler : IEventHandler<Events.WechatLoginSuccessEventData>, ITransientDependency
    {
        private readonly IRepository<MemberUser, long> _memberRepo;

        public UpdateSessionKeyWhenWechatLoginSuccessEventHandler(
            IRepository<MemberUser, long> memberRepo)
        {
            _memberRepo = memberRepo;
        }

        [UnitOfWork]
        public virtual void HandleEvent(WechatLoginSuccessEventData eventData)
        {
            var member = _memberRepo.FirstOrDefault(eventData.UserId);
            if (member != null)
            {
                member.SessionKey = eventData.SessionKey;
            }
        }
    }
}
