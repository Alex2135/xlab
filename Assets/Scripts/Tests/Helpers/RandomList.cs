using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomList<T> 
{
    private List<T> list;

    public RandomList(IEnumerable<T> _collection)
    {
        list = new List<T>();
        foreach (var item in _collection)
        {
            if (item is ICloneable _clonable)
                list.Add((T)_clonable.Clone());
            else
                list.Add(item);
        }
    }

    public List<T> GetRandomSubsetWithRightItem(T _item, int _capacity, Func<T, T, bool> _comparer)
    {
        if (_item is ICloneable c && c == null) throw new ArgumentNullException("_item is null");
        if (_comparer == null) throw new ArgumentNullException("_comparer is null");
        if (_capacity < 0 || _capacity >= list.Count) throw new ArgumentOutOfRangeException("_capacity out of list range");

        var result = new List<T>();

        for (int i = 0; i < list.Count; i++)
        {
            if (_comparer(list[i], _item))
            {
                result.Add(list[i]);
                list.RemoveAt(i);
            }
        }

        if (result.Count == 0) throw new ArgumentException("_item not in list!");

        for (int i = 1; i < _capacity; i++)
        {
            var index = UnityEngine.Random.Range(0, list.Count);
            var item = list[index];
            result.Add(item);
            list.RemoveAt(index);
        }

        return result;
    }
}