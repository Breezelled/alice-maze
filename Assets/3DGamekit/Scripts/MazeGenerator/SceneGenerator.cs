using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public GameObject wall;
    public GameObject gate;
    public GameObject exit;
    public GameObject chomper;
    public GameObject grenadier;
    public GameObject spitter;
    private MazeWall mazeWall;
    private List<int[]> dirs = new List<int[]> { new int[]{ 1, 0 }, new int[]{ -1, 0 }, new int[]{ 0, 1 }, new int[]{ 0, -1 } };
    private int[] endPoint;
    // Start is called before the first frame update
    void Start()
    {
        mazeWall = new MazeWall(constant.mapLength, constant.mapLength);

        CutWallCreateRoad();

        InstantiateWall();

        MakeDoorAtEndPoint(endPoint);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CutWallCreateRoad()
    {
        System.Random r = new System.Random();
        // random move wall to create road
        int remain = constant.roadNum;
        // start point had been visited
        remain--;
        while (remain > 0)
        {
            int[] curwall = mazeWall.nearbyWallList[r.Next(mazeWall.nearbyWallList.Count)];
            for (int i = 0; i < dirs.Count; i++)
            {
                int[] d = dirs[i];
                int x = curwall[0] + d[0];
                int y = curwall[1] + d[1];
                if (BoundaryJudgment(x, y))
                {
                    // have not been visited && not a wall
                    if (!mazeWall.road[x, y] && !mazeWall.wall[x, y])
                    {
                        mazeWall.road[x, y] = true;
                        mazeWall.wall[curwall[0], curwall[1]] = false;
                        mazeWall.nearbyWallList.Remove(curwall);
                        int rate = r.Next(100);
                        // create enemies in this place
                        if (rate <= constant.enemiesGenerateRate)
                        {
                            CreateRandomEnemies(x, y);
                        }
                        remain--;
                        if (remain == 0)
                        {
                            endPoint = new int[] { x, y };
                        }
                        // add new nearby wall
                        for (int j = 0; j < dirs.Count; j++)
                        {
                            int[] d1 = dirs[j];
                            int x1 = x + d1[0];
                            int y1 = y + d1[1];
                            // not out of bound && is a nearby wall
                            if (BoundaryJudgment(x1, y1) && mazeWall.wall[x1, y1])
                            {
                                mazeWall.nearbyWallList.Add(new int[] { x1, y1 });
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    void InstantiateWall()
    {
        for (int i = 0; i < constant.mapLength; i++)
        {
            for (int j = 0; j < constant.mapLength; j++)
            {
                if (mazeWall.wall[i, j])
                {
                    Instantiate(wall,
                    new Vector3(constant.wallStartPosition + i * 10, constant.wallYPosition, constant.wallStartPosition + j * 10),
                    wall.transform.rotation);
                }
            }
        }
    }

    void MakeDoorAtEndPoint(int[] endPoint)
    {
        // Find orientation of the door
        for (int i = 0; i < dirs.Count; i++)
        {
            int[] d = dirs[i];
            int x = endPoint[0] + d[0];
            int y = endPoint[1] + d[1];
            if (!mazeWall.wall[x, y])
            {
                if (d[1] == 0)
                {
                    if (d[0] == 1)
                    {
                        // right
                        InstantiateDoor(270, new int[] {10, 0});
                        break;
                    }else if (d[0] == -1)
                    {
                        // left
                        InstantiateDoor(90, new int[] {-10, 0});
                        break;
                    }
                } else if (d[0] == 0)
                {
                    if (d[1] == 1)
                    {
                        // behind
                        InstantiateDoor(180, new int[] {0, 10});

                        break;
                    }else if (d[1] == -1)
                    {
                        // front
                        InstantiateDoor(0, new int[] {0, -10});
                        break;
                    }
                }
                
            }
        }
            
    }

    void InstantiateDoor(int yRotation, int[] direction)
    {
        Instantiate(gate,
        new Vector3(constant.wallStartPosition + endPoint[0] * 10, constant.grenadierYPosition, constant.wallStartPosition + endPoint[1] * 10),
        Quaternion.Euler(0, yRotation, 0));
        exit.transform.position = new Vector3(constant.wallStartPosition + endPoint[0] * 10, constant.wallYPosition, constant.wallStartPosition + endPoint[1] * 10);
        exit.transform.localScale = new Vector3(0.001f, 0.5f, 0.001f);
        Instantiate(grenadier,
        new Vector3(constant.wallStartPosition + endPoint[0] * 10 + direction[0], constant.grenadierYPosition, constant.wallStartPosition + endPoint[1] * 10 + direction[1]),
        Quaternion.Euler(0, yRotation, 0));
    }

    void CreateRandomEnemies(int i, int j)
    {
        Instantiate(spitter,
        new Vector3(constant.wallStartPosition + i * 10 + 1, constant.spitterYPosition, constant.wallStartPosition + j * 10 + 1),
        spitter.transform.rotation);
        Instantiate(spitter,
        new Vector3(constant.wallStartPosition + i * 10 - 1, constant.spitterYPosition, constant.wallStartPosition + j * 10 + 1),
        Quaternion.Euler(0, 180, 0));

    }

    bool BoundaryJudgment(int i, int j)
    {
        return i > 0 && i < constant.mapLength - 1 && j > 0 && j < constant.mapLength - 1;
    }
}
