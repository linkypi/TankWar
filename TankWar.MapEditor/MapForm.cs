using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TankWar.Library.MapSystem;


namespace TankWar.MapEditor
{
    public partial class MapForm : Form
    {
        #region 成员变量
        // 地形物数组
        private Tile[] tiles = new Tile[8];

        // 当前地形物
        private Tile currentTile = null;

        // 当前地图层数
        // 0为地形层；1为地形物层
        private int currentLayer = 0;


        private int row = 0;
        private int col = 0;

        private Map map = null;

        private BufferedGraphics bufferedGraphics;

        #endregion

        public MapForm()
        {
            InitializeComponent();

            // 初始化双缓冲绘图环境
            InitBufferedGraphics();

            // 初始化地形物数据
            InitTiles();
            
            // 初始化地图
            InitMap();

            // 初始化地形物控制面板
            //InitPnlTile();  
        }

        private void InitMap()
        {
            this.map = new Map(tiles[2].Bits);
        }

        private void InitBufferedGraphics()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            int width = Tile.TILE_WIDTH * Map.MAP_COLS;
            int height = Tile.TILE_HEIGHT * Map.MAP_ROWS;
            context.MaximumBuffer = new Size(width + 1, height + 1);
            bufferedGraphics = context.Allocate(this.pnlMap.CreateGraphics(), new Rectangle(0, 0, width, height));
        }

        private void InitPnlTile()
        {
            if (this.toolStripComboBox1.SelectedIndex < 0)
            {
                this.toolStripComboBox1.SelectedIndex = 0;
            }

            this.pnlTile.Controls.Clear();

            if (this.toolStripComboBox1.SelectedItem.ToString().Equals("地形"))
            {
                for (int i = 2; i < tiles.Length; i+=4)
                {
                    PictureBox box = new PictureBox();
                    box.Width = Tile.TILE_WIDTH;
                    box.Height = Tile.TILE_HEIGHT;
                    box.Image = tiles[i].Bits;
                    box.Tag = tiles[i];
                    box.Click += new EventHandler(box_Click);
                    box.Dock = DockStyle.Left;

                    this.pnlTile.Controls.Add(box);
                }

                this.currentLayer = 0;
            }

            if (this.toolStripComboBox1.SelectedItem.ToString().Equals("地形物"))
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    if (i == 2 || i == 3 || i==6) continue;

                    PictureBox box = new PictureBox();
                    box.Width = Tile.TILE_WIDTH;
                    box.Height = Tile.TILE_HEIGHT;
                    box.Image = tiles[i].Bits;
                    box.Tag = tiles[i];
                    box.Click += new EventHandler(box_Click);
                    box.Dock = DockStyle.Left;
                    

                    this.pnlTile.Controls.Add(box);
                }

                int left = 5 * Tile.TILE_WIDTH + 30;
                int top = 5;

                string[] texts = { "坦克子弹都可通过", "只能被子弹打破", "只有子弹可以穿过","不能通过不能破坏", "是军旗" };

                for (int i = 0; i < texts.Length; i++)
                {
                    RadioButton rbt = new RadioButton();
                    rbt.Text = texts[i] ;
                    rbt.Tag = i;
                    rbt.Width = 120;

                    if (i == 0) rbt.Checked = true;
                    
                    rbt.Left = left;
                    rbt.Top = top;

                    left += rbt.Width + 10;

                    rbt.Click += new EventHandler(rbt_Click);
                    this.pnlTile.Controls.Add(rbt);
                }
                this.currentLayer = 1;
            }
            this.currentTile = null;
        }

        void rbt_Click(object sender, EventArgs e)
        {
            if (this.currentTile == null) return;

            RadioButton rbt = sender as RadioButton;

            if (rbt != null)
            {
                int attrNum = int.Parse(rbt.Tag.ToString());
                this.currentTile.Attribute = (TileAttribute)attrNum;
            }
        }

        void box_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < pnlTile.Controls.Count;i++ )
            {
                PictureBox box = pnlTile.Controls[i] as PictureBox;

                if (box == null) continue;

                box.BorderStyle = BorderStyle.None;

            }

            PictureBox currentBox = sender as PictureBox;

            if (currentBox != null)
            {
                currentBox.BorderStyle = BorderStyle.Fixed3D;

                this.currentTile = currentBox.Tag as Tile;
            }

            for (int i = 0; i < pnlTile.Controls.Count; i++)
            {
                RadioButton rbt = pnlTile.Controls[i] as RadioButton;

                if (rbt == null) continue;

                rbt.Checked = rbt.Tag.ToString().Equals(((int)this.currentTile.Attribute).ToString());

            }

        }

        private void InitTiles()
        {
            Bitmap bmp = new Bitmap("map.bmp");

            for (int i = 0; i < tiles.Length-1; i++)
            {
                Bitmap bit = new Bitmap(Tile.TILE_WIDTH, Tile.TILE_HEIGHT);

                Graphics g = Graphics.FromImage(bit);

                Rectangle sr = new Rectangle(Tile.TILE_WIDTH * i, 0, Tile.TILE_WIDTH, Tile.TILE_HEIGHT);
                Rectangle dr = new Rectangle(0, 0, Tile.TILE_WIDTH, Tile.TILE_HEIGHT);

                g.DrawImage(bmp, dr, sr, GraphicsUnit.Pixel);

                Tile tile = new Tile(bit);
                tile.Bits.MakeTransparent(Map.MAP_TRANSPARENT_COLOR);

                this.tiles[i] = tile;
            }

            this.tiles[this.tiles.Length - 1] = new Tile(new Bitmap("symbol.bmp"));
            this.tiles[this.tiles.Length - 1].Attribute = TileAttribute.Flag;
            this.tiles[this.tiles.Length - 1].Bits.MakeTransparent(Map.MAP_TRANSPARENT_COLOR);

        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            this.toolStripComboBox1.SelectedIndex = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Graphics g = bufferedGraphics.Graphics;

            map.Draw(0, g);

            map.Draw(1, g);

            DrawCurrentTile(g);

            bufferedGraphics.Render(this.pnlMap.CreateGraphics());
        }

        private void DrawCurrentTile(Graphics g)
        {
            // 鼠标移出地图不再绘制
            // 要在鼠标移出事件中修改row和col为-1
            if (this.row < 0 || this.col < 0) return;

            int currentX = col * Tile.TILE_WIDTH;
            int currentY = row * Tile.TILE_HEIGHT;


            if (this.currentTile != null)
            {
                this.currentTile.Draw(g, currentX, currentY);
            }
        }

        private void pnlMap_MouseMove(object sender, MouseEventArgs e)
        {
            this.row = e.Y / Tile.TILE_HEIGHT;
            this.col = e.X / Tile.TILE_WIDTH;

            this.lblCurrentPoint.Text = string.Format("第{0}行,第{1}列", row+1, col+1);
        }

        private void pnlMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.currentTile != null)
            {
                this.map.SetCell(this.currentLayer, row, col, this.currentTile.Copy());
            }

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.map.LevelNo = int.Parse(this.toolStripTextBox1.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请输入正确的关卡号。");
                return;
            }

            this.saveFileDialog1.FileName = "地图" + this.map.LevelNo;

            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = this.saveFileDialog1.FileName;
                this.map.Save(fileName);
            }
        }

        private void 加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = this.openFileDialog1.FileName;
                this.map.Load(fileName);

            }
        }

        private void pnlMap_MouseLeave(object sender, EventArgs e)
        {
            this.row = -1;
            this.col = -1;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.InitPnlTile();
        }

        private void 清空CToolStripMenuItem_Click(object sender, EventArgs e)
        {
             this.map = new Map(tiles[2].Bits);
        }
    }
}
