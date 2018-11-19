using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    public float placementThreshold;    // chance of empty space
	public float placeTrap; // chance of space holding a trap
	public PathNode[,] g_maze;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;     
		placeTrap = 0.05f;               
    }

    public PathNode[,] FromDimensions(int sizeRows, int sizeCols)
    {
        PathNode[,] maze = new PathNode[sizeRows, sizeCols];
        
		int maxRows = maze.GetUpperBound(0);
    	int maxColumns = maze.GetUpperBound(1);

		for (int i = 0; i <= maxRows; i++) {
        	for (int j = 0; j <= maxColumns; j++) {
				maze[i, j] = new PathNode();
			}
		}

    	for (int i = 0; i <= maxRows; i++) {
        	for (int j = 0; j <= maxColumns; j++) {

            	if (i == 0 || j == 0 || i == maxRows || j == maxColumns) {
            		maze[i, j].data = 1;
					maze[i, j].walkable = false;
            	}
            	else if (i % 2 == 0 && j % 2 == 0) {
                	if (Random.value > placementThreshold) {
                    	
                    	maze[i, j].data = 1;
						maze[i, j].walkable = false;

                    	int x = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    	int y = x != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    	maze[i+x, j+y].data = 1;
						maze[i+x, j+y].walkable = false;
                	} 
            	}

				if (maze[i, j].data == 0 && Random.value < placeTrap) {
					Debug.Log("Trap");

					maze[i, j].data = 2;
					maze[i, j].trap = true;
				}
        	}
    	}

		g_maze = maze;

        return maze;
    }

	public bool validMaze(int startRow, int startColumn, int goalRow, int goalColumn) {
		bool valid = false;

		if (startRow == goalRow && startColumn == goalColumn) {
			valid = true;
		} else {
			if (g_maze[startRow - 1, startColumn].data == 0) {

			}
		}
		

		return valid;
	}
}

