using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameObject monkey;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        monkey = GameObject.Find("Monkey");
        navMeshAgent = monkey.GetComponent<NavMeshAgent>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        goBackward();
        goForward();
        rotateLeft();
        rotateRight();
    }

    void OnTriggerEnter(Collider collider)
    {
        SceneManager.LoadScene("MazeScene");
    }

    void goForward() 
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            navMeshAgent.velocity += monkey.transform.forward/80;
        }
    }

    void goBackward()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            navMeshAgent.velocity -= monkey.transform.forward/50;
        }
    }

    void rotateRight() 
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            monkey.transform.Rotate(new Vector3(0f,0.5f,0f));
        }
    }

    void rotateLeft()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            monkey.transform.Rotate(new Vector3(0f, -0.5f, 0f));
        }
    }


}
