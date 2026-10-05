using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.Repositories
{
    public interface IHLSevenSegmentRepository
    {
        List<HLSevenSegmentEntity> GetHLSevenSegmentData();
        List<HLSevenSegmentDetailEntity> GetHLSevenSegmentDetailData(int segmentID);
        List<HLSevenCompanyConfigEntity> GetHLSevenCompanyConfigEntity(int CompanyID,int SegmentID);
        int InsertHLSevenCompanyConfig(HLSevenCompanyConfigEntity hlSevenconfig);
        List<HlFieldsEntity> GetHLSevenConfigs(int residentId, string segmentDesc);
        int CloneHlSevenCompanyConfig(HLSevenCloneEntity hlCloneConfig);
        List<HlFieldsEntity> GetHLSevenConfigsBysegments(HLSevenConfigsEntity hlConfigs);
        List<HLSevenOutboundDisplaySegmentEntity> GetHLSevenOutboundDisplaySegmentsData();
        List<CompanyDropEntity> GetHlsevenEnabledCompanyDetails();
        List<CustomHLSevenOutboundDisplaySegmentGridDataEntity> GetHLSevenOutboundDisplaySegmentsGridData(int segmentId, Nullable<int> companyId);
        int InsertUpdateHlsevenOutboundDisplayConfigsData(CustomHLSevenOutboundDisplayCompanyConfigEntity displayConfigs);

        List<HLSevenOutboundDisplayCompanyConfigEntity> GetOutboundHLSevenConfigs(int residentId,int segmentId,int? facilityID=null);
    }
}
