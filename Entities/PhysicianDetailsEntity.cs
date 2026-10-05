using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class PhysicianDetailsEntity
    {
        public int Physician_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string PhysicianNPI { get; set; }
        public string PhysicianLName { get; set; }
        public string PhysicianFName { get; set; }
        public string PhysicianAddress1 { get; set; }
        public string PhysicianAddress2 { get; set; }
        public string PhysicianCity { get; set; }
        public string PhysicianState { get; set; }
        public string PhysicianZip { get; set; }
        public int Physician_Status { get; set; }
        public Nullable<int> Physician_CreatedBy { get; set; }
        public System.DateTime Physician_CreatedDate { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> PhysicianCountryID { get; set; }
        public string Country_Name { get; set; }
        public string CreatedBy { get; set; }
        public string PhysicianFullName { get; set; }
        public string NurseStation_Name { get; set; }
        public int NurseStation_Status { get; set; }
        public string NurseStations { get; set; }
        public string Facility_Name { get; set; }
        public int Facility_Status { get; set; }
        public string NursestationIds { get; set; }
        public string OldPhysicianNPI { get; set; }
        public string OldFacilityId { get; set; }
        public Nullable<int> Credentials { get; set; }
        public Nullable<int> PrimarySpec { get; set; }
        public string SupervisingPhy { get; set; }
        public List<Licenses> LicensesData { get; set; }
        public Nullable<int> PId { get; set; }
        public string Physician_Phone { get; set; }
        public string FacId { get; set; }
        public string Facid { get; set; }


    }
    public class Licenses
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public int PrescriberId { get; set; }

    }

    public class PhysicianGridEntityNew
    {
        public string Facility_Name { get; set; }
        public string NurseStation_Name { get; set; }
        public string PhysicianNPI { get; set; }
        public string PhysicianName { get; set; }
        public string PhysicianCity { get; set; }
        public string PhysicianState { get; set; }
        public string PhysicianCountry { get; set; }
        public string PhysicianZip { get; set; }
        public int Physician_Status { get; set; }
        public int Facility_Status { get; set; }
        public string Credentials { get; set; }
        public string PrimarySpec { get; set; }
        public string SupervisingPhy { get; set; }
        public string Physician_Phone { get; set; }

        public string Facility_Id { get; set; }
        public int ColorFlag { get; set; }



    }
    public class PhysicianGridEntity
    {
        public string Facility_Name { get; set; }
        public string NurseStation_Name { get; set; }
        public string PhysicianNPI { get; set; }
        public string PhysicianName { get; set; }
        public string PhysicianCity { get; set; }
        public string PhysicianState { get; set; }
        public string PhysicianCountry { get; set; }
        public string PhysicianZip { get; set; }
        public int Physician_Status { get; set; }
        public int Facility_Status { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public string Credentials { get; set; }
        public string PrimarySpec { get; set; }
        public string SupervisingPhy { get; set; }
        public string Physician_Phone { get; set; }


    }
  

    public class PhysicianDropEntity
    {
        public int Physician_Id { get; set; }
        public string PhysicianNPI { get; set; }
        public string PhysicianLName { get; set; }
        public string PhysicianFName { get; set; }
        public string PhysicianFullName { get; set; }
        public int Credentials { get; set; }
        public int PStatus { get; set; }

        public string SPhy { get; set; }

        public int SPhyStatus { get; set; }

        public string SphyName { get; set; }
        public string CredeValue { get; set; }

    }

}
