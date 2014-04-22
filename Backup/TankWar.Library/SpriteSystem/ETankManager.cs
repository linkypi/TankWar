using System;
using System.Collections.Generic;
using System.Text;
using TankWar.Library.MapSystem;
using System.Drawing;

namespace TankWar.Library.SpriteSystem
{
    public class ETankManager
    {
        /// <summary>
        /// 对多可以生成的坦克数量
        /// </summary>
        public const int MAX_TANK_NUM = 5;

        private List<Tank> eTankList = null;

        public List<Tank> ETankList
        {
            get { return eTankList; }
            set { eTankList = value; }
        }

        private Map inMap;

        public Map InMap
        {
            get { return inMap; }
            set { inMap = value; }
        }



        public ETankManager(Map map)
        {
            this.eTankList = new List<Tank>();
            this.InMap = map;
            this.AddNewETank();
        }



        public void AddNewETank()
        {
            if (this.ETankList.Count >= MAX_TANK_NUM) return;

            ETank tank = new ETank(this.InMap);
            this.ETankList.Add(tank);

            this.InMap.Sprites.Add(tank);
        }

        public void DrawETanks(Graphics g)
        {
            RemoveDiedTanks();

            foreach (ETank tank in ETankList)
            {
                tank.Move();
            }

            foreach (ETank tank in ETankList)
            {
                tank.Draw(g);
            }
        }

        private void RemoveDiedTanks()
        {
            for (int i = ETankList.Count - 1; i >= 0; i--)
            {
                Tank tank = ETankList[i];
                if (tank.HP <= 0)
                {
                    tank.Visible = false;
                    tank.Active = false;
                    ETankList.Remove(tank);
                }
            }
        }
    }
}
