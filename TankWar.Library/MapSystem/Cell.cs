using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace TankWar.Library.MapSystem
{
    public class Cell
    {
        #region 成员变量

        private int layer;
        private int row;
        private int col;
        private Tile tile;
        private bool disponsed;

        #endregion

        #region 成员属性

        public bool Disponsed
        {
            get { return disponsed; }
            set { disponsed = value; }
        }

        public TileAttribute TileAttribute
        {
            get
            {
                return this.tile.Attribute;
            }
            set
            {
                this.tile.Attribute = value;
            }
        }

        public Tile Tile
        {
            get
            {
                return this.tile;
            }
        }

        public Bitmap TileBitMap
        {
            get
            {
                return this.tile.Bits;
            }
        }

        public int X
        {
            get
            {
                return this.col * Tile.TILE_WIDTH;
            }
        }

        public int Y
        {
            get
            {
                return this.row * Tile.TILE_HEIGHT;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                Rectangle rect = new Rectangle(this.X,this.Y,Tile.TILE_WIDTH,Tile.TILE_WIDTH);

                return rect;
            }
        }

        public bool CanPase
        {
            get
            {
                if (TileAttribute == TileAttribute.CanPasse) return true;

                return false;
            }
        }

        #endregion

        #region 构造函数

        public Cell()
        {
            this.tile = new Tile();
        }

        public Cell(int layer, int row, int col, Bitmap bmp)
        {
            InitCell(layer, row, col, new Tile(bmp));
        }

        public Cell(int layer, int row, int col, Tile tile)
        {
            InitCell(layer, row, col, tile);
        }
#endregion

        private void InitCell(int layer, int row, int col, Tile tile)
        {
            this.layer = layer;
            this.row = row;
            this.col = col;
            this.Disponsed = false;

            this.tile = tile;
        }

        /// <summary>
        /// 从数据流中读取图格数据
        /// </summary>
        /// <param name="stream"></param>
        public void Load(Stream input)
        {
            BinaryReader br = new BinaryReader(input);

            this.layer = br.ReadInt32();
            this.row = br.ReadInt32();
            this.col = br.ReadInt32();

            tile.Load(input);
        }

        /// <summary>
        /// 将图格数据写入数据流
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream output)
        {
            BinaryWriter bw = new BinaryWriter(output);

            bw.Write(this.layer);
            bw.Write(this.row);
            bw.Write(this.col);

            this.tile.Save(output);
        }

        public void Draw(Graphics g)
        {
            if (this.Disponsed) return;

            this.Tile.Draw(g, this.X, this.Y);

        }
    }
}
