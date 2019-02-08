using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitacore
{
    class Program
    {
         
            public static class Globals
            {
                public static bool vis = false; // Modifiable
            }


        //копировать массивы как вариант 
            public class Graph<T>
            { 
                public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

                public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
                {
                    foreach (var vertex in vertices)
                    {
                        AddVertex(vertex);
                        Console.WriteLine(vertex);
                    }

                    foreach (var edge in edges)
                    {
                        AddEdge(edge);
                        Console.WriteLine(edge);
                    }
                }

                public void AddVertex(T vertex)
                {
                    AdjacencyList[vertex] = new HashSet<T>();
                }

                public void AddEdge(Tuple<T, T> edge)
                {
                    if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
                    {
                        AdjacencyList[edge.Item1].Add(edge.Item2);//откуда и куда
                    }
                }
            }

         
            public static void Main(string[] args)
            {
             
               string[] ff = System.IO.File.ReadAllLines(@"..\..\movements2.txt"); 
               List<int> x = new List<int>();
               List<int> y = new List<int>();
               for (int i = 0; i < ff.Length; i++)
                {
                    string[] ss = ff[i].Split(';'); 
                    x.Add(Convert.ToInt32(ss[0]));
                    y.Add(Convert.ToInt32(ss[1]));
                }
                List<int> z = x.Concat(y).Distinct().ToList();
                Console.WriteLine(z.Count);
                int[] vertices = z.ToArray();

                var edges = new Tuple<int, int>[ff.Length];
                for (int i = 0; i < ff.Length; i++)
                {
                    string[] ss = ff[i].Split(';');
                    edges[i] = Tuple.Create(Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1]));  
                 }  

                var graph = new Graph<int>(vertices, edges);

                Console.WriteLine(string.Join(", ", BFS(graph, 6)));
                var path = DFS(graph, 6);
                Console.WriteLine(string.Join(", ", path));
                //allDFS(graph, 6);
                Console.ReadKey();
            }
    //6, 19, 4, 14, 20, 15, 10, 28, 7, 23, 0, 3, 22, 25, 13, 17     
            
            public static HashSet<T> BFS<T>(Graph<T> graph, T start)
            { 
                var visited = new HashSet<T>();//уже посетили, без дублей - O(1)

                if (!graph.AdjacencyList.ContainsKey(start))
                {
                    return visited;
                }

                var queue = new Queue<T>();
                queue.Enqueue(start);

                while (queue.Count > 0)
                {
                    var vertex = queue.Dequeue();

                    if (visited.Contains(vertex))
                    {
                        continue;
                    }

                    visited.Add(vertex);

                    foreach (var neighbor in graph.AdjacencyList[vertex])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor);//всех соседей
                        }
                    }
                } 
                return visited;
            }


            public static int allDFS<T>(Graph<T> graph, T start)
            {
                int pathNumber = 0;
                DFS(graph, start);
                return pathNumber;
            }

        //6, 4, 10, 15, 23, 22, 25, 14, 28, 3, 19, 20, 7, 13, 17, 0
        //6, 19, 14, 28, 3, 25, 13, 17, 10, 0, 15, 23, 22, 4, 20, 7 reversed
            public static HashSet<T> DFS<T>(Graph<T> graph, T start)//visited передавать?
            { 
                    var visited = new HashSet<T>();//создается с каждым запуском -> глобально/изменить на свойство?

                    if (!graph.AdjacencyList.ContainsKey(start))
                    {
                        return visited;
                    }

                    var stack = new Stack<T>();
                    stack.Push(start);

                    while (stack.Count > 0)
                    {
                        var vertex = stack.Pop();
                        Console.WriteLine(vertex);  
                        visited.Add(vertex);
                        // graph.AdjacencyList[vertex].Reverse();
                         foreach (var neighbor in graph.AdjacencyList[vertex])
                        { 
                            if (!visited.Contains(neighbor))
                            { 
                               stack.Push(neighbor);  
                               Globals.vis = true;
                              // DFS(graph, neighbor);

                            }
                                
                        }

                     }
                    return visited;
            }
         }
    } 
