using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        AudioSource.PlayClipAtPoint(spawnSound, gameObject.transform.position,0.4f);

        navMeshSurface = GameObject.Find("Maze").GetComponent<NavMeshSurface>();
        turtleAgent = monsterTurtle.AddComponent<NavMeshAgent>();
        turtleAgent.speed = 1f;
        turtleAgent.radius = 0.1f;
        
        mazeExit = GameObject.Find("Exit");
        turtleAgent.SetDestination(mazeExit.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == mazeExit.transform.position) Destroy(this);
    }
}
