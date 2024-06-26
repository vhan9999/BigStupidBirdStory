import { _decorator, Component, ModelRenderer, Node, Sprite, Vec3 } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('test')
export class test extends Component {
    @property
    priority;

    start() {
        this.node.setSiblingIndex(this.priority);
    }

    update(deltaTime: number) {
        
    }
}


