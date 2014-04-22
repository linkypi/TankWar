using System;
using System.Collections.Generic;
using System.Text;

namespace TankWar.Library.SpriteSystem
{
    public class Explosion:Sprite
    {


        public override int FrameMax
        {
            get
            {
                return 4;
            }
        }

        public Explosion()
            : base("EXP.BMP")
        {

            this.Speed = 0;

            this.AdvanceFrameMode = AdvanceFrameMode.Stop;

            this.Attribute = SpriteAttribute.None;
        }

        
    }
}
