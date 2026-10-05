using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class HLSevenSegmentService : IHLSevenSegmentService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IHLSevenSegmentRepository _hLSevenSegmentRepository;
        private readonly ILogger _log;
        public HLSevenSegmentService(IAutoMapper autoMapper, IHLSevenSegmentRepository hLSevenSegmentRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._hLSevenSegmentRepository = hLSevenSegmentRepository;
            this._log = log;
        }
        public async Task<List<HLSevenSegmentEntity>> GetHLSevenSegmentData()
        {
            this._log.Debug("---Executing GetHLSevenSegmentData() in HLSevenSegmentService----");
            return await Task.FromResult<List<HLSevenSegmentEntity>>(this._hLSevenSegmentRepository.GetHLSevenSegmentData());
        }
        public async Task<List<HLSevenSegmentDetailEntity>> GetHLSevenSegmentDetailData(int segmentID)
        {
            this._log.Debug("---Executing GetHLSevenSegmentDetailData() in HLSevenSegmentService----");
            return await Task.FromResult<List<HLSevenSegmentDetailEntity>>(this._hLSevenSegmentRepository.GetHLSevenSegmentDetailData(segmentID));
        }
        public async Task<List<HLSevenCompanyConfigEntity>> GetHLSevenCompanyConfigEntity(int CompanyID, int SegmentID)
        {
            this._log.Debug("---Executing GetHLSevenCompanyConfigEntity() in HLSevenSegmentService----");
            return await Task.FromResult<List<HLSevenCompanyConfigEntity>>(this._hLSevenSegmentRepository.GetHLSevenCompanyConfigEntity(CompanyID, SegmentID));
        }
        public async Task<int> InsertHLSevenCompanyConfig(HLSevenCompanyConfigEntity hlSevenconfig)
        {
            this._log.Debug("---Executing InsertHLSevenCompanyConfig() in HLSevenSegmentService----");
            return await Task.FromResult(this._hLSevenSegmentRepository.InsertHLSevenCompanyConfig(hlSevenconfig));
        }

        public async Task<List<HlFieldsEntity>> GetHLSevenConfigs(int residentId, string segmentDesc)
        {
            this._log.Debug("---Executing GetHLSevenConfigs() in HLSevenSegmentService----");
            return await Task.FromResult<List<HlFieldsEntity>>(this._hLSevenSegmentRepository.GetHLSevenConfigs(residentId, segmentDesc));
        }

        public async Task<int> CloneHlSevenCompanyConfig(HLSevenCloneEntity hlCloneConfig)
        {
            this._log.Debug("---Executing CloneHlSevenCompanyConfig() in HLSevenSegmentService----");
            return await Task.FromResult(this._hLSevenSegmentRepository.CloneHlSevenCompanyConfig(hlCloneConfig));

        }
        public async Task<List<HlFieldsEntity>> GetHLSevenConfigsBysegments(HLSevenConfigsEntity hlConfigs)
        {
            this._log.Debug("---Executing GetHLSevenConfigsByArray() in HLSevenSegmentService----");
            return await Task.FromResult<List<HlFieldsEntity>>(this._hLSevenSegmentRepository.GetHLSevenConfigsBysegments(hlConfigs));
        }

        public async Task<List<HLSevenOutboundDisplaySegmentEntity>> GetHLSevenOutboundDisplaySegmentsData()
        {
            this._log.Debug("---Executing GetHLSevenOutboundDisplaySegmentsData() in HLSevenSegmentService----");
            return await Task.FromResult<List<HLSevenOutboundDisplaySegmentEntity>>(this._hLSevenSegmentRepository.GetHLSevenOutboundDisplaySegmentsData());
        }

        public async Task<List<CompanyDropEntity>> GetHlsevenEnabledCompanyDetails()
        {
            this._log.Debug("---Executing GetHlsevenEnabledCompanyDetails() in HLSevenSegmentService----");
            return await Task.FromResult<List<CompanyDropEntity>>(this._hLSevenSegmentRepository.GetHlsevenEnabledCompanyDetails());
        }

        public async Task<List<CustomHLSevenOutboundDisplaySegmentGridDataEntity>> GetHLSevenOutboundDisplaySegmentsGridData(int segmentId, Nullable<int> companyId)
        {
            this._log.Debug("---Executing GetHLSevenOutboundDisplaySegmentsGridData() in HLSevenSegmentService----");
            return await Task.FromResult<List<CustomHLSevenOutboundDisplaySegmentGridDataEntity>>(this._hLSevenSegmentRepository.GetHLSevenOutboundDisplaySegmentsGridData(segmentId,companyId));
        }

        public async Task<int> InsertUpdateHlsevenOutboundDisplayConfigsData(CustomHLSevenOutboundDisplayCompanyConfigEntity displayConfigs)
        {
            this._log.Debug("---Executing InsertUpdateHlsevenOutboundDisplayConfigsData() in HLSevenSegmentService----");
            return await Task.FromResult<int>(this._hLSevenSegmentRepository.InsertUpdateHlsevenOutboundDisplayConfigsData(displayConfigs));
        }
        public async Task<List<HLSevenOutboundDisplayCompanyConfigEntity>> GetOutboundHLSevenConfigs(int residentId, int segmentId, int? facilityID = null)
        {
            this._log.Debug("---Executing GetOutboundHLSevenConfigs() in HLSevenSegmentService----");
            return await Task.FromResult<List<HLSevenOutboundDisplayCompanyConfigEntity>>(this._hLSevenSegmentRepository.GetOutboundHLSevenConfigs(residentId, segmentId, facilityID));
        }
    }
}
