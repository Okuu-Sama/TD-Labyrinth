using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

// The following class contain all the methods related to the generation of the maze
public class MazeGenerator
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

public enum directions : int
    {
        N = 1,
        S = 2,
        E = 4,
        W = 8
    }
    // 1 means the cell of the maze is open on the north
    // 2 means the cell of the maze is open on the south
    // 4 means the cell of the maze is open on the east
    // 8 means the cell of the maze is open on the west
    // 4+8 = 12 means the cell of the maze is open on the east and west
    static int getOpposite(int direction) 
    {
        switch (direction) 
        {
            case 1:
                return 2;
            case 2:
                return 1;
            case 4:
                return 8;
            case 8:
                return 4;
            default:
                return 0;
        }
    }

    static int getXPosition(int direction)
    {
        switch (direction)
        {
            case 1:
                return 0;
            case 2:
                return 0;
            case 4:
                return 1;
            case 8:
                return -1;
            default:
                return 0;
        }
    }

    static int getYPosition(int direction)
    {
        switch (direction)
        {
            case 1:
                return -1;
            case 2:
                return 1;
            case 4:
                return 0;
            case 8:
                return 0;
            default:
                return 0;
        }
    }

    //Recursive backtracker maze generation
    public static void carve_passage(int x, int y, int[,] grid, int gridWidth, int gridHeight)
    {
        int[] dir= new int[4] { (int) directions.N , (int)directions.S ,  (int)directions.E ,  (int)directions.W  };

        //Randomizing the directions order to get a random generation
        System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());
        dir = dir.OrderBy(element => rand.Next()).ToArray();

        int currentDir;
        int new_x, new_y;

        //For each direction for the current cell
        for (int k = 0;k<dir.Length;k++) {

            //Taking the randomly chosen direction
            currentDir = dir[k];
            
            //Choosing coordinates of the next cell
            new_x = x + getXPosition(currentDir);
            new_y = y + getYPosition(currentDir);
            //Debug.Log("nx is " + new_x + " and ny is " + new_y);

            //If we haven't visited the cell and it lies within the bounds of the maze
            if (new_y >= 0 && new_y <= (gridWidth - 1) && new_x >= 0 && new_x <= (gridHeight - 1) && grid[new_y, new_x] == 0)
            {
                //Carving the cell in the chosen direction
                grid[y, x] += currentDir;
                grid[new_y, new_x] += getOpposite(currentDir);
                //Debug.Log("CURRENT CELL IS " + grid[y, x]);
                //Changing to new cell
                carve_passage(new_x, new_y, grid, gridWidth, gridHeight);
            }
        }
    }
    
   
}
