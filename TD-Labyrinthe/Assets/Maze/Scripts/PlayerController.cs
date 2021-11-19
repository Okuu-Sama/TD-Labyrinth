using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// The following class define the behaviour of the monkey the player can control during the game
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
    [SerializeField]
    private AudioClip spawningSound;
    [SerializeField]
    private AudioClip bananaPickUpSound;
    [SerializeField]
    private AudioClip reachExitSound;

    private string totalBananaFile;
    // Start is called before the first frame update
    void Start()
    {
        //Playing the spawning sound
        AudioSource.PlayClipAtPoint(spawningSound, gameObject.transform.position);
        AudioSource audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        audioSource.PlayDelayed(10f);

        //Adding NavMeshAgent to the Monkey
        monkey = GameObject.Find("Monkey");
        navMeshAgent = monkey.GetComponent<NavMeshAgent>();

        //Getting bananas data
        string bananasFile;
        if (Application.isEditor)
        {
            bananasFile = System.IO.File.ReadAllText("Assets/Maze/Data/bananas.txt");
        }
        else
        {
            bananasFile = System.IO.File.ReadAllText(Application.streamingAssetsPath + "bananas.txt");
        }
        totalBanana = Int32.Parse(bananasFile);

    }

    // Update is called once per frame
    void Update()
    {
        //Controls for the player
        goBackward();
        goForward();
        rotateLeft();
        rotateRight();
    }

    void OnTriggerEnter(Collider collider)
    {
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        //Debug.Log("nb of banana in lvl: " + levelManager.GetBananaNumber());

        //If the player collides with the exit
        if (collider.gameObject.name == "Exit" && levelManager.GetBananaNumber() == pickedUpBanana)
        {
            //SFX and VFX for the exit
            GameObject.Find("FadeOutTransition").GetComponent<RawImage>().enabled = true;
            GameObject.Find("FadeOutTransition").GetComponent<Animator>().SetTrigger("playfadeout");
            AudioSource.PlayClipAtPoint(reachExitSound, gameObject.transform.position,0.3f);

            //Resetting the number of picked up banana for the level
            pickedUpBanana = 0;
            StartCoroutine(waitForSFXBeforeLoad());
        }
        //If the player collides wirh a banana
        if (collider.gameObject.name== "Banana(Clone)") 
        {
            //Playing pick up sound
            AudioSource.PlayClipAtPoint(bananaPickUpSound, gameObject.transform.position,0.5f);

            //Destroying the banana and updating scores
            Destroy(collider.gameObject);
            pickedUpBanana++;
            totalBanana++;
            //System.IO.File.WriteAllText("Assets/Maze/Data/bananas.txt", totalBanana.ToString());

            //Saving scores
            if (Application.isEditor)
            {
                System.IO.File.WriteAllText("Assets/Maze/Data/bananas.txt", totalBanana.ToString());
            }
            else
            {
                System.IO.File.WriteAllText(Application.streamingAssetsPath + "banana.txt", totalBanana.ToString());
            }

            //Updating scores on screen
            totalBananasText.text = totalBanana.ToString();
            levelBananasText.text = pickedUpBanana.ToString();
            //Debug.Log("Bananas of this level = " + pickedUpBanana + " Total bananas = " + totalBanana);

            //If the picked up banana is the last one
            if(pickedUpBanana == levelManager.GetBananaNumber())
            {
                //Spawning the turtle at this position
                Vector3 turtleSpawn = collider.gameObject.transform.position;
                GameObject Turtle = Instantiate(turtlePrefab, turtleSpawn, Quaternion.identity);
            }

        }

    }

    void goForward() 
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            navMeshAgent.velocity += monkey.transform.forward*0.05f;
        }
    }

    void goBackward()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            navMeshAgent.velocity -= monkey.transform.forward* 0.05f;
        }
    }

    void rotateRight() 
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            monkey.transform.Rotate(new Vector3(0f,0.5f*3,0f));
        }
    }

    void rotateLeft()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            monkey.transform.Rotate(new Vector3(0f, -0.5f*3, 0f));
        }
    }

    IEnumerator waitForSFXBeforeLoad()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MazeScene");
    }
}
