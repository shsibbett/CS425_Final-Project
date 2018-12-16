using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    public float placementThreshold;    // chance of empty space
	public PathNode[,] g_maze;

    public MazeDataGenerator()
    {
        placementThreshold = .1f;                    
    }

    public PathNode[,] FromDimensions(int sizeRows, int sizeCols, float trapChance)
    {
        PathNode[,] maze = new PathNode[sizeRows, sizeCols];
        
		int maxRows = maze.GetUpperBound(0);
    	int maxColumns = maze.GetUpperBound(1);

		for (int i = 0; i <= maxRows; i++) {
        	for (int j = 0; j <= maxColumns; j++) {
				maze[i, j] = new PathNode();
				maze[i, j].position = new Vector3 (j * 4, 0, i * 4);
				maze[i, j].row = i;
				maze[i, j].column = j;
			}
		}

    	for (int i = 0; i <= maxRows; i++) {
        	for (int j = 0; j <= maxColumns; j++) {

            	if (i == 0 || j == 0 || i == maxRows || j == maxColumns) { // borders
            		maze[i, j].data = 1;
					maze[i, j].debug = "==";
            	}
            	else if (i % 2 == 0 && j % 2 == 0) { // wall tile chance
                	if (Random.value > placementThreshold) {
                    	
                    	maze[i, j].data = 1;

                    	int x = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                    	int y = x != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                    	maze[i+x, j+y].data = 1;
						maze[i+x, j+y].debug = "==";
                	} 
            	} else if (Random.value < trapChance) { // trap tile chance
						maze[i, j].data = 2;
						maze[i, j].trap = true;
						maze[i, j].debug = ";;;;";
				} else { // walkable tile
						maze[i, j].data = 0;
						maze[i, j].walkable = true;
						maze[i, j].debug = "....";
				}	
        	}
    	}

		g_maze = maze;

        return maze;
    }

	public bool validMaze(int startRow, int startColumn, int goalRow, int goalColumn) { // checks if generated maze is valid
		bool valid = false;

		List<PathNode> path = new List<PathNode>();

		path.Add(g_maze[startRow, startColumn]); // add maze start tile to path list

		while(path.Count > 0) {
			path[0].visited = true;

			if (path[0].row == goalRow && path[0].column == goalColumn) { // found goal
				valid = true;
				path[0].debug = "****";
			} else {
					PathNode neighbor = g_maze[path[0].row - 1, path[0].column]; // north tile

					if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) { // if not a wall or trap tile
						//Debug.Log("Adding neighbor");
						neighbor.visited = true;
						path.Add(neighbor);
						neighbor.debug = "    ";
					}

					neighbor = g_maze[path[0].row, path[0].column - 1]; // west tile
					
					if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) {
						//Debug.Log("Adding neighbor");
						neighbor.visited = true;
						path.Add(neighbor);
						neighbor.debug = "    ";
					}

					neighbor = g_maze[path[0].row, path[0].column + 1]; // east tile

					if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) {
						//Debug.Log("Adding neighbor");
						neighbor.visited = true;
						path.Add(neighbor);
						neighbor.debug = "    ";
					}
					neighbor = g_maze[path[0].row + 1, path[0].column]; // south tile

					if (neighbor.data == 0 && neighbor.walkable && !neighbor.visited && !neighbor.trap) {
						//Debug.Log("Adding neighbor");
						neighbor.visited = true;
						path.Add(neighbor);
						neighbor.debug = "    ";
					}
			}

			path.Remove(path[0]);

			if (valid) {
				break;
			}
		}

		return valid;
	}
}

