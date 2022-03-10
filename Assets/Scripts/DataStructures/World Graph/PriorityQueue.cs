using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PriorityQueue<T>
{
    //use linked list and then sort it in ascending order based on time to travel
    public List<(T, double)> contents;
    public int Count { get { return contents.Count; } }

    public PriorityQueue() { contents = new List<(T, double)>(); }

    public void Insert(T n, double priority)
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

    public T ExtractMin()
    {
        T n = contents[0].Item1;
        contents.RemoveAt(0);
        return n;
    }

    public T Peak()
    {
        T n = contents[0].Item1;
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

