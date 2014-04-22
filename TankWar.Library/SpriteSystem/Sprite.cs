using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TankWar.Library.MapSystem;

namespace TankWar.Library.SpriteSystem
{
    /// <summary>
    /// 精灵属性
    /// </summary>
    public enum SpriteAttribute
    {
        UndirectionalBitmap,        // 图形没有方向性
        NoCellCollision,            // 不和单元格碰撞
        NoTankCollision,            // 不和坦克碰撞
        NoBulletCollision,          // 不和子弹碰撞
        None                        // 具有以上四种属性
    }

    /// <summary>
    /// 动画绘制模式
    /// </summary>
    public enum AdvanceFrameMode
    {
        /// <summary>
        /// 动画结束后重新开始
        /// </summary>
        Wrap,

        /// <summary>
        /// 动画结束后停止
        /// </summary>
        Stop
    }

    /// <summary>
    /// 精灵移动方向
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }


    public delegate void CheckCellCollisionHandler(Cell cell);
    public delegate void CheckBoundCollisionHandler();


    public class Sprite
    {
        #region 常量

        public const int SPRITE_DEFAULT_SPEED = 4;

        public static readonly Color SPRITE_TRANSPARENT_COLOR = Color.Black;

        public const int SPRITE_DEFAULT_WIDTH = 32;
        public const int SPRITE_DEFAULT_HEIGHT = 32;

        #endregion


        public event CheckCellCollisionHandler CellCollisioned;
        public event CheckBoundCollisionHandler BoundCollisioned;



        private int x;

        /// <summary>
        /// 角色位置：X坐标
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        private int y;

        /// <summary>
        /// 角色位置：Y坐标
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        private int frameNo;

        /// <summary>
        /// 动画的当前帧编号
        /// </summary>
        public int FrameNo
        {
            get { return frameNo; }
            set { frameNo = value; }
        }

        
        private int speed;

        /// <summary>
        /// 移动速度
        /// </summary>
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private int collisionCount;

        /// <summary>
        /// 连续碰撞次数
        /// </summary>
        public int CollisionCount
        {
            get { return collisionCount; }
            set { collisionCount = value; }
        }

 

        private bool active;

        /// <summary>
        /// 是否进行动作
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private bool visible;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }





        /// <summary>
        /// 占用的矩形区域
        /// </summary>
        public Rectangle Rect
        {
            get 
            { 
                return new Rectangle(this.X,this.Y,this.Width,this.Height); 
            }
        }

        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private Direction direction;

        /// <summary>
        /// 移动方向
        /// </summary>
        public Direction Direction
        {
            get { return direction; }
            set 
            { 
                direction = value;
                // 改变方向后，连续碰撞次数清空
                this.CollisionCount++;
            }
        }

        private SpriteAttribute attribute;

        /// <summary>
        /// 属性
        /// </summary>
        public SpriteAttribute Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }

        private AdvanceFrameMode advanceFrameMode;

        /// <summary>
        /// 动画结束后的处理方式
        /// </summary>
        public AdvanceFrameMode AdvanceFrameMode
        {
            get { return advanceFrameMode; }
            set { advanceFrameMode = value; }
        }

        private Bitmap bits;

        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap Bits
        {
            get { return bits; }
            set { bits = value; }
        }

        private int centreX;

        /// <summary>
        /// 中心X坐标
        /// </summary>
        public int CentreX
        {
            get { return centreX; }
            set { centreX = value; }
        }

        private int centreY;

        /// <summary>
        /// 中心Y坐标
        /// </summary>
        public int CentreY
        {
            get { return centreY; }
            set { centreY = value; }
        }

        private Map inMap;

        public Map InMap
        {
            get { return inMap; }
            set { inMap = value; }
        }


        /// <summary>
        /// 动画帧的最大数量
        /// </summary>
        public virtual int FrameMax
        {
            get
            {
                return 1;
            }
        }

        public Sprite()
        {
            // 初始化
            Init("", null,SPRITE_DEFAULT_WIDTH,SPRITE_DEFAULT_HEIGHT);

        }

        public Sprite(string fileName)
        {
            // 初始化
            Init(fileName, null, SPRITE_DEFAULT_WIDTH, SPRITE_DEFAULT_HEIGHT);

        }

        public Sprite(string fileName,int width,int height)
        {
            // 初始化
            Init(fileName, null,width,height);

        }

        public Sprite(string fileName,Map map)
        {
            // 初始化
            Init(fileName, map, SPRITE_DEFAULT_WIDTH, SPRITE_DEFAULT_HEIGHT);
            
        }

        public virtual void Draw(Graphics g)
        {
            // 如果不可见,则不绘制
            if(!Visible) return;

            // 动画帧编号不合法则不绘制
            if(FrameNo>FrameMax || FrameNo<0) return;

            // 将精灵绘出
            // 要考虑方向性

            Bitmap bmp = new Bitmap(this.Width, this.Height);
            Graphics tg = Graphics.FromImage(bmp);

            Rectangle sr = new Rectangle((int)this.Direction * this.Width, this.FrameNo * this.Height, this.Width, this.Height);
            Rectangle dr = new Rectangle(0, 0, this.Width, this. Height);

            tg.DrawImage(this.Bits, dr, sr, GraphicsUnit.Pixel);

            g.DrawImage(bmp, this.X, this.Y);
        }

        /// <summary>
        /// 边界碰撞检测
        /// </summary>
        /// <returns></returns>
        public bool CheckBoundCollisions()
        {
            int orgX = this.X;
            int orgY = this.Y;

            // 是否超过上方
            if(this.Y < 0)
            {
                this.Y = 0;
            }

            // 是否超过左方
            if(this.X < 0 )
            {
                this.X = 0;
            }

            // 是否超过右方
            if(this.X+this.Width>inMap.Width)
            {
                this.X = inMap.Width - this.Width;
            }

            // 是否超过下方
            if(this.Y+this.Height>inMap.Height)
            {
                this.Y = inMap.Height-this.Height;
            }


            if (orgX != this.X || orgY != this.Y)
            {
                if (this.BoundCollisioned != null)
                {
                    this.BoundCollisioned();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 地形物的碰撞检测
        /// </summary>
        /// <returns></returns>
        public bool CheckCellCollisions()
        {

            int col = x / Tile.TILE_WIDTH;
            int row = y / Tile.TILE_HEIGHT;

            bool isChecked = false;

            Cell cell = null;

            // 是否和当前所在单元格碰撞
            cell = this.InMap[1, row, col];
            isChecked = IsCellCollision(cell);

            if (!isChecked)
            {

                if (this.Direction == Direction.Up)
                {
                    cell = this.InMap[1, row - 1, col];
                    isChecked = IsCellCollision(cell);
                }

                if (this.Direction == Direction.Down)
                {
                    cell = this.InMap[1, row + 1, col];
                    isChecked = IsCellCollision(cell);
                }

                if (this.Direction == Direction.Left)
                {
                    cell = this.InMap[1, row, col - 1];
                    isChecked = IsCellCollision(cell);
                }

                if (this.Direction == Direction.Right)
                {
                    cell = this.InMap[1, row, col + 1];
                    isChecked = IsCellCollision(cell);
                }

            }

            if (isChecked)
            {
                if (CellCollisioned != null)
                {
                    this.CellCollisioned(cell);
                }
            }

            return isChecked;
        }

        /// <summary>
        /// 精灵的碰撞检测
        /// 需要子类重写，实现具体的碰撞逻辑
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckSpriteCollisions(List<Sprite> sprites)
        {
            return false;
        }

        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckCollisions()
        {
            bool isChecked = false;

            if(this.attribute != SpriteAttribute.NoCellCollision)
            {
                if (CheckCellCollisions())
                {
                    isChecked = true;
                }
            }

            if (CheckBoundCollisions())
            {
                isChecked = true;
            }

            if (CheckSpriteCollisions(this.InMap.Sprites))
            {
                isChecked = true;
            }

            return isChecked;
 
        }

        public bool IsYanchi = false;

        public int CountOfYanChi = 0;

        private int count = 0;

        public virtual void Move()
        {

            // 不动的不处理
            if(!Active) return;

            // 不只一副动画图片
            if(FrameMax>0)
            {
                if (IsYanchi)
                {
                    if (count < CountOfYanChi)
                    {
                        count++;
                    }
                    else
                    {
                        count = 0;
                        FrameNo++;
                    }
                }
                else
                {
                    FrameNo++;
                }


                // 已经绘制过一轮了
                if(FrameNo>FrameMax)
                {
                    // 重新开始
                    if(this.AdvanceFrameMode == AdvanceFrameMode.Wrap)
                    {
                        this.FrameNo = 0;
                    }
                    else
                    {
                        // 保持在最后一张动画
                        // 并停止不动
                        this.FrameNo = this.FrameMax;
                        this.Active = false;
                    }
                }
            }


            if(Speed<=0) return;

            switch(Direction)
            {
                case Direction.Up:
                    this.Y -= this.Speed;
                    break;

                case Direction.Down:
                    this.Y += this.Speed;
                    break;

                case Direction.Left:
                    this.X -= this.Speed;
                    break;

                case Direction.Right:
                    this.X += this.Speed;
                    break;
            }

            CheckCollisions();

            
            
        }

        /// <summary>
        /// 随机设置移动方向
        /// </summary>
        public void RandomDirection()
        {
            Random r = new Random();
            int d = r.Next(0, 4);
            this.Direction = (Direction)d;
        }

        /// <summary>
        /// 随机设置精灵的位置
        /// </summary>
        public void RandomPosition()
        {
            int Width = Map.MAP_COLS * Tile.TILE_WIDTH - Sprite.SPRITE_DEFAULT_WIDTH;
            int Height = Map.MAP_ROWS * Tile.TILE_HEIGHT - Sprite.SPRITE_DEFAULT_HEIGHT;

            Random r = new Random();

            this.X = r.Next(0, Width);
            this.Y = r.Next(0, Height);
        }

        /// <summary>
        /// 与另一个角色中间对齐
        /// </summary>
        /// <param name="sprite"></param>
        public void CenterWith(Sprite sprite)
        {
            int cx = sprite.Rect.Left + sprite.Rect.Width / 2;
            int cy = sprite.Rect.Top + sprite.Rect.Height / 2;

            CenterBy(cx, cy);
        }

        public  void CenterBy(int x, int y)
        {
            this.X = x - this.Rect.Width / 2;
            this.Y = y - this.Rect.Height / 2;
        }



        /// <summary>
        /// 角色是否和某一个单元格物体碰撞了
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool IsCellCollision(Cell cell)
        {
            if (cell == null) return false;

            if (cell.Disponsed) return false;

            bool isChecked = false;

            if (cell.Tile.Attribute != TileAttribute.CanPasse)
            {
                if (this.Rect.IntersectsWith(cell.Rectangle))
                {
                    isChecked = true;
                }
            }

            return isChecked;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="fileName"></param>
        private void Init(string fileName,Map map,int width,int height)
        {
            this.X = 0;
            this.Y = 0;
            this.FrameNo = 0;


            this.Active = true;
            this.visible = true;


            this.AdvanceFrameMode = AdvanceFrameMode.Wrap;
            this.Speed = SPRITE_DEFAULT_SPEED;


            // 角色的 bitmap
            this.Bits = new Bitmap(fileName);
            this.Bits.MakeTransparent(SPRITE_TRANSPARENT_COLOR);


            this.Width = width;
            this.Height = height;

            this.InMap = map;
        }
    }
}
