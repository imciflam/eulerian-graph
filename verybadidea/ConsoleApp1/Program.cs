﻿using System;
using System.Collections.Generic;
using System.Linq;

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


            //adding vertices
            /*string[] ff = System.IO.File.ReadAllLines(@"..\..\movements2.txt");
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
            int[] vertices = z.ToArray();*/


            List<int> hardcodedvertices = new List<int>() { 6, 4, 19, 14, 20, 3, 28, 10 };
            //var hardcodedvertices = new[] { 6, 4, 19, 14, 20, 3, 28, 10 };

            //adding new tuple
            /*string[] ff = System.IO.File.ReadAllLines(@"..\..\movements2.txt"); 
            for (int i = 0; i < ff.Length; i++)
            {
                string[] ss = ff[i].Split(';');
                edges.Add( new Edge(Convert.ToInt32(ss[0]), Convert.ToInt32(ss[1])));
            } */
            List<Edge> hardcodededges = new List<Edge>();
            hardcodededges.Add(new Edge(6,19));
            hardcodededges.Add(new Edge(6,6));
            hardcodededges.Add(new Edge(6,4));
            hardcodededges.Add(new Edge(19,14));
            hardcodededges.Add(new Edge(19,20));
            hardcodededges.Add(new Edge(14,28));
            hardcodededges.Add(new Edge(14, 10));
            hardcodededges.Add(new Edge(28, 3));
            hardcodededges.Add(new Edge(3, 10));
            hardcodededges.Add(new Edge(10, 20));
            hardcodededges.Add(new Edge(20, 4));
            hardcodededges.Add(new Edge(4, 6));


            Ways(hardcodededges, 6, new List<int>(), listWays);

            int counter = 0;
            foreach (List<int> subList in listWays)
            {
                if (subList.Count == hardcodedvertices.Count)
                {
                    counter++; 
                }

                //foreach (int item in subList)
                //{
                //    Console.WriteLine(item);
                //}
            }

            foreach (var i in listWays)
            {
                Print(i);
                //if (listWays[i].Count = hardcodedvertices.Count)


            }

            Console.WriteLine("vsego putey "+ counter);
            Console.ReadLine();
        }
    }
}