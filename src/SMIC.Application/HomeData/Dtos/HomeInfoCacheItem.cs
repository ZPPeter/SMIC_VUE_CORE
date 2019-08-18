using Abp.AutoMapper;
namespace SMIC.HomeData
{
    [AutoMapFrom(typeof(HomeInfo))]
    public class HomeInfoCacheItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
