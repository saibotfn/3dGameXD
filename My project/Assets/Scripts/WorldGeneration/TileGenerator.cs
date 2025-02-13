using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public int dimensions;
    public GameObject tempCell;
    public List<Cell> grid = new List<Cell>();
    public int tileSize;


    void Start()
    {
        // Create grid of Cell's with dimensions
        for (int x = 0; x < dimensions; x++)
        {
            for (int y = 0; y < dimensions; y++)
            {
                GameObject temp = Instantiate(tempCell, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.identity);
                grid.Add(temp.GetComponent<Cell>());
            }
        }

        //Call collapseCell the amount by the amount of Cell's there are
        for (int i = 0; i < dimensions * dimensions; i++)
        {
            collapseCell();
        }
    }

    public void collapseCell()
    {
        List<Cell> sortedGrid = grid.OrderBy(l => l.possibleTiles.Length).ToList(); //Create a sorted version of the grid
        int index = 0; // Set an index to 0
        int lowestEntropy = sortedGrid[0].possibleTiles.Length; //Select the cell with the lowest entropy
        //Find how many Cell's share the lowest entropy and count with index
        foreach (Cell cell in sortedGrid) {
            if (cell.possibleTiles.Length == lowestEntropy) {
                index++;
            } else {
                break;
            }
        }


        List<Cell> slicedSortedList = sortedGrid.GetRange(0, index); //Create new list with only lowest entropy Cell's
        int randIndex = UnityEngine.Random.Range(0, slicedSortedList.Count); //Pick a random index in the sliced list
        Cell cellToCollapse = slicedSortedList[randIndex]; //Select the Cell of that index
        cellToCollapse.collapse(); //Collapse the chosen Cell


        grid.Remove(cellToCollapse); //Remove the collapsed cell from the grid

        collapseNeighbors(cellToCollapse, randIndex); //Reduce possible tiles of the collapsed cell's neighbors
    }

    public void collapseNeighbors(Cell collapsedCell, int tileIndex)
    {
        foreach(Cell neighborCheck in grid)//Check every cell if its a neighbor to the collapsed Cell
        {
            //Up
            if (neighborCheck.transform.localPosition.x == collapsedCell.transform.localPosition.x)
            {
                if (neighborCheck.transform.localPosition.z == collapsedCell.transform.localPosition.z + tileSize)
                {
                    neighborCheck.reducePossibleTiles(collapsedCell.tile.upNeighbors); //Call reduce method on the neighbor
                }

                //Down
                else if (neighborCheck.transform.localPosition.z == collapsedCell.transform.localPosition.z - tileSize)
                {
                    neighborCheck.reducePossibleTiles(collapsedCell.tile.downNeighbors);
                }
            }

            //Right
            else if (neighborCheck.transform.localPosition.x == collapsedCell.transform.localPosition.x + tileSize)
            {
                if (neighborCheck.transform.localPosition.z == collapsedCell.transform.localPosition.z)
                {
                    neighborCheck.reducePossibleTiles(collapsedCell.tile.rightNeighbors);
                }
            }

            //Left
            else if (neighborCheck.transform.localPosition.x == collapsedCell.transform.localPosition.x - tileSize)
            {
                if (neighborCheck.transform.localPosition.z == collapsedCell.transform.localPosition.z)
                {
                    neighborCheck.reducePossibleTiles(collapsedCell.tile.leftNeighbors);
                }
            }

        }
    }
}
