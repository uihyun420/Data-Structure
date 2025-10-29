using System;
using System.Collections.Generic;
using UnityEngine;
public class PriorityQueueTest : MonoBehaviour
{
    new PriorityQueue<string, int> pq = new PriorityQueue<string, int>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            pq.Enqueue("Low", 10);
            pq.Enqueue("High", 1);
            pq.Enqueue("Medium", 5);

            Debug.Log(pq.Dequeue()); // "High" (우선순위 1)
            Debug.Log(pq.Dequeue()); // "Medium" (우선순위 5)
            Debug.Log(pq.Dequeue()); // "Low" (우선순위 10) 
        }
    }
}
