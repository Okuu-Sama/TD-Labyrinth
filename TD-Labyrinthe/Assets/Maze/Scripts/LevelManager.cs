using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// The following class define the behaviour of the level management (instantiating the maze and its components)
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject bananaPrefab;
    private int bananaNumber;
    private float maxDistance = 0f;
    private Vector3 position;
    // Start is called before the first frame update
    void Awake()
    {
       //Setting the size of the maze
        int gridWidth = 10;
        int gridHeight = 10;
        int[,] grid = new int[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                grid[i, j] = 0;
            }
        }


        //Generate the maze with given size
        MazeGenerator.carve_passage(0, 0, grid, gridWidth, gridHeight);

        //Putting the maze content into a file for tests
        string text = null;
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                text = text + grid[i, j] + " ";
            }
            text = text + "\n";
        }

        //Using a copy of the grid because it is modified to instantiate the maze in the scene
        int[,] gridCopy = new int[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                gridCopy[i,j] = grid[i, j];
            }
        }
        //Debug.Log(Application.persistentDataPath);
        //System.IO.File.WriteAllText(Application.persistentDataPath + "/Assets/Maze/Data/maze.txt", text);

        instanciateMaze(gridCopy, gridWidth, gridHeight);

        //Using NavMeshSurface components
        GameObject maze = GameObject.Find("Maze");
        NavMeshSurface navMeshSurface = maze.AddComponent(typeof(NavMeshSurface)) as NavMeshSurface;
        navMeshSurface.BuildNavMesh();
        GameObject monkey = GameObject.Find("Monkey");
        NavMeshAgent navMeshAgent = monkey.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        
        //Getting dead end for the bananas
        bool[,] deadEnd = new bool[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                deadEnd[i, j] = false;
            }
        }

        getDeadEnd(grid, gridWidth, gridHeight, deadEnd);
        
        /*for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Debug.Log(deadEnd[i,j]);
            }
        }*/
        
        int indexI = 0;
        int indexJ = 0;

        //Computation of maximum distance among all the dead ends
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (deadEnd[i, j]==true)
                {
                    NavMeshPath path=new NavMeshPath();
                    //Debug.Log("Dead end at index " + i + " " + j);
                    
                    navMeshAgent.CalculatePath(new Vector3(5 * i, 0, 5 * j), path);
                    navMeshAgent.SetPath(path);
                    float distance = ExtensionScript.GetPathRemainingDistance(navMeshAgent);
                    //Debug.Log("Distance =  " +distance);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        position = new Vector3(5 * i, 0, 5 * j);
                        indexI = i;
                        indexJ = j;
                    }
                }
            }
        }
        //Debug.Log("Max distance is " + maxDistance + " at index "+ indexI + " "+indexJ);

        //Instantiating the exit of the maze
        navMeshAgent.Warp(new Vector3(0f,0f,0f));
        GameObject collider = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), new Vector3(position.x, position.y+3,position.z), Quaternion.identity);
        
        //Hiding the primitive that was just created
        GameObject cube = GameObject.Find("Cube");
        cube.SetActive(false);

        collider.name = "Exit";
        collider.transform.localScale=new Vector3(4f,4f,4f);
        collider.GetComponent<MeshRenderer>().enabled = false;
        collider.GetComponent<BoxCollider>().isTrigger = true;

        //Instantiating bananas at all dead ends except at the exit of the maze
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (deadEnd[i, j] ==true)
                {
                    NavMeshPath path = new NavMeshPath();
                    //Debug.Log("Dead end at index " + i + " " + j);

                    navMeshAgent.CalculatePath(new Vector3(5 * i, 0, 5 * j), path);
                    navMeshAgent.SetPath(path);
                    float distance = ExtensionScript.GetPathRemainingDistance(navMeshAgent);
                    //Debug.Log("Distance =  " +distance);

                    //If it is not the start or the end of the maze
                    if ( (i,j)!=(0,0) && (i,j)!=(position.x/5,position.z/5))
                    {
                        //Spawning a banana at this position and updating the number of bananas in the maze
                        //Debug.Log("Adding banana at " + i + ", "+j);
                        bananaNumber++;
                        GameObject banana = Instantiate(bananaPrefab, new Vector3(5 * i, 1f, 5 * j), Quaternion.identity);
                        
                    }
                }
            }
        }

        //Putting the monkey at the beginning of the maze
        navMeshAgent.Warp(new Vector3(0f, 0f, 0f));
        //navMeshAgent.SetDestination(position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Filling the maze borders because the algorithm only carve a passage and doesn't create borders for the maze
    void fillMazeBorders(int[,] grid, int gridWidth, int gridHeight)
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (i == 0)
                {
                    grid[i, j] -= 1;
                }
                if (i == gridWidth - 1)
                {
                    grid[i, j] -= 2;
                }
                if (j == 0)
                {
                    grid[i, j] -= 4;
                }
                if (j == gridHeight - 1)
                {
                    grid[i, j] -= 8;
                }
            }
        }
    }

    
    void getDeadEnd(int[,] grid, int gridWidth, int gridHeight, bool[,] deadEnd)
    {

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                //Debug.Log(grid[i,j]);
                if (grid[i, j] == 1 || grid[i, j] == 2 || grid[i, j] == 4 || grid[i, j] == 8)
                {
                    deadEnd[i, j] = true;
                }
            }
        }
    }

    void instanciateMaze(int[,] grid, int gridWidth, int gridHeight)
    {
        GameObject maze = GameObject.Find("Maze");
        Transform mazeTransform = maze.transform;
        
        //East and South walls are there to fill gaps between walls
        bool[,] eastWall = new bool[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                eastWall[i, j] = false;
            }
        }

        bool[,] southWall = new bool[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                southWall[i, j] = false;
            }
        }

        
        //For each cell of the maze
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                
                int[] dir = new int[4] { (int)MazeGenerator.directions.W, (int)MazeGenerator.directions.E, (int)MazeGenerator.directions.S, (int)MazeGenerator.directions.N };
                
                //Instantiating the ground first
                Instantiate(groundPrefab, new Vector3(5 * i, 0, 5 * j), Quaternion.identity,mazeTransform);
                for (int k = 0; k < dir.Length; k++)
                {
                    //Removing every value of the possible directions to know where to put the walls
                    //For instance 1 means the cell is open on north, which means we have to put walls at the south, east and west
                    //If we can't remove 8 to the total, we know we have to put a west wall, same goes for other directions
                    
                    //If we can remove the corresponding number it means no wall of this type should be added
                    if (grid[i, j] - dir[k] >= 0)
                    {
                        //Removing it from the total of the cell to go to the next type of wall
                        grid[i, j] -= dir[k];

                    }
                    //If we can't remove the corresponding value it means we have to put a wall
                    else
                    {
                        //If it is a north or south wall
                        if (dir[k] == 1 || dir[k] == 2)
                        {
                            //Instantiating north wall
                            if (dir[k] == 1)
                            {
                                if (i == 0)
                                {
                                    //Wall prefab is only a pillar of the right height
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity, mazeTransform);

                                    //Stretching the pillar to get a wall of the right type
                                    newWall.transform.localScale = new Vector3(0.25f, 5f, 5f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x - 2.375f, newWall.transform.localPosition.y, newWall.transform.localPosition.z);

                                }
                                //Only putting north wall where there isn't already a south wall above
                                if (i > 0 && southWall[i - 1, j] == false)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity, mazeTransform);
                                    newWall.transform.localScale = new Vector3(0.25f, 5f, 5f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x - 2.375f, newWall.transform.localPosition.y, newWall.transform.localPosition.z);

                                }

                            }
                            else
                            {
                                GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity, mazeTransform);
                                newWall.transform.localScale = new Vector3(0.25f, 5f, 5f);
                                //If we put a wall at south

                                southWall[i, j] = true;
                                newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x + 2.375f, newWall.transform.localPosition.y, newWall.transform.localPosition.z);
                            }
                        }
                        //If it is a east or west wall
                        else
                        {
                            //Instantiating east wall
                            if (dir[k] == 4)
                            {
                                //If we put a wall at east
                                eastWall[i, j] = true;
                                GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity, mazeTransform);
                                if (i == 0)
                                {
                                    newWall.transform.localScale = new Vector3(5f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x, newWall.transform.localPosition.y, newWall.transform.localPosition.z + 2.375f);

                                }
                                //There is a small gap elsewhere compared to the first line of the maze
                                else
                                {
                                    newWall.transform.localScale = new Vector3(5.25f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x - 0.125f, newWall.transform.localPosition.y, newWall.transform.localPosition.z + 2.375f);

                                }




                            }
                            //Instantiating west wall
                            else
                            {
                                if (j == 0)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity, mazeTransform);
                                    newWall.transform.localScale = new Vector3(5f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x, newWall.transform.localPosition.y, newWall.transform.localPosition.z - 2.375f);

                                }

                                //Only putting west wall where there isn't already a east wall on the column before
                                if (j > 0 && eastWall[i, j - 1] == false)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity, mazeTransform);
                                    newWall.transform.localScale = new Vector3(5f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x, newWall.transform.localPosition.y, newWall.transform.localPosition.z - 2.375f);

                                }

                            }
                        }

                    }
                }
                
            }
        }
    }

    public int GetBananaNumber()
    {
        return bananaNumber;
    }
    
}
