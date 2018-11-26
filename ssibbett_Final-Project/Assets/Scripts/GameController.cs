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
        generator.GenerateNewMaze(13, 15, OnStartTrigger, OnGoalTrigger);

        float x = generator.startColumn * generator.corridorWidth;
        float y = 1;
        float z = generator.startRow * generator.corridorWidth;
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
        //Debug.Log("row: " + currentRow + "  column: " + currentColumn);
        if (generator.data[currentRow, currentColumn].data == 2) {
            StartCoroutine(Respawn());
        }
    }
    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Escaped!");
        player.enabled = false; // make player immobile while generating new maze

        Invoke("StartNewMaze", 4); // generate new maze
        
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Maze Start!");
    }

    private IEnumerator Respawn() {
        Debug.Log("Trap Activated");

        Instantiate(death, player.transform.position, Quaternion.identity);

        float z = player.transform.position.z;
        float x = player.transform.position.x;

        int currentRow = (int) (z / generator.corridorWidth);
        int currentColumn = (int) (x / generator.corridorWidth);

        player.transform.position = new Vector3(x,0,z);
        player.enabled = false;       
        yield return new WaitForSeconds(3);

        float startX = generator.startColumn * generator.corridorWidth;
        float startZ = generator.startRow * generator.corridorWidth;

        player.transform.position = new Vector3(startX,1,startZ);
        player.enabled = true;        
    }
}


