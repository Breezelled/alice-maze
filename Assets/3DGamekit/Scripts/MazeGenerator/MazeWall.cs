using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall
{
    // true == wall exist, false == wall not exist
    public bool[,] wall;
    public bool[,] road;
    public List<int[]> nearbyWallList;

    // init maze, start at road[1, 1]
    public MazeWall(int row, int col)
    {
        wall = new bool[row, col];
        road = new bool[row, col];
        // boundary
        for (int i = 0; i < col; i++)
        {
            wall[i, 0] = true;
            wall[i, col - 1] = true;
        }

        for (int i = 0; i < col; i++)
        {
            wall[0, i] = true;
            wall[row-1, i] = true;
        }

        // interior
        for (int i = 1; i < row; i++)
        {
            for (int j = 1; j < col; j++)
            {
                wall[i, j] = true;
            }
        }
        for (int i = 1; i < row; i+=2)
        {
            for (int j = 1; j < col; j+=2)
            {
                wall[i, j] = false;
            }
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                road[i, j] = false;
            }
        }
        road[1, 1] = true;
        nearbyWallList = new List<int[]> { new int[] { 1, 2 }, new int[] { 2, 1 } };

    }
}
