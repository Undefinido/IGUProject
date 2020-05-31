using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HuntAndKillMazeAlgorithm : MazeAlgorithm {

	private int currentRow = 0;
	private int currentColumn = 0;

	private bool courseComplete = false;

    public GameObject player, salida;
    public float size;

    // Adds all the dead ends to a list
    private List<KeyValuePair<int, int>> myList = new List<KeyValuePair<int, int>>();

    public HuntAndKillMazeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells) {
	}

	public override void CreateMaze (GameObject p, GameObject s, float sz) {
        player = p;
        salida = s;
        this.size = sz;

        HuntAndKill ();
	}

	private void HuntAndKill() {
		mazeCells [currentRow, currentColumn].visited = true;
        
		while (! courseComplete) {
			Kill(); // Will run until it hits a dead end.
			Hunt(); // Finds the next unvisited cell with an adjacent visited cell. If it can't find any, it sets courseComplete to true.
		}

        detectAllDeadEnds();
        createPlayerAndExit();
    }

	private void Kill() {
		while (RouteStillAvailable (currentRow, currentColumn)) {
			// int direction = Random.Range (1, 5);
			int direction = ProceduralNumberGenerator.GetNextNumber ();

			if (direction == 1 && CellIsAvailable (currentRow - 1, currentColumn)) {
				// North
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].northWall);
				DestroyWallIfItExists (mazeCells [currentRow - 1, currentColumn].southWall);
                mazeCells[currentRow, currentColumn].northWall = null;
                mazeCells[currentRow - 1, currentColumn].southWall = null;
                currentRow--;
			} else if (direction == 2 && CellIsAvailable (currentRow + 1, currentColumn)) {
				// South
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].southWall);
				DestroyWallIfItExists (mazeCells [currentRow + 1, currentColumn].northWall);
                mazeCells[currentRow + 1, currentColumn].northWall = null;
                mazeCells[currentRow, currentColumn].southWall = null;
                currentRow++;
			} else if (direction == 3 && CellIsAvailable (currentRow, currentColumn + 1)) {
				// east
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].eastWall);
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn + 1].westWall);
                mazeCells[currentRow, currentColumn].eastWall = null;
                mazeCells[currentRow, currentColumn + 1].westWall = null;
                currentColumn++;
			} else if (direction == 4 && CellIsAvailable (currentRow, currentColumn - 1)) {
				// west
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn].westWall);
				DestroyWallIfItExists (mazeCells [currentRow, currentColumn - 1].eastWall);
                mazeCells[currentRow, currentColumn - 1].eastWall = null;
                mazeCells[currentRow, currentColumn].westWall = null;
                currentColumn--;
			}

			mazeCells [currentRow, currentColumn].visited = true;
		}
	}

	private void Hunt() {
		courseComplete = true; // Set it to this, and see if we can prove otherwise below!

		for (int r = 0; r < mazeRows; r++) {
			for (int c = 0; c < mazeColumns; c++) {
				if (!mazeCells [r, c].visited && CellHasAnAdjacentVisitedCell(r,c)) {
					courseComplete = false; // Yep, we found something so definitely do another Kill cycle.
					currentRow = r;
					currentColumn = c;
					DestroyAdjacentWall (currentRow, currentColumn);
					mazeCells [currentRow, currentColumn].visited = true;
					return; // Exit the function
				}
			}
		}
	}


	private bool RouteStillAvailable(int row, int column) {
		int availableRoutes = 0;

		if (row > 0 && !mazeCells[row-1,column].visited) {
			availableRoutes++;
		}

		if (row < mazeRows - 1 && !mazeCells [row + 1, column].visited) {
			availableRoutes++;
		}

		if (column > 0 && !mazeCells[row,column-1].visited) {
			availableRoutes++;
		}

		if (column < mazeColumns-1 && !mazeCells[row,column+1].visited) {
			availableRoutes++;
		}

		return availableRoutes > 0;
	}

	private bool CellIsAvailable(int row, int column) {
		if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells [row, column].visited) {
			return true;
		} else {
			return false;
		}
	}

	private void DestroyWallIfItExists(GameObject wall) {
		if (wall != null) {
			GameObject.Destroy (wall);
		}
	}

	private bool CellHasAnAdjacentVisitedCell(int row, int column) {
		int visitedCells = 0;

		// Look 1 row up (north) if we're on row 1 or greater
		if (row > 0 && mazeCells [row - 1, column].visited) {
			visitedCells++;
		}

		// Look one row down (south) if we're the second-to-last row (or less)
		if (row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
			visitedCells++;
		}

		// Look one row left (west) if we're column 1 or greater
		if (column > 0 && mazeCells [row, column - 1].visited) {
			visitedCells++;
		}

		// Look one row right (east) if we're the second-to-last column (or less)
		if (column < (mazeColumns-2) && mazeCells [row, column + 1].visited) {
			visitedCells++;
		}

		// return true if there are any adjacent visited cells to this one
		return visitedCells > 0;
	}

	private void DestroyAdjacentWall(int row, int column) {
		bool wallDestroyed = false;

		while (!wallDestroyed) {
			// int direction = Random.Range (1, 5);
			int direction = ProceduralNumberGenerator.GetNextNumber ();

			if (direction == 1 && row > 0 && mazeCells [row - 1, column].visited) {
				DestroyWallIfItExists (mazeCells [row, column].northWall);
				DestroyWallIfItExists (mazeCells [row - 1, column].southWall);
                mazeCells[row, column].northWall = null;
                mazeCells[row - 1, column].southWall = null;
                wallDestroyed = true;
			} else if (direction == 2 && row < (mazeRows-2) && mazeCells [row + 1, column].visited) {
				DestroyWallIfItExists (mazeCells [row, column].southWall);
				DestroyWallIfItExists (mazeCells [row + 1, column].northWall);
                mazeCells[row + 1, column].northWall = null;
                mazeCells[row, column].southWall = null;
                wallDestroyed = true;
			} else if (direction == 3 && column > 0 && mazeCells [row, column-1].visited) {
				DestroyWallIfItExists (mazeCells [row, column].westWall);
				DestroyWallIfItExists (mazeCells [row, column-1].eastWall);
                mazeCells[row, column].westWall = null;
                mazeCells[row, column - 1].eastWall = null;
                wallDestroyed = true;
			} else if (direction == 4 && column < (mazeColumns-2) && mazeCells [row, column+1].visited) {
				DestroyWallIfItExists (mazeCells [row, column].eastWall);
				DestroyWallIfItExists (mazeCells [row, column+1].westWall);
                mazeCells[row, column + 1].westWall = null;
                mazeCells[row, column].eastWall = null;
                wallDestroyed = true;
			}
		}

	}

    // Busca todos los caminos sin salida
    private void detectAllDeadEnds() {

        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                if (checkIfIsDeadEnd(r,c) == true)
                {
                    //Debug.Log("Dead end en r= " + r + " c= " + c);
                    myList.Add(new KeyValuePair<int, int>(r, c));
                }
            }
        }
    }
    
    // Comprueba si es un camino sin salida
    private bool checkIfIsDeadEnd(int row, int column) {
        int numberOfWalls = 0;        

        // North
        if (row == 0 || mazeCells[row - 1, column].southWall != null || mazeCells[row,column].northWall != null)
        {
            numberOfWalls++;
        }

        // South
        if (mazeCells[row, column].southWall != null)
        {
            numberOfWalls++;
        }

        // West
        if (column == 0 || mazeCells[row, column - 1].eastWall != null || mazeCells[row,column].westWall != null)
        {
            numberOfWalls++;
        }

        // East
        if (mazeCells[row, column].eastWall != null)
        {
            numberOfWalls++;
        }
        return numberOfWalls == 3;
        
    }

    // Searches for the longest distance between two dead-ends and places player and exit (Euclidean Distance)
    private void createPlayerAndExit() {

        float maxValue = 0;
        float currentValue = 0;
        KeyValuePair<int, int> a = new KeyValuePair<int, int>();
        KeyValuePair<int, int> b = new KeyValuePair<int, int>();
        foreach (var item in myList)
        {
            int x1 = item.Key;  //row
            int y1 = item.Value;    //column

            foreach (var item2 in myList)
            {
                int x2 = item2.Key;
                int y2 = item2.Value;

                currentValue = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
                if (currentValue > maxValue)
                {
                    a = new KeyValuePair<int, int>(x1, y1);
                    b = new KeyValuePair<int, int>(x2, y2);
                    maxValue = currentValue;
                }
            }
        }
        
        GameObject pl = GameObject.Instantiate(player, new Vector3(a.Key * size, 0, a.Value * size), Quaternion.identity);
        pl.name = player.name;
        pl.transform.parent = GameObject.Find("Lab").transform;

        GameObject sl = GameObject.Instantiate(salida, new Vector3(b.Key * size, 0, b.Value * size), Quaternion.identity);
        sl.name = salida.name;
        sl.transform.parent = GameObject.Find("Lab").transform;

        /* RANDOMIZER
         Coloca la bola en la primera fila en una posicion aleatoria
         Coloca la salida en la ultima fila en una posicion aleatoria
         */
        //int posX_player = Random.Range(0, mazeRows);
        //int posX_Salida = Random.Range(0, mazeColumns);

        //GameObject pl = GameObject.Instantiate(player, new Vector3(posX_player * size, 0, 0), Quaternion.identity);
        //pl.name = player.name;
        //pl.transform.parent = GameObject.Find("Lab").transform;

        //GameObject sl = GameObject.Instantiate(salida, new Vector3(posX_Salida * size, 0, (mazeRows - 1) * size), Quaternion.identity);
        //sl.name = salida.name;
        //sl.transform.parent = GameObject.Find("Lab").transform;

        //set finishWindow attached to salida script
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name.Equals("SalidaLabWindow"))
                sl.transform.GetComponentInChildren<SalidaLab>().finishWindow = go;

            if(go.name.Equals("HasGanadoWindow"))
                sl.transform.GetComponentInChildren<SalidaLab>().youWonWindow = go;
        }
    }
}
