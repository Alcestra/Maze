using System.Collections.Generic;
using UnityEngine;

public class MapLocation 
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z) 
    {
        x = _x;
        z = _z;
    }



}

public class Maze : MonoBehaviour
{
    public List<MapLocation> Directions = new List<MapLocation>()
    {
            new MapLocation (1,0),
            new MapLocation (0,1),
            new MapLocation (-1,0),
            new MapLocation (0,-1)
    };

    //maze size  
    public List<MapLocation> pillarLocations = new List<MapLocation>();
    public int Width = 30; // x
    public int Depth = 30; // z
    public byte[,] map;
    public int scale = 6;

    //pieces 
    public GameObject straight;
    public GameObject crossRoads;
    public GameObject tSplit;
    public GameObject deadEnd;
    public GameObject corner;
    public GameObject floorPiece;
    public GameObject wallPiece;
    public GameObject CielingPiece;

    public GameObject Pillar;
    public GameObject Door;

    public GameObject FPC;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        Generate();
        AddRooms(4,4,5);//last numbers have to do with how many rooms, how far on the x can they be and how far on the z can they be
        DrawMap();
        PlaceFPC();
    }
    public virtual void AddRooms(int count, int minSize, int maxSize)
    {
        for (int c = 0; c < count; c++)
        {
            int startX = Random.Range(3,Width -3);
            int startZ = Random.Range(3, Depth - 3);
            int roomWidth = Random.Range(minSize, maxSize);
            int roomDepth = Random.Range(minSize, maxSize);

            for (int x = startX; x < Width-3 && x < startX +roomWidth; x++)
            {
                for (int z = startZ; z < Depth - 3 && z <  startZ + roomDepth; z++)
                {
                    map[x, z] = 0;
                }
            }
        }
    }

    private void InitialiseMap()
    {
        map = new byte[Width, Depth];
        for (int z = 0; z < Depth; z++)
            for (int x = 0; x < Width; x++)
            {
                map[x, z] = 1; //1 wall / 0 cirridor               
            }
    }

    public virtual void PlaceFPC()
    {
        for (int z = 0; z < Depth; z++)
            for (int x = 0; x < Width; x++)
            {
                if(map[x,z] == 0)
                {   
                    FPC.transform.position = new Vector3(x * scale, 0, z * scale);
                    return;
                }
            }            
    }

    public virtual void Generate()
    {
        for (int z = 0; z < Depth; z++)
            for (int x = 0; x < Width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    map[x, z] = 0;
            }
    }

    void DrawMap()
    {
        for (int z = 0; z < Depth; z++)
            for (int x = 0; x < Width; x++)
            {
                if (map[x, z] == 1)
                {

                }
                // Vertical straight
                else if (Search2D(x, z,new int[] {5,0,5,1,0,1,5,0,5}))                    
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = straight = Instantiate(straight, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);

                }
                // Horizontal straight
                else if (Search2D(x, z,new int[] {5,1,5,0,0,0,5,1,5}))                    
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(straight, pos, Quaternion.identity);

                }
                //cross roads
                else if (Search2D(x, z, new int[] { 1, 0, 1,0, 0, 0, 1, 0, 1 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    Instantiate(crossRoads, pos, Quaternion.identity);                    
                }
                // end horizontal -|
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 5, 1, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(deadEnd, pos, Quaternion.identity);
                    go.transform.Rotate(0, -180, 0);
                }
                // end horiontal |-
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 1, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                   Instantiate(deadEnd, pos, Quaternion.identity);
                    
                }// end vertical T
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 1, 5, 0, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(deadEnd, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                // end vertical T upside down
                else if (Search2D(x, z, new int[] { 5, 0, 5, 1, 0, 1, 5, 1, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(deadEnd, pos, Quaternion.identity);
                    go.transform.Rotate(0, -90, 0);
                }
                //corner right top
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 1, 1, 0, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(corner, pos, Quaternion.identity);
                    go.transform.Rotate(0, -180, 0);

                }//corner left top
                else if (Search2D(x, z, new int[] { 5, 1, 5, 1, 0, 0, 5, 0, 1 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(corner, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                //corner left bottom
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 1, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(corner, pos, Quaternion.identity);                   
                }
                //corner right bottom
                else if (Search2D(x, z, new int[] { 1, 0, 5, 5, 0, 1, 5, 1, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(corner, pos, Quaternion.identity);
                    go.transform.Rotate(0, -90, 0);
                }
                // Vertical up T
                else if (Search2D(x, z, new int[] { 1, 0, 1, 0, 0, 0, 5, 1, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(tSplit, pos, Quaternion.identity);
                    go.transform.Rotate(0, -90, 0);
                }
                // Vertical down T
                else if (Search2D(x, z, new int[] { 5, 1, 5, 0, 0, 0, 1, 0, 1 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(tSplit, pos, Quaternion.identity);
                    go.transform.Rotate(0, 90, 0);
                }
                // Horizontal left T
                else if (Search2D(x, z, new int[] { 1, 0, 5, 0, 0, 1, 1, 0, 5 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(tSplit, pos, Quaternion.identity);
                    go.transform.Rotate(0, 180, 0);
                }
                //Horizontal right T
                else if (Search2D(x, z, new int[] { 5, 0, 1, 1, 0, 0, 5, 0, 1 }))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(tSplit, pos, Quaternion.identity);
                }
                else if (map[x,z] == 0 && CountSqueareNeighbours(x,z) > 1 && CountDiagonalNeighbours(x,z)>= 1
                    || CountSqueareNeighbours(x,z) >= 1 && CountDiagonalNeighbours(x,z) > 1)
                {
                    GameObject floor = Instantiate(floorPiece);
                    floor.transform.position = new Vector3(x * scale, 0, z * scale);

                    GameObject ceiling = Instantiate(CielingPiece);
                    ceiling.transform.position = new Vector3(x * scale, 0, z * scale);

                    //WALLS
                    GameObject pillarCorner;
                    LocateWalls(x, z);
                    if (top)
                    {
                        GameObject wall1 = Instantiate(wallPiece);
                        wall1.transform.position= new Vector3(x * scale, 0, z * scale);
                        wall1.name = "Top Wall";

                        //pillars
                        if (map[x + 1, z] == 0&& map[x + 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x,z)))
                        {
                            pillarCorner  = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3(x * scale, 0, z * scale);
                            pillarCorner.name = "Top Right ";
                            pillarLocations.Add(new MapLocation(x, z));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);
                        }
                        if (map[x - 1, z] == 0 && map[x - 1, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x-1, z)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3((x-1) * scale, 0, z * scale);
                            pillarCorner.name = "Top Left ";
                            pillarLocations.Add(new MapLocation(x-1, z));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);

                        }
                    }
                    if (bottom)
                    {
                        GameObject wall2 = Instantiate(wallPiece);
                        wall2.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall2.transform.Rotate(0, 180, 0);
                        wall2.name = "Bottom Wall";

                        //pillars
                        if (map[x + 1, z] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z-1)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3(x * scale, 0, (z-1) * scale);
                            pillarCorner.name = "Bottom Right ";
                            pillarLocations.Add(new MapLocation(x, z-1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);


                        }
                        if (map[x - 1, z] == 0 && map[x - 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x-1, z-1)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3((x - 1) * scale, 0, (z-1) * scale);
                            pillarCorner.name = "Bottom Left ";
                            pillarLocations.Add(new MapLocation(x-1, z-1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);

                        }
                    }
                    if (right)
                    {
                        GameObject wall3 = Instantiate(wallPiece);
                        wall3.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall3.transform.Rotate(0, 90, 0);
                        wall3.name = "Right Wall";
                        //pillars
                        if (map[x + 1, z +1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3(x * scale, 0, z * scale);
                            pillarCorner.name = "right top ";
                            pillarLocations.Add(new MapLocation(x, z));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);


                        }
                        if (map[x, z -1] == 0 && map[x + 1, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x, z-1)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3(x * scale, 0, (z - 1) * scale);
                            pillarCorner.name = "Right bottom ";
                            pillarLocations.Add(new MapLocation(x, z-1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);

                        }

                    }
                    if (left)
                    {
                        GameObject wall4 = Instantiate(wallPiece);
                        wall4.transform.position = new Vector3(x * scale, 0, z * scale);
                        wall4.transform.Rotate(0, -90, 0);
                        wall4.name = "Left Wall";
                        //pillars
                        if (map[x - 1, z + 1] == 0 && map[x, z + 1] == 0 && !pillarLocations.Contains(new MapLocation(x-1, z)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3((x -1) * scale, 0, z * scale);
                            pillarCorner.name = "Left top ";
                            pillarLocations.Add(new MapLocation(x-1, z));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);

                        }
                        if (map[x -1 , z - 1] == 0 && map[x, z - 1] == 0 && !pillarLocations.Contains(new MapLocation(x-1, z-1)))
                        {
                            pillarCorner = Instantiate(Pillar);
                            pillarCorner.transform.position = new Vector3( (x - 1) * scale, 0, (z - 1) * scale);
                            pillarCorner.name = "Left bottom ";
                            pillarLocations.Add(new MapLocation(x-1, z-1));
                            pillarCorner.transform.localScale = new Vector3(1.01f, 1f, 1.01f);

                        }
                    }

                    //DOORS
                    GameObject doorWay;
                    LocateDoors(x, z);
                    if (top)
                    {
                        doorWay = Instantiate(Door);
                        doorWay.transform.position = new Vector3(x * scale, 0, z * scale);
                        doorWay.transform.Rotate(0, 180, 0);
                        doorWay.name = "Top Doorway";
                        doorWay.transform.Translate(0, 0, 0.01f);
                    }
                    if (bottom)
                    {
                        doorWay = Instantiate(Door);
                        doorWay.transform.position = new Vector3(x * scale, 0, (z-1) * scale);
                        doorWay.name = "Bottom Doorway";
                        doorWay.transform.Translate(0, 0, 0.01f);

                    }
                    if (right)
                    {
                        doorWay = Instantiate(Door);
                        doorWay.transform.position = new Vector3((x+1) * scale, 0, z * scale);
                        doorWay.transform.Rotate(0,90, 0);
                        doorWay.name = "Right Doorway";
                        doorWay.transform.Translate(0, 0, 0.01f);

                    }
                    if (left)
                    {
                        doorWay = Instantiate(Door);
                        doorWay.transform.position = new Vector3((x-1) * scale, 0, z * scale);
                        doorWay.transform.Rotate(0,-90, 0);
                        doorWay.name = "Left Doorway";
                        doorWay.transform.Translate(0, 0, 0.01f);
                    }
                }
                else    
                {                    
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject block = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    block.transform.localScale = new Vector3(scale, scale, scale);
                    block.transform.position = pos;                    
                }
            }
    }
    bool top;
    bool bottom;
    bool left;
    bool right;

    public void LocateWalls(int x, int z)
    {
        top = false;
        bottom = false;
        left = false;
        right = false;

        if (x <= 0 || x >= Width - 1 || z <= 0 || z >= Depth - 1) return;
        if (map[x, z + 1] == 1) top = true;
        if (map[x, z - 1] == 1) bottom = true;
        if (map[x + 1, z] == 1) right = true;
        if (map[x-1 , z] == 1) left = true;
    }
    public void LocateDoors(int x, int z)
    {
        top = false;
        bottom = false;
        left = false;
        right = false;

        if (x <= 0 || x >= Width - 1 || z <= 0 || z >= Depth - 1) return;
        if (map[x, z + 1] == 0 && map[x-1, z+1] == 1 && map[x+1,z+1]==1) top = true;
        if (map[x, z - 1] == 0 && map[x-1, z-1] == 1 && map[x+1,z-1]==1) bottom = true;
        if (map[x + 1, z] == 0 && map[x+1 ,z+1] == 1 && map[x+1,z-1]==1) right = true;
        if (map[x - 1, z] == 0 && map[x-1, z+1] == 1 && map[x-1,z-1]==1) left = true;
    }

    bool Search2D(int c, int r, int[] pattern)
    {


        int count = 0;
        int pos = 0;
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
               if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)
                    count++;
                pos++;
            }
        }

        return (count == 9);

    }


    public int CountSqueareNeighbours(int x, int z) 
    {
        int Count = 0;
        if (x <= 0 || x >= Width - 1 || z <= 0 || z >= Depth - 1) return 5;
        if (map[x, z-1] == 0) Count++;
        if (map[x, z+1] == 0) Count++;
        if (map[x - 1, z] == 0) Count++;
        if (map[x + 1, z] == 0) Count++;
        return Count;
    }


    public int CountDiagonalNeighbours(int x, int z)
    {
        int Count = 0;
        if (x <= 0 || x >= Width - 1 || z <= 0 || z >= Depth - 1) return 5;
        if (map[x-1,z+1] == 0) Count++;
        if (map[x-1,z-1] == 0) Count++;
        if (map[x+1,z+1] == 0) Count++;
        if (map[x+1,z-1] == 0) Count++;

        return Count;
    }

    public int CountAllNeighbours(int x, int z) 
    {
       return CountSqueareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }

}

