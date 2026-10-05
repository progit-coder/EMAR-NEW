import {TreeNode} from '../models/treenode';
export const treeNodeReducer = (state: any = [], action) => {
  switch (action.name) {
    case 'LOAD_NODES':
      return state.nodes.map(n => {
        return new TreeNode(n.key,n.url,n.name);
      });
  }
};