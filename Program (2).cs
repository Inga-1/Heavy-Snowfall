using System;
using System.Collections.Generic;
using static System.Console;
namespace snowfall
{
    class Program
    {
    interface PriorityQueue<T> {
    int count { get; }
    void add(T elem, double priority);
    (T, double) extractMin();
    void decrease(T elem, double priority);
}

class BinaryHeap<T> : PriorityQueue<T> {
    class Node {
        public T elem;
        public double prio;
        public Node(T elem, double prio) { this.elem = elem; this.prio = prio; }
    }
    
    List<Node> a = new List<Node>();
    public Dictionary<T, int> pos = new Dictionary<T, int>();
    
    static int parent(int i) => (i - 1) / 2;
    static int left(int i) => 2 * i + 1;
    static int right(int i) => 2 * i + 2;
    
    public int count => a.Count;
    
    void swap(int i, int j) {
        (a[i], a[j]) = (a[j], a[i]);
        pos[a[i].elem] = i;
        pos[a[j].elem] = j;
    }
    
    void up_heap(int i) {
        while (i > 0) {
            int p = parent(i);
            if (a[p].prio <= a[i].prio)
                break;
            swap(i, p);
            i = p;
        }
    }
    
    public void add(T elem, double prio) {
        a.Add(new Node(elem, prio));
        pos[elem] = count - 1;
        up_heap(count - 1);
    }
    
    void down_heap(int i) {
        while (true) {
            int min = i;
            int l = left(i), r = right(i);
            if (l < count && a[l].prio < a[min].prio)
                min = l;
            if (r < count && a[r].prio < a[min].prio)
                min = r;
            if (min == i)
                break;
            
            swap(i, min);
            i = min;
        }
    }
            
    public (T, double) extractMin() {
        Node n = a[0];
        a[0] = a[count - 1];
        pos[a[0].elem] = 0;
        a.RemoveAt(count - 1);
        if (count > 0)
            down_heap(0);
        
        if (!pos.Remove(n.elem))
            throw new Exception("elem not found");
            
        return (n.elem, n.prio);
    }
    
    public void decrease(T elem, double prio) {
        int i;
        if (pos.ContainsKey(elem))
            i = pos[elem];
        else throw new Exception("elem not found");
        a[i].prio = prio;
        up_heap(i);
    }
}
class Graph {
    int num;
    List<(int, double)>[] edge;  // edge[v] is a list of pairs (w, weight), where v -> w

    Graph() {
        num = int.Parse(ReadLine());
        edge = new List<(int, double)>[num + 1];
        for (int i = 1 ; i <= num ; ++i)
            edge[i] = new List<(int, double)>();
        
        while (ReadLine() is string s) {
            string[] words = s.Split();
            int from = int.Parse(words[0]), to = int.Parse(words[1]);
            double weight = double.Parse(words[2]);
            edge[from].Add((to, weight));
            edge[to].Add((from, weight));
        }
    }
    Dictionary<int, string> dict= new Dictionary<int, string>();
    void dijkstra() {
        double[] dist = new double[num + 1];
        
        var q = new BinaryHeap<int>();
        
        q.add(1, 0.0);
        for (int i = 2 ; i <= num ; ++i) {
            dist[i] = double.PositiveInfinity;
            q.add(i, dist[i]);
        }
        q.decrease(1, 0.0);
        //dist[1]=0;
        while (q.count > 0) {
            (int v, double h) = q.extractMin();
            foreach ((int w, double d) in edge[v])
                if (d < dist[w] && q.pos.ContainsKey(w)) {
                    dist[w] = d;
                    q.decrease(w, dist[w]);
                    if (dict.ContainsKey(w)){
                        dict[w]= v+ " " +w;
                    }else{
                        dict.Add(w, v+" "+w);
                    }
                }
        }
        foreach(KeyValuePair<int, string> p in dict){
            WriteLine(p.Value);
        }
        double sum = 0.0;
        for (int i = 1 ; i <= num ; ++i)
            if (dist[i] < double.PositiveInfinity) {

                sum+= dist[i];
            }
            WriteLine($"{sum:f1}");
    }
    
    static void Main() {
        new Graph().dijkstra();
    }
}

    }
}
