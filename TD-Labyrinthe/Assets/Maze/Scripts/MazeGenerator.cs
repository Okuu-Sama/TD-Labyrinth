using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

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

    public static void carve_passage(int x, int y, int[,] grid, int gridWidth, int gridHeight)
    {
        int[] dir= new int[4] { (int) directions.N , (int)directions.S ,  (int)directions.E ,  (int)directions.W  };
        System.Random rand = new System.Random(System.Guid.NewGuid().GetHashCode());
        dir = dir.OrderBy(x => rand.Next()).ToArray();
        int currentDir;
        int new_x, new_y;
        for (int k = 0;k<dir.Length;k++) {
            currentDir = dir[k];
            new_x = x + getXPosition(currentDir);
            new_y = y + getYPosition(currentDir);
            //Debug.Log("nx is " + new_x + " and ny is " + new_y);
            //if ny.between?(0, grid.length-1) && nx.between?(0, grid[ny].length-1) && grid[ny][nx] == 0
            if (new_y >= 0 && new_y <= (gridWidth - 1) && new_x >= 0 && new_x <= (gridHeight - 1) && grid[new_y, new_x] == 0)
            {

                grid[y, x] += currentDir;
                grid[new_y, new_x] += getOpposite(currentDir);
                //Debug.Log("CURRENT CELL IS " + grid[y, x]);
                carve_passage(new_x, new_y, grid, gridWidth, gridHeight);
            }
        }
    }
    
   
}
