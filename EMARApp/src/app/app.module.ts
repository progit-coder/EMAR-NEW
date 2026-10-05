import { BrowserModule } from "@angular/platform-browser";
import { NgModule, ErrorHandler } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { DragScrollModule } from 'ngx-drag-scroll';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './components/header/header.component';
import { LoginComponent } from './components/login/login.component';
import { PersistanceService } from "./services/shared/persistance.service";
import { InterceptorService } from "./services/shared/interceptor.service";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { DataService } from "./services/shared/dataservice.service";
import { ReactiveFormsModule } from "@angular/forms"
import { FooterComponent } from './components/footer/footer.component';
import { rootRoutes } from "./app.routing";
import { AuthgaurdService } from "./services/shared/authgaurd.service";
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SharedService } from "./services/shared/shared.service";
import { APIConfiguration } from "./models/app.constants";
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { EmarModule } from "./emar/emar.module";
import { AlertComponent } from './_directives/index';
import { AlertService } from './_services/index';
import { GridFilterPipe } from './services/shared/grid-filter.pipe';
import { ReplacePipe } from './services/shared/replace.pipe';
import { HomecomponentComponent } from "./emar/components/homecomponent/homecomponent.component";
import { NgChatModule } from 'ng-chat';
//import { SocketIoModule, SocketIoConfig } from 'ng-socket-io';
//const config: SocketIoConfig = { url: 'http://localhost:3000', options: {} };
//const config: SocketIoConfig = { url: 'http://10.20.6.52:3000', options: {} };
import { CustomdatePipe } from './services/shared/customdate.pipe';
import { ResetpasswordComponent } from './components/resetpassword/resetpassword.component';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProgressbarmodalComponent } from './components/progressbarmodal/progressbarmodal.component';
import { MergeordersComponent } from "./emar/components/mergeorders/mergeorders.component";
import { ResidentreactivateComponent } from "./emar/components/residentreactivate/residentreactivate.component";
import { ResidentonleaveComponent } from "./emar/components/residentonleave/residentonleave.component";
import { OrdersendingsoonComponent } from "./emar/components/ordersendingsoon/ordersendingsoon.component";
import { RejectedrefillsComponent } from "./emar/components/rejectedrefills/rejectedrefills.component";
import { SearchDrugNameComponent } from "./emar/components/search-drug-name/search-drug-name.component";
import { DefaultscreenComponent } from "./emar/components/defaultscreen/defaultscreen.component";
import { DemographicinfoComponent } from "./emar/components/demographicinfo/demographicinfo.component";
import { UpdatevisitinfoComponent } from "./emar/components/updatevisitinfo/updatevisitinfo.component";
import { DemographichistorymodalComponent } from './emar/components/demographichistorymodal/demographichistorymodal.component';
import { VisitinfohistorymodalComponent } from './emar/components/visitinfohistorymodal/visitinfohistorymodal.component';
import { PatientallergyhistorymodalComponent } from './emar/components/patientallergyhistorymodal/patientallergyhistorymodal.component';
import { PatientdiagnosishistorymodalComponent } from './emar/components/patientdiagnosishistorymodal/patientdiagnosishistorymodal.component';
import { LiteralorderseditmodalComponent } from './emar/components/literalorderseditmodal/literalorderseditmodal.component';
import { DemographiceditmodalComponent } from './emar/components/demographiceditmodal/demographiceditmodal.component';
import { VistinfoeditmodalComponent } from './emar/components/vistinfoeditmodal/vistinfoeditmodal.component';
import { PatientallergyeditmodalComponent } from './emar/components/patientallergyeditmodal/patientallergyeditmodal.component';
import { PatientdiagnosiseditmodalComponent } from './emar/components/patientdiagnosiseditmodal/patientdiagnosiseditmodal.component';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { AdminresetpasswordmodalComponent } from './emar/components/adminresetpasswordmodal/adminresetpasswordmodal.component';
import { AppGlobalErrorhandlerService } from './services/shared/app-global-errorhandler.service';
import { TransferresidentmodalComponent } from './emar/components/transferresidentmodal/transferresidentmodal.component';
import { DischargeresidentmodalComponent} from './emar/components/dischargeresidentmodal/dischargeresidentmodal.component';
import { UpdateadmitvisitinfoComponent } from './emar/components/updateadmitvisitinfo/updateadmitvisitinfo.component';
import { ControlledsubstancediscrepancyreasonComponent } from './emar/components/controlledsubstancediscrepancyreason/controlledsubstancediscrepancyreason.component';
import { OrdersdiscardComponent } from './emar/components/ordersdiscard/ordersdiscard.component';
import { RemovemarmodalComponent } from './emar/components/removemarmodal/removemarmodal.component';
import { ResidentmergeComponent } from './emar/components/residentmerge/residentmerge.component';
import { PendingforreviewComponent } from  './emar/components/pendingforreview/pendingforreview.component';
import { OrdersgridprndocumentationComponent } from './emar/components/ordersgridprndocumentation/ordersgridprndocumentation.component';
import { Ordersgrid72hourscheckComponent } from './emar/components/ordersgrid72hourscheck/ordersgrid72hourscheck.component';
import { AngularDraggableModule } from 'angular2-draggable';
import {NoCacheHeadersInterceptor} from "./services/shared/interceptor.service";
import { SearchpharmacynameComponent } from "./emar/components/searchpharmacyname/searchpharmacyname.component";
import { PharmacydiscardComponent } from "./emar/components/pharmacydiscard/pharmacydiscard.component";
import { EkitmedsqtyupdatesdiscardComponent } from "./emar/components/ekitmedsqtyupdatesdiscard/ekitmedsqtyupdatesdiscard.component";
import { TodaysdatealertComponent } from "./emar/components/todaysdatealert/todaysdatealert.component";
import { OtherallergyalertComponent } from "./emar/components/otherallergyalert/otherallergyalert.component";
import { SingleLoginComponent } from './single-login/single-login.component';
@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LoginComponent,
    FooterComponent,
    SidebarComponent,
    AlertComponent,
    HomecomponentComponent,
    CustomdatePipe,
    ResetpasswordComponent,
    ProgressbarmodalComponent,
    SingleLoginComponent,

    //GridFilterPipe,
  ],
  entryComponents: [
    ProgressbarmodalComponent,
    MergeordersComponent,
    ResidentreactivateComponent,
    ResidentonleaveComponent,
    OrdersendingsoonComponent,
    RejectedrefillsComponent,
    SearchDrugNameComponent,
    SearchpharmacynameComponent,
    DefaultscreenComponent,
    DemographicinfoComponent,
    UpdatevisitinfoComponent,
    DemographichistorymodalComponent,
    VisitinfohistorymodalComponent,
    PatientallergyhistorymodalComponent,
    PatientdiagnosishistorymodalComponent,
    LiteralorderseditmodalComponent,
    DemographiceditmodalComponent,
    VistinfoeditmodalComponent,
    PatientallergyeditmodalComponent,
    PatientdiagnosiseditmodalComponent,
    AdminresetpasswordmodalComponent,
    TransferresidentmodalComponent,
    DischargeresidentmodalComponent,
    UpdateadmitvisitinfoComponent,
    ControlledsubstancediscrepancyreasonComponent,
    OrdersdiscardComponent,
    RemovemarmodalComponent,
    ResidentmergeComponent,
    PendingforreviewComponent,
    OrdersgridprndocumentationComponent,
    Ordersgrid72hourscheckComponent,
    PharmacydiscardComponent,
    EkitmedsqtyupdatesdiscardComponent,
    TodaysdatealertComponent,
    OtherallergyalertComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    DragScrollModule,
    RouterModule.forRoot(rootRoutes, { useHash: true,onSameUrlNavigation: 'reload', }),
    HttpClientModule, NgMultiSelectDropDownModule.forRoot(),
    NgxLoadingModule.forRoot({ animationType: ngxLoadingAnimationTypes.rectangleBounce, primaryColour: '#2E77BB' }),
    ReactiveFormsModule,
    EmarModule, NgChatModule,
    //SocketIoModule.forRoot(config),
    NgIdleKeepaliveModule.forRoot(),
    NgbModule.forRoot(),
    Ng4LoadingSpinnerModule.forRoot(),
    AngularDraggableModule
  ],
  providers: [PersistanceService, CustomdatePipe,GridFilterPipe,ReplacePipe, AuthgaurdService, DataService, SharedService, APIConfiguration, AlertService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: NoCacheHeadersInterceptor,
      multi: true
    },
    {
      provide: ErrorHandler,
      useClass: AppGlobalErrorhandlerService,
      //multi: true
    },
  ],
  exports: [RouterModule],
  bootstrap: [AppComponent]
})
export class AppModule { }
