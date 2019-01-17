using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	private bool exitPlaced;

	public Vector2 ExitPos { get; set; }
	public GameObject ExitObj { get; set; }
	public GameObject ExitOpenedObj { get; set; }
	public int Cols = 64;
    public int Rows = 64;
    public int ChanceRoom = 100;
    public Amount WallsAmount = new Amount(30, 70);
    public Vector2Int roomSize = new Vector2Int(15, 12);
    public int outerWallsThickness = 1;

    public GameObject board;

    public SpritePool Floor;
    public SpritePool Walls;
	public SpritePool Exit;
	public SpritePool OpenedExit;

	private List<GameObject> instanced = new List<GameObject>();

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

    public Vector2 RandomFreePosition()
    {
        while (true)
        {
            int x = Random.Range(0, Cols);
            int y = Random.Range(0, Rows);
            if (dungeonMap[x + availableHorizontalSpace * y] == Tile.Floor)
                return new Vector2(x, y);
        }
    }

    private enum Direction { North, East, South, West };
    private enum Tile { Unused, Wall, Floor, Corridor, Exit };

    private int objects;
    private Tile[] dungeonMap = { };
    private Transform boardHolder = null;

    private int availableHorizontalSpace;
    private int availableVerticalSpace;

    private static bool IsWall(int x, int y, int xlen, int ylen, int xt, int yt, Direction d)
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

    private static IEnumerable<PointI> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d)
    {
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

    private Tile GetCellType(int x, int y)
    {
        try
        {
            return dungeonMap[x + availableHorizontalSpace * y];
        }
        catch (IndexOutOfRangeException)
        {
            return Tile.Wall;
        }
    }

    void SetCell(int x, int y, Tile celltype)
    {
        dungeonMap[x + availableHorizontalSpace * y] = celltype;
    }

    private bool MakeCorridor(int x, int y, int length, Direction direction)
    {
        int len = Random.Range(0, length);
        const Tile Floor = Tile.Corridor;

        int xtemp;
        int ytemp = 0;

        switch (direction)
        {
            case Direction.North:
                if (x < 0 || x > availableHorizontalSpace) return false;
                xtemp = x;

                for (ytemp = y; ytemp > (y - len); ytemp--)
                {
                    if (ytemp < 0 || ytemp > availableVerticalSpace) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (ytemp = y; ytemp > (y - len); ytemp--)
                {
                    SetCell(xtemp, ytemp, Floor);
                }

                break;

            case Direction.East:
                if (y < 0 || y > availableVerticalSpace) return false;
                ytemp = y;

                for (xtemp = x; xtemp < (x + len); xtemp++)
                {
                    if (xtemp < 0 || xtemp > availableHorizontalSpace) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (xtemp = x; xtemp < (x + len); xtemp++)
                {
                    SetCell(xtemp, ytemp, Floor);
                }

                break;

            case Direction.South:
                if (x < 0 || x > availableHorizontalSpace) return false;
                xtemp = x;

                for (ytemp = y; ytemp < (y + len); ytemp++)
                {
                    if (ytemp < 0 || ytemp > availableVerticalSpace) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (ytemp = y; ytemp < (y + len); ytemp++)
                {
                    SetCell(xtemp, ytemp, Floor);
                }

                break;
            case Direction.West:
                if (ytemp < 0 || ytemp > availableVerticalSpace) return false;
                ytemp = y;

                for (xtemp = x; xtemp > (x - len); xtemp--)
                {
                    if (xtemp < 0 || xtemp > availableHorizontalSpace) return false;
                    if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                }

                Corridors++;
                for (xtemp = x; xtemp > (x - len); xtemp--)
                {
                    SetCell(xtemp, ytemp, Floor);
                }

                break;
        }

        return true;
    }

    private IEnumerable<Tuple> GetSurroundingPoints(PointI v)
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

    private IEnumerable<TupleWithTile> GetSurroundings(PointI v)
    {
        return
            GetSurroundingPoints(v)
                .Select(r => new TupleWithTile(r.Item1, r.Item2, GetCellType(r.Item1.X, r.Item1.Y)));
    }

    public bool InBounds(int x, int y)
    {
        return x > 0 && x < Rows && y > 0 && y < Cols;
    }

    public bool InBounds(PointI v)
    {
        return this.InBounds(v.X, v.Y);
    }

    private bool MakeRoom(int x, int y, int xlength, int ylength, Direction direction)
    {
        int xlen = Random.Range(4, xlength);
        int ylen = Random.Range(4, ylength);

        const Tile Floor = Tile.Floor;
        const Tile Wall = Tile.Wall;
        
        var points = GetRoomPoints(x, y, xlen, ylen, direction).ToArray();

        if (
            points.Any(
                s =>
                s.Y < 0 || s.Y > availableVerticalSpace || s.X < 0 || s.X > availableHorizontalSpace || GetCellType(s.X, s.Y) != Tile.Unused)) return false;

        foreach (var p in points)
        {
			var tile = Wall;
			if (!IsWall(x, y, xlen, ylen, p.X, p.Y, direction))
			{
				if (!exitPlaced && Random.Range(0, 100) < 10)
				{
					tile = Tile.Exit;
					ExitPos = new Vector2(p.X, p.Y);
					exitPlaced = true;
				} else
				{
					tile = Floor;
				}
			}
            SetCell(p.X, p.Y, tile);
        }

        return true;
    }

    private Tile[] GetDungeon()
    {
        return dungeonMap;
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
			case Tile.Exit:
				ExitObj = Exit.GetRandomSprite();
				return ExitObj;
            default:
                return Walls.GetRandomSprite();
        }
    }

    public void ShowDungeon()
    {
        boardHolder = Instantiate(board, new Vector3(0, 0, 0), Quaternion.identity).transform;
        Coin[] junk = FindObjectsOfType<Coin>();
        for (int i = 0; i < junk.Length; i++)
        {
            Destroy(junk[i].gameObject);
        }
        for (int y = 0; y < availableVerticalSpace; y++)
        {
            for (int x = 0; x < availableHorizontalSpace; x++)
            {
                GameObject instance = Instantiate(GetCellTile(x, y), new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
				instanced.Add(instance);
            }
        }
		StartCoroutine(OpenExitCountdown());
        AstarPath.active.Scan();
    }

	public IEnumerator OpenExitCountdown()
	{
		var timer = Time.time;
		while (true)
		{
			var elapsedTime = (int)(Time.time - timer);

            int toTimer = Mathf.Clamp(30 - elapsedTime, 0, 30);

            if (toTimer != 0)
            {
                GameObject.Find("Timer").GetComponent<Text>().text = "Exit opens in " + toTimer.ToString() + " seconds";
            }
            else
            {
                GameObject.Find("Timer").GetComponent<Text>().text = "Exit opened";
            }

			if (elapsedTime >= 30)
			{
				ExitOpenedObj = Instantiate(OpenedExit.GetRandomSprite(), ExitPos, Quaternion.identity);
				instanced.Add(ExitOpenedObj);
				break;
			}
			yield return null;
		}
		yield return null;
	}

    private Direction RandomDirection()
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

    Vector2Int DirectionToXY(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Vector2Int(0, -1);
            case Direction.East:
                return new Vector2Int(1, 0);
            case Direction.South:
                return new Vector2Int(0, 1);
            case Direction.West:
                return new Vector2Int(-1, 0);
            default:
                throw new InvalidOperationException();
        }
    }

    public bool CreateDungeon(int inx, int iny, int inobj)
    {
        objects = inobj < 1 ? 10 : inobj;

        if (inx < 3) availableHorizontalSpace = 3;
        else if (inx > Cols) availableHorizontalSpace = Cols;
        else availableHorizontalSpace = inx;

        if (iny < 3) availableVerticalSpace = 3;
        else if (iny > Rows) availableVerticalSpace = Rows;
        else availableVerticalSpace = iny;

        dungeonMap = new Tile[availableHorizontalSpace * availableVerticalSpace];

        Initialize();

        int roomSizeX = Random.Range(4, roomSize.x);
        int roomSizeY = Random.Range(4, roomSize.y);
        Direction randomDirection = RandomDirection();
        MakeRoom(availableHorizontalSpace / 2, availableVerticalSpace / 2, roomSizeX, roomSizeY, randomDirection); // getrand saken f????r att slumpa fram riktning p?? rummet
        Vector2Int directionModifier = DirectionToXY(randomDirection);
        StartPosition = new Vector2(availableHorizontalSpace / 2 + directionModifier.x, availableVerticalSpace / 2 + directionModifier.y);

        int currentFeatures = 1;

        for (int countingTries = 0; countingTries < 1000; countingTries++)
        {
            if (currentFeatures == objects)
            {
                break;
            }

            int newx = 0;
            int xmod = 0;
            int newy = 0;
            int ymod = 0;
            Direction? validTile = null;

            for (int testing = 0; testing < 1000; testing++)
            {
                newx = Random.Range(outerWallsThickness, availableHorizontalSpace - outerWallsThickness);
                newy = Random.Range(outerWallsThickness, availableVerticalSpace - outerWallsThickness);

                if (GetCellType(newx, newy) == Tile.Wall || GetCellType(newx, newy) == Tile.Corridor)
                {
                    var surroundings = GetSurroundings(new PointI() { X = newx, Y = newy });

                    var canReach =
                        surroundings.FirstOrDefault(s => s.Item3 == Tile.Corridor || s.Item3 == Tile.Floor);
                    if (canReach == null)
                    {
                        continue;
                    }
                    validTile = canReach.Item2;
                    directionModifier = DirectionToXY(canReach.Item2);
                    xmod = directionModifier.x;
                    ymod = directionModifier.y;

                    if (GetCellType(newx, newy + 1) == Tile.Corridor)
                    {
                        validTile = null;
                    }

                    else if (GetCellType(newx - 1, newy) == Tile.Corridor)
                        validTile = null;
                    else if (GetCellType(newx, newy - 1) == Tile.Corridor)
                        validTile = null;
                    else if (GetCellType(newx + 1, newy) == Tile.Corridor)
                        validTile = null;


                    if (validTile.HasValue) break;
                }
            }

            if (validTile.HasValue)
            {
                int feature = Random.Range(0, 100);
                if (feature <= ChanceRoom)
                {
                    roomSizeX = Random.Range(4, roomSize.x);
                    roomSizeY = Random.Range(4, roomSize.y);
                    if (MakeRoom(newx + xmod, newy + ymod, roomSizeX, roomSizeY, validTile.Value))
                    {
                        currentFeatures++;

                        SetCell(newx, newy, Tile.Corridor);
                        SetCell(newx + xmod, newy + ymod, Tile.Floor);
                    }
                }
                else if (feature >= ChanceRoom)
                {
                    if (MakeCorridor(newx + xmod, newy + ymod, 6, validTile.Value))
                    {
                        currentFeatures++;

                        SetCell(newx, newy, Tile.Corridor);
                    }
                }
            }
        }

        return true;
    }

    void Initialize()
    {
		exitPlaced = false;
        for (int y = 0; y < availableVerticalSpace; y++)
        {
            for (int x = 0; x < availableHorizontalSpace; x++)
            {
                if (y < outerWallsThickness || y > availableVerticalSpace - outerWallsThickness || x < outerWallsThickness || x > availableHorizontalSpace - outerWallsThickness)
                {
                    SetCell(x, y, Tile.Wall);
                }
                else
                {
                    SetCell(x, y, Tile.Unused);
                }
            }
        }
    }

    public void SetupScene()
    {
        Initialize();
        CreateDungeon(Cols, Rows, Random.Range(WallsAmount.Min, WallsAmount.Max));
        ShowDungeon();
    }

    public class PointI
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }
    }

    private class Tuple
    {
        public PointI Item1;
        public Direction Item2;

        public Tuple(PointI pointI, Direction direction)
        {
            Item1 = pointI;
            Item2 = direction;
        }
    }

    private class TupleWithTile
    {
        public PointI Item1;
        public Direction Item2;
        public Tile Item3;

        public TupleWithTile(PointI pointI, Direction direction, Tile tile)
        {
            Item1 = pointI;
            Item2 = direction;
            Item3 = tile;
        }
    }

	public void Rebuild()
	{
		for (var i = 0; i < instanced.Count; ++i)
		{
			DestroyImmediate(instanced[i]);
		}
		SetupScene();
	}
}