import { Injectable } from '@angular/core';
import {Subject, Observable} from 'rxjs';
import { treeNodeReducer } from '../../models/reducer';
@Injectable()
export class StoreService {
  private dispatcher = new Subject();
  private treeNodes = {};

  private nodes = {};
  constructor() { 
    this.dispatcher.subscribe((action) => this.handleAction(action));
  }
  handleAction(action)
  {
    if(action.name == 'LOAD_NODES') {
      if (this.nodes[action.key]) {
        this.treeNodes[action.key].next(this.nodes[action.key]);
      }
      else {
         let node=[{key:1,url:"Admit",name:"Admit"},{key:2,url:"Legal",name:"Legal"},{key:3,url:"Contracts",name:"Contracts"}];
            let res={nodes:node};
              this.nodes[action.key] = treeNodeReducer(res, action);
              this.treeNodes[action.key].next(this.nodes[action.key]);        
      }
    }
  }
  getTreeNodes(key){
    if(!this.treeNodes.hasOwnProperty(key)){
      this.treeNodes[key] = new Subject();
    }
    return this.treeNodes[key].asObservable();
  }
  dispatchAction(action){
    this.dispatcher.next(action);
  }
}
