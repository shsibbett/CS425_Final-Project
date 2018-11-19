using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    //1
    public bool showDebug;
	private MazeDataGenerator mazeGenerator;

    
    [SerializeField] private Material floorMat;
    [SerializeField] private Material wallMat;
    [SerializeField] private Material ceilingMat;
    [SerializeField] private Material startMat;
    [SerializeField] private Material goalMat;
    [SerializeField] private Material trapMat;
	private MazeMeshGenerator meshGenerator;

    public PathNode[,] data { get; private set; }

    public float corridorWidth { get; private set; }
    public float corridorHeight { get; private set; }

    public int startRow { get; private set; }
    public int startColumn { get; private set; }

    public int goalRow { get; private set; }
    public int goalColumn
    {
        get; 
        private set;
    }

    void Awake()
    {
		meshGenerator = new MazeMeshGenerator();

		mazeGenerator = new MazeDataGenerator();

        data = new PathNode[,]
        {
            {new PathNode(), new PathNode(), new PathNode()},
            {new PathNode(), new PathNode(), new PathNode()},
            {new PathNode(), new PathNode(), new PathNode()}
        };
    }
    
    public void GenerateNewMaze(int rows, int columns,
        TriggerEventHandler startCallback=null, TriggerEventHandler goalCallback=null)
    {
        if (rows % 2 == 0 && columns % 2 == 0)
        {
            Debug.LogError("Use odd numbers for dungeon size.");
        }

        DestroyPrevMaze();

        data = mazeGenerator.FromDimensions(rows, columns);

        GenerateStart();
        GenerateGoal();

        bool valid = mazeGenerator.validMaze(startRow, startColumn, goalRow, goalColumn);

        corridorWidth = meshGenerator.width;
        corridorHeight = meshGenerator.height;

        DisplayMaze();

        PlaceStartTrigger(startCallback);
        PlaceGoalTrigger(goalCallback);
    }


        void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }

        PathNode[,] maze = data;
        int maxRows = maze.GetUpperBound(0);
        int maxColumns = maze.GetUpperBound(1);

        string message = "";

        for (int i = maxRows; i >= 0; i--)
        {
            for (int j = 0; j <= maxColumns; j++)
            {
                if (maze[i, j].data == 0)
                {
                    message += "....";
                }
                else
                {
                    message += "==";
                }
            }
            message += "\n";
        }

        //4
        GUI.Label(new Rect(20, 20, 500, 500), message);
    }

    private void DisplayMaze()
    {
        GameObject obj = new GameObject();
        obj.transform.position = Vector3.zero;
        obj.name = "Procedural Maze";
        obj.tag = "Generated";

        MeshFilter filter = obj.AddComponent<MeshFilter>();
        filter.mesh = meshGenerator.FromData(data);
        
        MeshCollider mc = obj.AddComponent<MeshCollider>();
        mc.sharedMesh = filter.mesh;

        MeshRenderer mesh_renderer = obj.AddComponent<MeshRenderer>();
        mesh_renderer.materials = new Material[4] {floorMat, wallMat, ceilingMat, trapMat};
    }

    public void DestroyPrevMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject obj in objects) {
            Destroy(obj);
        }
    }

    private void GenerateStart()
    {
        PathNode[,] maze = data;
        int maxRows = maze.GetUpperBound(0);
        int maxColumns = maze.GetUpperBound(1);

        for (int i = 0; i <= maxRows; i++)
        {
            for (int j = 0; j <= maxColumns; j++)
            {
                if (maze[i, j].data == 0)
                {
                    startRow = i;
                    startColumn = j;
                    return;
                }
            }
        }
    }

    private void GenerateGoal()
    {
        PathNode[,] maze = data;
        int maxRows = maze.GetUpperBound(0);
        int maxColumns = maze.GetUpperBound(1);

        // loop top to bottom, right to left
        for (int i = maxRows; i >= 0; i--)
        {
            for (int j = maxColumns; j >= 0; j--)
            {
                if (maze[i, j].data == 0)
                {
                    goalRow = i;
                    goalColumn = j;
                    return;
                }
            }
        }
    }

    private void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(startColumn * corridorWidth, .5f, startRow * corridorWidth);
        go.name = "Start Trigger";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    private void PlaceGoalTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(goalColumn * corridorWidth, .5f, goalRow * corridorWidth);
        go.name = "Goal";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = goalMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

}

