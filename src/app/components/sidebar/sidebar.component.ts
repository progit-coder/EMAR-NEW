import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../../services/shared/shared.service';
import { PersistanceService } from '../../services/shared/persistance.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { OrdersdiscardComponent } from 'src/app/emar/components/ordersdiscard/ordersdiscard.component';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Output() saveChanges: EventEmitter<any> = new EventEmitter();
  public armenu: menu[] = [];
  public ioMenu: menu[] = [];
  public ADTmenu: menu[] = [];
  public Ordersmenu: menu[] = [];
  public AssessmentsMenu: menu[] = [];
  public EmarMenu: menu[] = [];
  public DashboardsMenu: menu[] = [];
  public MailboxMenu: menu[] = [];
  pageConfig = {};
  screenNames: any;
  showInbound: boolean = false;
  showOutbound: boolean = false;
  displayName;
  iscollapsed = true;
  public selectedItem: any;
  public selectedSubItem: any;
  public valueChangesFlag: number = 0;
  modalOption: NgbModalOptions = {};

  constructor(private router: Router, private modalService: NgbModal, private sharedService: SharedService, private persistanceService: PersistanceService) {
  }


  ngOnInit() {
    this.sharedService.saveChangesFlag.subscribe(res => this.valueChangesFlag = res);
    this.sharedService.screenPermissionsSource.subscribe(screens => {
      this.screenNames = screens;
      if (this.screenNames.length != undefined && this.screenNames.length != 0) {
        this.companyclick();
        this.ADTclick();
        this.Ordersclick();
        this.bindIndividualLinks();
        this.LoadAssessments();
        this.LoadEmar();
        this.LoadDashboards();
        this.LoadMailbox();
      }
      else {
        let arScreenConfig = this.persistanceService.get("screenNames");
        if (arScreenConfig != null)
          this.sharedService.updateScreenNames(arScreenConfig);
      }
      this.displayName = this.persistanceService.get('displayname');
      if (this.displayName != null)
        this.displayName = this.displayName.substring(1, this.displayName.length - 1);
      // console.log(this.armenu);
      // console.log(this.ADTmenu);
    });
    // $('.admin').addClass('menu-open active');
    //  this.selectedItem='admin';
  }
  showAndHideMenu(cls) {
    
    if (this.valueChangesFlag == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlag = 0;
          event.stopPropagation();
          if (this.selectedItem == cls) {
            this.selectedItem = '';
          }
          else {
            this.selectedItem = '';
            this.selectedItem = cls;
            if(cls =='order')
            {
              let pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
              if (pageConfig != undefined && pageConfig["AccessRead"] != 0) {
              this.router.navigate(["home/ordergrid"]);
              this.selectedSubItem ='Orders';
              }
            }
            else if(cls =='adt')
            {
              let pageConfig = this.persistanceService.getPermissionsByScreen("ResidentGrid");
              if (pageConfig != undefined && pageConfig["AccessRead"] != 0) {
              this.router.navigate(["home/residentgrid"]);
              this.selectedSubItem ='Resident Grid';
              }
            }
            else if(cls =='emar')
            {
              let pageConfig = this.persistanceService.getPermissionsByScreen("EMAR");
              if (pageConfig != undefined && pageConfig["AccessRead"] != 0) {
              this.router.navigate(["home/administration"]);
              this.selectedSubItem ='eMAR';
              }
            }
          }
        }
        modalRef.close();
      });
    }
    else {
      event.stopPropagation();
      if (this.selectedItem == cls) {
        this.selectedItem = '';
      }
      else {
        this.selectedItem = '';
        this.selectedItem = cls;
        if(cls =='order')
        {
          let pageConfig = this.persistanceService.getPermissionsByScreen("Orders");
          if (pageConfig != undefined && pageConfig["AccessRead"] != 0) {
          this.router.navigate(["home/ordergrid"]);
          this.selectedSubItem ='Orders';
          }
        }
        else if(cls =='adt')
        {
          let pageConfig = this.persistanceService.getPermissionsByScreen("ResidentGrid");
          if (pageConfig != undefined && pageConfig["AccessRead"] != 0) {
          this.router.navigate(["home/residentgrid"]);
          this.selectedSubItem ='Resident Grid';
          }
        }
        else if(cls =='emar')
        {
          let pageConfig = this.persistanceService.getPermissionsByScreen("EMAR");
          if (pageConfig != undefined && pageConfig["AccessRead"] != 0) {
          this.router.navigate(["home/administration"]);
          this.selectedSubItem ='eMAR';
          }
        }
      }
    }
  }
  showAndHideSubMenu(route) {  
    window.scroll(0,0);
    if (this.valueChangesFlag == 1) {
      const modalRef = this.modalService.open(OrdersdiscardComponent, { size: 'lg', windowClass: '' });
      modalRef.componentInstance.result.subscribe((receivedResult) => {
        if (receivedResult == 2) {
          modalRef.close();
        }
        else if (receivedResult == 1) {
          this.sharedService.saveChangesOrderInfo(0);
          this.valueChangesFlag = 0;
          this.router.navigate([route]);
        }
        modalRef.close();
      });
    }
    else {
      // if (route == 'home/outbound')
      //   this.selectedSubItem = 'outbound';
      // else if (route == 'home/inbound')
      //   this.selectedSubItem = 'inbound';
      // else
      //   this.selectedSubItem = '';

      this.router.navigate([route]);
    }
  }
  bindIndividualLinks() {
   this.ioMenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "Inbound") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            //this.showInbound = true;
            this.ioMenu.push({ name: "Inbound Files", route: "/home/inbound" });
          }
        }
        else if (this.screenNames[index]["name"] == "Outbound") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            //this.showOutbound = true;
            this.ioMenu.push({ name: "Outbound Files", route: "/home/outbound" });
          }
        }
      }
    }
  }
  public companyclick(): void {
    this.armenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "Dashboard") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Dashboard", route: "/home/dashboard" });
          }
        }
        else if (this.screenNames[index]["name"] == "CompanyMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Company Master", route: "/home/companymaster" });
          }
        }
        else if (this.screenNames[index]["name"] == "CompanyConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Company Configuration", route: "/home/companyconfiguration" });
          }
        }

      }
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "FacilityMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Facility Master", route: "/home/facilitymaster" });
          }
        }
        else if (this.screenNames[index]["name"] == "NursingStationMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Nursing Station Master", route: "/home/nursestation" });
          }
        }
        else if (this.screenNames[index]["name"] == "FloorMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Floor Master", route: "/home/floor" });
          }
        }
        else if (this.screenNames[index]["name"] == "WingMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Wing Master", route: "/home/wing" });
          }
        }
        else if (this.screenNames[index]["name"] == "RoomMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Room Master", route: "/home/room" });
          }
        }
        else if (this.screenNames[index]["name"] == "BedMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Bed Master", route: "/home/bed" });
          }
        }
        else if (this.screenNames[index]["name"] == "CompanyBedConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Company to Bed Configuration ", route: "/home/companybedconfig" });
          }
        }
        else if (this.screenNames[index]["name"] == "CompanyBedHierarchy") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Company to Bed Hierarchy", route: "/home/companytobedmapping" });
          }
        }



        else if (this.screenNames[index]["name"] == "Users") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Users", route: "/home/emaruser" });
          }
        }
        else if (this.screenNames[index]["name"] == "RoleMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Role Master", route: "/home/role" });
          }
        } else if (this.screenNames[index]["name"] == "RoleConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Role Configuration ", route: "/home/roleconfig" });
          }
        }
        else if (this.screenNames[index]["name"] == "HLSegmentFieldsConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "HL Segment Fields Configuration ", route: "/home/hlsegment" });
          }
        }
        else if (this.screenNames[index]["name"] == "ADTFieldsDisplayConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "ADT Fields Display Configuration ", route: "/home/outboundhlsegmentfile" });
          }
        }
        else if (this.screenNames[index]["name"] == "FTPConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "FT Configuration", route: "/home/fteconfig" });
          }
        }
        else if (this.screenNames[index]["name"] == "AllergyMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Allergy Master", route: "/home/allergymaster" });
          }
        }
        else if (this.screenNames[index]["name"] == "ICD10Master") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "ICD10 Master", route: "/home/icdmaster" });
          }
        }
        else if (this.screenNames[index]["name"] == "FrequencyMapping") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Frequency Mapping", route: "/home/frequencymapping" });
          }
        }
        else if (this.screenNames[index]["name"] == "ColorPicker") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Resident Color Configuration", route: "/home/colorpicker" });
          }
        }
        else if (this.screenNames[index]["name"] == "PrescriberDetails") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Prescriber Details", route: "/home/physciandetails" });
          }
        }
        else if (this.screenNames[index]["name"] == "MeasurementandOtherChecksMaster") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Measurements and User Input Master", route: "/home/measurementanduserinputmaster" });
          }
        }
        else if (this.screenNames[index]["name"] == "Refill/DiscontinueRejectionMailConfiguration") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Refill/Discontinue Rejection Mail Configuration", route: "/home/refillrejectmailconfig" });
          }
        }
        else if (this.screenNames[index]["name"] == "MailConfig") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Mail Configuration", route: "/home/mailconfig" });
          }
        }
        else if (this.screenNames[index]["name"] == "PharmacyDetails") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.armenu.push({ name: "Pharmacy Details", route: "/home/pharmacydetails" });
          }
        }
      }
    }
  }
  public ADTclick(): void {

    this.ADTmenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "ResidentGrid") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.ADTmenu.push({ name: "Resident Grid", route: "/home/residentgrid" });
          }
        }
        else if (this.screenNames[index]["name"] == "AdminApproval") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.ADTmenu.push({ name: "Approval", route: "/home/adminapproval" });
          }
        }
        else if (this.screenNames[index]["name"] == "Integrations") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.ADTmenu.push({ name: "Integrations", route: "/home/integrations" });
          }
        }
        else if (this.screenNames[index]["name"] == "DocumentManager") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.ADTmenu.push({ name: "Document Manager", route: "/home/documentmanager" });
          }
        }
        // this.ADTmenu.push({ name: "Resident Information", route: "/residentinformation" });
        // this.router.navigate(['/residentgrid']);
      }
    }
  }
  public Ordersclick(): void {
  
    this.Ordersmenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "Orders") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "Orders", route: "/home/ordergrid" });
          }
        }
        else if (this.screenNames[index]["name"] == "DrFirstFiles") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "Dr.First Prescriptions", route: "/home/drfirstfiles" });
          }
        }
        else if (this.screenNames[index]["name"] == "Stock") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            if (this.Ordersmenu.find(e => e.name == "Stock/eKit") == undefined)
              this.Ordersmenu.push({ name: "Stock/eKit", route: "/home/stock" });
          }
        }
        else if (this.screenNames[index]["name"] == "Ekit") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            if (this.Ordersmenu.find(e => e.name == "Stock/eKit") == undefined)
              this.Ordersmenu.push({ name: "Stock/eKit", route: "/home/stock" });
          }
        }
        // else if (this.screenNames[index]["name"] == "AcknowledgeOrders") {
        //   this.pageConfig = this.screenNames[index]["values"];
        //   if (this.pageConfig["AccessRead"] == 1) {
        //     this.Ordersmenu.push({ name: "Acknowledge Orders", route: "/home/acknowledgeorders" });
        //   }
        // }        
        else if (this.screenNames[index]["name"] == "ControlledSubstance") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "Controlled Substance Count", route: "/home/controlsubstance" });
          }
        }
        else if (this.screenNames[index]["name"] == "PharmacyCheck-inMedication") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "Pharmacy Medication Check-in", route: "/home/checkinmed" });
          }
        }
        else if (this.screenNames[index]["name"] == "EkitMedicationCheck-in") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "E-kit Medication Check-In/Updates", route: "/home/ekitmedscheckin" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrderCertification") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "Order Certification", route: "/home/certificationorders" });
          }
        }
        else if (this.screenNames[index]["name"] == "CPOE") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "CPOE", route: "/home/ordergridcpoe" });
          }
        }
        else if (this.screenNames[index]["name"] == "ProfileCertification") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.Ordersmenu.push({ name: "Profile Certification", route: "/home/profilecertificationorders" });
          }
        }
      }
    }
  }

  public LoadEmar(): void {

    this.EmarMenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "EMAR") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.EmarMenu.push({ name: "eMAR", route: "/home/administration" });
          }
        }
        else if (this.screenNames[index]["name"] == "PRNDocumentation") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.EmarMenu.push({ name: "PRN-Document", route: "/home/prndocumentation" });
          }
        }
        else if (this.screenNames[index]["name"] == "SeventyTwoHourChecks") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.EmarMenu.push({ name: "72 Hour Check", route: "/home/72hourchecks" });
          }
        }
        else if (this.screenNames[index]["name"] == "DocumentAdministeredOrders") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.EmarMenu.push({ name: "Document Administered Orders ", route: "/home/DocumentAdmin" });
          }
        }
      }
    }
  }
  public LoadAssessments(): void {

    this.AssessmentsMenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "Vitals") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.AssessmentsMenu.push({ name: "Vitals", route: "/home/vitals" });
          }
        }
        else if (this.screenNames[index]["name"] == "WeightLog") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.AssessmentsMenu.push({ name: "Weight Log", route: "/home/weightlog" });
          }
        }
        else if (this.screenNames[index]["name"] == "FoodIntake") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.AssessmentsMenu.push({ name: "Food Intake", route: "/home/foodintake" });
          }
        }
        else if (this.screenNames[index]["name"] == "NurseNotes") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.AssessmentsMenu.push({ name: "Nurse Notes", route: "/home/nursenotes" });
          }
        }
        else if (this.screenNames[index]["name"] == "Behavior") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.AssessmentsMenu.push({ name: "Behavior", route: "/home/behaviour" });
          }
        }
      }
    }
  }
  public LoadDashboards(): void {

    this.DashboardsMenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        // if (this.screenNames[index]["name"] == "VitalsDashboard") {
        //   this.pageConfig = this.screenNames[index]["values"];
        //   if (this.pageConfig["AccessRead"] == 1) {
        //     this.DashboardsMenu.push({ name: "Vitals Dashboard", route: "/home/vitalsdashboard" });
        //   }
        // }
        // else if (this.screenNames[index]["name"] == "DynamicDashboard") {
        //   this.pageConfig = this.screenNames[index]["values"];
        //   if (this.pageConfig["AccessRead"] == 1) {
        //     this.DashboardsMenu.push({ name: "Dynamic Dashboard", route: "/home/dynamicdashboard" });
        //   }
        // }
        // else
        if (this.screenNames[index]["name"] == "Census") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Census Dashboard", route: "/home/censusdashboard" });
          }
        }
        else if (this.screenNames[index]["name"] == "72HoursFollow-upReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "72 Hour Follow-Up Report", route: "/home/seventytwodr" });
          }
        }
        else if (this.screenNames[index]["name"] == "MAR by Therapeutic Category") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "MAR by Therapeutic Category", route: "/home/therapeutical" });
          }
        }
        else if (this.screenNames[index]["name"] == "Therapeutical Order Report") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Orders by Therapeutic Classification Report", route: "/home/therapeuticalorder" });
          }
        }
        else if (this.screenNames[index]["name"] == "RefusedByResidentReportCustom") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Medication Refusal Report Custom Dates", route: "/home/medref" });
          }
        }
        else if (this.screenNames[index]["name"] == "PRNReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "PRN Report", route: "/home/prndr" });
          }
        }
        else if (this.screenNames[index]["name"] == "ScheduledOrderReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Scheduled Order Report", route: "/home/orderdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrdersWithoutBarcodeReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Orders Without Barcode Report", route: "/home/barcodedr" });
          }
        }
        else if (this.screenNames[index]["name"] == "BiometricReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Biometric Bypass Report", route: "/home/biometericdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrdersforControlledSubstancesReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Orders for Controlled Substances Report", route: "/home/ordercontrolsubstancedr" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrderwithUserInputsReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Measurements and Other Checks Report", route: "/home/orderwithfavouritiesdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "ControlledMedicationSignoffReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Controlled Medication  Sign-Off Report", route: "/home/ordersignoffdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrdersonHoldReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Orders on Hold Report", route: "/home/orderholddr" });
          }
        }
        else if (this.screenNames[index]["name"] == "RefusedByResidentReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Medication Refusal Report Monthly", route: "/home/refusedbyresdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrderChangeReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Order Change Report", route: "/home/orderchangedr" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrdersAdministeredwithoutScanningReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Orders Administered without Scanning", route: "/home/scanningbypassdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "MedicationDestructionReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Medication Destruction Report", route: "/home/destructiondr" });
          }
        }
        else if (this.screenNames[index]["name"] == "FloorStockReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Floor Stock Report", route: "/home/floorstockdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "MedicationCheck-inReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Medication Check-in Report", route: "/home/pharmacymedsdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "PsychiatricReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Psychiatric Report", route: "/home/psychiatricdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "Nurses'NotesReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Nurses' Notes Report", route: "/home/nursenotesdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "PrescriberNoteReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Prescriber Notes Report", route: "/home/prescribernotesdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "HL7InboundReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "HL7 Inbound Messages Report", route: "/home/hlseveninbounddr" });
          }
        }
        else if (this.screenNames[index]["name"] == "HL7OutboundErrorReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "HL7 Outbound Error Report", route: "/home/hlsevenoutbounderrordr" });
          }
        }
        else if (this.screenNames[index]["name"] == "MARReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "MAR Report", route: "/home/mardr" });
          }
        }
        else if (this.screenNames[index]["name"] == "UserActivityReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "User Activity Report", route: "/home/useractivitydr" });
          }
        }
        else if (this.screenNames[index]["name"] == "E-Kit/On-siteMedicationInventoryReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "E-Kit/On-site Medication Active Inventory Report", route: "/home/ekitdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "E-Kit/On-siteMedicationDispensingReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "E-Kit/On-site Medication Dispensing Report", route: "/home/ekitmedsdispensingdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "E-Kit/On-siteMedicationCheckinReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "E-Kit/On-site Medication Check-in Report", route: "/home/medicationcheckindr" });
          }
        }
        else if (this.screenNames[index]["name"] == "E-Kit/On-siteMedicationQtyonhandUpdateReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "E-Kit/On-site Medication Qty On-Hand Updates Report", route: "/home/Medqtyonhandupdatedr" });
          }
        }
        else if (this.screenNames[index]["name"] == "ListofUsersReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "List of Users Report", route: "/home/adminusersdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "SetupConfigurationReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Setup Configuration  Report", route: "/home/setupconfigdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "RefillRequestedReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Refill Request Report", route: "/home/refilldr" });
          }
        }
        else if (this.screenNames[index]["name"] == "ForcedPassReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Forced Pass Report ", route: "/home/DocAdminReport" });
          }
        }
        else if (this.screenNames[index]["name"] == "OrderCertificationReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Order Certification Report", route: "/home/certifiedordersdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "MedicationExpirationDateReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Medication Expiration Date Report", route: "/home/pharmacymedsexpdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "E-Kit/On-siteMedicationExpirationDateReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "E-Kit/On-site Medication Expiration Date Report", route: "/home/ekitmedsexpdr" });
          }
        }
        else if (this.screenNames[index]["name"] == "ProfileCertificationReport") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.DashboardsMenu.push({ name: "Profile Certification Report", route: "/home/profilecertifiedordersdr" });
          }
        }
      }
    }
  }
  public LoadMailbox(): void {

    this.MailboxMenu = [];
    if (this.screenNames.length > 0) {
      for (let index = 0; index < this.screenNames.length; index++) {
        if (this.screenNames[index]["name"] == "Mailbox") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.MailboxMenu.push({ name: "Mailbox", route: "/home/mailbox" });
          }
        }
        else if (this.screenNames[index]["name"] == "Alerts") {
          this.pageConfig = this.screenNames[index]["values"];
          if (this.pageConfig["AccessRead"] == 1) {
            this.MailboxMenu.push({ name: "Alerts/Notifications", route: "/home/alerts" });
          }
        }
      }
    }
  }
}
export class menu {
  public name: string = "";
  public route: string = "";

}