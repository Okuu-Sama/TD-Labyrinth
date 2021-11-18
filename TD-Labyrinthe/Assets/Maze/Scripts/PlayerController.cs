using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject monkey;
    private NavMeshAgent navMeshAgent;
    private int pickedUpBanana = 0;
    private int totalBanana;
    [SerializeField]
    private Text totalBananasText;
    [SerializeField]
    private Text levelBananasText;
    [SerializeField]
    private GameObject turtlePrefab;
    private string totalBananaFile;
    // Start is called before the first frame update
    void Start()
    {
        monkey = GameObject.Find("Monkey");
        navMeshAgent = monkey.GetComponent<NavMeshAgent>();
        string bananasFile = System.IO.File.ReadAllText("Assets/Maze/Data/bananas.txt");
        totalBanana = Int32.Parse(bananasFile);

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
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        Debug.Log("nb of banana in lvl: " + levelManager.GetBananaNumber());
        if (collider.gameObject.name == "Exit" && levelManager.GetBananaNumber() == pickedUpBanana)
        {
            pickedUpBanana = 0;
            SceneManager.LoadScene("MazeScene");
        }
        if (collider.gameObject.name== "Banana(Clone)") 
        {
            Destroy(collider.gameObject);
            pickedUpBanana++;
            totalBanana++;
            System.IO.File.WriteAllText("Assets/Maze/Data/bananas.txt", totalBanana.ToString());
            totalBananasText.text = "Player total: " + totalBanana.ToString();
            levelBananasText.text = "Current banana number: " + pickedUpBanana.ToString();
            //Debug.Log("Bananas of this level = " + pickedUpBanana + " Total bananas = " + totalBanana);

            if(pickedUpBanana == levelManager.GetBananaNumber())
            {
                Vector3 turtleSpawn = collider.gameObject.transform.position;
                
                GameObject Turtle = Instantiate(turtlePrefab, turtleSpawn, Quaternion.identity);
            }

        }

    }

    void goForward() 
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            navMeshAgent.velocity += monkey.transform.forward*0.01f;
        }
    }

    void goBackward()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            navMeshAgent.velocity -= monkey.transform.forward*0.01f;
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
