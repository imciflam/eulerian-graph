using System;
using System.Collections.Generic;

namespace ConsoleApp5
{
    class Edge
    {
        public int u, v;

        public Edge(int u, int v)
        {
            this.u = u;
            this.v = v;
        }
    }

    class Program
    {

        static bool Find(List<int> list, int x)
        {
            if (list == null) return false;
            else
            {
                foreach (var i in list)
                {
                    if (i == x) return true;
                }
            }
            return false;
        }

        static bool Compare(List<int> L1, List<int> L2)
        {
            if (L1.Count == L2.Count)
            {
                for (var i = 0; i < L1.Count; i++)
                    if (L1[i] != L2[i]) return false;
            }
            else return false;
            return true;
        }

        static void AddWay(List<int> list, List<List<int>> listWays)
        {
            int i = 0;
            bool temp = true;
            while (i < listWays.Count && temp)
            {
                if (Compare(list, listWays[i])) temp = false;
                else i++;
            }
            if (temp || i == listWays.Count)
            {
                listWays.Add(new List<int>());
                foreach (var j in list)
                {
                    listWays[listWays.Count - 1].Add(j);
                }
            }
        }

        static void Ways(List<Edge> edges, int numberEdge, List<int> list, List<List<int>> listWays)
        {

            foreach (var edge in edges)
            {
                if (!Find(list, numberEdge))
                {
                    if (edge.u == numberEdge)
                    {
                        list.Add(numberEdge);

                        if (!Find(list, edge.v))
                        {
                            Ways(edges, edge.v, list, listWays);
                        }
                        else
                        {
                            AddWay(list, listWays);
                        }
                        list.Remove(numberEdge);
                    }
                    else if (edge.v == numberEdge)
                    {
                        list.Add(numberEdge);

                        if (!Find(list, edge.u))
                        {
                            Ways(edges, edge.u, list, listWays);
                        }
                        else
                        {
                            AddWay(list, listWays);
                        }
                        list.Remove(numberEdge);
                    }
                }
            }
        }

        static void Print(List<int> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1) Console.Write($"{list[i]}, ");
                else Console.Write(list[i]);
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            List<Edge> edges = new List<Edge>();
            List<List<int>> listWays = new List<List<int>>();  

            string[] ff = System.IO.File.ReadAllLines(@"..\..\movements2.txt"); 
            for (int i = 0; i < ff.Length; i++)
            {
                string[] ss = ff[i].Split(';');
                edges.Add( new Edge(Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1])));
            } 
            List<Edge> hardcodededges = new List<Edge>();
            hardcodededges.Add(new Edge(6,19));
            hardcodededges.Add(new Edge(19,14));
            hardcodededges.Add(new Edge(14,28));
            hardcodededges.Add(new Edge(28,3));
            hardcodededges.Add(new Edge(3,25));
            hardcodededges.Add(new Edge(25,13));
            hardcodededges.Add(new Edge(13, 17));
            hardcodededges.Add(new Edge(6, 28)); 
            hardcodededges.Add(new Edge(6, 6));




            Ways(edges, 6, new List<int>(), listWays);
            foreach (var i in listWays) Print(i);
            Console.ReadLine();
        }
    }
}