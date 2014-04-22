using System;
using System.Collections.Generic;
using System.Text;

namespace TankWar.Library.MapSystem
{
    /// <summary>
    /// 一个图片的属性
    /// </summary>
    public enum TileAttribute
    {
        CanPasse,               // 坦克子弹都可通过
        CanBreak,               // 只能被子弹打破
        BulletCanPass,          // 只有子弹可以穿过
        None,                   // 不能通过不能破坏
        Flag                    // 是军旗
        
    }
}
