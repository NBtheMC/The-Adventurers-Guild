using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PriorityQueue
{
    //use linked list and then sort it in ascending order based on time to travel
    public List<(WorldLocation, double)> contents;
    public int Count { get { return contents.Count; } }

    public PriorityQueue() { contents = new List<(WorldLocation, double)>(); }

    public void Insert(WorldLocation n, double priority)
    {
        if(contents.Count == 0)
            contents.Add((n, priority));
        else
        {
            int i = 0;
            while (i < contents.Count)
            {
                if(contents[i].Item2 > priority)
                {
                    break;
                }
                i++;
            }
            contents.Insert(i, (n, priority));
        }     
    }

    public void DecreaseKey(WorldLocation n, double d)
    {
        for (int i = 0; i < contents.Count; i++)
        {
            if (contents[i].Item1 == n)
            {
                contents.RemoveAt(i);
                break;
            }
        }

        Insert(n, d);
    }

    public bool Contains(WorldLocation item)
    {
        for(int i = 0; i < contents.Count; i++)
        {
            if (contents[i].Item1 == item)
                return true;
        }
        return false;
    }

    public WorldLocation ExtractMin()
    {
        WorldLocation n = contents[0].Item1;
        contents.RemoveAt(0);
        return n;
    }

    public WorldLocation Peak()
    {
        WorldLocation n = contents[0].Item1;
        return n;
    }

    public bool IsEmpty()
    {
        return contents.Count == 0;
    }

    public void Clear()
    {
        contents.Clear();
    }
}

