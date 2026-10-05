using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.ServiceLayer
{
    public interface IHLSevenSegmentService
    {
        Task<List<HLSevenSegmentEntity>> GetHLSevenSegmentData();
        Task<List<HLSevenSegmentDetailEntity>> GetHLSevenSegmentDetailData(int segmentID);
        Task<List<HLSevenCompanyConfigEntity>> GetHLSevenCompanyConfigEntity(int CompanyID,int SegmentID);
        Task<int> InsertHLSevenCompanyConfig(HLSevenCompanyConfigEntity hlSevenconfig);
        Task <List<HlFieldsEntity>> GetHLSevenConfigs(int residentId, string segmentDesc);
        Task<int> CloneHlSevenCompanyConfig(HLSevenCloneEntity hlCloneConfig);
        Task<List<HlFieldsEntity>> GetHLSevenConfigsBysegments(HLSevenConfigsEntity hlConfigs);
        Task<List<HLSevenOutboundDisplaySegmentEntity>> GetHLSevenOutboundDisplaySegmentsData();
        Task<List<CompanyDropEntity>> GetHlsevenEnabledCompanyDetails();
        Task<List<CustomHLSevenOutboundDisplaySegmentGridDataEntity>> GetHLSevenOutboundDisplaySegmentsGridData(int segmentId, Nullable<int> companyId);
        Task<int> InsertUpdateHlsevenOutboundDisplayConfigsData(CustomHLSevenOutboundDisplayCompanyConfigEntity displayConfigs);
        Task<List<HLSevenOutboundDisplayCompanyConfigEntity>> GetOutboundHLSevenConfigs(int residentId, int segmentId, int? facilityID = null);
    }
}
