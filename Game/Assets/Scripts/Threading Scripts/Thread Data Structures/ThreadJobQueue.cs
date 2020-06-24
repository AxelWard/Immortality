using System;
using System.Collections.Generic;

public class ThreadJobQueue<T> : List<T>
{
    new public void Add(T item) { throw new NotSupportedException(); }
    new public void AddRange(IEnumerable<T> collection) { throw new NotSupportedException(); }
    new public void Insert(int index, T item) { throw new NotSupportedException(); }
    new public void InsertRange(int index, IEnumerable<T> collection) { throw new NotSupportedException(); }
    new public void Reverse() { throw new NotSupportedException(); }
    new public void Reverse(int index, int count) { throw new NotSupportedException(); }
    new public void Sort() { throw new NotSupportedException(); }
    new public void Sort(Comparison<T> comparison) { throw new NotSupportedException(); }
    new public void Sort(IComparer<T> comparer) { throw new NotSupportedException(); }
    new public void Sort(int index, int count, IComparer<T> comparer) { throw new NotSupportedException(); }
    new public T[] ToArray() { throw new NotSupportedException(); }

    public void Enqueue(T item)
    {
        base.Add(item);
    }

    public T Dequeue()
    {
        var t = base[0];
        RemoveAt(0);
        return t;
    }

    public T Peek()
    {
        return base[0];
    }

    new public void Remove(T item)
    {
        foreach(T check in base.ToArray())
        {
            if(item.Equals(check))
            {
                base.Remove(check);
            }
        }
    }

    new public bool Contains(T item)
    {
        foreach(T check in base.ToArray())
        {
            if (item.Equals(check))
            {
                return true;
            }
        }

        return false;
    }

    public bool HasAJobToProcess()
    {
        if(base.Count > 0)
        {
            return true;
        }

        return false;
    }
}
