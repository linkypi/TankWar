using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace TankWar.Library.MapSystem
{
    public class Tile
    {
        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;

        private TileAttribute attribute;

        public TileAttribute Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }
        private bool disponsed;

        public bool Disponsed
        {
            get { return disponsed; }
            set { disponsed = value; }
        }

        private Bitmap bits;

        public Bitmap Bits
        {
            get { return bits; }
            set { bits = value; }
        }

 

        public Tile()
        {
            Init(TileAttribute.CanPasse, false, new Bitmap(TILE_WIDTH, TILE_HEIGHT));
        }

        public Tile(TileAttribute attribute, Bitmap bits)
        {
            Init(attribute,false, bits);
        }



        public Tile(Bitmap bits)
        {
            Init(TileAttribute.CanPasse, false, bits);
        }




        private void Init(TileAttribute attribute,bool disponsed, Bitmap bits)
        {
            this.Attribute = attribute;
            this.Disponsed = disponsed;
            this.bits = bits;
        }

        public void Save(Stream output)
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(output);

                // 图片属性写入
                bw.Write((int)Attribute);

                // 图片是否已经不用的属性写入
                bw.Write(Disponsed);

                // 原始图片的分辨率可能不同
                // 分辨率不同，则图片的Size不同
                // 这里统一一下
                Bitmap bit = new Bitmap(TILE_WIDTH, TILE_HEIGHT);

                Graphics g = Graphics.FromImage(bit);

                Rectangle sr = new Rectangle(0, 0, 32, 32);
                Rectangle dr = new Rectangle(0, 0, 32, 32);

                g.DrawImage(this.Bits, dr, sr, GraphicsUnit.Pixel);

                this.Bits = bit;

                // 图片数据写入
                // 首先写入内存流
                MemoryStream ms = new MemoryStream();
                Bits.Save(ms, ImageFormat.Bmp);

                // 然后将内存流的数据写入一个byte数组
                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);

                // 将byte数组的数据写入文件流
                output.Write(buffer, 0, buffer.Length);

            }
            catch (Exception ex)
            {
                throw new Exception("保存Tile发生错误：\n" + ex.Message);
            }
        }

        public void Load(Stream input)
        {
            try
            {
                BinaryReader br = new BinaryReader(input);

                this.Attribute = (TileAttribute)br.ReadInt32();

                this.Disponsed = br.ReadBoolean();



                // 创建和图片一样大小的byte数组
                MemoryStream ms = new MemoryStream();
                this.Bits.Save(ms, ImageFormat.Bmp);
                byte[] buffer = new byte[ms.Length];

                // 将文件流中的数据读取到数组
                input.Read(buffer, 0, buffer.Length);

                // 内存流关联数组
                ms = new MemoryStream(buffer);

                // 读取图片数据
                this.Bits = (Bitmap)Bitmap.FromStream(ms);

            }
            catch (Exception ex)
            {
                throw new Exception("读取Tile发生错误：\n" + ex.Message);
            }
        }

        public Tile Copy()
        {
            Tile tile = new Tile();

            tile.Attribute = this.Attribute;
            tile.Bits = (Bitmap)this.Bits.Clone();
            tile.Disponsed = this.Disponsed;

            return tile;
        }



        public void Draw(Graphics g,int x,int y)
        {
            if (Disponsed) return;

            this.bits.MakeTransparent(Map.MAP_TRANSPARENT_COLOR);

            g.DrawImage(this.Bits, x, y, TILE_WIDTH, TILE_HEIGHT);
        }

    }
}
