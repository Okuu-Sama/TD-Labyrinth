using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private GameObject wallPrefab;
    // Start is called before the first frame update
    void Start()
    {
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



        MazeGenerator.carve_passage(0, 0, grid, gridWidth, gridHeight);


        string text = null;
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                text = text + grid[i, j] + " ";
            }
            text = text + "\n";
        }

        System.IO.File.WriteAllText("Assets/maze.txt", text);
        instanciateMaze(grid, gridWidth, gridHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    void instanciateMaze(int[,] grid, int gridWidth, int gridHeight)
    {

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

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                
                int[] dir = new int[4] { (int)MazeGenerator.directions.W, (int)MazeGenerator.directions.E, (int)MazeGenerator.directions.S, (int)MazeGenerator.directions.N };
                Instantiate(groundPrefab, new Vector3(5 * i, 0, 5 * j), Quaternion.identity);
                for (int k = 0; k < dir.Length; k++)
                {
                    if (grid[i, j] - dir[k] >= 0)
                    {

                        grid[i, j] -= dir[k];

                    }
                    else
                    {

                        if (dir[k] == 1 || dir[k] == 2)
                        {

                            if (dir[k] == 1)
                            {
                                if (i == 0)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity);
                                    newWall.transform.localScale = new Vector3(0.25f, 5f, 5f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x - 2.375f, newWall.transform.localPosition.y, newWall.transform.localPosition.z);

                                }
                                if (i > 0 && southWall[i - 1, j] == false)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity);
                                    newWall.transform.localScale = new Vector3(0.25f, 5f, 5f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x - 2.375f, newWall.transform.localPosition.y, newWall.transform.localPosition.z);

                                }

                            }
                            else
                            {
                                GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity);
                                newWall.transform.localScale = new Vector3(0.25f, 5f, 5f);
                                //If we put a wall at south

                                southWall[i, j] = true;
                                newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x + 2.375f, newWall.transform.localPosition.y, newWall.transform.localPosition.z);
                            }
                        }
                        else
                        {

                            if (dir[k] == 4)
                            {
                                //If we put a wall at east
                                eastWall[i, j] = true;
                                GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity);
                                if (i == 0)
                                {
                                    newWall.transform.localScale = new Vector3(5f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x, newWall.transform.localPosition.y, newWall.transform.localPosition.z + 2.375f);

                                }
                                else
                                {
                                    newWall.transform.localScale = new Vector3(5.25f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x - 0.125f, newWall.transform.localPosition.y, newWall.transform.localPosition.z + 2.375f);

                                }




                            }
                            else
                            {
                                if (j == 0)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity);
                                    newWall.transform.localScale = new Vector3(5f, 5f, 0.25f);
                                    newWall.transform.localPosition = new Vector3(newWall.transform.localPosition.x, newWall.transform.localPosition.y, newWall.transform.localPosition.z - 2.375f);

                                }
                                if (j > 0 && eastWall[i, j - 1] == false)
                                {
                                    GameObject newWall = Instantiate(wallPrefab, new Vector3(5 * i, 3, 5 * j), Quaternion.identity);
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
}
