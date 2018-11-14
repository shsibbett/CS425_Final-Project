using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    
    [SerializeField] private FpsMovement player;

    private MazeConstructor generator;

    private bool goalReached;

    
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

        float x = generator.startCol * generator.hallWidth;
        float y = 1;
        float z = generator.startRow * generator.hallWidth;
        player.transform.position = new Vector3(x, y, z);

        goalReached = false;
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
        Debug.Log("Goal!");
        goalReached = true;

        Debug.Log("Finish!");
        player.enabled = false;

        Invoke("StartNewMaze", 4);
        
    }

    private void OnStartTrigger(GameObject trigger, GameObject other)
    {

    }
}


