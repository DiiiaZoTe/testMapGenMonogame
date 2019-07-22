using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestMapMono {
    class Map {
        private int rows;
        private int columns;
        private Room[,] roomArray;
        private int roomAmount;
        private Room firstRoom;
        private Room fursthestRoom;
        private Vector2 position;
        private int displaySize;

        public Map(int rows = 4, int columns = 4, int roomAmount = 1) {
            Random rand = new Random();
            this.rows = rows < 1 ? 1 : rows;
            this.columns = columns < 1 ? 1 : columns;
            if (roomAmount > rows * columns || roomAmount == 0)
                roomAmount = rows * columns;
            else if (roomAmount < 1)
                roomAmount = 1;
            this.roomAmount = rand.Next(roomAmount, rows * columns + 1);
            this.roomArray = new Room[rows,columns];
            GenerateRooms();
            this.fursthestRoom = FindFurthestFromFirst();
            this.displaySize = Math.Min(MyGame.Instance.Window.ClientBounds.Width / this.columns, MyGame.Instance.Window.ClientBounds.Height / this.rows);
            this.position = new Vector2((MyGame.Instance.Window.ClientBounds.Width - this.displaySize * this.columns) / 2,
                                        (MyGame.Instance.Window.ClientBounds.Height - this.displaySize * this.rows) / 2);
        }

        public int Rows { get => rows; set => rows = value; }
        public int Columns { get => columns; set => columns = value; }
        public int RoomAmount { get => roomAmount; set => roomAmount = value; }
        internal Room[,] RoomArray { get => roomArray; set => roomArray = value; }
        internal Room FirstRoom { get => firstRoom; }
        internal Room FursthestRoom { get => fursthestRoom; }
        public int DisplaySize { get => displaySize; set => displaySize = value; }
        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }

        public void GenerateRooms() {
            List<Room> tmpRoomList = new List<Room>(this.roomAmount);
            Random random = new Random();
            int firstRow = random.Next(this.rows);
            int firstColumn = random.Next(this.columns);
            Room RoomOne = new Room(1, firstRow, firstColumn, 0);
            this.firstRoom = RoomOne;
            CheckAroundRoom(RoomOne);
            tmpRoomList.Add(RoomOne);
            this.roomArray[firstRow, firstColumn] = RoomOne;

            while (tmpRoomList.Count < this.roomAmount) {
                int roomNumber = tmpRoomList.Count;
                int fromRoom = random.Next(0, roomNumber);
                int direction = random.Next(4);
                if (direction == Direction.top && tmpRoomList[fromRoom].Doors[Direction.top] == 0) { // top
                    int r = tmpRoomList[fromRoom].Row - 1;
                    int c = tmpRoomList[fromRoom].Column;
                    if (this.roomArray[r, c] == null) {
                        Room newRoom = new Room(roomNumber + 1, r, c, tmpRoomList[fromRoom].DistanceFromFirst + 1, new int[] { 0, 1, 0, 0 });
                        tmpRoomList[fromRoom].Doors[direction] = 1;
                        CheckAroundRoom(newRoom);
                        this.roomArray[r, c] = newRoom;
                        tmpRoomList.Add(newRoom);
                    }
                }
                if (direction == Direction.bottom && tmpRoomList[fromRoom].Doors[Direction.bottom] == 0) { // bottom
                    int r = tmpRoomList[fromRoom].Row + 1;
                    int c = tmpRoomList[fromRoom].Column;
                    if (this.roomArray[r, c] == null) {
                        Room newRoom = new Room(roomNumber + 1, r, c, tmpRoomList[fromRoom].DistanceFromFirst + 1, new int[] { 1, 0, 0, 0 });
                        tmpRoomList[fromRoom].Doors[direction] = 1;
                        CheckAroundRoom(newRoom);
                        this.roomArray[r, c] = newRoom;
                        tmpRoomList.Add(newRoom);
                    }
                }
                if (direction == Direction.left && tmpRoomList[fromRoom].Doors[Direction.left] == 0) { // left
                    int r = tmpRoomList[fromRoom].Row;
                    int c = tmpRoomList[fromRoom].Column - 1;
                    if (this.roomArray[r, c] == null) {
                        Room newRoom = new Room(roomNumber + 1, r, c, tmpRoomList[fromRoom].DistanceFromFirst + 1, new int[] { 0, 0, 0, 1 });
                        tmpRoomList[fromRoom].Doors[direction] = 1;
                        CheckAroundRoom(newRoom);
                        this.roomArray[r, c] = newRoom;
                        tmpRoomList.Add(newRoom);
                    }
                }
                if (direction == Direction.right && tmpRoomList[fromRoom].Doors[Direction.right] == 0) { // right
                    int r = tmpRoomList[fromRoom].Row;
                    int c = tmpRoomList[fromRoom].Column + 1;
                    if (this.roomArray[r, c] == null) {
                        Room newRoom = new Room(roomNumber + 1, r, c, tmpRoomList[fromRoom].DistanceFromFirst + 1, new int[] { 0, 0, 1, 0 });
                        tmpRoomList[fromRoom].Doors[direction] = 1;
                        CheckAroundRoom(newRoom);
                        this.roomArray[r, c] = newRoom;
                        tmpRoomList.Add(newRoom);
                    }
                }
            }
        }

        private void CheckAroundRoom(Room room) {
            if (room.Row == 0)
                room.Doors[Direction.top] = -1;
            if (room.Row == this.rows - 1)
                room.Doors[Direction.bottom] = -1;
            if (room.Column == 0)
                room.Doors[Direction.left] = -1;
            if (room.Column == this.columns - 1)
                room.Doors[Direction.right] = -1;
        }

        public Room FindFurthestFromFirst() {
            Room furthestRoom = this.firstRoom;
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    if (this.roomArray[i, j] == null) continue;
                    if (this.roomArray[i, j].DistanceFromFirst > furthestRoom.DistanceFromFirst)
                        furthestRoom = this.roomArray[i, j];
                }
            }
            return furthestRoom;
        }

        /* find furthest from first using FindFurthest()
        public Object[] FindFurthestFromFirst() {
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    if (this.roomArray[i, j] == null) continue;
                    if (this.roomArray[i, j].RoomNumber == 1)
                        return FindFurthest(this.roomArray[i, j]);
                }
            }
            return new object[] { null, null }; // this should never happened
        }
        */

        public Object[] FindFurthest(Room room) {
            return FindFurthestHelper(room, 0, 0, new Object[] { room, 0 });
        }

        private Object[] FindFurthestHelper(Room room, int fromDirection, int n, Object[] furthest) {
            if ((room.Doors[Direction.top] != 1 || fromDirection == Direction.bottom) &&
                (room.Doors[Direction.bottom] != 1 || fromDirection == Direction.top) &&
                (room.Doors[Direction.left] != 1 || fromDirection == Direction.right) &&
                (room.Doors[Direction.right] != 1 || fromDirection == Direction.left)) {
                if (n > (int)furthest[1])
                    return new Object[] { room, n };
                return furthest;
            }
            for(int toDirection=0; toDirection < 4; toDirection++) {
                int toRow = room.Row;
                int toColumn = room.Column;
                bool gotoNextRoom = false;
                if(room.Doors[Direction.top] == 1 && toDirection == Direction.top && fromDirection != Direction.bottom) {
                    gotoNextRoom = true;
                    toRow--;
                }
                if (room.Doors[Direction.bottom] == 1 && toDirection == Direction.bottom && fromDirection != Direction.top) {
                    gotoNextRoom = true;
                    toRow++;
                }
                if (room.Doors[Direction.left] == 1 && toDirection == Direction.left && fromDirection != Direction.right) {
                    gotoNextRoom = true;
                    toColumn--;
                }
                if (room.Doors[Direction.right] == 1 && toDirection == Direction.right && fromDirection != Direction.left) {
                    gotoNextRoom = true;
                    toColumn++;
                }
                if (gotoNextRoom)
                    furthest = FindFurthestHelper(this.roomArray[toRow, toColumn], toDirection, n + 1, furthest);
            }
            return furthest;
        }

        public void ConsoleMap() {
            for(int i = 0; i < this.rows; i++) {
                for(int line = 0; line < 4; line++) {
                    for (int j = 0; j < this.columns; j++) {
                        Room room = this.roomArray[i, j];
                        if (room == null)
                            Console.Write("* * * * ");
                        else {
                            if(line == 0) {
                                if (room.Doors[Direction.top] == 1)
                                    Console.Write("  _||_  ");
                                else
                                    Console.Write("  ____  ");
                            }
                            if(line == 1 || line == 2) {
                                String str = "";
                                if (room.Doors[Direction.left] == 1)
                                    str += "==";
                                else
                                    str += " |";
                                if (room.Doors[Direction.bottom] == 1 && line == 2)
                                    str += "_  _";
                                else if (line == 1)
                                    str += "    ";
                                else
                                    str += "____";
                                if (room.Doors[Direction.right] == 1)
                                    str += "==";
                                else
                                    str += "| ";
                                Console.Write(str);
                            }
                            if(line == 3) {
                                if (room.Doors[Direction.bottom] == 1)
                                    Console.Write("   ||   ");
                                else
                                    Console.Write("        ");
                            }
                        }
                    }
                    Console.Write("\n");
                }
            }
        }

        public Room MouseOverRoom() {
            float w = MyGame.Instance.Window.ClientBounds.Width;
            float h = MyGame.Instance.Window.ClientBounds.Height;
            float z = Camera2D.Instance.Zoom;
            int selectedRow = (int)Math.Floor(
                    ((Mouse.GetState().Position.Y / z) + ((h - (h / z)) / 2) - this.Y ) / (float)this.DisplaySize
                );
            int selectedColumn = (int)Math.Floor(
                    ((Mouse.GetState().Position.X / z) + ((w - (w / z)) / 2) - this.X) / (float)this.DisplaySize
                );
            if (selectedRow >= 0 && selectedRow < this.rows && selectedColumn >= 0 && selectedColumn < this.columns)
                return this.roomArray[selectedRow, selectedColumn];
            return null;
        }

        public void Draw(SpriteBatch sb, List<Texture2D> textures, List<SpriteFont> fonts) {
            Room mouseOver = MouseOverRoom();
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    if(roomArray[i,j] != null)
                        if(roomArray[i,j] == mouseOver)
                            roomArray[i, j].Draw(sb, textures, fonts, new Rectangle((int)X + j * this.displaySize, (int)Y + i * this.displaySize, this.displaySize, this.displaySize), this.fursthestRoom.DistanceFromFirst, true);
                        else
                            roomArray[i, j].Draw(sb, textures, fonts, new Rectangle((int)X + j * this.displaySize, (int)Y + i * this.displaySize, this.displaySize, this.displaySize), this.fursthestRoom.DistanceFromFirst);
                }
            }
        }

        public override string ToString() {
            string str = "";
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    if (this.roomArray[i, j] == null)
                        str += " * ";
                    else
                        str += " " + this.roomArray[i, j].RoomNumber.ToString() + " ";
                }
                str += "\n";
            }
            return str;
        }
    }
}
