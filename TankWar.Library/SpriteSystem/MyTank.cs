using System;
using System.Collections.Generic;
using System.Text;
using TankWar.Library.MapSystem;

namespace TankWar.Library.SpriteSystem
{
    public class MyTank:Tank
    {
 
        public MyTank(Map map)
            : base("MYTANK.BMP",map)
        {
            this.Direction = Direction.Up;
            this.Attribute = SpriteAttribute.None;
            this.HP = 10;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            if (this.HP <= 0)
            {
                this.Visible = false;
                this.Active = false;
            }

            base.Draw(g);
        }

    }
}
