using UnityEngine;

public class Room
{
    public bool visited = false;
    public bool[] dirState = new bool[4];
    public bool blind = false;
    public int depth = 0;
    public Room(bool visited)
    {
        this.visited = visited;
        this.dirState = dirState;
        this.blind = blind;
        this.depth = depth;
    }
}
