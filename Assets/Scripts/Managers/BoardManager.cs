using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using SplitDirection = BoardManager.Board.BinarySpacePartitionTree.TreeNode.SplitDirection;
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
				return 0;
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
		public static Transform BoardHolder = new GameObject("Board").transform;
		public static List<Chamber> Chambers = new List<Chamber>();
		public static Vector2 Padding = new Vector2(3f, 3f);
		public static Vector2 StartPosition = Vector2.zero;

		public class Chamber
		{
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

			public Chamber(Vector2 A, Vector2 B)
			{
				this.A = A;
				this.B = B;
				CanSpawnEnemimes = true;
				Scheme = new List<List<Tile>>();
			}

			public virtual void Generate()
			{
				for (var i = A.y; i < B.y; ++i)
				{
					var row = new List<Tile>();
					for (var j = A.x; j < B.x; ++j)
					{
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
						if (!(finalPartitionsCount >= QuantityThreshold && Random.Range(0f, 1f) <= 0.2f))
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
		}

		public static void Generate()
		{
			Clear();
			Partitioner.Clear();
			Partitioner.MakePartition();
			foreach (var partition in Partitioner.ChamberPartitions)
			{
				if (Random.Range(0f, 1f) <= 0.2f)
				{
					continue;
				}
				var chamber = new Chamber(partition.A + Padding, partition.B - Padding);
				chamber.Generate();
				chamber.Modify();
				chamber.Instantiate();
				chamber.SpawnEnemies();
				Chambers.Add(chamber);
				StartPosition = chamber.Center;
			}
		}
	}
}