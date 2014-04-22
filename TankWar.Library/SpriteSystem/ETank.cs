using System;
using System.Collections.Generic;
using System.Text;
using TankWar.Library.MapSystem;
using System.Drawing;

namespace TankWar.Library.SpriteSystem
{
    
    public class ETank:Tank
    {
        /// <summary>
        /// 地方坦克射击子弹的概率
        /// </summary>
        public const double PROBAB_ETANK_SHOT_BULLET = 0.1;

        /// <summary>
        /// 地方坦克连续碰撞次数上限
        /// </summary>
        public const int MAX_ETANK_COLLISION_COUNT = 5;

        private Star star = null;

        public ETank(Map map):base("ETANK.BMP",map)
        {
            this.Direction = Direction.Down;
            this.Attribute = SpriteAttribute.None;
            this.HP = 5;


            // 出现的地点：左上角，正上方，右上角

            this.Y = 0;

            Random r = new Random();
            int pos = r.Next(3);

            switch(pos)
            {
                case 0:
                    this.X = 0;
                    break;

                case 1:
                    this.X = (InMap.Width - this.Width)/2;
                    break;

                case 2:
                    this.X = InMap.Width - this.Width;
                    break;
            }

            // 先利用Star类制造金光闪闪的效果
            this.star = new Star();
            this.star.CenterWith(this);

            this.CellCollisioned += new CheckCellCollisionHandler(ETank_CellCollisioned);
            this.BoundCollisioned += new CheckBoundCollisionHandler(ETank_BoundCollisioned);
        }

        void ETank_BoundCollisioned()
        {
            // 如果有碰撞就记录连续碰撞次数
            this.CollisionCount++;
        }

        void ETank_CellCollisioned(Cell cell)
        {
            // 如果有碰撞就记录连续碰撞次数
            this.CollisionCount++;
        }

        public override void Move()
        {
            // 如果金光闪闪的效果还没有结束
            if (this.star != null)
            {
                this.star.Move();
                if (!this.star.Active)
                {
                    this.star = null;
                }
            }
            else
            {
                // 随机发射子弹
                Random r = new Random();
                if (r.NextDouble() < PROBAB_ETANK_SHOT_BULLET)
                {
                    this.FireBullet();
                }

                // 如果连续碰撞次数太多，就要转向
                if (CollisionCount > MAX_ETANK_COLLISION_COUNT)
                {
                    this.RandomDirection();
                    this.CollisionCount = 0;
                }
            }


            base.Move();
        }

        public override void Draw(Graphics g)
        {
            if (this.star == null)
            {
                base.Draw(g);
            }
            else
            {
                this.star.Draw(g);
            }
        }
    }
}
