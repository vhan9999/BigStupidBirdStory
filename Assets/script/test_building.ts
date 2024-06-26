import { _decorator, Component, Node, Event, Vec2, director, Collider2D, Camera, find, UITransform, PolygonCollider2D, Intersection2D} from 'cc';
import { grid_manage } from './grid_manage';
const { ccclass, property } = _decorator;

@ccclass('test_building')
export class test_building extends Component {

    

    @property({ type: Node })
    public targetNode: Node = null;

    private gridmanager: grid_manage = null;
    private child: Node = null;

    private isEdit : boolean = false;//is edit

    private tempPos : Vec2;//pos before move
    private startPoint : Vec2;// click down
    private endPoint : Vec2;// click up

    private cameraNode: Node;
    private camera: Camera;

    start() {
        this.cameraNode = find('Canvas/Camera');
        this.camera = this.cameraNode.getComponent(Camera);

        this.tempPos = new Vec2(this.node.position.x, this.node.position.y);
        this.node.on(Node.EventType.TOUCH_START, this.onTouchStart, this);
        this.node.on(Node.EventType.TOUCH_MOVE, this.onTouchMove, this);
        this.node.on(Node.EventType.TOUCH_END, this.onTouchEnd, this);
        this.node.on(Node.EventType.TOUCH_CANCEL, this.onTouchCancel, this);

        this.gridmanager = this.targetNode.getComponent(grid_manage);
        this.child = this.node.getChildByName("grid");
    }

    // @ts-ignore
    onTouchStart(event: EventTouch) {
        this.startPoint = event.getLocation();
        event.preventSwallow = true;//can penetration click
    }

    // @ts-ignore
    onTouchMove(event: EventTouch) {
        let touchLocation : Vec2 = event.getLocation();
        if(this.isEdit){
            event.preventSwallow = false;
            let nowPosFlag = this.tempPos.clone();//avoid to change "tempPos"

            let differenceReal : Vec2 = touchLocation.subtract(this.startPoint);
            let differenceGrid = this.gridmanager.RealToGrid(differenceReal.x, differenceReal.y);
            differenceGrid = new Vec2(Math.trunc(differenceGrid.x), Math.trunc(differenceGrid.y));//remove decimal

            let newPosReal = nowPosFlag.add(this.gridmanager.GridToReal(differenceGrid.x,differenceGrid.y));
            if(!newPosReal.equals(new Vec2(this.node.position.x, this.node.position.y))){
                this.node.setPosition(newPosReal.x,newPosReal.y);
            }
        }
        else{
            event.preventSwallow = true;
            return;
        }
    }

    // @ts-ignore
    onTouchEnd(event: EventTouch) {
        this.endPoint = event.getLocation();
        if(this.isInPolygon(this.endPoint)){
            event.preventSwallow = false;
        }
        else{
            event.preventSwallow = true;
            return;
        }
        this.tempPos = new Vec2(this.node.position.x, this.node.position.y);
        if(this.startPoint.equals(this.endPoint)){
            if(!this.isEdit && !this.gridmanager.isEditing){
                this.child.active = true;
                this.isEdit = true;
                this.gridmanager.isEditing = true;
            }
            else{
                this.child.active = false;
                this.isEdit = false;
                this.gridmanager.isEditing = false;
            }
        }
    }

    // @ts-ignore
    onTouchCancel(event: EventTouch) {
        // 获取触摸取消的位置
        let touchLocation = event.getLocation();
        
        this.tempPos = new Vec2(this.node.position.x, this.node.position.y);
    }

    //whether click in collider
    isInPolygon(clickPoint :Vec2){
        let point = this.camera.screenPointToRay(clickPoint.x, clickPoint.y);
        let nodepos = this.node.getComponent(UITransform).convertToNodeSpaceAR(point.o);
        let Point = new Vec2(nodepos.x, nodepos.y);
        let nodePoints = this.node.getComponent(PolygonCollider2D).points;
        if (Intersection2D.pointInPolygon(Point, nodePoints)) {
            return true;
        }
        else{
            return false;
        }
    }
}


