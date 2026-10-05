using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CheckInMedsGridEntity
    {
        public string ResidentName { get; set; }
        public int ResidentNSFlag { get; set; }
        public List<CheckInMedsEntity> Data { get; set; }
    }

    public class CheckInMedsEntity
    {
        public int POrder_Id { get; set; }
        public int PQuantity_Id { get; set; }
        public string DrugName { get; set; }
        public string OnHand { get; set; }
        public int OrderStatus { get; set; }
        public string ResidentName { get; set; }
        public int ResidentNSFlag { get; set; }
        public string NSName { get; set; }
        public int ControlledMed { get; set; }
        public int ResidentStatus { get; set; }
        public string NewBarcode { get; set; }
        public int barcodeCheck { get; set; }
        public string LotNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string Directions { get; set; }
        public int nursetationID { get; set; }
        public int FacilityID { get; set; }
    }
    public class CheckInSelectedMedsEntity
    {
        public int POrder_Id { get; set; }
        public int PQuantity_Id { get; set; }
        public string OnHand { get; set; }
        public string Barcode { get; set; }
        public DateTime CheckInDate { get; set; }
        public string LotNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class OrderStockTransEntity
    {
        public int? POrder_Id { get; set; }
        public string DrugName { get; set; }
        public string Inhand { get; set; }
        public string TransOS_CreatedBy { get; set; }
        public Nullable<System.DateTime> TransOS_CreatedDate { get; set; }
        public string LotNumber { get; set; }
    }
    public class PharmacyStatus
    {
        public string Status { get; set; }
    }
        public class PharmacyInfoEntity
    {
        public int? Facility_Id { get; set; }
        public string NurseStation_Id { get; set; }
        public string Pharmacy_Name { get; set; }
        public string Pharmacy_Address1 { get; set; }
        public string Pharmacy_Address2 { get; set; }
        public string Pharmacy_City { get; set; }
        public string Pharmacy_State { get; set; }
        public string Pharmacy_Zip { get; set; }
        public int? Pharmacy_CountryId { get; set; }
        public int? Pharmacy_Retail { get; set; }
        public int? Pharmacy_Mail_Order { get; set; }
        public int? Pharmacy_Speciality { get; set; }
        public int? Pharmacy_LTC { get; set; }
        public int? Pharmacy_IHD { get; set; }
        public int? Pharmacy_24_Hours { get; set; }
        public int? Pharmacy_EPCS_Enabled { get; set; }
        public string Pharmacy_Phone { get; set; }
        public string Pharmacy_Fax { get; set; }
        public int? Pharmacy_Status { get; set; }
        public int? Pharmacy_CreatedBy { get; set; }
        public int? Pharmacy_Id { get; set; }
        public string NCPDP { get; set; }
        public string NPI { get; set; }

    }

    public class PharmacyDataEntity
    {
        public int? Facility_Id { get; set; }
        public string Facility_Name { get; set; }
        public string NurseStation_Name { get; set; }
        public string PharmacyName { get; set; }
        public string Pharmacy_City { get; set; }
        public string Pharmacy_State { get; set; }
        public string PhysicianCountry { get; set; }
        public int? Pharmacy_Zip { get; set; }
        public int? Facility_Status { get; set; }
        public int? Pharmacy_Status { get; set; }
    }
}