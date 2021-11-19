using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// The following class define the behaviour of the turtle appearing during the game
public class MonsterIABehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterTurtle;
    [SerializeField]
    private AudioClip spawnSound;
    private NavMeshSurface navMeshSurface;
    private NavMeshAgent turtleAgent;

    private GameObject mazeExit;

    // Start is called before the first frame update
    void Start()
    {
        // Playing the spawning sound of the turtle
        AudioSource.PlayClipAtPoint(spawnSound, gameObject.transform.position,0.4f);

        navMeshSurface = GameObject.Find("Maze").GetComponent<NavMeshSurface>();
        turtleAgent = monsterTurtle.AddComponent<NavMeshAgent>();
        turtleAgent.speed = 1f;
        turtleAgent.radius = 0.1f;
        
        // We set the destination of the turtle to the exit of the maze, to guide the player
        mazeExit = GameObject.Find("Exit");
        turtleAgent.SetDestination(mazeExit.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Upon reaching the exit of the maze the turtle is destroyed
        if (this.transform.position == mazeExit.transform.position) Destroy(gameObject);
    }
}
