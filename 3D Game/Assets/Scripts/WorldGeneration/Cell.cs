using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private int tileSideSize = 3;
    public Tile[] possibleTiles;
    public Tile tile;
    [SerializeField] private Tile airTile;
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile UpWall;
    [SerializeField] private Tile rightWall;
    [SerializeField] private Tile downWall;
    [SerializeField] private Tile leftWall;

    public Cell upCell;
    public Cell downCell;
    public Cell leftCell;
    public Cell rightCell;

    public void collapse(string state)
    {
        switch (state)
        {
            case "air":
                Instantiate(airTile, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                tile = airTile;
                break;
            case "floor":
                Instantiate(floorTile, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                tile = floorTile;
                break;
            case "up":
                Instantiate(UpWall, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                tile = UpWall;
                break;
            case "right":
                Instantiate(rightWall, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                tile = rightWall;
                break;
            case "down":
                Instantiate(downWall, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                tile = downWall;
                break;
            case "left":
                Instantiate(leftWall, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                tile = leftWall;
                break;
            default:
                if (possibleTiles.Length == 0)
                {
                    Instantiate(floorTile, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
                    tile = floorTile;
                }
                else
                {
                    int randomIndex = UnityEngine.Random.Range(0, possibleTiles.Length); //Pick random index from possible tiles
                    Instantiate(possibleTiles[randomIndex], new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity); //Instantiate the random gameobject from possible tiles

                    tile = possibleTiles[randomIndex]; //Sets current tile to the chosen tile
                }
                break;
        };
    }

    public void reducePossibleTiles(int[] inputArray, string dir)
    {
        List<Tile> matchingTiles = new List<Tile>();
        bool match = true;
        if (possibleTiles.Length > 0)
        {
            switch (dir)
            {
                case "up":
                    foreach (Tile tile in possibleTiles)
                    {
                        for (int n = 0; n < tileSideSize; n++)
                        {
                            if (tile.downNeighbors[n] != inputArray[n])
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            matchingTiles.Add(tile);
                        }
                        match = true;
                    }
                    break;
                case "right":
                    foreach (Tile tile in possibleTiles)
                    {
                        for (int n = 0; n < tileSideSize; n++)
                        {
                            if (tile.leftNeighbors[n] != inputArray[n])
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            matchingTiles.Add(tile);
                        }
                        match = true;
                    }
                    break;
                case "down":
                    foreach (Tile tile in possibleTiles)
                    {
                        for (int n = 0; n < tileSideSize; n++)
                        {
                            if (tile.upNeighbors[n] != inputArray[n])
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            matchingTiles.Add(tile);
                        }
                        match = true;
                    }
                    break;
                case "left":
                    foreach (Tile tile in possibleTiles)
                    {
                        for (int n = 0; n < tileSideSize; n++)
                        {
                            if (tile.rightNeighbors[n] != inputArray[n])
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            matchingTiles.Add(tile);
                        }
                        match = true;
                    }
                    break;
            }
        }

        int i = 0; //Create an index at 0
        foreach (Tile tile in matchingTiles.ToArray()) //Goes through all the matching tiles
        {
            possibleTiles[i] = tile; //Sets the possible tiles to the matching tiles
            i++;
        }

        Array.Resize(ref possibleTiles, i); //Resize possible tiles to only have the matching tiles
    }
}
