using UnityEngine;

public class Crawler : Maze
{
    public override void Generate()
    {
        for (int i = 0; i < 3; i++)
        {
            CrawlH();
        }

        for (int i = 0; i < 3; i++)
        {
            CrawlV();
        
        }
    }

    private void CrawlV()
    {
        bool done = false;
        int x = Random.Range(1, Width);
        int z = 1;

        while (!done)
        {

            map[x, z] = 0;
            if (Random.Range(0, 100) < 50)
                x += Random.Range(-1, 2);
            else
                z += Random.Range(0, 2);
            done |= (x < 1 || x >= Width-1 || z < 1 || z >= Depth-1);
        }

    }

    private void CrawlH()
    {
        bool done = false;
        int x = 1;
        int z = Random.Range(0, Depth);

        while (!done)
        {

            map[x, z] = 0;
            if (Random.Range(0, 100) < 50)
                x += Random.Range(0, 2);
            else
                z += Random.Range(-1, 2);
            done |= (x < 1 || x >= Width-1|| z < 1 || z >= Depth-1);
        }

    }
}
