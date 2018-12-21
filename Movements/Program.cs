using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Movements
{
    class Program
    {
        /// <summary>
        /// Движение между отделениями
        /// </summary>
        class Movement
        {
            /// <summary>
            /// Отделение откуда
            /// </summary>
            public string From;
            /// <summary>
            /// Отделение куда
            /// </summary>
            public string To;
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
                //Movement[] ms = new Movement[]
                //{
                //    new Movement() { From = "334", To = "331", index = 0 },
                //    new Movement() { From = "331", To = "334", index = 1 },
                //    new Movement() { From = "332", To = "331", index = 2 },
                //    //new Movement() { From = "125", To = "112", index = 4 }
                //};
                //string begin = "332";

                string[] ff = System.IO.File.ReadAllLines(@"..\..\movements2.txt");
                Movement[] ms = new Movement[ff.Length];
                for (int i = 0; i < ff.Length; i++)
                {
                    string[] ss = ff[i].Split(';');
                    ms[i] = new Movement() { From = ss[0], To = ss[1], index = i };
                }
                string begin = ms[0].From;


                // поиск вариантов переходов
               // Stopwatch start = Stopwatch.StartNew();
                //List<int[]> result = FindMovements(begin, ms);
               // start.Stop();
              //  Console.WriteLine(start);

                // печать результатов
               // PrintResults(ms, begin, result);

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
        static void PrintResults(Movement[] ms, string begin, List<int[]> result)
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
                                //Console.Write(movement.From);
                                //Console.Write(" -> ");
                                //Console.Write(movement.To);

                                string b = prev == null ? begin : prev.To;

                                // проверка с предыдущим
                                if (movement.From != b)
                                {
                                    Console.Write(" ОШИБКА!");
                                }
                                prev = movement;
                            }
                        }
                        //Console.WriteLine();
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
       /* static List<int[]> FindMovements(string firstDivision, Movement[] ms)
        {
            // TODO
        }*/
    }
}
