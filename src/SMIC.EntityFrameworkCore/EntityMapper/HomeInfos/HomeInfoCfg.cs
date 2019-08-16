

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMIC.HomeData;

namespace SMIC.EntityMapper.HomeInfos
{
    public class HomeInfoCfg : IEntityTypeConfiguration<HomeInfo>
    {
        public void Configure(EntityTypeBuilder<HomeInfo> builder)
        {
            //schema ָ�����������
            //builder.ToTable("HomeInfos", YoYoAbpefCoreConsts.SchemaNames.CMS);
            builder.ToTable("HomeInfos"); // Ĭ�� schema -> dbo             
            
			builder.Property(a => a.User).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length16);
			builder.Property(a => a.Title).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length32);
			builder.Property(a => a.Description).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length256);
			builder.Property(a => a.CreationTime).HasMaxLength(YoYoAbpefCoreConsts.EntityLengthNames.Length16);

        }
    }
}


