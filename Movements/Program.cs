using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Movements
{ 
    class Program
    { 
        class Graph<T>
        {
            public Dictionary<int, List<Movement>> AdjacencyList { get; } = new Dictionary<int, List<Movement>>();

            public Graph(IEnumerable<int> vertices, IEnumerable<Movement> ms)
            {
                foreach (var vertex in vertices)
                {
                    AddVertex(vertex);
                }

                foreach (var edge in ms)
                {
                    AddEdge(edge);
                }
            }

            private void AddVertex(int vertex)
            {
                AdjacencyList[vertex] = new List<Movement>();
            }

            private void AddEdge(Movement ms)
            {
                if (AdjacencyList.ContainsKey((ms.From)) && AdjacencyList.ContainsKey((ms.To)))
                {
                    AdjacencyList[(ms.From)].Add((ms)); 
                }
            }

        } 
        class Movement
        { 
            public int From; 
            public int To; 
            public int index;
             
        } 
        static void Main(string[] args)
        {
            try
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
                int[] vertices = z.ToArray(); 
                 
                var ms = new Movement[ff.Length]; 
                for (int i = 0; i < ff.Length; i++)
                {
                    string[] ss = ff[i].Split(';');
                    ms[i] = new Movement() { From = Int32.Parse(ss[0]), To = Int32.Parse(ss[1]), index = i };
                }
                int begin = ms[0].From;

                var graph = new Graph<int>(vertices, ms);
                  
                Stopwatch start = Stopwatch.StartNew();
                List<int[]> result = FindMovements(begin, ms, graph);
                start.Stop();
                 
                Console.WriteLine(start.ElapsedMilliseconds);
                PrintResults(ms, begin, result);
                Console.WriteLine("\nГотово!");
                Console.ReadKey();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        } 

        static void PrintResults(Movement[] ms, int begin, List<int[]> result)
        {
            Console.WriteLine(result.Count); 
            if (result == null || result.Count == 0)
            {
                Console.WriteLine("Переходов не найдено.");
            }
            else
            {
                Console.WriteLine("Найденные переходы:");

                foreach (int[] list in result)
                {
                    Movement prev = null;
                    for (int i = 0; i < ms.Length; i++)
                    {
                        if (i >= list.Length)
                        {
                            Console.Write("Ошибка: отсутствует переход.");
                        }
                        else
                        {
                            int idx = list[i];
                            if (idx < 0 || idx >= ms.Length)
                            {
                                Console.Write("Ошибка: индекс за пределами диапазона.");
                            }
                            else
                            {
                                Movement movement = ms[idx];

                                int b = prev == null ? begin : prev.To;

                                if (movement.From != b)
                                {
                                    Console.Write(" ОШИБКА!");
                                }
                                prev = movement;
                            }
                        }
                    }
                    if (list.Length > ms.Length)
                    {
                        Console.WriteLine("Ошибка: имеются лишние переходы.");
                    } 
                }
            }
        } 

        static List<int[]> FindMovements(int firstDivision, Movement[] ms, Graph<int> graph)
        {
            int from = firstDivision;
            List<int[]> paths = new List<int[]>();
            List<int> currentPath = new List<int>();
            InspectPath(from, currentPath, ms, ref paths, graph);
            return paths;
        }
        static void InspectPath(int from, List<int> currentPath, Movement[] ms, ref List<int[]> paths, Graph<int> graph)
        { 
            if (currentPath.Count == ms.Length)
            {
                paths.Add(currentPath.ToArray());
            } 
            else
            {
                List<Movement> ad = graph.AdjacencyList[from];
                foreach (Movement next in ad.ToList())
                {
                    if (currentPath.Contains(next.index))
                    {
                        continue;
                    }
                    currentPath.Add(next.index);
                    InspectPath(next.To, currentPath, ms, ref paths, graph);
                    currentPath.Remove(next.index);
                }
                return;
            }
        }
    }
}
