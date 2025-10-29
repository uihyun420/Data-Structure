using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement Element, TPriority Priority)> heap;

    public PriorityQueue()
    {
        heap = new List<(TElement, TPriority)>();
    }

    public int Count => heap.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        // TODO: 구현
        // 1. 새 요소를 리스트 끝에 추가
        // 2. HeapifyUp으로 힙 속성 복구

        heap.Add((element, priority));
        HeapifyUp(heap.Count - 1);
    }

    public TElement Dequeue()
    {
        // TODO: 구현
        // 1. 빈 큐 체크 및 예외 처리
        // 2. 루트 요소 저장
        // 3. 마지막 요소를 루트로 이동
        // 4. HeapifyDown으로 힙 속성 복구
        // 5. 저장된 루트 요소 반환

        if (heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어있습니다.");
        }

        int lastIndex = heap.Count - 1;
        var rootElement = heap[0].Element;

        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);     
        
        if(heap.Count >0)
        {
            HeapifyDown(0);
        }

        return rootElement;
    }

    public TElement Peek()
    {
        // TODO: 구현
        // 1. 빈 큐 체크 및 예외 처리
        // 2. 루트 요소 반환
        
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어있습니다.");
        }
        return heap[0].Element;
    }

    public void Clear()
    {
        // TODO: 구현
        heap.Clear();
    }

    private void HeapifyUp(int index)
    {
        // TODO: 구현
        // 현재 노드가 부모보다 작으면 교환하며 위로 이동

        while (index > 0)
        {
            int parrentIndex = (index - 1) / 2;

            if (heap[index].Priority.CompareTo(heap[parrentIndex].Priority) >= 0)
                break;

            var temp = heap[index];
            heap[index] = heap[parrentIndex];
            heap[parrentIndex] = temp;            

            index = parrentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        // TODO: 구현
        // 현재 노드가 자식보다 크면 더 작은 자식과 교환하며 아래로 이동
        int lastIndex = heap.Count - 1;

        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int smallest = index;

            if (leftChildIndex <= lastIndex && heap[leftChildIndex].Priority.CompareTo(heap[smallest].Priority) < 0)
            {
                smallest = leftChildIndex;
            }

            if(rightChildIndex <= lastIndex && heap[rightChildIndex].Priority.CompareTo(heap[smallest].Priority) < 0)
            {
                smallest = rightChildIndex;
            }

            if(smallest == index) break;

            var temp = heap[index];
            heap[index] = heap[smallest];
            heap[smallest] = temp;

            index = smallest;
        }
    }
}
