using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using TankWar.Library.SpriteSystem;

namespace TankWar.Library.MapSystem
{
    public class Map
    {
        #region  常量

        /// <summary>
        /// 地图横向单元格数量
        /// </summary>
        public const int MAP_COLS = 25;

        /// <summary>
        /// 地图纵向单元格数量
        /// </summary>
        public const int MAP_ROWS = 18;

        /// <summary>
        /// 最多到地形物层
        /// </summary>
        public const int MAP_LAYERS = 2;

        /// <summary>
        /// 对多可以生成的精灵数量
        /// </summary>
        public const int MAX_SPRITE_NUM = 5;


        #endregion

        #region 成员变量
        // 地图数据：格式层、行、列
        // 每一层是由18行25列组成的一张地图，一共三层
        private Cell[,,] maps;

        //目前加载的关卡号
        private int levelNo;

        // 角色的起始位置
        private int roleStartX, roleStartY;

        public static readonly Color MAP_TRANSPARENT_COLOR = Color.Black;     // 透明的颜色

        // 图片缓存，提高绘图效率
        // 每一层一个单独的buffer
        private Bitmap[] buffers = new Bitmap[MAP_LAYERS];

        private List<Sprite> sprites = new List<Sprite>();

        #endregion

        #region 成员属性

        public List<Sprite> Sprites
        {
            get { return sprites; }
            set { sprites = value; }
        }

        /// <summary>
        /// 当前关卡号
        /// </summary>
        public int LevelNo
        {
            set
            {
                this.levelNo = value;
            }
            get
            {
                return this.levelNo;
            }
        }

        public int Width
        {
            get
            {
                return MAP_COLS * Tile.TILE_WIDTH;
            }
        }

        public int Height
        {
            get
            {
                return MAP_ROWS * Tile.TILE_HEIGHT;
            }
        }

        /// <summary>
        /// 角色的起始位置的X坐标
        /// </summary>
        public int RoleStartX
        {
            set
            {
                this.roleStartX = value;
            }
            get
            {
                return this.roleStartX;
            }
        }

        /// <summary>
        /// 角色的起始位置的Y坐标
        /// </summary>
        public int RoleStartY
        {
            set
            {
                this.roleStartY = value;
            }
            get
            {
                return this.roleStartY;
            }
        }
        #endregion

        #region  构造函数

        public Map()
        {
            this.maps = new Cell[MAP_LAYERS, MAP_ROWS, MAP_COLS];
            Bitmap image = new Bitmap(Tile.TILE_WIDTH, Tile.TILE_HEIGHT);
            InitMaps(image);
        }

        public Map(Bitmap image)
        {
            maps = new Cell[MAP_LAYERS, MAP_ROWS, MAP_COLS];
            InitMaps(image);
        }
        #endregion

        /// <summary>
        /// 索引器，获取或者设置第layer层，第row行，第col列的单元格对象
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Cell this[int layer, int row, int col]
        {
            get
            {
                if (layer < 0 || layer >= MAP_LAYERS) return null;

                if (row < 0 || row >= MAP_ROWS) return null;

                if (col < 0 || col >= MAP_COLS) return null;

                return this.maps[layer, row, col];
            }
            set
            {
                this.maps[layer, row, col] = value;
                DrawBuffer();
            }
            
        }

        public void SetCell(int layer, int row, int col,Tile tile)
        {
            this.maps[layer, row, col] = new Cell(layer,row,col,tile);
            DrawBuffer();
        }

        /// <summary>
        /// 绘制地图
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="g"></param>
        public void Draw(int layer, Graphics g)
        {
            if (buffers[layer] == null) return;
           
            g.DrawImage(buffers[layer], 0, 0, MAP_COLS * Tile.TILE_WIDTH, MAP_ROWS * Tile.TILE_HEIGHT);
        }

        public void DrawBuffer()
        {
            InitBuffer();

            for (int layer = 0; layer < MAP_LAYERS; layer++)
            {
                Graphics g = Graphics.FromImage(buffers[layer]);

                for (int row = 0; row < MAP_ROWS; row++)
                {
                    for (int col = 0; col < MAP_COLS; col++)
                    {
                        this.maps[layer, row, col].Draw(g);
                    }
                }
            }
        }

        private void InitBuffer()
        {
            for (int i = 0; i < buffers.Length; i++)
            {
                buffers[i] = new Bitmap(MAP_COLS * Tile.TILE_WIDTH, MAP_ROWS * Tile.TILE_HEIGHT);
            }
        }

        /// <summary>
        /// 初始化地图。第一层图片用默认图片
        /// </summary>
        /// <param name="image">第一层图片</param>
        private void InitMaps(Bitmap image)
        {
            for (int layer = 0; layer < MAP_LAYERS; layer++)
            {
                for (int row = 0; row < MAP_ROWS; row++)
                {
                    for (int col = 0; col < MAP_COLS; col++)
                    {
                        // 地形使用同样的图片
                        if (layer == 0)
                        {
                            maps[layer, row, col] = new Cell(layer, row, col, image);
                        }
                        else
                        {
                            maps[layer, row, col] = new Cell(layer, row, col, new Bitmap(Tile.TILE_WIDTH, Tile.TILE_HEIGHT));
                        }
                    }
                }
            }

            DrawBuffer();
        }

        public void RemoveDiedSprites()
        {
            for (int i = Sprites.Count - 1; i >= 0; i--)
            {
                if (!Sprites[i].Active)
                {
                    Sprites.RemoveAt(i);
                }
            }
        }

        public void Save(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(this.levelNo);

            for (int layer = 0; layer < MAP_LAYERS; layer++)
            {
                for (int row = 0; row < MAP_ROWS; row++)
                {
                    for (int cell = 0; cell < MAP_COLS; cell++)
                    {
                        maps[layer, row, cell].Save(fs);
                    }
                }
            }


            fs.Close();

        }

        public void Load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            this.levelNo = br.ReadInt32();

            for (int layer = 0; layer < MAP_LAYERS; layer++)
            {
                for (int row = 0; row < MAP_ROWS; row++)
                {
                    for (int cell = 0; cell < MAP_COLS; cell++)
                    {
                        maps[layer, row, cell].Load(fs);
                    }
                }
            }

            DrawBuffer();
            this.sprites.Clear();

            fs.Close();
        }
    }
}
