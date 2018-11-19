using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
	public int data {get; set;}
	public bool walkable {get; set;}
	public bool visited {get; set;}
	public bool trap {get; set;}

	public PathNode() {
		data = 0;
		walkable = true;
		visited = false;
		trap = false;
	}
}

