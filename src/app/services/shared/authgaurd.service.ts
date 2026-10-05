import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { PersistanceService } from './persistance.service'
@Injectable()
export class AuthgaurdService implements CanActivate {

  constructor(private persistanceService: PersistanceService) { }

  canActivate() {
    if (this.persistanceService.isUserLoggedIn) {
      return true;
    }
    else {
      this.persistanceService.clear();
      return false;
    }
  }
}
