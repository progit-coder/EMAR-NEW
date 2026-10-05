import { Component, OnInit, Input } from '@angular/core';
import { TreeNode } from '../../../models/treenode';
import { StoreService } from '../../../services/shared/store.service';
import { TreenodeService } from '../../../services/shared/treenode.service';
@Component({
  selector: 'app-treeview',
  templateUrl: './treeview.component.html',
  styleUrls: ['./treeview.component.css'],
  providers:[StoreService,TreenodeService]
})
export class TreeviewComponent implements OnInit {

  @Input() root:TreeNode;
  children:any;
  items = [];
  subscription;
  constructor(private _store:StoreService, private _treeNodeService:TreenodeService) { }

  ngOnInit() {
    this.subscription = this._store.getTreeNodes(this.root.key).subscribe(res => {
      this.items = res;
    });
    this._treeNodeService.loadTreeNodes(this.root);
  }

}
