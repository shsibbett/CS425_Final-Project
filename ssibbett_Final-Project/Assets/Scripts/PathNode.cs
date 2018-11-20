using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {
	public int row {get; set;}
	public int column {get; set;}
	public int data {get; set;}
	public bool walkable {get; set;}
	public bool visited {get; set;}
	public bool trap {get; set;}
	public string debug {get; set;}

	public PathNode() {
		row = 0;
		column = 0;
		data = -1;
		walkable = false;
		visited = false;
		trap = false;
		debug = "";
	}
}

