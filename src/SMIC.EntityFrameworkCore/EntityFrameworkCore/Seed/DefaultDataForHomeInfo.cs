using System;
using SMIC.EntityFrameworkCore;
using SMIC.HomeData;
using System.Collections.Generic;
using System.Linq;

namespace SMIC.EntityFrameworkCore.Seed
{
    public class DefaultDataForHomeInfo
    {
        private readonly SMICDbContext _context;

        private static readonly List<HomeInfo> _homeInfos;

        public DefaultDataForHomeInfo(SMICDbContext context)
        {
            _context = context;
        }

        static DefaultDataForHomeInfo()
        {
            _homeInfos = new List<HomeInfo>()
            {
                // Description,Title 可空
                new HomeInfo("在使用过程中如有什么问题，请联系管理员!","欢迎使用本系统"),
            };
        }

        public void Create()
        {
            foreach (var homeInfo in _homeInfos)
            {
                _context.HomeInfos.Add(homeInfo);
                _context.SaveChanges();
            }
        }

    }
}
