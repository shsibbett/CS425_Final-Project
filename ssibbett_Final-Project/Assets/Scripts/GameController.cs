using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    
    [SerializeField] private FpsMovement player;

    private MazeConstructor generator;
    public ParticleSystem death;
    public GameObject burn_mark;

    int count = 1;
    int rows = 7;
    int columns = 9;

    public float trapChance = 0.25f; 
    
    void Start() {
        generator = GetComponent<MazeConstructor>();
        StartNewGame();
    }

    private void StartNewGame()
    {
        StartNewMaze();
    }

    private void StartNewMaze()
    {
        Debug.Log("count: " + count);
        Debug.Log("rows: " + rows + "  columns: " + columns + "  trapChance: " + trapChance);

        generator.GenerateNewMaze(rows, columns, trapChance, OnStartTrigger, OnGoalTrigger);

        int x = generator.startColumn * generator.corridorWidth;
        int y = 1;
        int z = generator.startRow * generator.corridorWidth;

        player.transform.position = new Vector3(x, y, z);

        player.enabled = true;
    }

    void Update()
    {
        if (!player.enabled)
        {
            return;
        }

        float z = player.transform.position.z;
        float x = player.transform.position.x;

        int currentRow = (int) (z / generator.corridorWidth);
        int currentColumn = (int) (x / generator.corridorWidth);

        float floatRow = (z / generator.corridorWidth) - currentRow;
        float floatColumn = (x / generator.corridorWidth) - currentColumn;

        if (floatRow >= 0.5) {
            currentRow++;
        } 

        if (floatColumn >= 0.5) {
            currentColumn++;
        }

        // Debug.Log("row: " + currentRow + "  column: " + currentColumn + "  trap: " + generator.data[currentRow, currentColumn].data);
        // Debug.Log("f row: " + floatRow + "  f column: " + floatColumn);

        if (generator.data[currentRow, currentColumn].data == 2) {
            Vector3 position = generator.data[currentRow, currentColumn].position;
            position.y = 0.001f;
            Instantiate(burn_mark, position, Quaternion.identity);

            StartCoroutine(Respawn());
        }
    }
    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Escaped!");
        player.enabled = false; // make player immobile while generating new maze

        if (count % 3 != 0) {
            trapChance += 0.05f;
        } else {
            rows += 2;
            columns += 2;
            trapChance -= 0.1f;
        }

        count++;

        Invoke("StartNewMaze", 4); // generate new maze
        
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        //Debug.Log("Maze Start!");
    }

    private IEnumerator Respawn() {
        Debug.Log("Trap Activated");

        player.enabled = false;

        Instantiate(death, player.transform.position, Quaternion.identity);

        float z = player.transform.position.z;
        float x = player.transform.position.x;

        int currentRow = (int) (z / generator.corridorWidth);
        int currentColumn = (int) (x / generator.corridorWidth);

        player.transform.position = new Vector3(x,0,z);
        yield return new WaitForSeconds(3);

        float startX = generator.startColumn * generator.corridorWidth;
        float startZ = generator.startRow * generator.corridorWidth;

        player.transform.position = new Vector3(startX,1,startZ);
        player.enabled = true;        
    }
}


