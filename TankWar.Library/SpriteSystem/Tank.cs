using System;
using System.Collections.Generic;
using System.Text;
using TankWar.Library.MapSystem;
using System.Drawing;

namespace TankWar.Library.SpriteSystem
{
    public class Tank:Sprite
    {
        private List<Bullet> bulletList = null;

        private int hP;

        /// <summary>
        /// 生命值
        /// </summary>
        public int HP
        {
            get { return hP; }
            set { hP = value; }
        }

        private int score;

        /// <summary>
        /// 被摧毁后主角的得分
        /// </summary>
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        private int bulletNum;

        /// <summary>
        /// 当前子弹数量
        /// </summary>
        public int BulletNum
        {
            get { return bulletNum; }
            set { bulletNum = value; }
        }

        private int maxBulletNum;

        /// <summary>
        /// 同一时间子弹数量上限
        /// </summary>
        public int MaxBulletNum
        {
            get { return maxBulletNum; }
            set { maxBulletNum = value; }
        }


        private bool superMode;

        /// <summary>
        /// 无敌模式
        /// </summary>
        public bool SuperMode
        {
            get { return superMode; }
            set { superMode = value; }
        }


        public Tank(string fileName)
            : base(fileName, null)
        {
            Init();
        }


        public Tank(string fileName, Map map)
            : base(fileName, map)
        {
            Init();
        }

        public override bool CheckSpriteCollisions(List<Sprite> sprites)
        {
            bool isChecked = false;

            for (int i = sprites.Count - 1; i >= 0;i-- )
            {
                Sprite sprite = sprites[i];
                if (sprite is Tank)
                {
                    isChecked = CheckTankCollisions(sprite);
                }
            }

            return isChecked;
        }

        private bool CheckTankCollisions(Sprite tank)
        {
            bool isChecked = false;

            // 坦克和坦克的碰撞

            if (tank == this) return false;

            Rectangle tr = new Rectangle(tank.X, tank.Y, tank.Width, tank.Height);
            Rectangle sr = new Rectangle(this.X, this.Y, this.Width, this.Height);

            if (tr.IntersectsWith(sr))
            {

                // 如果是同一类坦克
                if (tank.GetType() == this.GetType())
                {
                    ChangeTankDirection(tank);

                    ChangeTankDirection(this);
                }
                else
                {
                    //if (this is ETank)
                    //{
                    //    this.FireBullet();
                    //    TankStop(this);
                    //}
                    //else
                    //{
                    //    TankStop(this);
                    //}
                }
                

                isChecked = true;
            }

            

            return isChecked;
        }

        private void TankStop(Sprite tank)
        {
            switch (tank.Direction)
            {
                case Direction.Up:
                    this.Y += this.Speed;
                    break;

                case Direction.Down:
                    this.Y -= this.Speed;
                    break;

                case Direction.Left:
                    this.X += this.Speed;
                    break;

                case Direction.Right:
                    this.Y -= this.Speed;
                    break;
            }
        }

        private static void ChangeTankDirection(Sprite tank)
        {
            switch (tank.Direction)
            {
                case Direction.Up:
                    tank.Direction = Direction.Down;
                    break;

                case Direction.Down:
                    tank.Direction = Direction.Up;
                    break;

                case Direction.Left:
                    tank.Direction = Direction.Right;
                    break;

                case Direction.Right:
                    tank.Direction = Direction.Left;
                    break;
            }
        }

        public override void Draw(Graphics g)
        {
            
            MoveBullets();

            DrawBullets(g);

            base.Draw(g);
        }

        private void MoveBullets()
        {
            RemoveBullets();

            foreach (Bullet bullet in bulletList)
            {
                bullet.Move();
            }
        }


        private void DrawBullets(Graphics g)
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.Draw(g);
            }
        }

        

        private void RemoveBullets()
        {
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                Bullet bullet = bulletList[i];

                if (!bullet.Active)
                {
                    this.bulletList.Remove(bullet);
                }
            }
        }


        public void FireBullet()
        {
            
            // 当前子弹数量达到上限了
            if(this.BulletNum>this.MaxBulletNum) return;

            
            Bullet bullet = new Bullet(this);
            bullet.Direction = this.Direction;
            bullet.Speed = Bullet.BULLET_DEFAULT_SPEED;
            bullet.CenterWith(this);
            bullet.InMap = this.InMap;

            this.BulletNum++;

            this.bulletList.Add(bullet);

            // 放入碰撞检测精灵表
            this.InMap.Sprites.Add(bullet);

        }

        private void Init()
        {
            this.MaxBulletNum = 1;
            this.BulletNum = 0;

            this.superMode = false;

            this.bulletList = new List<Bullet>();

            this.CellCollisioned += new CheckCellCollisionHandler(Tank_CellCollisioned);
        }

        void Tank_CellCollisioned(Cell cell)
        {
            if (!cell.CanPase)
            {
                switch (this.Direction)
                {
                    case Direction.Up:
                        this.Y += this.Speed;
                        break;

                    case Direction.Down:
                        this.Y -= this.Speed;
                        break;

                    case Direction.Left:
                        this.X += this.Speed;
                        break;

                    case Direction.Right:
                        this.X -= this.Speed;
                        break;
                }
            }
        }

        
    }
}
