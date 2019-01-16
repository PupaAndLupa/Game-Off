using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using SplitDirection = BoardManager.Board.BinarySpacePartitionTree.TreeNode.SplitDirection;
using ChamberWall = BoardManager.Board.Chamber.Wall;
using DigDirection = BoardManager.Board.DigDirection;
using System.Security.Cryptography;

static class DirectionExtensions
{
	public static SplitDirection Toggle(this SplitDirection direction)
	{
		switch (direction)
		{
			case SplitDirection.Vertical:
				return SplitDirection.Horizontal;
			case SplitDirection.Horizontal:
				return SplitDirection.Vertical;
			default:
				return SplitDirection.Any;
		}
	}
}

static class ChamberWallExtensions
{
	public static ChamberWall Inverse(this ChamberWall wall)
	{
		switch (wall)
		{
			case ChamberWall.Top:
				return ChamberWall.Bottom;
			case ChamberWall.Right:
				return ChamberWall.Left;
			case ChamberWall.Bottom:
				return ChamberWall.Top;
			case ChamberWall.Left:
				return ChamberWall.Right;
			default:
				return ChamberWall.Any;
		}
	}

	public static ChamberWall Random(this ChamberWall wall)
	{
		var types = new List<ChamberWall> { ChamberWall.Top, ChamberWall.Bottom, ChamberWall.Left, ChamberWall.Right };
		return types[UnityEngine.Random.Range(0, types.Count)];
	}
}

static class DigDirectionExtensions
{
	public static DigDirection Inverse(this DigDirection direction)
	{
		switch (direction)
		{
			case DigDirection.Up:
				return DigDirection.Down;
			case DigDirection.Right:
				return DigDirection.Left;
			case DigDirection.Down:
				return DigDirection.Up;
			case DigDirection.Left:
				return DigDirection.Right;
			default:
				return DigDirection.Any;
		}
	}
}

static class IEnumerableExtensions
{
	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
	{
		IList<T> list = enumerable.ToList();
		RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
		int n = list.Count;
		while (n > 1)
		{
			byte[] box = new byte[1];
			do provider.GetBytes(box);
			while (!(box[0] < n * (Byte.MaxValue / n)));
			int k = (box[0] % n--);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
		enumerable = list.AsEnumerable();
		return enumerable;
	}
}

public class BoardManager : MonoBehaviour
{
	public GameObject BoardHolder;

    public Vector2 RandomFreePosition()
    {
        var chamber = Board.Chambers[Random.Range(0, Board.Chambers.Count)];
        var row = chamber.Scheme[Random.Range(1, chamber.Scheme.Count - 1)];
        return row[Random.Range(1, row.Count - 1)].Position;
    }

	public static class LevelDesign
	{
		public class SpritePool
		{
			private string pathPrefix;
			private string targetDir;
			private GameObject[] prefabs;

			public int Length
			{
				get { return prefabs.Length; }
			}

			public SpritePool(string pathPrefix, string targetDir)
			{
				this.pathPrefix = pathPrefix;
				this.targetDir = targetDir;
			}

			public GameObject this[int index]
			{
				get { return prefabs[index]; }
				set { prefabs[index] = value; }
			}

			public void Load(string id)
			{
				prefabs = Resources.LoadAll(pathPrefix + id + targetDir).Cast<GameObject>().ToArray();
			}

			public GameObject GetRandom()
			{
				return prefabs[Random.Range(0, prefabs.Length)];
			}
		}

		public static SpritePool Floor = new SpritePool("Board/Levels/", "/Floor");
		public static SpritePool Wall  = new SpritePool("Board/Levels/", "/Wall");
		public static SpritePool WallEdgeNorth = new SpritePool("Board/Levels/", "/WallEdgeNorth");
		public static SpritePool WallEdgeEast  = new SpritePool("Board/Levels/", "/WallEdgeEast");
		public static SpritePool WallEdgeSouth = new SpritePool("Board/Levels/", "/WallEdgeSouth");
		public static SpritePool WallEdgeWest  = new SpritePool("Board/Levels/", "/WallEdgeWest");
		public static SpritePool WallCornerNorthEast = new SpritePool("Board/Levels/", "/WallCornerNorthEast");
		public static SpritePool WallCornerSouthEast = new SpritePool("Board/Levels/", "/WallCornerSouthEast");
		public static SpritePool WallCornerSouthWest = new SpritePool("Board/Levels/", "/WallCornerSouthWest");
		public static SpritePool WallCornerNorthWest = new SpritePool("Board/Levels/", "/WallCornerNorthWest");
		public static SpritePool DoorClosedHorizontal = new SpritePool("Board/Levels/", "/DoorClosedHorizontal");
		public static SpritePool DoorOpenedHorizontal = new SpritePool("Board/Levels/", "/DoorOpenedHorizontal");
		public static SpritePool DoorClosedVertical   = new SpritePool("Board/Levels/", "/DoorClosedVertical");
		public static SpritePool DoorOpenedVertical   = new SpritePool("Board/Levels/", "/DoorOpenedVertical");

		public static void Load(string id)
		{
			Floor.Load(id);
			Wall.Load(id);
			WallEdgeNorth.Load(id);
			WallEdgeEast.Load(id);
			WallEdgeSouth.Load(id);
			WallEdgeWest.Load(id);
			WallCornerNorthEast.Load(id);
			WallCornerSouthEast.Load(id);
			WallCornerSouthWest.Load(id);
			WallCornerNorthWest.Load(id);
			DoorClosedHorizontal.Load(id);
			DoorOpenedHorizontal.Load(id);
			DoorClosedVertical.Load(id);
			DoorOpenedVertical.Load(id);
		}
	}

	public enum TileType
	{
		None,
		Floor,
		Wall,
		WallEdgeNorth,
		WallEdgeEast,
		WallEdgeSouth,
		WallEdgeWest,
		WallCornerNorthEast,
		WallCornerSouthEast,
		WallCornerSouthWest,
		WallCornerNorthWest,
		DoorHorizontal,
		DoorVertical,
	}

	private static Dictionary<TileType, Func<Vector3, GameObject>> instancer = new Dictionary<TileType, Func<Vector3, GameObject>> {
		{ TileType.None,                position => { return null; } },
		{ TileType.Floor,               position => { return Instantiate(LevelDesign.Floor.GetRandom(), position, Quaternion.identity);                } },
		{ TileType.Wall,                position => { return Instantiate(LevelDesign.Wall.GetRandom(), position, Quaternion.identity);                 } },
		{ TileType.WallEdgeNorth,       position => { return Instantiate(LevelDesign.WallEdgeNorth.GetRandom(), position, Quaternion.identity);        } },
		{ TileType.WallEdgeEast,        position => { return Instantiate(LevelDesign.WallEdgeEast.GetRandom(), position, Quaternion.identity);         } },
		{ TileType.WallEdgeSouth,       position => { return Instantiate(LevelDesign.WallEdgeSouth.GetRandom(), position, Quaternion.identity);        } },
		{ TileType.WallEdgeWest,        position => { return Instantiate(LevelDesign.WallEdgeWest.GetRandom(), position, Quaternion.identity);         } },
		{ TileType.WallCornerNorthEast, position => { return Instantiate(LevelDesign.WallCornerNorthEast.GetRandom(), position, Quaternion.identity);  } },
		{ TileType.WallCornerSouthEast, position => { return Instantiate(LevelDesign.WallCornerSouthEast.GetRandom(), position, Quaternion.identity);  } },
		{ TileType.WallCornerSouthWest, position => { return Instantiate(LevelDesign.WallCornerSouthWest.GetRandom(), position, Quaternion.identity);  } },
		{ TileType.WallCornerNorthWest, position => { return Instantiate(LevelDesign.WallCornerNorthWest.GetRandom(), position, Quaternion.identity);  } },
		{ TileType.DoorHorizontal,      position => { return Instantiate(LevelDesign.DoorClosedHorizontal.GetRandom(), position, Quaternion.identity); } },
		{ TileType.DoorVertical,        position => { return Instantiate(LevelDesign.DoorClosedVertical.GetRandom(), position, Quaternion.identity);   } },
	};

	public static class Board
	{
		public enum DigDirection
		{
			Any,
			Up,
			Right,
			Down,
			Left
		}

		public static Transform BoardHolder = new GameObject("Board").transform;
		public static List<Chamber> Chambers = new List<Chamber>();
		public static Vector2 Padding = new Vector2(3f, 3f);
		public static Vector2 StartPosition = Vector2.zero;
		public static Dictionary<string, bool> AssignedPositionsMap = new Dictionary<string, bool>();
		public static Dictionary<string, Vector2> PathwaysPositionsMap = new Dictionary<string, Vector2>();
		public static List<GameObject> InstancedPathways = new List<GameObject>();

		public static void SetBoardHolder(GameObject boardHolder)
		{
			BoardHolder = Instantiate(boardHolder, new Vector3(0, 0, 0), Quaternion.identity).transform;
		}

		public class Chamber
		{
			public enum Wall
			{
				Any,
				Top,
				Right,
				Bottom,
				Left
			}

			public class Tile
			{
				public TileType Type;
				public Vector2 Position;

				public Tile(TileType type, Vector2 position)
				{
					Type = type;
					Position = position;
				}
			}

			protected bool CanSpawnEnemimes { get; set; }

			public List<List<Tile>> Scheme { get; private set; }
			public Vector2 A { get; private set; }
			public Vector2 B { get; private set; }
			public Vector2 Center
			{
				get { return (A + B) / 2; }
			}
			public List<GameObject> InstancedTiles { get; private set; }
			public List<Chamber> Connections { get; private set; }

			public Chamber(Vector2 A, Vector2 B)
			{
				this.A = A;
				this.B = B;
				CanSpawnEnemimes = true;
				Scheme = new List<List<Tile>>();
				Connections = new List<Chamber>();
			}

			public Vector2 RandomIndexAtWall(Wall wall)
			{
				switch (wall)
				{
					case Wall.Top:
						return new Vector2(Random.Range(1, Scheme[0].Count - 1), 0);
					case Wall.Right:
						return new Vector2(Scheme[0].Count - 1, Random.Range(1, Scheme.Count - 1));
					case Wall.Bottom:
						return new Vector2(Random.Range(1, Scheme[0].Count - 1), Scheme.Count - 1);
					case Wall.Left:
						return new Vector2(0, Random.Range(1, Scheme.Count - 1));
					default:
						return Vector2.zero;
				}
			}

			public virtual void Generate()
			{
				for (var i = A.y; i < B.y; ++i)
				{
					var row = new List<Tile>();
					for (var j = A.x; j < B.x; ++j)
					{
						AssignedPositionsMap[new Vector2(j, i).ToString()] =  true;
						if (i == A.y && j == A.x)
						{
							row.Add(new Tile(TileType.WallCornerNorthWest, new Vector2(j, i)));
						}
						else if (i == A.y && j == B.x - 1)
						{
							row.Add(new Tile(TileType.WallCornerNorthEast, new Vector2(j, i)));
						}
						else if (i == B.y - 1 && j == A.x)
						{
							row.Add(new Tile(TileType.WallCornerSouthWest, new Vector2(j, i)));
						}
						else if (i == B.y - 1 && j == B.x - 1)
						{
							row.Add(new Tile(TileType.WallCornerSouthEast, new Vector2(j, i)));
						}
						else if (i == A.y)
						{
							row.Add(new Tile(TileType.WallEdgeNorth, new Vector2(j, i)));
						}
						else if (j == A.x)
						{
							row.Add(new Tile(TileType.WallEdgeWest, new Vector2(j, i)));
						}
						else if (i == B.y - 1)
						{
							row.Add(new Tile(TileType.WallEdgeSouth, new Vector2(j, i)));
						}
						else if (j == B.x - 1)
						{
							row.Add(new Tile(TileType.WallEdgeEast, new Vector2(j, i)));
						}
						else
						{
							row.Add(new Tile(TileType.Floor, new Vector2(j, i)));
						}
					}
					Scheme.Add(row);
				}
			}

			public virtual void Modify()
			{

			}

			public virtual void SpawnEnemies()
			{

			}

			public void Instantiate()
			{
				InstancedTiles = new List<GameObject>();
				foreach (var row in Scheme)
				{
					foreach (var tile in row)
					{
						var instanced = instancer[tile.Type].Invoke(tile.Position);
						if (instanced != null)
						{
							instanced.transform.SetParent(BoardHolder);
							InstancedTiles.Add(instanced);
						}
					}
				}
			}

			public void Destroy()
			{
				for (var i = 0; i < InstancedTiles.Count; ++i)
				{
					UnityEngine.Object.Destroy(InstancedTiles[i]);
				}
				InstancedTiles.Clear();
			}
		}

		public class Start : Chamber
		{
			public Start(Vector2 A, Vector2 B) : base(A, B)
			{
				CanSpawnEnemimes = false;
			}

			public override void Generate()
			{

			}

			public override void Modify()
			{

			}
		}

		public class Shop : Chamber
		{
			public Shop(Vector2 A, Vector2 B) : base(A, B)
			{
				CanSpawnEnemimes = false;
			}

			public override void Generate()
			{

			}

			public override void Modify()
			{

			}
		}

		public class Treasury : Chamber
		{
			public Treasury(Vector2 A, Vector2 B) : base(A, B)
			{
				CanSpawnEnemimes = false;
			}

			public override void Generate()
			{

			}

			public override void Modify()
			{

			}
		}

		public class BinarySpacePartitionTree
		{
			public class TreeNode : IEnumerable<TreeNode>, IDisposable
			{
				public enum SplitDirection
				{
					Any,
					Vertical,
					Horizontal
				}

				private bool isDisposed = false;
				SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

				public Vector2 A { get; private set; }
				public Vector2 B { get; private set; }
				public TreeNode Parent { get; private set; }
				public List<TreeNode> Children { get; private set; }

				public int Width
				{
					get { return Math.Abs((int)(B.x - A.x)); }
				}

				public int Height
				{
					get { return Math.Abs((int)(B.y - A.y)); }
				}

				public int Area
				{
					get { return Width * Height; }
				}

				public TreeNode(TreeNode parent, Vector2 A, Vector2 B)
				{
					Parent = parent;
					Children = new List<TreeNode>();
					this.A = A;
					this.B = B;
				}

				public List<TreeNode> Split(SplitDirection direction, float ratio)
				{
					Func<float, int> division = v => (int)Math.Floor((v * ratio));
					switch (direction)
					{
						case SplitDirection.Vertical:
							Children = new List<TreeNode> {
								new TreeNode(this, A, B - new Vector2(Width - division(Width), 0f)),
								new TreeNode(this, A + new Vector2(division(Width), 0f), B)
							};
							break;
						case SplitDirection.Horizontal:
							Children = new List<TreeNode> {
								new TreeNode(this, A, B - new Vector2(0f, Height - division(Height))),
								new TreeNode(this, A + new Vector2(0f, division(Height)), B)
							};
							break;
					}
					return Children;
				}

				public bool IsLast()
				{
					return Children.Count == 0;
				}

				public void Clear()
				{
					for (var i = 0; i < Children.Count; ++i)
					{
						Children[i].Clear();
						Children[i].Dispose();
					}
				}

				public IEnumerable<TreeNode> GetFurthestDescendants()
				{
					if (IsLast())
					{
						yield return this;
					}
					else
					{
						foreach (var child in Children)
						{
							foreach (var node in child.GetFurthestDescendants())
							{
								yield return node;
							}
						}
					}
				}

				public IEnumerator<TreeNode> GetEnumerator()
				{
					return Children.GetEnumerator();
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}

				public void Dispose()
				{
					Dispose(true);
					GC.SuppressFinalize(this);
				}

				protected virtual void Dispose(bool disposing)
				{
					if (isDisposed)
					{
						return;
					}

					if (disposing) {
						handle.Dispose();
					}

					isDisposed = true;
				}
			}

			private static int finalPartitionsCount = 0;

			public TreeNode Root { get; private set; }
			public int AreaThreshold { get; set; }
			public int QuantityThreshold { get; set; }

			private List<TreeNode> chamberPartitions;
			public List<TreeNode> ChamberPartitions
			{
				get
				{
					if (chamberPartitions == null)
					{
						chamberPartitions = new List<TreeNode>();
						foreach (var node in Root.GetFurthestDescendants())
						{
							chamberPartitions.Add(node);
						}
					}
					return chamberPartitions;
				}
			}

			public BinarySpacePartitionTree(int width, int height, int areaThreshold, int quantityThreshold)
			{

				Root = new TreeNode(null, Vector2.zero, new Vector2(width, height));
				AreaThreshold = areaThreshold;
				QuantityThreshold = quantityThreshold;
			}

			public void MakePartition()
			{
				Split(Root, SplitDirection.Vertical);
			}

			private void Split(TreeNode node, SplitDirection direction)
			{
				var newDirection = direction.Toggle();
				foreach (var child in node.Split(direction, Random.Range(0.45f, 0.55f)))
				{
					if (child.Area >= 1.8f * AreaThreshold)
					{
                        if (!(finalPartitionsCount >= QuantityThreshold && Random.Range(0f, 1f) <= 0.00001f))
                        {
                            Split(child, newDirection);
                        }
                    }
					else
					{
						++finalPartitionsCount;
					}
				}
			}

			public void Clear()
			{
				Root.Clear();
				chamberPartitions = null;
				finalPartitionsCount = 0;
			}
		}

		public static BinarySpacePartitionTree Partitioner = new BinarySpacePartitionTree(128, 128, 256, 30);

		public static void LoadLevel(string id)
		{
			LevelDesign.Load(id);
		}

		public static void Clear()
		{
			foreach (var chamber in Chambers)
			{
				chamber.Destroy();
			}
			AssignedPositionsMap.Clear();
			PathwaysPositionsMap.Clear();
			for (var i = 0; i < InstancedPathways.Count; ++i)
			{
				Destroy(InstancedPathways[i]);
			}
			InstancedPathways.Clear();
		}

		public static void Generate()
		{
			Clear();
			Partitioner.Clear();
			Partitioner.MakePartition();
			Debug.Log(AssignedPositionsMap);
			foreach (var partition in Partitioner.ChamberPartitions)
			{
				var chamber = new Chamber(partition.A + Padding, partition.B - Padding);
				chamber.Generate();
				chamber.Modify();
				Chambers.Add(chamber);
			}
			for (var i = 0; i < Chambers.Count - 1; ++i)
			{
				Connect(Chambers[i], Chambers[i + 1]);
			}
			foreach(var chamber in Chambers)
			{
				chamber.Instantiate();
				chamber.SpawnEnemies();
				StartPosition = chamber.Center;
			}
			foreach (var point in PathwaysPositionsMap)
			{
				var surroundings = new List<Vector2>
				{
					point.Value + new Vector2(0, 1),
					point.Value + new Vector2(1, 1),
					point.Value + new Vector2(1, 0),
					point.Value + new Vector2(1, -1),
					point.Value + new Vector2(0, -1),
					point.Value + new Vector2(-1, -1),
					point.Value + new Vector2(-1, 0),
					point.Value + new Vector2(-1, 1),
				};

				foreach (var area in surroundings)
				{
					var value = false;
					var pos = Vector2.zero;
					if (
						!AssignedPositionsMap.TryGetValue(area.ToString(), out value) && 
						!PathwaysPositionsMap.TryGetValue(area.ToString(), out pos)
					)
					{
						var wall = instancer[TileType.Wall].Invoke(area);
						if (wall != null)
						{
							wall.transform.SetParent(BoardHolder);
							InstancedPathways.Add(wall);
						}
					}
				}

				var floor = instancer[TileType.Floor].Invoke(point.Value);
				if (floor != null)
				{
					floor.transform.SetParent(BoardHolder);
					InstancedPathways.Add(floor);
				}
			}
			AstarPath.active.Scan();
		}

		public static void Connect(Chamber first, Chamber second)
		{
			if (first.Connections.Contains(second))
			{
				return;
			}
			ChamberWall doorPlacement = ChamberWall.Left;
			var firstDoorIndex = first.RandomIndexAtWall(doorPlacement);
			var secondDoorIndex = second.RandomIndexAtWall(doorPlacement.Inverse());
			var firstDoorPos = first.Scheme[(int)firstDoorIndex.y][(int)firstDoorIndex.x].Position;
			var secondDoorPos = second.Scheme[(int)secondDoorIndex.y][(int)secondDoorIndex.x].Position;
			first.Scheme[(int)firstDoorIndex.y][(int)firstDoorIndex.x] = new Chamber.Tile(TileType.Floor, firstDoorPos);
			second.Scheme[(int)secondDoorIndex.y][(int)secondDoorIndex.x] = new Chamber.Tile(TileType.Floor, secondDoorPos);

			var direction = ChamberWallToDigDirection(doorPlacement);
			var directionDest = ChamberWallToDigDirection(doorPlacement.Inverse());
			var path = new Dictionary<string, Vector2>();

			var currentPos = firstDoorPos;
			for (var i = 0; i < Padding.x; ++i)
			{
				currentPos = Dig(currentPos, direction);
				path.Add(currentPos.ToString(), currentPos);
			}
			var destinationPos = secondDoorPos;
			for (var i = 0; i < Padding.x; ++i)
			{
				destinationPos = Dig(destinationPos, directionDest);
				path.Add(destinationPos.ToString(), destinationPos);
			}
			MakePathway(currentPos, destinationPos, path, direction);
			foreach (var point in path)
			{
				PathwaysPositionsMap[point.Key] = point.Value;
			}
			first.Connections.Add(second);
			second.Connections.Add(first);
		}

		public static DigDirection ChamberWallToDigDirection(ChamberWall wall)
		{
			switch (wall)
			{
				case ChamberWall.Top:
					return DigDirection.Up;
				case ChamberWall.Right:
					return DigDirection.Right;
				case ChamberWall.Bottom:
					return DigDirection.Down;
				case ChamberWall.Left:
					return DigDirection.Left;
				default:
					return DigDirection.Any;
			}
		}

		public static void MakePathway(Vector2 currentPos, Vector2 destinationPos, Dictionary<string, Vector2> path, DigDirection direction)
		{
			var newDirection = direction;
			if (direction == DigDirection.Left || direction == DigDirection.Right)
			{
				if (currentPos.y < destinationPos.y)
				{
					newDirection =
						CheckDirectionAccessibility(currentPos, DigDirection.Up) ?
						DigDirection.Up :
						direction;
				}
				else if (currentPos.y > destinationPos.y)
				{
					newDirection =
						CheckDirectionAccessibility(currentPos, DigDirection.Down) ?
						DigDirection.Down :
						direction;
				}
			}
			else
			{
				if (currentPos.x < destinationPos.x)
				{
					newDirection =
						CheckDirectionAccessibility(currentPos, DigDirection.Right) ?
						DigDirection.Right :
						direction;
				}

				if (currentPos.x > destinationPos.x)
				{
					newDirection =
						CheckDirectionAccessibility(currentPos, DigDirection.Left) ?
						DigDirection.Left :
						direction;
				}
			}
			
			if (CheckDirectionAccessibility(currentPos, newDirection))
			{
				var newPos = Dig(currentPos, newDirection);
				path[newPos.ToString()] = newPos;
				if (!(newPos.x == destinationPos.x && newPos.y == destinationPos.y))
				{
					MakePathway(newPos, destinationPos, path, newDirection);
				}
			}
		}

		public static bool CheckDirectionAccessibility(Vector2 pos, DigDirection direction)
		{
			Vector2 shift = Vector2.zero;
			switch (direction)
			{
				case DigDirection.Left:
					shift.x = -1;
					break;
				case DigDirection.Right:
					shift.x = 1;
					break;
				case DigDirection.Up:
					shift.y = 1;
					break;
				case DigDirection.Down:
					shift.y = -1;
					break;
			}
			var value = true;
			for (var i = 1; i <= Padding.x + 1; ++i)
			{
				if (AssignedPositionsMap.TryGetValue((pos + i * shift).ToString(), out value))
				{
					return false;
				}
			}
			return true;
		}

		public static Vector2 Dig(Vector2 currentPos, DigDirection direction)
		{
			switch (direction)
			{
				case DigDirection.Up:
					currentPos.y += 1;
					break;
				case DigDirection.Right:
					currentPos.x += 1;
					break;
				case DigDirection.Down:
					currentPos.y -= 1;
					break;
				case DigDirection.Left:
					currentPos.x -= 1;
					break;
			}
			return currentPos;
		} 
	}
}