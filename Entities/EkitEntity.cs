using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class EkitEntity
    {
        public int Ekit_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string NurseStationName { get; set; }
        public string DrugName { get; set; }
        public string InHand { get; set; }
        public string Barcode { get; set; }
        public string NDC { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> Ekit_Status { get; set; }
        public Nullable<int> Ekit_CreatedBy { get; set; }
        public Nullable<System.DateTime> Ekit_CreatedOn { get; set; }
        public string GPICode { get; set; }
        public string UserName { get; set; }
        public string FacilityName { get; set; }
        public int? Facility_Id { get; set; }
        public int FacilityStatus { get; set; }
        public int NurseStatioStatus { get; set; }
        public string Trackable { get; set; }
        public int TrackableBit { get; set; }
        public int SharedeKitBit { get; set; }
        public string Shared { get; set; }
        public string ControlSubstance { get; set; }
        public int ControlSubstanceBit { get; set; }
        public string MergedDrugNames { get; set; }
    }
    public partial class EkitCustomEntity
    {
        public int Ekit_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string DrugName { get; set; }
        public string InHand { get; set; }
        public string[] Barcode { get; set; }
        public string NDC { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> Ekit_Status { get; set; }
        public Nullable<int> Ekit_CreatedBy { get; set; }
        public Nullable<System.DateTime> Ekit_CreatedOn { get; set; }
        public string GPICode { get; set; }
        public Nullable<int> TrackableBit { get; set; }
        public Nullable<int> Sharedekitbit { get; set; }
        public string InitialQuantity { get; set; }
        public Nullable<int> CertifiedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> CertifiedDate { get; set; }
        public Nullable<int> CheckInFlag { get; set; }
        public Nullable<int> ControlSubstance { get; set; }
    }
    public partial class EkitGridEntity
    {
        public List<EkitEntity> GridData { get; set; }
        public int TotalRecords { get; set; }
    }
    public class EKitMedsEntity
    {
        public int Ekit_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string DrugName { get; set; }
        public string InHand { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> Ekit_Status { get; set; }
        public Nullable<int> Ekit_CreatedBy { get; set; }
        public Nullable<System.DateTime> Ekit_CreatedOn { get; set; }
        public string BarCodeDetails { get; set; }
    }
    public class BarcodeCheckEntity
    {

        public string Barcode { get; set; }
        public string AGiveCodeIdentifier { get; set; }
    }


    public class EkitdrugEntity
    {
        public Nullable<int> facility_id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string DrugName { get; set; }
        public string GpiCode { get; set; }
        public Nullable<decimal> TotalNonExpiredInhand { get; set; }
        public Nullable<decimal> TotalExpiredInhand { get; set; }

    }
    public class DestroyEkitGrid
    {
        public string Drugname { get; set; }
        public string Inhand { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> Ekit_Id { get; set; }
        public string BarcodeDetail { get; set; }
        public string NonExpiredQty { get; set; }
        public string currentQuantity { get; set; }
        public string reason { get; set; }
        public string nursestationname { get; set; }
    }

    public class EkitlotEntity
    {
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string barcodedetail { get; set; }
        public string Inhand { get; set; }
        public int Ekit_Id { get; set; }
        public Nullable<System.DateTime> Updated_date { get; set; }
    }
    public class EkitDestroyQuantity
    {
        public string DUserName { get; set; }
        public string DPassword { get; set; }
        public string AUserName { get; set; }
        public string APassword { get; set; }
        public Nullable<int> Ekit_CreatedBy { get; set; }
        public Nullable<System.DateTime> Ekit_CreatedOn { get; set; }
        public Nullable<int> Ekit_Id { get; set; }
        public string InHandqty { get; set; }
        public Nullable<int> ApprovalUserId { get; set; }
        public Nullable<int> DestroyerUserId { get; set; }
        public Nullable<int> LoggedInUserId { get; set; }
        public string notexpiredqty { get; set; }
        public string notexpiredqtyreason { get; set; }
        public string Reason { get; set; }
    }
    public class InsertEkitLotEntity
    {
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string DrugName { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Inhand { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> Ekit_CreatedBy { get; set; }
        public string GpiCode { get; set; }
        public Int32 EkitUpdate { get; set; }
        public Int32 EditEkit_id { get; set; }
        public Nullable<System.DateTime> Ekit_CreatedOn { get; set; }
        public string Reason { get; set; }
    }
    public class DrugInfo
    {
        public string DrugName { get; set; }
        public string GPI { get; set; }
    }
    public class UpdateEkitDrugQty
    {
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> Ekit_Id { get; set; }
        public string InHandqty { get; set; }
        public string remainingekitids { get; set; }
    }
    public class FlagekitGrid
    {
        public Nullable<int> Ekit_Id { get; set; }
        public string LotNumber { get; set; }
        public string Barcode { get; set; }
    }
    public class ekitLostGrid
    {
        public string Inhand { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> Ekit_Id { get; set; }
        public Nullable<int> QtyAdminster { get; set; }
        public string BarcodeMatch { get; set; }
        public string Barcode { get; set; }
        public string remaining_ekit_ids { get; set; }
    }
    public class InsertDrugBarcEkit
    {
        public string DrugName { get; set; }
        public string Barcode { get; set; }
    }
    public class DrugEkitEntity
    {
        public string DrugName { get; set; }
    }
    public class DtmsEntity
    {

        public int AlertId { get; set; }
        public string AlertContent { get; set; }
       

    }
    public class DrugAllergyEntity
    {
        public int allergyAlertId { get; set; }
        public string alertContent { get; set; }
    }

}
