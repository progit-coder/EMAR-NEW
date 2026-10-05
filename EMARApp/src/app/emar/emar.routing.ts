import { Routes } from '@angular/router';
import { AuthgaurdService } from '../services/shared/authgaurd.service';
import { CompanymasterComponent } from './components/companymaster/companymaster.component';
import { CompanyconfigurationComponent } from './components/companyconfiguration/companyconfiguration.component';
import { FacilitymasterComponent } from './components/facilitymaster/facilitymaster.component';
import { DevicemasterComponent } from './components/devicemaster/devicemaster.component';
import { EmaruserComponent } from './components/emaruser/emaruser.component';
import { FloormasterComponent } from './components/floormaster/floormaster.component';
import { NursestationmasterComponent } from './components/nursestationmaster/nursestationmaster.component';
import { WingmasterComponent } from './components/wingmaster/wingmaster.component';
import { RoommasterComponent } from './components/roommaster/roommaster.component';
import { BedmasterComponent } from './components/bedmaster/bedmaster.component';
import { RolemasterComponent } from './components/rolemaster/rolemaster.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { RoleconfigComponent } from './components/roleconfig/roleconfig.component';
import { UserfacilityroleconfigComponent } from './components/userfacilityroleconfig/userfacilityroleconfig.component';
import { HlsegmentfieldsconfigComponent } from './components/hlsegmentfieldsconfig/hlsegmentfieldsconfig.component';
import { FteconfigurationComponent } from './components/fteconfiguration/fteconfiguration.component';
import { InboundfilesComponent } from './components/inboundfiles/inboundfiles.component';
import { OutboundfilesComponent } from './components/outboundfiles/outboundfiles.component';
import { ResidentgridComponent } from './components/residentgrid/residentgrid.component';
import { ResidentinformationComponent } from './components/residentinformation/residentinformation.component';
import { AllergymasterComponent } from './components/allergymaster/allergymaster.component';
import { IcdmasterComponent } from './components/icdmaster/icdmaster.component';
import { IntegrationsComponent } from './components/integrations/integrations.component';
import { CompanybedconfigComponent } from './components/companybedconfig/companybedconfig.component';
import { FrequencymappingComponent } from './components/frequencymapping/frequencymapping.component';
import { AdminapprovalComponent } from './components/adminapproval/adminapproval.component';
import { WeightComponent } from './components/weight/weight.component';
import { FluidintakeComponent } from './components/fluidintake/fluidintake.component';
import { BehaviourComponent } from './components/behaviour/behaviour.component';
import { VitalsComponent } from './components/vitals/vitals.component';
import { NursenotesComponent } from './components/nursenotes/nursenotes.component';
import { AppComponent } from '../app.component';
import { HomecomponentComponent } from './components/homecomponent/homecomponent.component';
import { SeventytwohourchecksComponent } from './components/seventytwohourchecks/seventytwohourchecks.component';
import { PrndocumentationComponent } from './components/prndocumentation/prndocumentation.component';
import { ControlsubstancecountComponent } from './components/controlsubstancecount/controlsubstancecount.component';
//import { DynamicdashboardComponent } from './components/dynamicdashboard/dynamicdashboard.component';
import { DocumentmanagerComponent } from './components/documentmanager/documentmanager.component';
import { VitalsdashboardComponent } from './components/vitalsdashboard/vitalsdashboard.component';
import { CensusdashboardComponent } from './components/censusdashboard/censusdashboard.component';
import { ControlsubstanceComponent } from './components/controlsubstance/controlsubstance.component';
import { SeventytwocheckdrComponent } from './components/seventytwocheckdr/seventytwocheckdr.component';
import { PrndrComponent } from './components/prndr/prndr.component';
import { OrderdrComponent } from './components/orderdr/orderdr.component';
import { BarcodedrComponent } from './components/barcodedr/barcodedr.component';
import { BiometericdrComponent } from './components/biometericdr/biometericdr.component';
import { OrdercontrolsubstancedrComponent } from './components/ordercontrolsubstancedr/ordercontrolsubstancedr.component';
import { OrderwithfavouritiesdrComponent } from './components/orderwithfavouritiesdr/orderwithfavouritiesdr.component';
import { OrdercontrolsignoffdrComponent } from './components/ordercontrolsignoffdr/ordercontrolsignoffdr.component';
import { OrderholddrComponent } from './components/orderholddr/orderholddr.component';
import { RefusedbyresidentsdrComponent } from './components/refusedbyresidentsdr/refusedbyresidentsdr.component';
import { OrderchangedrComponent } from './components/orderchangedr/orderchangedr.component';
import { ScanningbypassdrComponent } from './components/scanningbypassdr/scanningbypassdr.component';
import { DestructiondrComponent } from './components/destructiondr/destructiondr.component';
import { FloorstockdrComponent } from './components/floorstockdr/floorstockdr.component';
import { PharmacymedsdrComponent } from './components/pharmacymedsdr/pharmacymedsdr.component';
import { MailindoxComponent } from './components/mailindox/mailindox.component';

import { PsychiatricdrComponent } from './components/psychiatricdr/psychiatricdr.component';
import { NursenotesdrComponent } from './components/nursenotesdr/nursenotesdr.component';
import { PrescribernotedrComponent } from './components/prescribernotedr/prescribernotedr.component';
import { MardrComponent } from './components/mardr/mardr.component';
import { HlseveninbounddrComponent } from './components/hlseveninbounddr/hlseveninbounddr.component';
import { HlsevenoutbounderrordrComponent } from './components/hlsevenoutbounderrordr/hlsevenoutbounderrordr.component';
import { UseractivitydrComponent } from './components/useractivitydr/useractivitydr.component';
import { AdministrationComponent } from './components/administration/administration.component';
import { ColorpickerComponent } from './components/colorpicker/colorpicker.component';
import { StockComponent } from './components/stock/stock.component';
import { EkitComponent } from './components/ekit/ekit.component';
import { AcknowledgeordersComponent } from './components/acknowledgeorders/acknowledgeorders.component';
import { ReadmailComponent } from './components/readmail/readmail.component';
import { ComposemailComponent } from './components/composemail/composemail.component';
import { AlertsComponent } from './components/alerts/alerts.component';
import { PhysciandetailsComponent } from './components/physciandetails/physciandetails.component';
import { OrdergridComponent } from './components/ordergrid/ordergrid.component';
import { OrderinfoComponent } from './components/orderinfo/orderinfo.component';
import { EkitdrComponent } from './components/ekitdr/ekitdr.component';
import { StockdrComponent } from './components/stockdr/stockdr.component';
import { DrfirstfilesComponent } from './components/drfirstfiles/drfirstfiles.component';
import { AdminusersdrComponent } from './components/adminusersdr/adminusersdr.component';
import { SetupconfigdatadrComponent } from './components/setupconfigdatadr/setupconfigdatadr.component';
import { RefilldrComponent } from './components/refilldr/refilldr.component';
import { ErrorPage404Component } from './components/error-page404/error-page404.component';
import { ErrorPage500Component } from './components/error-page500/error-page500.component';
import { OutboundhlsegmentfielddisplayconfigComponent } from './components/outboundhlsegmentfielddisplayconfig/outboundhlsegmentfielddisplayconfig.component';
import { CheckinmedsComponent } from './components/checkinmeds/checkinmeds.component';
import {EkitmedsdispensingdrComponent} from './components/ekitmedsdispensingdr/ekitmedsdispensingdr.component';
import { DocumentcheckedordersComponent} from './components/documentcheckedorders/documentcheckedorders.component';
import { DocadmindrComponent } from './components/docadmindr/docadmindr.component';
import { CompanytobedmapingComponent } from './components/companytobedmaping/companytobedmaping.component';
import { CertificationordersComponent } from './components/certificationorders/certificationorders.component';
import { CertifiedordersdrComponent } from './components/certifiedordersdr/certifiedordersdr.component';
import { MeasurementsuserinputmasterComponent } from './components/measurementsuserinputmaster/measurementsuserinputmaster.component';
import { EkitmedscheckinComponent } from './components/ekitmedscheckin/ekitmedscheckin.component';
import { PharmacymedsexpirydrComponent } from './components/pharmacymedsexpirydr/pharmacymedsexpirydr.component';
import { EkitmedsexpirationdrComponent } from './components/ekitmedsexpirationdr/ekitmedsexpirationdr.component';
import { RefillrejectionmailconfigComponent } from './components/refillrejectionmailconfig/refillrejectionmailconfig.component';
import { OrderinfocpoeComponent } from './components/orderinfocpoe/orderinfocpoe.component';
import { OrdergridcpoeComponent } from './components/ordergridcpoe/ordergridcpoe.component';
import { ProfilecertificationordersComponent } from './components/profilecertificationorders/profilecertificationorders.component';
import { ProfilecertifiedordersdrComponent } from './components/profilecertifiedordersdr/profilecertifiedordersdr.component';
import { MailconfigComponent } from './components/mailconfig/mailconfig.component';
import { TherapeuticalComponent } from './components/therapeutical/therapeutical.component';
import { TherapeuticalorderComponent } from './components/therapeuticalorder/therapeuticalorder.component';
import { MedrefComponent } from './components/medref/medref.component';
import { PharmacydetailsComponent } from './components/pharmacydetails/pharmacydetails.component';
import { DrXmlComponent } from './components/dr-xml/dr-xml.component';
import { MedicationcheckindrComponent } from './components/medicationcheckindr/medicationcheckindr.component';
import { MedqtyonhandupdatedrComponent } from './components/medqtyonhandupdatedr/medqtyonhandupdatedr.component';

export const emarRoutes: Routes = [
   {
      path: 'home',
      component: HomecomponentComponent, 
      children: [
         { path: 'companymaster', component: CompanymasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'companyconfiguration', component: CompanyconfigurationComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'facilitymaster', component: FacilitymasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'devicemaster', component: DevicemasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'emaruser', component: EmaruserComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },

         { path: 'floor', component: FloormasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'nursestation', component: NursestationmasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'wing', component: WingmasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'room', component: RoommasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'bed', component: BedmasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'role', component: RolemasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'roleconfig', component: RoleconfigComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'dashboard', component: DashboardComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'usrfacroleconfig', component: UserfacilityroleconfigComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'hlsegment', component: HlsegmentfieldsconfigComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'fteconfig', component: FteconfigurationComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'inbound', component: InboundfilesComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'outbound', component: OutboundfilesComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'residentgrid', component: ResidentgridComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'residentinformation', component: ResidentinformationComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'allergymaster', component: AllergymasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'icdmaster', component: IcdmasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'integrations', component: IntegrationsComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'companybedconfig', component: CompanybedconfigComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'frequencymapping', component: FrequencymappingComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'adminapproval', component: AdminapprovalComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'foodintake', component: FluidintakeComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'behaviour', component: BehaviourComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'weightlog', component: WeightComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'vitals', component: VitalsComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'nursenotes', component: NursenotesComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'prndocumentation', component: PrndocumentationComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: '72hourchecks', component: SeventytwohourchecksComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'controlsubstance', component: ControlsubstancecountComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         //{ path: 'dynamicdashboard', component: DynamicdashboardComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'documentmanager', component: DocumentmanagerComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'vitalsdashboard', component: VitalsdashboardComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'censusdashboard', component: CensusdashboardComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'DrControlSubstance', component: ControlsubstanceComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'mailbox', component: MailindoxComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'seventytwodr', component: SeventytwocheckdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'prndr', component: PrndrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'orderdr', component: OrderdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'barcodedr', component: BarcodedrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'biometericdr', component: BiometericdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ordercontrolsubstancedr', component: OrdercontrolsubstancedrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'orderwithfavouritiesdr', component: OrderwithfavouritiesdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ordersignoffdr', component: OrdercontrolsignoffdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'orderholddr', component: OrderholddrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'refusedbyresdr', component: RefusedbyresidentsdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'orderchangedr', component: OrderchangedrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'scanningbypassdr', component: ScanningbypassdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'destructiondr', component: DestructiondrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'floorstockdr', component: FloorstockdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'pharmacymedsdr', component: PharmacymedsdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'psychiatricdr', component: PsychiatricdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'nursenotesdr', component: NursenotesdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'prescribernotesdr', component: PrescribernotedrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'mardr', component: MardrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'hlseveninbounddr', component: HlseveninbounddrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'hlsevenoutbounderrordr', component: HlsevenoutbounderrordrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'useractivitydr', component: UseractivitydrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'administration', component: AdministrationComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'colorpicker', component: ColorpickerComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'stock', component: StockComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ekit', component: EkitComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
		   { path: 'acknowledgeorders', component: AcknowledgeordersComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'readmail', component: ReadmailComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'composemail', component: ComposemailComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'alerts', component: AlertsComponent, pathMatch: 'full', canActivate: [AuthgaurdService],runGuardsAndResolvers: 'always', },
         { path: 'physciandetails', component: PhysciandetailsComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ordergrid', component: OrdergridComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'orderinfo', component: OrderinfoComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ekitdr', component: EkitdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'stockdr', component: StockdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'drfirstfiles', component: DrfirstfilesComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'adminusersdr', component: AdminusersdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'setupconfigdr', component: SetupconfigdatadrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'refilldr', component: RefilldrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'checkinmed', component: CheckinmedsComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'error404', component: ErrorPage404Component, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'error500', component: ErrorPage500Component, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'outboundhlsegmentfile', component: OutboundhlsegmentfielddisplayconfigComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ekitmedsdispensingdr', component: EkitmedsdispensingdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'DocumentAdmin', component: DocumentcheckedordersComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'DocAdminReport', component: DocadmindrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'companytobedmapping', component: CompanytobedmapingComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'certificationorders', component: CertificationordersComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'certifiedordersdr', component: CertifiedordersdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'measurementanduserinputmaster', component: MeasurementsuserinputmasterComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ekitmedscheckin', component: EkitmedscheckinComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'pharmacymedsexpdr', component: PharmacymedsexpirydrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ekitmedsexpdr', component: EkitmedsexpirationdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'refillrejectmailconfig', component: RefillrejectionmailconfigComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'orderinfocpoe', component: OrderinfocpoeComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'ordergridcpoe', component: OrdergridcpoeComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'profilecertificationorders', component: ProfilecertificationordersComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'profilecertifiedordersdr', component: ProfilecertifiedordersdrComponent, pathMatch: 'full', canActivate: [AuthgaurdService] },
         { path: 'mailconfig',component:MailconfigComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: 'therapeutical',component:TherapeuticalComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: 'therapeuticalorder',component:TherapeuticalorderComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: 'medref',component:MedrefComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: 'pharmacydetails',component:PharmacydetailsComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: 'drxml',component:DrXmlComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: 'medicationcheckindr',component:MedicationcheckindrComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path:'Medqtyonhandupdatedr', component:MedqtyonhandupdatedrComponent,pathMatch:'full', canActivate: [AuthgaurdService]},
         { path: '**', redirectTo: 'error404' },
      ] , canActivate: [AuthgaurdService] 
   }
]

