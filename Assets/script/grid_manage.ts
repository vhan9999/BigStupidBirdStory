import { _decorator, CCObject, Component, log, Node, Vec2 } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('grid_manage')
export class grid_manage extends Component {
    
    isEditing = false;//whether player edit a building
    CELL_WIDTH = 112;
    CELL_HEIGHT = 56;
    
    GridToReal(gridX: number, gridY: number): Vec2 {
        const x = (gridX - gridY) * (this.CELL_WIDTH / 2);
        const y = (gridX + gridY) * (this.CELL_HEIGHT / 2);
        return new Vec2(x, y);
    }

    RealToGrid(X: number, Y: number): Vec2 {
        const gridX = ((X / (this.CELL_WIDTH / 2)) + (Y / (this.CELL_HEIGHT / 2))) / 2
        const gridY = ((Y / (this.CELL_HEIGHT / 2)) - (X / (this.CELL_WIDTH / 2))) / 2
        return new Vec2(gridX, gridY);
    }
}


