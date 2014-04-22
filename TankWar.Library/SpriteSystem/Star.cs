using System;
using System.Collections.Generic;
using System.Text;

namespace TankWar.Library.SpriteSystem
{
    public class Star:Sprite
    {
        public override int FrameMax
        {
            get
            {
                return 2;
            }
        }

        public Star()
            : base("STAR.BMP")
        {
            this.Speed = 0;
            this.Attribute = SpriteAttribute.UndirectionalBitmap;
            this.AdvanceFrameMode = AdvanceFrameMode.Stop;

            this.IsYanchi = true;
            this.CountOfYanChi = 3;
        }
    }
}
