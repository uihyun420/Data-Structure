using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class ChainingHashTable<TKey, TValue> : IDictionary<TKey, TValue>
{
    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.75;

    private LinkedList<KeyValuePair<TKey, TValue>>[] table;

    private int size;
    private int count;

    private int GetIndex(TKey key, int size)  // 해시 함수 : 키의 해시코드 테이블 크기로 모듈로 연산
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        int hash = key.GetHashCode();
        return Math.Abs(hash) % size;
    }

    public ChainingHashTable()
    {
        size = DefaultCapacity;
        table = new LinkedList<KeyValuePair<TKey, TValue>>[size];
        count = 0;

        // 링크드리스트 배열 초기화 
        for(int i = 0; i < size; i++)
        {
            table[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }
            throw new KeyNotFoundException("키를 찾을 수 없습니다.");
        }
        set
        {
            if (key == null)    // 키 널체크
            {
                throw new ArgumentNullException(nameof(key));
            }

            if ((double)count / size >= LoadFactor) // 적재율 초과 시 리사이즈
            {
                Resize();
            }

            int index = GetIndex(key, size);
            var bucket = table[index];

            if(bucket == null)
            {
                bucket = new LinkedList<KeyValuePair<TKey, TValue>>();
                table[index] = bucket;
            }

            foreach(var kvp in bucket)
            {
                if(kvp.Key.Equals(key))
                {
                    bucket.Remove(kvp);
                    break;
                }
            }

            bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
            count++;
        }
    }


    //public TValue this[TKey key] 
    //{
    //    get
    //    {
    //        if(key == null)
    //            throw new ArgumentNullException(nameof(key));
    //        int index = GetIndex(key, size);
    //        var bucket = table[index]; // 해시 함수의 결과값에 따라 데이터가 저장되는 해시 테이블의 각 위치

    //        if(bucket != null)
    //        {
    //            foreach(var kvp in bucket)
    //            {
    //                if (kvp.Key.Equals(key))
    //                {
    //                    return kvp.Value;
    //                }
    //            }
    //        }

    //        throw new KeyNotFoundException("키 없음!");
    //    }
    //    set
    //    {
    //        if(key == null)
    //            throw new ArgumentNullException();

    //        int index = GetIndex(key, size);
    //        var bucket = table[index];

    //        if(bucket == null) // 데이터가 저장되는 곳이 비어 있으면 할당 해주기 
    //        {
    //            bucket = new LinkedList<KeyValuePair<TKey, TValue>>();
    //            table[index] = bucket;
    //        }

    //        foreach(var kvp in bucket)
    //        {
    //            if(kvp.Key.Equals(key))
    //            {
    //                bucket.Remove(kvp);
    //                break;
    //            }
    //        }

    //        bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
    //    }
    //}

    public ICollection<TKey> Keys => table.SelectMany(bucket => bucket.Select(kvp => kvp.Key)).ToList();    

    public ICollection<TValue> Values => table.SelectMany(bucket => bucket.Select(kvp => kvp.Value)).ToList();

    public int Count => count;

    public bool IsReadOnly => false;

    public void Resize()
    {
        int newSize = size * 2;
        var newTable = new LinkedList<KeyValuePair<TKey, TValue>>[newSize];

        for (int i = 0; i < newSize; i++) // 새배열 초기화
        {
            newTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
        }

        for (int i = 0; i < size; i++) // 기존 테이블 데이터를 새 테이블로 
        {
            var bucket = table[i];
            if (bucket != null)
            {
                foreach (var kvp in bucket)
                {
                    int newIndex = GetIndex(kvp.Key, newSize);
                    newTable[newIndex].AddLast(kvp);
                }
            }
        }

        table = newTable;
        size = newSize;
    }


    public void Add(TKey key, TValue value)
    {
        if(key == null)
            throw new ArgumentNullException();

        int index = GetIndex(key, size);
        var bucket = table[index];

        if(bucket == null)
        {
            bucket= new LinkedList<KeyValuePair<TKey, TValue>>();
            table[index] = bucket;
        }

        foreach( var kvp in bucket)
        {
            if(kvp.Key.Equals(key))
                throw new ArgumentException("키 중복");
        }

        bucket.AddLast(new KeyValuePair<TKey, TValue>(key, value));
        count++;

        if ((double)count / size > LoadFactor)
            Resize();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        for(int i = 0; i < size; i++)
        {
            table[i].Clear();   
        }
        count = 0;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if (item.Key == null)
            throw new ArgumentNullException();

        int index = GetIndex(item.Key, size);
        var bucket = table[index];

        if (bucket != null)
        {
            foreach (var kvp in bucket)
            {
                if (kvp.Key.Equals(item.Key) && kvp.Key.Equals(item.Value))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null) 
            throw new ArgumentNullException();

        int index = GetIndex(key, size);
        var bucket = table[index];

        if(bucket != null)
        {
            foreach(var kvp in bucket)
            {
                if(kvp.Key.Equals(key))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        int index = arrayIndex;
        foreach(var kvp in this)
        {
            array[index++] = kvp;   
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach(var kvp in bucket)
                {
                    yield return kvp;
                }
            }
        }        
    }

    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException();

        int index = GetIndex(key, size);
        var bucket = table[index];

        if(bucket != null)
        {
            var node = bucket.First;
            while(node != null)
            {
                if(node.Value.Key.Equals(key))
                {
                    bucket.Remove(node);
                    count--;
                    return true;
                }
                node = node.Next;
            }
        }
        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if(item.Key == null)
            throw new ArgumentNullException();

        int index = GetIndex(item.Key, size);
        var bucket = table[index];

        if(bucket != null)
        {
            var node = bucket.First;
            while(node != null)
            {
                if(node.Value.Equals(item.Value) && node.Value.Key.Equals(item.Key))
                {
                    bucket.Remove(node);
                    count--;
                    return true;
                }
                node = node.Next;
            }
        }
        return false;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if(key == null)
            throw new ArgumentNullException();

        int index = GetIndex(key, size);
        var bucket = table[index];

        if(bucket != null)
        {
            foreach(var kvp in bucket)
            {
                if(kvp.Key.Equals(key))
                {
                    value = kvp.Value;
                    return true;
                }
            }
        }
        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
    
