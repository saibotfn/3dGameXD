using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private int roomDim;
    [SerializeField] private int dimensions;
    [SerializeField] private GameObject tempCell;
    private List<Cell> grid = new List<Cell>();
    private List<List<Room>> roomGrid = new List<List<Room>>();
    [SerializeField] private int tileSize;
    List<List<int>> directions = new List<List<int>> { };
    [SerializeField] private bool run = true;

    void Start()
    {
        //Create grid with extra layer of Cell's around rooms
        int gridSize = dimensions * roomDim + roomDim + 1;
        for (int x = 0; x < gridSize; x++) 
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject temp = Instantiate(tempCell, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.identity);
                grid.Add(temp.GetComponent<Cell>());
            }
        }
        createRooms();
    }
    private void Update()
    {
        if (run)
        {
            if (grid.Count <= 0)
            {
                Debug.Log("STOP");
                run = false;
            }
            collapseCell();
            if (grid.Count <= 0)
            {
                Debug.Log("STOP");
                run = false;
            }
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
        cellToCollapse.collapse("none"); //Collapse the chosen Cell


        grid.Remove(cellToCollapse); //Remove the collapsed cell from the grid

        collapseNeighbors(cellToCollapse); //Reduce possible tiles of the collapsed cell's neighbors
    }

    public void collapseNeighbors(Cell collapsedCell)
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

    private void createRooms()
    {
        int gridSize = dimensions * roomDim + roomDim + 1;
        bool wallsMade = false;
        bool anyFound = false;
        while (!wallsMade)
        {
            anyFound = false;
            for (int i = 0; i < grid.Count; i++)
            {
                if (grid[i].transform.localPosition.x == 0 ||
                    grid[i].transform.localPosition.x == (dimensions * roomDim + roomDim) * tileSize ||
                    grid[i].transform.localPosition.z == 0 ||
                    grid[i].transform.localPosition.z == (dimensions * roomDim + roomDim) * tileSize)
                {
                    grid[i].collapse("air");
                    collapseNeighbors(grid[i]);
                    grid.Remove(grid[i]);
                    anyFound = true;
                    break;
                }
            }
            if (!anyFound)
            {
                wallsMade = true;
            }
        }

        //Creating a grid for the rooms
        for (int x = 0; x < roomDim; x++)
        {
            roomGrid.Add(new List<Room>());
            for (int y = 0; y < roomDim; y++)
            {
                roomGrid[x].Add(new Room(false));
            }
        }

        //Creating a list of directions (Up, Right, Down, Left)
        for(int i = 0; i < 4; i++)
        {
            directions.Add(new List<int>());
        }
        directions[0] = new List<int> { 0, 1 };
        directions[1] = new List<int> { 1, 0 };
        directions[2] = new List<int> { 0, -1 };
        directions[3] = new List<int> { -1, 0 };

        int X = UnityEngine.Random.Range(0, roomDim);
        int Y = UnityEngine.Random.Range(0, roomDim);
        List<Cell> cells = new List<Cell>(); //Cells to remove from grid after

        roomGrid = generateMaze(roomGrid, X, Y, 0);
        
        for (int x = 0; x < roomDim; x++)
        {
            for (int y = 0; y < roomDim; y++)
            {
                List<Cell> cellsToRemove = new List<Cell>();
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
                        cell.collapse("floor");
                        collapseNeighbors(cell);
                        cellsToRemove.Add(cell);
                        

                        for (int i = 0; i < 4; i++)
                        {
                            if (roomGrid[x][y].dirState[i])
                            {
                                switch (i)
                                {
                                    case 0:
                                        cells = createPath(cell, "Up", dimensions, cells);
                                        break;
                                    case 1:
                                        cells = createPath(cell, "Right", dimensions, cells);
                                        break;
                                    case 2:
                                        cells = createPath(cell, "Down", dimensions, cells);
                                        break;
                                    case 3:
                                        cells = createPath(cell, "Left", dimensions , cells);
                                        break;
                                }
                            }
                        }
                    }
                }
                foreach (Cell cell in cellsToRemove)
                {
                    grid.Remove(cell);
                }
            }
        }
        
        foreach (Cell cell in cells)
        {
            grid.Remove(cell);
        }

        generateRoomWalls();
    }

    private List<Cell> createPath(Cell cell ,string dir, int depth, List<Cell> maze)
    {
        if (depth < 0) return maze;

        maze.Add(cell);

        foreach (Cell neighborCheck in grid)//Check every cell if its a neighbor to the collapsed Cell
        {
            bool neighbor = false;
            switch (dir)
            {
                case "Up":
                    if (neighborCheck.transform.localPosition.x == cell.transform.localPosition.x)
                    {
                        if (neighborCheck.transform.localPosition.z == cell.transform.localPosition.z + tileSize)
                        {
                            maze = createPath(neighborCheck, "Up", depth - 1, maze);
                            neighbor = true;
                        }  
                    }
                    break;
                case "Right":
                    if (neighborCheck.transform.localPosition.x == cell.transform.localPosition.x + tileSize)
                    {
                        if (neighborCheck.transform.localPosition.z == cell.transform.localPosition.z)
                        {
                            maze = createPath(neighborCheck, "Right", depth - 1, maze);
                            neighbor = true;
                        }
                    }
                    break;
                case "Down":
                    if (neighborCheck.transform.localPosition.x == cell.transform.localPosition.x)
                    {
                        if (neighborCheck.transform.localPosition.z == cell.transform.localPosition.z - tileSize)
                        {
                            maze = createPath(neighborCheck, "Down", depth - 1, maze);
                            neighbor = true;
                        }
                    }
                    
                    break;
                case "Left":
                    if (neighborCheck.transform.localPosition.x == cell.transform.localPosition.x - tileSize)
                    {
                        if (neighborCheck.transform.localPosition.z == cell.transform.localPosition.z)
                        {
                            maze = createPath(neighborCheck, "Left", depth - 1, maze);
                            neighbor = true;
                        }
                    }
                    break;
            };
            if (neighbor)
            {
                if(depth == (dimensions / 2) + 1)
                {
                    foreach(Cell doorNeighbor in grid)
                    {
                        if(dir == "Up" || dir == "Down")
                        {
                            //Check right
                            if (doorNeighbor.transform.localPosition.x == neighborCheck.transform.localPosition.x + tileSize)
                            {
                                if (doorNeighbor.transform.localPosition.z == neighborCheck.transform.localPosition.z)
                                {
                                    doorNeighbor.collapse("right");
                                    collapseNeighbors(doorNeighbor);
                                    maze.Add(doorNeighbor);
                                }
                            }
                            //Check Left
                            else if (doorNeighbor.transform.localPosition.x == neighborCheck.transform.localPosition.x - tileSize)
                            {
                                if (doorNeighbor.transform.localPosition.z == neighborCheck.transform.localPosition.z)
                                {
                                    doorNeighbor.collapse("left");
                                    collapseNeighbors(doorNeighbor);
                                    maze.Add(doorNeighbor);
                                }
                            }
                        }
                        else if(dir == "Right" || dir == "Left")
                        {
                            //Check Up
                            if (doorNeighbor.transform.localPosition.x == neighborCheck.transform.localPosition.x)
                            {
                                if (doorNeighbor.transform.localPosition.z == neighborCheck.transform.localPosition.z + tileSize)
                                {
                                    doorNeighbor.collapse("up");
                                    collapseNeighbors(doorNeighbor);
                                    maze.Add(doorNeighbor);
                                }
                                //Check Down
                                else if (doorNeighbor.transform.localPosition.z == neighborCheck.transform.localPosition.z - tileSize)
                                {
                                    doorNeighbor.collapse("down");
                                    collapseNeighbors(doorNeighbor);
                                    maze.Add(doorNeighbor);
                                }
                            }
                        }
                    }
                }
                neighborCheck.collapse("floor");
                collapseNeighbors(neighborCheck);
            }
        }
        return maze;
    }

    private List<List<Room>> generateMaze(List<List<Room>> rooms, int x, int y, int depth)
    {
        rooms[x][y].visited = true;
        rooms[x][y].depth = depth;
        List<int[]> possibleDir = new List<int[]>();
        
        while (true)
        {

            possibleDir.Clear();
            
            foreach (List<int> dir in directions)
            {
                if (x + dir[0] < 0 || x + dir[0] > roomDim - 1 || y + dir[1] < 0 || y + dir[1] > roomDim - 1) { }
                else if (rooms[x + dir[0]][y + dir[1]].visited == false)
                {
                    possibleDir.Add(new int[] { dir[0], dir[1] });
                }
            }

            if (possibleDir.Count == 0) //Return if there are no directions to take
            {
                rooms[x][y].blind = true; //Mark the room as a blind way
                return rooms;
            }

            int index = UnityEngine.Random.Range(0, possibleDir.Count);
            int X = possibleDir[index][0];
            int Y = possibleDir[index][1];

            switch (X, Y) //Save what direction it takes
            {
                case (0,1):
                    rooms[x][y].dirState[0] = true;
                    break;
                case (1,0):
                    rooms[x][y].dirState[1] = true;
                    break;
                case (-1, 0):
                    rooms[x][y].dirState[3] = true;
                    break;
                case (0,-1):
                    rooms[x][y].dirState[2] = true;
                    break;
            }
            rooms = generateMaze(rooms, x + X, y + Y, depth + 1);
            possibleDir.RemoveAt(index);
        }
    }

    private void generateRoomWalls()
    {
        for (int x = 0; x < roomDim; x++)
        {
            for (int z = 0; z < roomDim; z++)
            {
                List<Cell> cellsToRemove = new List<Cell>();
                foreach (Cell cell in grid)
                {
                    int centerX = (dimensions * (x + 1) + x + 1) * tileSize;
                    int centerZ = (dimensions * (z + 1) + z + 1) * tileSize;

                    if (cell.transform.localPosition.x == centerX || cell.transform.localPosition.z == centerZ)
                    {
                        cellsToRemove.Add(cell);
                    }
                }

                foreach(Cell cell in cellsToRemove)
                {
                    cell.collapse("air");
                    collapseNeighbors(cell);
                    grid.Remove(cell);
                }
            }
        }
    }
}
