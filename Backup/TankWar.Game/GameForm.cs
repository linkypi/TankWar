using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TankWar.Library.MapSystem;
using TankWar.Library.SpriteSystem;


namespace TankWar.Game
{
    public partial class GameForm : Form
    {
        MyTank mytank = null;                       // 我方坦克
        Map map = null;                             // 当前使用的地图

        ETankManager eTankManager = null;           // 地方坦克管理器

        private BufferedGraphics bufferedGraphics;  // 双缓冲环境

        private bool isGameOver = false;            // 游戏是否结束
        private string gameOverMsg = null;          // 游戏结束的消息

        private string[] mappaths;                  // 保存所有地图的文件路径

        public GameForm()
        {
            InitializeComponent();

            Init();

            InitBufferedGraphics();
        }

        private void Init()
        {
            LoadMap();

            this.map = new Map();
            this.map.Load(mappaths[0]);
            ((ToolStripMenuItem)this.mapMenu.DropDownItems[0]).Checked = true;

            InitData();
        }

        private void InitData()
        {
            this.eTankManager = new ETankManager(this.map);

            this.mytank = new MyTank(map);
            this.mytank.X = 280;
            this.mytank.Y = 520;
            this.map.Sprites.Add(this.mytank);

            isGameOver = false;
            gameOverMsg = null;
        }

        private void InitBufferedGraphics()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            int width = Tile.TILE_WIDTH * Map.MAP_COLS;
            int height = Tile.TILE_HEIGHT * Map.MAP_ROWS;
            context.MaximumBuffer = new Size(width + 1, height + 1);
            bufferedGraphics = context.Allocate(this.pnlMap.CreateGraphics(), new Rectangle(0, 0, width, height));
        }


        private void GameForm_Load(object sender, EventArgs e)
        {

        }


        private void LoadMap()
        {
            try
            {
                string path = Application.StartupPath;
                if (!path.EndsWith("\\"))
                {
                    path += "\\";
                }
                path += "Maps";

                mappaths = Directory.GetFiles(path);

                foreach (string mappath in mappaths)
                {
                    string name = mappath.Substring(mappath.LastIndexOf("\\") + 1, mappath.LastIndexOf(".") - mappath.LastIndexOf("\\")-1);
                    ToolStripMenuItem item = new ToolStripMenuItem(name);
                    item.Tag = mappath;
                    item.Click += new EventHandler(item_Click);
                    this.mapMenu.DropDownItems.Add(item);
                }

            }
            catch (Exception ex)
            {
                this.map = null;
                MessageBox.Show("加载地图错误:\n" + ex.Message);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem currentItem = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem item in this.mapMenu.DropDownItems)
            {
                item.Checked = false;
            }

            this.map.Load(currentItem.Tag.ToString());
            currentItem.Checked = true;

            InitData();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.map.RemoveDiedSprites();

            Graphics g = bufferedGraphics.Graphics;

            this.map.Draw(0, g);

            this.eTankManager.DrawETanks(g);

            this.mytank.Draw(g);

            this.map.Draw(1, g);

            CheckGameOver(g);

            bufferedGraphics.Render(this.pnlMap.CreateGraphics());

        }

        private void CheckGameOver(Graphics g)
        {
            if (isGameOver)
            {
                DrawGameOver(g, gameOverMsg);
                return;
            }

            if (this.mytank.HP <= 0)
            {
                this.gameOverMsg = "游戏结束。你输了！";
                this.isGameOver = true;
            }

            if (this.eTankManager.ETankList.Count <= 0)
            {
                this.gameOverMsg = "游戏结束。你赢了！";
                this.isGameOver = true;
            }


            gameOverMsg = GameMsg.GetGameOverMessage();

            if (!string.IsNullOrEmpty(gameOverMsg))
            {
                isGameOver = true;
            }
        }

        private void DrawGameOver(Graphics g, string msg)
        {
            int x = this.map.Width / 2 - 200;
            int y = this.map.Height / 2 - 50;

            Font font = new Font("黑体", 40);
            g.DrawString(msg, font, Brushes.Red, x, y);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    this.mytank.Direction = Direction.Up;
                    this.mytank.Move();
                    break;

                case Keys.Down:
                    this.mytank.Direction = Direction.Down;
                    this.mytank.Move();
                    break;

                case Keys.Left:
                    this.mytank.Direction = Direction.Left;
                    this.mytank.Move();
                    break;

                case Keys.Right:
                    this.mytank.Direction = Direction.Right;
                    this.mytank.Move();
                    break;

                case Keys.Space:
                    this.mytank.FireBullet();
                    break;
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.eTankManager.AddNewETank();
        }


    }
}
