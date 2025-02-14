using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Tile[] possibleTiles;
    public Tile tile;
    [SerializeField] private Tile airTile;
    [SerializeField] private Tile floorTile;

    public void collapse(string state)
    {
        if (state == "air") //Sets Cell to air tile
        {
            Instantiate(airTile, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
            tile = airTile;
        }
        else if (state == "floor") //Sets Cell to floor tile
        {
            Instantiate(floorTile, new Vector3(transform.localPosition.x, 0, transform.localPosition.z), Quaternion.identity);
            tile = floorTile;
        }
        else
        {
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
        }
    }

    public void reducePossibleTiles(Tile[] inputArray)
    {
        List<Tile> tempList = new List<Tile>(); //Create a temperary list
        foreach (Tile tile in possibleTiles) //Goes through all possible tiles
        {
            if (inputArray.Contains(tile)) //Checks if the give array has the same tile
            {
                tempList.Add(tile); //Adds the tile to the temperary list
            }
        }

        int i = 0; //Create an index at 0
        foreach (Tile tile in tempList.ToArray()) //Goes through all the matching tiles
        {
            possibleTiles[i] = tile; //Sets the possible tiles to the matching tiles
            i++;
        }

        Array.Resize(ref possibleTiles, i); //Resize possible tiles to only have the matching tiles
    }
}
