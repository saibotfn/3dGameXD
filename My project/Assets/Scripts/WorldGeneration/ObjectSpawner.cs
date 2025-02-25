using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    private List<Cell> grid;
    private List<List<Room>> roomGrid;
    private int tileSize;
    private int dimensions;
    public void spawnObjects(List<Cell> Grid, List<List<Room>> RoomGrid, int TileSize, int Dimensions)
    {
        Debug.Log("Run");

        grid = Grid;
        roomGrid = RoomGrid;
        tileSize = TileSize;
        dimensions = Dimensions;

        spawnEnemys();
    }

    private void spawnEnemys()
    {
        for (int x = 0; x < roomGrid.Count; x++)
        {
            for (int y = 0; y < roomGrid[x].Count; y++)
            {
                if (roomGrid[x][y].depth != 0)
                {
                    foreach (Cell cell in grid)
                    {
                        bool isCenter = false;
                        if (x == 0 && y == 0)
                        {
                            int center = (dimensions / 2 + 1) * tileSize;

                            if (cell.transform.localPosition.x == center && cell.transform.localPosition.z == center)
                            {
                                isCenter = true;
                            }

                        }
                        else if (x == 0)
                        {
                            int centerX = (dimensions / 2 + 1) * tileSize;
                            int centerZ = ((dimensions / 2 + 1) + dimensions * y + y) * tileSize;

                            if (cell.transform.localPosition.x == centerX && cell.transform.localPosition.z == centerZ)
                            {
                                isCenter = true;
                            }

                        }
                        else if (y == 0)
                        {
                            int centerX = ((dimensions / 2 + 1) + dimensions * x + x) * tileSize;
                            int centerZ = (dimensions / 2 + 1) * tileSize;

                            if (cell.transform.localPosition.x == centerX && cell.transform.localPosition.z == centerZ)
                            {
                                isCenter = true;
                            }

                        }
                        else
                        {
                            int centerX = ((dimensions / 2 + 1) + dimensions * x + x) * tileSize;
                            int centerZ = ((dimensions / 2 + 1) + dimensions * y + y) * tileSize;

                            if (cell.transform.localPosition.x == centerX && cell.transform.localPosition.z == centerZ)
                            {
                                isCenter = true;
                            }

                        }
                        if (isCenter)
                        {
                            Debug.Log(x + "," + y);
                            
                            Instantiate(enemyPrefab, cell.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }
}
