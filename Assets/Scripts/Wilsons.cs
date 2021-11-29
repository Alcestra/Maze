using System.Collections.Generic;
using UnityEngine;

public class Wilsons : Maze
{
   

    List<MapLocation> notUsed = new List<MapLocation>();
    public override void Generate()
    {      

        // make a random starting point
        int x = Random.Range(2, Width - 1);
        int z = Random.Range(2, Depth - 1);
        map[x, z] = 2;

       while(GetAvailabeCells()> 1)
        {
            RandomWalk();
        }
    }

    int CountSquareMazeNeighbours(int x, int z) 
    {
        int Count = 0;
        for (int D = 0; D < Directions.Count; D++)
        {
            int NX = x + Directions[D].x;
            int NZ = z + Directions[D].z;
            if (map[NX, NZ] == 2) 
            {
                Count++;
            }

        }
        return Count;    
    }

    int GetAvailabeCells()
    {
        notUsed.Clear();
        for(int z = 1; z < Depth -1; z++)
            for (int x = 1; x < Width -1; x++)
            {
                if(CountSquareMazeNeighbours(x,z) == 0)
                {
                    notUsed.Add(new MapLocation(x, z));
                }
            }

        return notUsed.Count;
    }


    void RandomWalk()
    {

        List<MapLocation> inWalk = new List<MapLocation>();

        int cx;
        int cz;
        int rstartIndex = Random.Range(0, notUsed.Count);

        cx = notUsed[rstartIndex].x;
        cz = notUsed[rstartIndex].z;


        inWalk.Add(new MapLocation(cx, cz));

        int loop = 0;
        bool validPath = false;
        while (cx > 0 && cx < Width - 1 && cz > 0 && cz < Depth - 1 && loop < 5000 && !validPath) 
        {
            map[cx, cz] = 0;
            if (CountSquareMazeNeighbours(cx, cz) > 1)
                break;

            int RD = Random.Range(0, Directions.Count);
            int NX = cx + Directions[RD].x;
            int NZ = cz + Directions[RD].z;
            
            if (CountSqueareNeighbours(NX, NZ) < 2) 
            {
                cx = NX;
                cz = NZ;
                inWalk.Add(new MapLocation(cx, cz));

            }
            validPath = CountSquareMazeNeighbours(cx, cz) == 1;

            loop++;
        }
        if (validPath)
        {

            map[cx, cz] = 0;
            Debug.Log("pathFound");

            foreach (MapLocation m in inWalk)
            {
                map[m.x, m.z] = 2;
            }
            inWalk.Clear();

        }
        else
        {
            foreach (MapLocation m in inWalk)
                map[m.x, m.z] = 1;
        }
            inWalk.Clear();
        
    }
    
}
