using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    
    [SerializeField] private FpsMovement player;

    private MazeConstructor generator;

    private bool exitReached;

    
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

        exitReached = false;
        player.enabled = true;
    }

    void Update()
    {
        if (!player.enabled)
        {
            return;
        }
    }
    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Exit!");
        exitReached = true;

        Debug.Log("Escaped!");
        player.enabled = false;

        Invoke("StartNewMaze", 4);
        
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {

    }
}


