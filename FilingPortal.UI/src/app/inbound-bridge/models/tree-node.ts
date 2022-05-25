import * as R from 'ramda';

export class TreeNode<T> {
  id: number;
  name: string;
  title: string;
  type: string;
  actions: { [key: string]: boolean } = {};
  parentId: number;
  children: TreeNode<T>[] = [];
  displayAsGrid: boolean = false;
  data: T[] = [];

  get isEmpty(): boolean {
    return R.isNil(this.data) || R.isEmpty(this.data);
  }

  getNode(name: string, id: number): TreeNode<T> {
    if (this.id === id && this.name === name) {
      return this;
    }
    for (let index = 0; index < this.children.length; index++) {
      const node = this.children[index].getNode(name, id);
      if (node) {
        return node;
      }
    }
    return null;
  }

  getData(includeChildrenData: boolean = false): T[] {
    const result = [...this.data];
    if (includeChildrenData) {
      this.children.forEach(c => {
        result.push(... c.getData(true));
      });
    }
    return result;
  }
}
