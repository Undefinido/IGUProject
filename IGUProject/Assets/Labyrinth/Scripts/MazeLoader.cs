using UnityEngine;
using System.Collections;

public class MazeLoader : MonoBehaviour {
    public static int mazeRows, mazeColumns;
	//public int mazeRows, mazeColumns;
	public GameObject wall, player, salida;
	public float size = 2f;

	private MazeCell[,] mazeCells;

	// Use this for initialization
	void Start () {
		InitializeMaze ();

		MazeAlgorithm ma = new HuntAndKillMazeAlgorithm (mazeCells);
		ma.CreateMaze (player, salida, size);
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void InitializeMaze() {

		mazeCells = new MazeCell[mazeRows,mazeColumns];

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				mazeCells [r, c] = new MazeCell ();

				// For now, use the same wall object for the floor!
				mazeCells [r, c] .floor = Instantiate (wall, new Vector3 (r*size, -(size/2f), c*size), Quaternion.identity) as GameObject;
				mazeCells [r, c] .floor.name = "Floor " + r + "," + c;
				mazeCells [r, c] .floor.transform.Rotate (Vector3.right, 90f);
                mazeCells[r, c].floor.transform.parent = GameObject.Find("Suelo").transform;

                if (c == 0) {
					mazeCells[r,c].westWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) - (size/2f)), Quaternion.identity) as GameObject;
					mazeCells [r, c].westWall.name = "West Wall " + r + "," + c;
                    mazeCells[r, c].westWall.transform.parent = GameObject.Find("Paredes").transform;
                }

				mazeCells [r, c].eastWall = Instantiate (wall, new Vector3 (r*size, 0, (c*size) + (size/2f)), Quaternion.identity) as GameObject;
				mazeCells [r, c].eastWall.name = "East Wall " + r + "," + c;
                mazeCells[r, c].eastWall.transform.parent = GameObject.Find("Paredes").transform;

                if (r == 0) {
					mazeCells [r, c].northWall = Instantiate (wall, new Vector3 ((r*size) - (size/2f), 0, c*size), Quaternion.identity) as GameObject;
					mazeCells [r, c].northWall.name = "North Wall " + r + "," + c;
					mazeCells [r, c].northWall.transform.Rotate (Vector3.up * 90f);
                    mazeCells[r, c].northWall.transform.parent = GameObject.Find("Paredes").transform;
                }

				mazeCells[r,c].southWall = Instantiate (wall, new Vector3 ((r*size) + (size/2f), 0, c*size), Quaternion.identity) as GameObject;
				mazeCells [r, c].southWall.name = "South Wall " + r + "," + c;
				mazeCells [r, c].southWall.transform.Rotate (Vector3.up * 90f);
                mazeCells[r, c].southWall.transform.parent = GameObject.Find("Paredes").transform;

                // Ceiling
                mazeCells [r,c].ceiling = Instantiate (wall, new Vector3((r * size), (size / 2f), c * size), Quaternion.identity) as GameObject;
                mazeCells [r, c].ceiling.name = "Ceiling " + r + "," + c;
                mazeCells[r, c].ceiling.transform.Rotate(Vector3.right, 90f);
                mazeCells[r,c].ceiling.transform.parent = GameObject.Find("Techo").transform;

                // Disable the render of the ceiling (so it's visible)
                Renderer render = mazeCells[r, c].ceiling.GetComponent<Renderer>();
                render.enabled = false;
            }
		}
	}
}
