using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Amount
    {
        public int Min;
        public int Max;

        public Amount(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    [Serializable]
    public class SpritePool
    {
        public GameObject[] sprites;

        public int Length
        {
            get { return sprites.Length; }
        }

        public SpritePool(GameObject[] sprites)
        {
            this.sprites = sprites;
        }

        public GameObject Get(int index)
        {
            return sprites[index];
        }

        public GameObject GetRandomSprite()
        {
            return sprites[Random.Range(0, sprites.Length)];
        }
    }

    public int Cols = 64;
    public int Rows = 64;
    // size of the map
    int _xsize;
    int _ysize;
    // define the %chance to generate either a room or a corridor on the map
    // BTW, rooms are 1st priority so actually it's enough to just define the chance
    // of generating a room
    public int ChanceRoom = 100;
    public Amount WallsAmount = new Amount(16, 32);
    public Vector2Int roomSize = new Vector2Int(15, 12);
    public int outerWallsThickness = 1;

    public SpritePool Floor;
    public SpritePool Walls;

    private Transform boardHolder;

    public int Corridors
    {
        get;
        private set;
    }

    public Vector2 StartPosition
    {
        get;
        private set;
    }

    public enum Direction { North, East, South, West };
    public enum Tile { Unused, Wall, Floor, Corridor };

    Tile[] _dungeonMap = { };

    int _objects;
    public int numberOfFeatures = 50;

    public static bool IsWall(int x, int y, int xlen, int ylen, int xt, int yt, Direction d)
    {
        Func<int, int, int> a = GetFeatureLowerBound;

        Func<int, int, int> b = IsFeatureWallBound;
        switch (d)
        {
            case Direction.North:
                return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y - ylen + 1;
            case Direction.East:
                return xt == x || xt == x + xlen - 1 || yt == a(y, ylen) || yt == b(y, ylen);
            case Direction.South:
                return xt == a(x, xlen) || xt == b(x, xlen) || yt == y || yt == y + ylen - 1;
            case Direction.West:
                return xt == x || xt == x - xlen + 1 || yt == a(y, ylen) || yt == b(y, ylen);
        }

        throw new InvalidOperationException();
    }

    public static int GetFeatureLowerBound(int c, int len)
    {
        return c - len / 2;
    }

    public static int IsFeatureWallBound(int c, int len)
    {
        return c + (len - 1) / 2;
    }

    public static int GetFeatureUpperBound(int c, int len)
    {
        return c + (len + 1) / 2;
    }

    public static IEnumerable<PointI> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d)
    {
        // north and south share the same x strategy
        // east and west share the same y strategy
        Func<int, int, int> a = GetFeatureLowerBound;
        Func<int, int, int> b = GetFeatureUpperBound;

        switch (d)
        {
            case Direction.North:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt > y - ylen; yt--) yield return new PointI { X = xt, Y = yt };
                break;
            case Direction.East:
                for (var xt = x; xt < x + xlen; xt++) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new PointI { X = xt, Y = yt };
                break;
            case Direction.South:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt < y + ylen; yt++) yield return new PointI { X = xt, Y = yt };
                break;
            case Direction.West:
                for (var xt = x; xt > x - xlen; xt--) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new PointI { X = xt, Y = yt };
                break;
            default:
                yield break;
        }
    }

    public Tile GetCellType(int x, int y)
    {
        try
        {
            return this._dungeonMap[x + this._xsize * y];
        }
        catch (IndexOutOfRangeException)
        {
            return Tile.Wall;
        }
    }

    void SetCell(int x, int y, Tile celltype)
    {
        this._dungeonMap[x + this._xsize * y] = celltype;
    }

    public bool MakeCorridor(int x, int y, int length, Direction direction)
    {
        // define the dimensions of the corridor (er.. only the width and height..)
        int len = Random.Range(0, length);
        const Tile Floor = Tile.Corridor;

        int xtemp;
        int ytemp = 0;

        switch (direction)
        {
            case Direction.North:
                // north
                // check if there's enough space for the corridor
                // start with checking it's not out of the boundaries
                if (x < 0 || x > this._xsize) return false;
                xtemp = x;

                // same thing here, to make sure it's not out of the boundaries
                for (ytemp = y; ytemp > (y - len); ytemp--)
                {
                    if (ytemp < 0 || ytemp > this._ysize) return false; // oh boho, it was!
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                // if we're still here, let's start building
                Corridors++;
                for (ytemp = y; ytemp > (y - len); ytemp--)
                {
                    this.SetCell(xtemp, ytemp, Floor);
                }

                break;

            case Direction.East:
                // east
                if (y < 0 || y > this._ysize) return false;
                ytemp = y;

                for (xtemp = x; xtemp < (x + len); xtemp++)
                {
                    if (xtemp < 0 || xtemp > this._xsize) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (xtemp = x; xtemp < (x + len); xtemp++)
                {
                    this.SetCell(xtemp, ytemp, Floor);
                }

                break;

            case Direction.South:
                // south
                if (x < 0 || x > this._xsize) return false;
                xtemp = x;

                for (ytemp = y; ytemp < (y + len); ytemp++)
                {
                    if (ytemp < 0 || ytemp > this._ysize) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (ytemp = y; ytemp < (y + len); ytemp++)
                {
                    this.SetCell(xtemp, ytemp, Floor);
                }

                break;
            case Direction.West:
                // west
                if (ytemp < 0 || ytemp > this._ysize) return false;
                ytemp = y;

                for (xtemp = x; xtemp > (x - len); xtemp--)
                {
                    if (xtemp < 0 || xtemp > this._xsize) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (xtemp = x; xtemp > (x - len); xtemp--)
                {
                    this.SetCell(xtemp, ytemp, Floor);
                }

                break;
        }

        // woot, we're still here! let's tell the other guys we're done!!
        return true;
    }

    public IEnumerable<Tuple> GetSurroundingPoints(PointI v)
    {
        var points = new[]
                         {
                                 new Tuple(new PointI { X = v.X, Y = v.Y + 1 }, Direction.North),
                                 new Tuple(new PointI { X = v.X - 1, Y = v.Y }, Direction.East),
                                 new Tuple(new PointI { X = v.X , Y = v.Y-1 }, Direction.South),
                                 new Tuple(new PointI { X = v.X +1, Y = v.Y  }, Direction.West),

                             };
        return points.Where(p => InBounds(p.Item1));
    }

    public IEnumerable<TupleWithTile> GetSurroundings(PointI v)
    {
        return
            this.GetSurroundingPoints(v)
                .Select(r => new TupleWithTile(r.Item1, r.Item2, this.GetCellType(r.Item1.X, r.Item1.Y)));
    }

    public bool InBounds(int x, int y)
    {
        return x > 0 && x < Rows && y > 0 && y < Cols;
    }

    public bool InBounds(PointI v)
    {
        return this.InBounds(v.X, v.Y);
    }

    public bool MakeRoom(int x, int y, int xlength, int ylength, Direction direction)
    {
        // define the dimensions of the room, it should be at least 4x4 tiles (2x2 for walking on, the rest is walls)
        int xlen = Random.Range(4, xlength);
        int ylen = Random.Range(4, ylength);

        // the tile type it's going to be filled with
        const Tile Floor = Tile.Floor;

        const Tile Wall = Tile.Wall;
        // choose the way it's pointing at

        var points = GetRoomPoints(x, y, xlen, ylen, direction).ToArray();

        // Check if there's enough space left for it
        if (
            points.Any(
                s =>
                s.Y < 0 || s.Y > this._ysize || s.X < 0 || s.X > this._xsize || this.GetCellType(s.X, s.Y) != Tile.Unused)) return false;

        foreach (var p in points)
        {
            this.SetCell(p.X, p.Y, IsWall(x, y, xlen, ylen, p.X, p.Y, direction) ? Wall : Floor);
        }

        // yay, all done
        return true;
    }

    public Tile[] GetDungeon()
    {
        return this._dungeonMap;
    }

    public GameObject GetCellTile(int x, int y)
    {
        switch (GetCellType(x, y))
        {
            case Tile.Unused:
                return Walls.GetRandomSprite();
            case Tile.Wall:
                return Walls.GetRandomSprite();
            case Tile.Floor:
                return Floor.GetRandomSprite();
            case Tile.Corridor:
                return Floor.GetRandomSprite();
            default:
                return Walls.GetRandomSprite();
        }
    }

    //used to print the map on the screen
    public void ShowDungeon()
    {
        boardHolder = new GameObject("Board").transform;

        for (int y = 0; y < this._ysize; y++)
        {
            for (int x = 0; x < this._xsize; x++)
            {
                GameObject instance = Instantiate(GetCellTile(x, y), new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    public Direction RandomDirection()
    {
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0:
                return Direction.North;
            case 1:
                return Direction.East;
            case 2:
                return Direction.South;
            case 3:
                return Direction.West;
            default:
                throw new InvalidOperationException();
        }
    }

    //and here's the one generating the whole map
    public bool CreateDungeon(int inx, int iny, int inobj)
    {
        this._objects = inobj < 1 ? 10 : inobj;

        // adjust the size of the map, if it's smaller or bigger than the limits
        if (inx < 3) this._xsize = 3;
        else if (inx > Cols) this._xsize = Cols;
        else this._xsize = inx;

        if (iny < 3) this._ysize = 3;
        else if (iny > Rows) this._ysize = Rows;
        else this._ysize = iny;

        // redefine the map var, so it's adjusted to our new map size
        this._dungeonMap = new Tile[this._xsize * this._ysize];

        // start with making the "standard stuff" on the map
        this.Initialize();

        /*******************************************************************************
        And now the code of the random-map-generation-algorithm begins!
        *******************************************************************************/

        // start with making a room in the middle, which we can start building upon
        int roomSizeX = Random.Range(4, roomSize.x);
        int roomSizeY = Random.Range(4, roomSize.y);
        this.MakeRoom(this._xsize / 2, this._ysize / 2, roomSizeX, roomSizeY, RandomDirection()); // getrand saken f????r att slumpa fram riktning p?? rummet

        // keep count of the number of "objects" we've made
        int currentFeatures = 1; // +1 for the first room we just made

        // then we sart the main loop
        for (int countingTries = 0; countingTries < 1000; countingTries++)
        {
            // check if we've reached our quota
            if (currentFeatures == this._objects)
            {
                break;
            }

            // start with a random wall
            int newx = 0;
            int xmod = 0;
            int newy = 0;
            int ymod = 0;
            Direction? validTile = null;

            // 1000 chances to find a suitable object (room or corridor)..
            for (int testing = 0; testing < 1000; testing++)
            {
                newx = Random.Range(outerWallsThickness, this._xsize - outerWallsThickness);
                newy = Random.Range(outerWallsThickness, this._ysize - outerWallsThickness);

                if (GetCellType(newx, newy) == Tile.Wall || GetCellType(newx, newy) == Tile.Corridor)
                {
                    var surroundings = this.GetSurroundings(new PointI() { X = newx, Y = newy });

                    // check if we can reach the place
                    var canReach =
                        surroundings.FirstOrDefault(s => s.Item3 == Tile.Corridor || s.Item3 == Tile.Floor);
                    if (canReach == null)
                    {
                        continue;
                    }
                    validTile = canReach.Item2;
                    switch (canReach.Item2)
                    {
                        case Direction.North:
                            xmod = 0;
                            ymod = -1;
                            break;
                        case Direction.East:
                            xmod = 1;
                            ymod = 0;
                            break;
                        case Direction.South:
                            xmod = 0;
                            ymod = 1;
                            break;
                        case Direction.West:
                            xmod = -1;
                            ymod = 0;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }


                    // check that we haven't got another door nearby, so we won't get alot of openings besides
                    // each other

                    if (GetCellType(newx, newy + 1) == Tile.Corridor) // north
                    {
                        validTile = null;

                    }

                    else if (GetCellType(newx - 1, newy) == Tile.Corridor) // east
                        validTile = null;
                    else if (GetCellType(newx, newy - 1) == Tile.Corridor) // south
                        validTile = null;
                    else if (GetCellType(newx + 1, newy) == Tile.Corridor) // west
                        validTile = null;


                    // if we can, jump out of the loop and continue with the rest
                    if (validTile.HasValue) break;
                }
            }

            if (validTile.HasValue)
            {
                // choose what to build now at our newly found place, and at what direction
                int feature = Random.Range(0, 100);
                if (feature <= ChanceRoom)
                { // a new room
                    roomSizeX = Random.Range(4, roomSize.x);
                    roomSizeY = Random.Range(4, roomSize.y);
                    if (this.MakeRoom(newx + xmod, newy + ymod, roomSizeX, roomSizeY, validTile.Value))
                    {
                        if (currentFeatures == 1)
                        {
                            StartPosition = new Vector2(newx + xmod, newy + ymod);
                        }
                        currentFeatures++; // add to our quota

                        // then we mark the wall opening with a door
                        this.SetCell(newx, newy, Tile.Corridor);

                        // clean up infront of the door so we can reach it
                        this.SetCell(newx + xmod, newy + ymod, Tile.Floor);
                    }
                }
                else if (feature >= ChanceRoom)
                { // new corridor
                    if (this.MakeCorridor(newx + xmod, newy + ymod, 6, validTile.Value))
                    {
                        // same thing here, add to the quota and a door
                        currentFeatures++;

                        this.SetCell(newx, newy, Tile.Corridor);
                    }
                }
            }
        }

        return true;
    }

    void Initialize()
    {
        for (int y = 0; y < this._ysize; y++)
        {
            for (int x = 0; x < this._xsize; x++)
            {
                // ie, making the borders of unwalkable walls
                if (y < outerWallsThickness || y > this._ysize - outerWallsThickness || x < outerWallsThickness || x > this._xsize - outerWallsThickness)
                {
                    this.SetCell(x, y, Tile.Wall);
                }
                else
                {                        // and fill the rest with dirt
                    this.SetCell(x, y, Tile.Unused);
                }
            }
        }
    }

    public void SetupScene()
    {
        Initialize();
        CreateDungeon(Cols, Rows, numberOfFeatures);
        ShowDungeon();
    }

    public class PointI
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }
    }

    public class Tuple
    {
        public PointI Item1;
        public Direction Item2;

        public Tuple(PointI pointI, BoardManager.Direction direction)
        {
            this.Item1 = pointI;
            this.Item2 = direction;
        }
    }

    public class TupleWithTile
    {
        public PointI Item1;
        public Direction Item2;
        public Tile Item3;

        public TupleWithTile(PointI pointI, BoardManager.Direction direction, Tile tile)
        {
            this.Item1 = pointI;
            this.Item2 = direction;
            this.Item3 = tile;
        }
    }
}