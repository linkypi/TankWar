using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TankWar.Library.MapSystem;

namespace TankWar.Library.SpriteSystem
{
    public class Bullet:Sprite
    {
        public const int BULLET_DEFAULT_SPEED = 10;


        private Tank tank;

        public Tank Tank
        {
            get { return tank; }
            set { tank = value; }
        }


        private Explosion explosion;

        public Explosion Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }

        private int actionPower;

        public int ActionPower
        {
            get { return actionPower; }
            set { actionPower = value; }
        }


        public override int FrameMax
        {
            get
            {
                return 0;
            }
        }

        public Bullet(Tank tank):base("bullet.bmp",16,16)
        {
            this.tank = tank;
            this.Explosion = null;
            this.Attribute = SpriteAttribute.None;
            this.ActionPower = 2;

            this.BoundCollisioned += new CheckBoundCollisionHandler(Bullet_BoundCollisioned);
            this.CellCollisioned += new CheckCellCollisionHandler(Bullet_CellCollisioned);
        }

        public void ActionTank(Tank tank)
        {
            this.FireExplosion(tank.Rect);
            tank.HP -= this.ActionPower;
        }

        void Bullet_CellCollisioned(Cell cell)
        {
            if (cell.TileAttribute == TileAttribute.CanBreak || cell.TileAttribute == TileAttribute.Flag)
            {
                this.FireExplosion(cell.Rectangle);
                cell.Disponsed = true;
                this.InMap.DrawBuffer();

                // 如果是军旗，就要发出游戏结束的消息
                if (cell.TileAttribute == TileAttribute.Flag)
                {
                    GameMsg.SendMsg("游戏结束，你输了");
                }

                return;
            }
            
            if (cell.TileAttribute == TileAttribute.None)
            {
                this.FireExplosion(cell.Rectangle);
                DestroyBullet();
                return;
            }
        }

        void Bullet_BoundCollisioned()
        {
            DestroyBullet();
        }

        private void DestroyBullet()
        {
            this.Visible = false;
            this.Active = false;
            this.tank.BulletNum--;
        }

        public void FireExplosion(Rectangle rect)
        {
            if (this.tank != null)
            {
                this.Explosion = new Explosion();
                LocateExplosion(rect);
            }
        }

        /// <summary>
        /// 将爆炸置于矩形区域中心
        /// </summary>
        /// <param name="rect"></param>
        public void LocateExplosion(Rectangle rect)
        {
            if (this.Explosion == null) return;

            int cx = rect.Left + rect.Width / 2;
            int cy = rect.Top + rect.Height / 2;

            this.Explosion.CenterBy(cx, cy);
        }

        public override void Move()
        {
            if(this.Explosion==null)
            {
                base.Move();
            }
            else
            {
                this.Explosion.Move();
                if (this.Explosion.FrameNo >= this.Explosion.FrameMax)
                {
                    DestroyBullet();
                }
            }
        }

        public override bool CheckSpriteCollisions(List<Sprite> sprites)
        {
            bool isChecked = false;

            foreach (Sprite sprite in sprites)
            {
                if (sprite is Tank)
                {
                    isChecked = CheckTankCollisions(sprite);
                }

                if (sprite is Bullet)
                {
                    isChecked = CheckBulletCollisions((Bullet)sprite);
                }
            }

            return isChecked;
        }

        private bool CheckBulletCollisions(Bullet b)
        {
            bool isChecked = false;

            Rectangle tr = new Rectangle(b.X, b.Y, b.Width, b.Height);
            Rectangle sr = new Rectangle(this.X, this.Y, this.Width, this.Height);

            if (tr.IntersectsWith(sr))
            {
                // 如果是对方子弹
                if (this.tank.GetType() != b.tank.GetType())
                {
                    this.FireExplosion(b.Rect);
                    b.FireExplosion(this.Rect);
                }

                isChecked = true;
            }

            return isChecked;
        }

        private bool CheckTankCollisions(Sprite tank)
        {
            bool isChecked = false;

            // 子弹和坦克的碰撞

            Rectangle tr = new Rectangle(tank.X, tank.Y, tank.Width, tank.Height);
            Rectangle sr = new Rectangle(this.X, this.Y, this.Width, this.Height);

            if (tr.IntersectsWith(sr))
            {

                // 如果是敌方的坦克
                if (this.Tank.GetType() != tank.GetType())
                {
                    this.ActionTank((Tank)tank);
                }

                isChecked = true;
            }



            return isChecked;
        }

        public override void Draw(Graphics g)
        {
            if (this.Explosion == null)
            {
                base.Draw(g);
            }
            else
            {
                this.Explosion.Draw(g);
            }
        }

        
    }
}
