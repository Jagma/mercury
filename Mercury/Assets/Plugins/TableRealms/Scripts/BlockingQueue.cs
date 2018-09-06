using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System;

class BlockingQueue<T> : IEnumerable<T> {
    private int count = 0;

    private Queue<T> queue = new Queue<T>();

    public T Dequeue() {
        lock (queue) {
            while (count <= 0) {
                Monitor.Wait(queue);
            }
            count--;
            return queue.Dequeue();
        }
    }

    public void Enqueue(T data) {
        if (data == null) {
            throw new ArgumentNullException("data");
        }

        lock (queue) {
            queue.Enqueue(data);
            count++;
            Monitor.Pulse(queue);
        }
    }

    public int Count() {
        return count;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        while (true) yield return Dequeue();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return ((IEnumerable<T>)this).GetEnumerator();
    }
}