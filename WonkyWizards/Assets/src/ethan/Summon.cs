using System;
using System.Collections;
using UnityEngine;
using TargetModeNamespace;

public class Summon : MonoBehaviour

{
	protected GameManager gm;

	//public static LevelManager lm;
	public static Vector3 gridCursorPoint;

	protected float cooldown;
	protected float timer;

	[SerializeField]
	protected GameObject projPrefab;
	[SerializeField]
	protected GameObject radarPrefab;
	protected SummonRadar summonRadar;

	protected int j, i;

	protected int health;

	protected targetingMode mode = targetingMode.WEAK;

	// init
	public virtual void Start()
	{
		// get singleton instance
		gm = GameManager.getSingleton();

		cooldown = 300f;
		timer = 0.0f;

		summonRadar = Instantiate(radarPrefab, gameObject.transform).GetComponent<SummonRadar>();

		health = this.getMaxHealth();
	}

	public virtual void FixedUpdate()
	{
		// tick up the timer
		timer += Time.deltaTime;

		Debug.Log(timer + ", " + summonRadar.getInRange());

		// check that cooldown is ready and that there is an enemy in range
		if (timer >= cooldown && summonRadar.getInRange())
		{
			// we have someting to target, so get it
			GameObject target = summonRadar.GetEnemy(mode);

			// summon a projectile with the apropriate target transform
			SummonProj(target.transform);

			// reset the cooldown timer
			timer = 0.0f;
		}
	}

	protected virtual void SummonProj(Transform target)
	{
		if (!target) Debug.Log("error, target set is null!");
		else Instantiate<GameObject>(projPrefab, transform.position, transform.rotation).GetComponent<SummonProj>().setTarget(target);
	}

	public virtual int getCost() { return 0; }

	public virtual int getMaxHealth() { return 0; }

	public virtual GameObject getProjectilePrefab() { return projPrefab; }


	public int getHealth ()
	{
		return this.health;
	}

	public void resetSummonHP () { this.health = getMaxHealth(); }

	public void takeDamage(int damage)
	{
		this.health -= damage;
		this.health = Math.Max(0, Math.Min(100, this.health));
	}

	public void takeHealing(int healing)
	{
		this.health += healing;
		this.health = Math.Max(0, Math.Min(100, this.health));
	}


	// Snaps a Vector3 to a grid
	public static Vector3 SnapOffset(Vector3 vec, Vector3 offset, float gridSize = 1.0f)
	{
		Vector3 snapped = vec + offset;
		snapped = new Vector3(
			Mathf.Round(snapped.x / gridSize) * gridSize,
			Mathf.Round(snapped.y / gridSize) * gridSize,
			Mathf.Round(snapped.z / gridSize) * gridSize);
		return snapped - offset;
	}

	// Returns whether or not the current grid plus one more spot is be placeable
	public static bool attemptPlacement(GameObject summon, Vector3 worldPos, int x, int y)
	{
		// get singleton copy
		GameManager gm = GameManager.getSingleton();
		if (gm == null)
		{
			Debug.Log("Singleton is null! Placement failed.");
			return false;
		}

		// check that the tile isn't unplaceable
		if (gm.getPlacementGrid()[x, y] == false)
		{
			Debug.Log("Position (" + x + "," + y + ") is not placeable.");
			return false;
		}

		// now test array with new pos added...

		// copy array to test on
		bool[,] newArray = gm.getPlacementGrid().Clone() as bool[,];
		//print2DArray(GameManager.getPlacementGrid());

		// add the proposed new position to the new array
		newArray[x, y] = false;

		// then test that the new grid is traversable
		if (isTraversable(newArray,
							LevelManager.getLevelRows(),
							LevelManager.getLevelCols(),
							LevelManager.getEnemySpawnPoint(),
							LevelManager.getLevelGoal()
		))
		{
			//Debug.Log("Traversability found.");

			// update the array in the GameManager
			gm.occupySpace(x, y);

			// instantiate the summon
			GameObject newSummon = Instantiate(summon, worldPos, Quaternion.identity);

			// set the new summon to be a child of the NavMesh (for David's pathfinding)
			newSummon.transform.parent = GameObject.FindGameObjectsWithTag("NavMesh")[0].transform;

			return true; // successfully placed tower
		}
		else
		{
			//Debug.Log("Intraversability error!");
			return false;
		}
	}

	public virtual void deleteSummon (int x, int y)
	{
		// get singleton copy
		GameManager gm = GameManager.getSingleton();

		gm.setSpaceAvailable(x, y);

		// delete this object
		Destroy(gameObject);
	}

	public void cycleTargetMode()
	{
		if (mode == targetingMode.LAST) mode = targetingMode.WEAK;
		else mode++;
	}
	public static bool isTraversable(bool[,] grid, int rows, int cols, Tuple<int, int> start, Tuple<int, int> goal)
	{
		//Debug.Log("rows: " + rows + ", cols: " + cols + ", start: " + start + ", goal: " + goal);

		// List of possible directions
		int[,] dir = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

		// Queue of spots to check
		Queue q = new Queue();

		// Add the spawn point to the queue first
		q.Enqueue(start);

		// Operate on the queue until it's empty
		while (q.Count > 0)
		{
			// Pull position from next queued item
			Tuple<int, int> p = (Tuple<int, int>)(q.Peek());
			// Then removes that item
			q.Dequeue();

			// Mark the position "traversed" (not traversable)
			grid[p.Item1, p.Item2] = false;

			// Check if the destination has been reached
			//Debug.Log(p);
			if (p.Equals(goal))
				return true;

			// Add neighboring locations to the queue
			for (int i = 0; i < 4; i++)
			{
				// Get position to check
				int a = p.Item1 + dir[i, 0];
				int b = p.Item2 + dir[i, 1];

				// Check that a,b is in bounds
				if (a >= 0 && b >= 0 &&
					a < rows && b < cols)
				{
					// create tuple for the spot we're considering
					Tuple<int, int> spot = new Tuple<int, int>(a, b);
					// check if the spot is the goal
					if (spot.Equals(goal)) return true;
					else if (grid[a, b] == true) q.Enqueue(new Tuple<int, int>(a, b));
				}
			}
		}

		// Goal position was never found
		return false;
	}
	protected static void print2DArray (bool[,] array)
	{
		//Debug.Log("Array:\n");
		String s;
		for (int i = 0; i < array.GetLength(0); i++)
		{
			s = "";
			s += "[";
			for (int j = 0; j < array.GetLength(1); j++)
			{
				s += array[i, j] ? "O" : "X";
				s += " ";
				//Console.Write((array[i, j] ? "1" : "X") + ", ");
			}
			s += "]\n";
			Debug.Log(s);
		}
	}
}