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
		placeTrap = 0.075f;               
    }

    public PathNode[,] FromDimensions(int sizeRows, int sizeCols)
    {
        PathNode[,] maze = new PathNode[sizeRows, sizeCols];
        
		int maxRows = maze.GetUpperBound(0);
    	int maxColumns = maze.GetUpperBound(1);

		for (int i = 0; i <= maxRows; i++) {
        	for (int j = 0; j <= maxColumns; j++) {
				maze[i, j] = new PathNode();
				maze[i, j].row = i;
				maze[i, j].column = j;
			}
		}

    	for (int i = 0; i <= maxRows; i++) {
        	for (int j = 0; j <= maxColumns; j++) {

            	if (i == 0 || j == 0 || i == maxRows || j == maxColumns) {
            		maze[i, j].data = 1;
					maze[i, j].debug = "==";
            	}
            	else if (i % 2 == 0 && j % 2 == 0) {
                	if (Random.value > placementThreshold) {
                    	
                    	maze[i, j].data = 1;

                    	int x = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    	int y = x != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    	maze[i+x, j+y].data = 1;
						maze[i+x, j+y].debug = "==";
                	} 
            	}

				if (maze[i, j].data == 0) { 
					maze[i, j].walkable = true;
					maze[i, j].debug = "....";
					if (Random.value < placeTrap) {
						maze[i, j].data = 2;
						maze[i, j].trap = true;
						maze[i, j].debug = "trap";
					}
				}
        	}
    	}

		g_maze = maze;

        return maze;
    }

	public bool validMaze(int startRow, int startColumn, int goalRow, int goalColumn) {
		bool valid = false;

		List<PathNode> path = new List<PathNode>();

		path.Add(g_maze[startRow, startColumn]);

		while(path.Count > 0) {
			//Debug.Log("current pos: " + path[0].row + ", " + path[0].column);
			path[0].visited = true;

			if (path[0].row == goalRow && path[0].column == goalColumn) {
				valid = true;
				path[0].debug = "go";
			} else {
				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						PathNode neighbor = g_maze[path[0].row + i, path[0].column + j];
						//Debug.Log("Neighbor at " + neighbor.row + ", " + neighbor.column);

						if (((i == -1 && j == -1) || (i == 1 && j == -1)) && g_maze[path[0].row, path[0].column - 1].walkable) { // if neighbor is northwest or southwest and no wall to the left of player
							if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) {
								//Debug.Log("Adding neighbor");
								path.Add(neighbor);
								neighbor.debug = "22";
							}
						} else if (((i == -1 && j == 1) || (i == 1 && j == 1)) && g_maze[path[0].row, path[0].column + 1].walkable) { // if neighbor is northeast or southeast and no wall to the right of player
							if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) {
								//Debug.Log("Adding neighbor");
								path.Add(neighbor);
								neighbor.debug = "22";
							}
						} else {
							if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) { // all other neighbors
								//Debug.Log("Adding neighbor");
								path.Add(neighbor);
								neighbor.debug = "22";
							}
						}
					}
				}
					//Debug.Log("looking for path");
			}
			//Debug.Log("");
			path.Remove(path[0]);

			if (valid) {
				break;
			}
		}

		return valid;
	}
}

