import { Injectable } from '@angular/core';
import {StoreService} from '../shared/store.service';
@Injectable()
export class TreenodeService {

  constructor(private _store:StoreService) { }
  loadTreeNodes(root){
    if(root.url) {
      this._store.dispatchAction({key: root.key, url: root.url, name: 'LOAD_NODES'});
    }
  }
}
