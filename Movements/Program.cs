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
            public Dictionary<int, HashSet<Movement>> AdjacencyList { get; } = new Dictionary<int, HashSet<Movement>>();

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

            public void AddVertex(int vertex)
            {
                AdjacencyList[vertex] = new HashSet<Movement>();
            }

            public void AddEdge(Movement ms)
            {
                if (AdjacencyList.ContainsKey((ms.From)) && AdjacencyList.ContainsKey((ms.To)))
                {
                    AdjacencyList[(ms.From)].Add((ms));//откуда и куда
                }
            }

        }
        /// <summary>
        /// Движение между отделениями
        /// </summary>
        class Movement
        {
            /// <summary>
            /// Отделение откуда
            /// </summary>
            public int From;
            /// <summary>
            /// Отделение куда
            /// </summary>
            public int To;
            /// <summary>
            /// Индекс перехода
            /// </summary>
            public int index;

            // Можно добавлять в класс вспомогательные члены
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

                var edges = new Tuple<int, int>[ff.Length];
                for (int i = 0; i < ff.Length; i++)
                {
                    string[] ss = ff[i].Split(';');
                    edges[i] = Tuple.Create(Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1]));
                }



                var ms = new Movement[ff.Length];//Movement[] 
                for (int i = 0; i < ff.Length; i++)
                {
                    string[] ss = ff[i].Split(';');
                    ms[i] = new Movement() { From = Int32.Parse(ss[0]), To = Int32.Parse(ss[1]), index = i };
                }
                int begin = ms[0].From;



                var graph = new Graph<int>(vertices, ms);


                // поиск вариантов переходов
                Stopwatch start = Stopwatch.StartNew();
                List<int[]> result = FindMovements(begin, ms, graph);
                start.Stop();

                // печать результатов
                PrintResults(ms, begin, result);

                Console.WriteLine("\nГотово!");
                Console.ReadKey();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        /// <summary>
        /// Печать результатов
        /// </summary>
        /// <param name="ms">Движения между отделениями</param>
        /// <param name="begin">Первое отделение (откуда старт)</param>
        /// <param name="result">Результат выполнения функции FindMovements()</param>
        static void PrintResults(Movement[] ms, int begin, List<int[]> result)
        {
            Console.WriteLine(result.Count);
            //return;


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

                    //Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Поиск всех вариантов движения, начиная с указанного отделения.
        /// </summary>
        /// <param name="firstDivision">Первое отделение (откуда старт)</param>
        /// <param name="ms">Движения между отделениями</param>
        /// <returns>Результат в виде списка индексов переходов между отделениями в исходном массиве</returns>
        static List<int[]> FindMovements(int firstDivision, Movement[] ms, Graph<int> graph)
        {
            int from = firstDivision;//начало обхода
            List<int[]> paths = new List<int[]>();//лист массивов проходов
            var currentPath = new List<int>();//саблист текущего обхода
            InspectPath(from, currentPath, ms, ref paths, graph);
            return paths;
        }
        static void InspectPath(int from, List<int> currentPath, Movement[] ms, ref List<int[]> paths, Graph<int> graph)
        {
            var ad = (graph.AdjacencyList[from]).ToList();
            ad.RemoveAll(item => currentPath.Contains(item.index));//can move contains to foreach lower, remove var and linq - will be faster, hashset->list


            if (ad.Count == 0)
            {
                if (currentPath.Count == ms.Length)
                {
                    paths.Add(currentPath.ToArray());
                }
                else
                {
                    return;
                }
            }
            else
            {
                foreach (var next in ad.ToList())
                {
                    currentPath.Add(next.index);
                    InspectPath(next.To, currentPath, ms, ref paths, graph);
                    currentPath.Remove(next.index);
                }
                return;
            }
        }
    }
}
