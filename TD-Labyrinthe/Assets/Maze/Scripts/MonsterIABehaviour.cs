using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterIABehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterTurtle;
    private NavMeshSurface navMeshSurface;
    private NavMeshAgent turtleAgent;

    private GameObject mazeExit;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.position = transform.position + new Vector3(0f,1f,0f);
        navMeshSurface = GameObject.Find("Maze").GetComponent<NavMeshSurface>();
        turtleAgent = monsterTurtle.AddComponent<NavMeshAgent>();
        turtleAgent.speed = 0.2f;
        turtleAgent.transform.position = turtleAgent.transform.position + new Vector3(0f, 5f, 0f);
        mazeExit = GameObject.Find("Exit");
        turtleAgent.SetDestination(mazeExit.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == mazeExit.transform.position) Destroy(this);
    }
}
