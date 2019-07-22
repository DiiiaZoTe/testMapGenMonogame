using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestMapMono {
    class Room {
        private int roomNumber;
        private int row;
        private int column;
        private int distanceFromFirst;
        private int[] doors;

        public Room(int roomNumber, int row, int column, int distanceFromFirst) {
            this.roomNumber = roomNumber;
            this.row = row;
            this.column = column;
            this.distanceFromFirst = distanceFromFirst;
            this.doors = new int[] { 0, 0, 0, 0 };
        }

        public Room(int roomNumber, int row, int column, int distanceFromFirst, int[] doors  ) {
            this.roomNumber = roomNumber;
            this.row = row;
            this.column = column;
            this.distanceFromFirst = distanceFromFirst;
            this.doors = doors;
        }

        public int RoomNumber { get => roomNumber; set => roomNumber = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public int[] Doors { get => doors; set => doors = value; }
        public int DistanceFromFirst { get => distanceFromFirst; }

        public void Draw(SpriteBatch sb, List<Texture2D> textures, List<SpriteFont> fonts, Rectangle pos) {
            SpriteFont font;
            if (pos.Width < 60)
                font = fonts[0];
            else
                font = fonts[1];
            sb.Draw(textures[4], pos, Color.White); //room
            sb.DrawString(font, this.roomNumber.ToString(), new Vector2(pos.X + pos.Width / 3, pos.Y + pos.Height / 3), Color.Black); //room number
            for (int i = 0; i < 4; i++) {
                if (this.doors[i] == 1) {
                    sb.Draw(textures[i], pos, Color.Maroon);
                }
            }
        }

        public void Draw(SpriteBatch sb, List<Texture2D> textures, List<SpriteFont> fonts, Rectangle pos, int furthestDistance, bool mouseSelected = false) {
            SpriteFont font;
            if (pos.Width < 60)
                font = fonts[0];
            else
                font = fonts[1];
            if (!mouseSelected) {
                float lerpAmount = this.roomNumber == 1 ? 0 : 1 / (float)((float)furthestDistance / (float)this.distanceFromFirst);
                sb.Draw(textures[4], pos, Color.Lerp(Color.White, Color.Maroon, lerpAmount)); //room
                sb.DrawString(font, this.roomNumber.ToString(), new Vector2(pos.X + pos.Width / 3, pos.Y + pos.Height / 3), this.roomNumber == 1 ? Color.Black : (lerpAmount < 0.7 ? Color.Black : Color.White)); //room number
                for (int i = 0; i < 4; i++) {
                    if (this.doors[i] == 1) {
                        sb.Draw(textures[i], pos, Color.Lerp(Color.WhiteSmoke, Color.Maroon, lerpAmount));
                    }
                }
            }
            else {
                sb.Draw(textures[4], pos, Color.LightGreen); //room
                sb.DrawString(font, this.roomNumber.ToString(), new Vector2(pos.X + pos.Width / 3, pos.Y + pos.Height / 3), Color.Black); //room number
                for (int i = 0; i < 4; i++) {
                    if (this.doors[i] == 1) {
                        sb.Draw(textures[i], pos, Color.LightGreen);
                    }
                }
            }
        }

        public override string ToString() {
            string str = string.Format("Room Number: {0}\n" +
                                       "Row: {1}\n" +
                                       "Column: {2}\n" +
                                       "Doors:\n" +
                                       "  - top: {3}\n" +
                                       "  - bottom: {4}\n" +
                                       "  - left: {5}\n" +
                                       "  - right: {6}\n",
                                       this.roomNumber, this.row, this.column, this.Doors[0], this.Doors[1], this.Doors[2], this.Doors[3]);
            return str;
        }
    }
}
