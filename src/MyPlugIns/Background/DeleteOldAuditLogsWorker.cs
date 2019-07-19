using System;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;

// 必须安装 abp.zero.common
// 否则找不到 AuditLog 
namespace MyPlugIn
{
    public class DeleteOldAuditLogsWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {        
        private readonly IRepository<AuditLog, long> _auditLogRepository;

        public DeleteOldAuditLogsWorker(AbpTimer timer, IRepository<AuditLog, long> auditLogRepository) : base(timer)
        {
            _auditLogRepository = auditLogRepository;

            Timer.Period = 5000;
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            Logger.Info("---------------- DeleteOldAuditLogsWorker 正在工作 ----------------");
            Logger.Info(TestModule.Hello);

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var fiveMinutesAgo = Clock.Now.Subtract(TimeSpan.FromMinutes(5));

                _auditLogRepository.Delete(log => log.ExecutionTime > fiveMinutesAgo);

                CurrentUnitOfWork.SaveChanges();
            }
        }
    }
}
