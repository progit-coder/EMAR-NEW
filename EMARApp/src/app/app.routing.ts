import { Routes } from '@angular/router';
//import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';

import { AuthgaurdService } from './services/shared/authgaurd.service';
import { CompanymasterComponent } from './emar/components/companymaster/companymaster.component';
import { FacilitymasterComponent } from './emar/components/facilitymaster/facilitymaster.component';
import { DevicemasterComponent } from './emar/components/devicemaster/devicemaster.component';
import { EmaruserComponent } from './emar/components/emaruser/emaruser.component';
import { FloormasterComponent } from './emar/components/floormaster/floormaster.component';
import { NursestationmasterComponent } from './emar/components/nursestationmaster/nursestationmaster.component';
import { WingmasterComponent } from './emar/components/wingmaster/wingmaster.component';
import { RoommasterComponent } from './emar/components/roommaster/roommaster.component';
import { BedmasterComponent } from './emar/components/bedmaster/bedmaster.component';
import { RolemasterComponent } from './emar/components/rolemaster/rolemaster.component';
import { DashboardComponent } from './emar/components/dashboard/dashboard.component';
import { RoleconfigComponent } from './emar/components/roleconfig/roleconfig.component';
import { UserfacilityroleconfigComponent } from './emar/components/userfacilityroleconfig/userfacilityroleconfig.component';
import { HlsegmentfieldsconfigComponent } from './emar/components/hlsegmentfieldsconfig/hlsegmentfieldsconfig.component';
import { FteconfigurationComponent } from './emar/components/fteconfiguration/fteconfiguration.component';
import { InboundfilesComponent } from './emar/components/inboundfiles/inboundfiles.component';
import { OutboundfilesComponent } from './emar/components/outboundfiles/outboundfiles.component';
import { ResidentgridComponent } from './emar/components/residentgrid/residentgrid.component';
import { ResidentinformationComponent } from './emar/components/residentinformation/residentinformation.component';
import { SingleLoginComponent } from './single-login/single-login.component';

export const rootRoutes: Routes = [
    { path: '', component: LoginComponent, pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    {path:'single-login',component:SingleLoginComponent},
    { path: '**', redirectTo: 'login' }
];

