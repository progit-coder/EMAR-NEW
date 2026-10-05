import { Component, Input, HostListener, ChangeDetectorRef, ElementRef } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { HeaderComponent } from '../../../components/header/header.component'
import { PersistanceService } from '../../../services/shared/persistance.service';
import { AlertService } from '../../../_services/index';
import { Ng4LoadingSpinnerModule, Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { ChatAdapter } from 'ng-chat';
//import { ChatAdapterClass } from '../../../models/chatadapterclass';
import { DataService } from '../../../services/shared/dataservice.service';
import { APIConfiguration } from '../../../models/app.constants';
import { Idle, EventTargetInterruptSource } from '@ng-idle/core';
// import { Keepalive } from '@ng-idle/keepalive';
import { ProgressbarmodalComponent } from './../../../components/progressbarmodal/progressbarmodal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgbModalRef } from '@ng-bootstrap/ng-bootstrap/modal/modal.module';
import { Subscription } from 'rxjs';
//import { Socket } from 'ng-socket-io';
import { HttpClient, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-homecomponent',
  templateUrl: './homecomponent.component.html',
  styleUrls: ['./homecomponent.component.css']
})
export class HomecomponentComponent {
  title = 'app';
  chatTitle = "Users List";
  isUserLoggedIn: any;
  isUserToken = false;
  public adapter: ChatAdapter;
  idleState = 'NOT_STARTED';
  timedOut = false;
  lastPing?: Date = null;
  progressBarPopup: NgbModalRef;
  onIdleStartSubscription: Subscription;
  onTimeoutWarningSubscription: Subscription;
  onTimeoutSubscription: Subscription;
  onIdleEndSubscription: Subscription;
  userId;
  appUserId: number;
  displayName: string;

  constructor(private element: ElementRef, public alertService: AlertService, private persistanceService: PersistanceService, public ng4LoadingSpinnerService: Ng4LoadingSpinnerService,
    private dataService: DataService, private config: APIConfiguration, private idle: Idle, private ngbModal: NgbModal, private http: HttpClient,
    //private socket: Socket, 
  ) {
    this.appUserId = this.persistanceService.get(this.config.loggedInUserKey)
    this.displayName = this.persistanceService.get("displayname");
    // this.joinRoom();
    // this.InitializeSocketListerners();

    // sets an idle timeout of 10 minutes.
    idle.setIdle(900);
    // sets a timeout period of 5 minutes.
    idle.setTimeout(300);
    // sets the interrupts like Keydown, scroll, mouse wheel, mouse down, and etc
    idle.setInterrupts([
      new EventTargetInterruptSource(
        this.element.nativeElement, 'keydown DOMMouseScroll mousewheel mousedown touchstart touchmove scroll')]);

    this.onIdleEndSubscription = idle.onIdleEnd.subscribe(() => {
      this.idleState = 'NO_LONGER_IDLE';
    });

    this.onTimeoutSubscription = idle.onTimeout.subscribe(() => {
      this.idleState = 'TIMED_OUT';
      this.timedOut = true;
      this.closeProgressForm();
    });

    this.onIdleStartSubscription = idle.onIdleStart.subscribe(() => {
      this.idleState = 'IDLE_START', this.openProgressForm(1);
    });

    this.onTimeoutSubscription = idle.onTimeoutWarning.subscribe((countdown: any) => {
      this.idleState = 'IDLE_TIME_IN_PROGRESS';
      this.progressBarPopup.componentInstance.count = (Math.floor((countdown - 1) / 60) + 1);
      this.progressBarPopup.componentInstance.progressCount = this.reverseNumber(countdown);
      this.progressBarPopup.componentInstance.countMinutes = (Math.floor(countdown / 60));
      this.progressBarPopup.componentInstance.countSeconds = countdown % 60;
    });

    // sets the ping interval to 15 seconds
    //keepalive.interval(15);
    /**
     *  // Keepalive can ping request to an HTTP location to keep server session alive
     * keepalive.request('<String URL>' or HTTP Request);
     * // Keepalive ping response can be read using below option
     * keepalive.onPing.subscribe(response => {
     * // Redirect user to logout screen stating session is timeout out if if response.status != 200
     * });
     */

    this.reset();
  }

  ngOnDestroy() {
    this.resetTimeOut();

  }

  reverseNumber(countdown: number) {
    return (300 - (countdown - 1));
  }

  reset() {
    this.idle.watch();
    this.idleState = 'Started.';
    this.timedOut = false;
  }

  openProgressForm(count: number) {
    this.progressBarPopup = this.ngbModal.open(ProgressbarmodalComponent, {
      backdrop: 'static',
      keyboard: false
    });
    this.progressBarPopup.componentInstance.count = count;
    this.progressBarPopup.result.then((result: any) => {
      if (result !== '' && 'logout' === result) {
        this.logout();
      } else {
        this.reset();
      }
    });
  }

  logout() {
    //this.resetTimeOut();
    this.persistanceService.logoutUser();
  }

  closeProgressForm() {
    this.progressBarPopup.close('logout');
  }

  resetTimeOut() {
    this.idle.stop();
    if (this.onIdleStartSubscription)
      this.onIdleStartSubscription.unsubscribe();
    if (this.onTimeoutSubscription)
      this.onTimeoutSubscription.unsubscribe();
    if (this.onTimeoutWarningSubscription)
      this.onTimeoutWarningSubscription.unsubscribe();
    if (this.onIdleEndSubscription)
      this.onIdleEndSubscription.unsubscribe();
  }


  ngOnInit() {
    if (this.ngbModal.hasOpenModals() == true)
      this.ngbModal.dismissAll();
    this.persistanceService.userLoggedIn.subscribe(res => this.isUserLoggedIn = res);

    if (JSON.parse(localStorage.getItem("userToken")) != null) {
      this.isUserToken = true;
    }
    else {
      this.isUserLoggedIn = false;
      this.isUserToken = false;
    }

  }

  startLoadingSpinner() {
    this.ng4LoadingSpinnerService.show();
    setTimeout(function () {
      this.ng4LoadingSpinnerService.hide();
    }.bind(this), 4000);
  }
  public messageSeen(event: any) {
    console.log(event);
  }
  // @HostListener('window:beforeunload', ['$event'])
  // beforeunloadHandler(event) {
  //   this.persistanceService.logoutUser();
  // }
  /*
  public joinRoom(): void 
  {
    let chatObj={username:this.displayName.substring(1,this.displayName.length-1),id:this.appUserId};
    this.socket.emit("join", chatObj);
  }
  public InitializeSocketListerners(): void
  {
    this.socket.on("generatedUserId", (userId) => {
      this.dataService.get<any>(this.config.Emar_Chat_UpdateSocketId+userId+"/"+this.appUserId)
      .subscribe(res=>{
        this.adapter=new ChatAdapterClass(userId,this.dataService,this.config,this.http,this.appUserId,this.socket);
        this.userId=userId;
      },error=>{});
      
    });
  }
  */
}

