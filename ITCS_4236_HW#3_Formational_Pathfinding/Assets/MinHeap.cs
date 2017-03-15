using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Code by ALLAN RIORDAN BOLL'S BLOG
//Thursday, December 08, 2011
//http://allanrbo.blogspot.com/2011/12/simple-heap-implementation-priority.html

class MinHeap<T> where T : Ground_Tile
{
    private List<T> data = new List<T>();

    public void Insert(T o)
    {
        data.Add(o);

        int i = data.Count - 1;
        while (i > 0)
        {
            int j = (i + 1) / 2 - 1;

            // Check if the invariant holds for the element in data[i]  
            T v = data[j];
            if (v.CompareTo(data[i]) < 0 || v.CompareTo(data[i]) == 0)
            {
                break;
            }

            // Swap the elements  
            T tmp = data[i];
            data[i] = data[j];
            data[j] = tmp;

            i = j;
        }
    }

    public T ExtractMin()
    {
        if (data.Count < 0)
        {
           Debug.Log("MinHeap is empty");
        }

        T min = data[0];
        data[0] = data[data.Count - 1];
        data.RemoveAt(data.Count - 1);
        this.MinHeapify(0);
        return min;
    }

    public T Peek()
    {
        return data[0];
    }

    public T Peek(T looking)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (looking.equals(data[i]))
            {
                return data[i];
            }
        }
        return data[0];
    }
    public void changeValuesAt(int i, int g, Ground_Tile newParent)
    {
        data[i].updateValues(g);
        data[i].parent = newParent;
        MinHeapify(i);
    }
    public int PeekL(T looking)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (looking.equals(data[i]))
            {
                return i;
            }
        }
        return 0;
    }
    public int Count
    {
        get { return data.Count; }
    }

    public bool contains(T altered)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (altered.equals(data[i]))
            {
                return true;
            }
        }
        return false;
    }

    private void MinHeapify(int i)
    {
        int smallest;
        int l = 2 * (i + 1) - 1;
        int r = 2 * (i + 1) - 1 + 1;

        if (l < data.Count && (data[l].CompareTo(data[i]) < 0))
        {
            smallest = l;
        }
        else
        {
            smallest = i;
        }

        if (r < data.Count && (data[r].CompareTo(data[smallest]) < 0))
        {
            smallest = r;
        }

        if (smallest != i)
        {
            T tmp = data[i];
            data[i] = data[smallest];
            data[smallest] = tmp;
            this.MinHeapify(smallest);
        }
    }
}
