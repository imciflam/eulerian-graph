using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Vitacore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TestTree<string>();

                Console.WriteLine("\nГотово!");
                Console.ReadKey();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                Console.ReadKey();
            }
        }

        static void TestTree<T>()
        {
            Tree<T> root = FromFile<T>(@"..\..\tree1.txt");

            PrintTree(root);
            Console.WriteLine();
            List<Tree<T>> ll = new List<Tree<T>>(GetAll<T>(root)); 
            PrintTree(root);
            return;
        }



        static IEnumerable<Tree<T>> GetAll<T>(Tree<T> root)
        {
            if (root == null)
            {
                yield return null;
            }

            Queue<Tree<T>> q = new Queue<Tree<T>>();
            q.Enqueue(root);
            q.Enqueue(null);

            while (q.Count != 0)
            {
                Tree<T> curr = q.Peek();
                q.Dequeue();

                if (curr != null)
                {
                    curr.Neighbour = q.Peek();

                    if (curr.Left != null)
                    {
                        q.Enqueue(curr.Left);
                    }
                    if (curr.Right != null)
                    {
                        q.Enqueue(curr.Right);
                    }
                }
                else if (q.Count != 0)
                {
                    q.Enqueue(null);
                }
            }

            yield return root;
        }

         


        //TODO

        /*
                static IEnumerable<Tree<T>> GetAll<T>(Tree<T> root)
                {
                    if (root == null)
                    {
                        yield return null;
                    }

                   Stack<Tree<T>> s = new Stack<Tree<T>>();
                    Queue<Tree<T>> q = new Queue<Tree<T>>();
                    Tree<T> curr = root;

                    while (curr != null || s.Count > 0)
                    {
                        while (curr != null)
                        {

                            s.Push(curr);
                            curr = curr.Left;
                        } 
                         curr = s.Pop();
                        if (curr.Data as string == "10")
                        { 
                            while (curr.Parent.Neighbour  != null)
                            {
                                curr = curr.Parent; 
                                curr.Neighbour = null; 
                            }
                            break;
                        } 
                        FindRightSibling(curr);
                        curr = curr.Right;
                    }  


                    yield return root;

                }

                static void  FindRightSibling<T>(Tree<T> root)
                {

                    //ok for a full tree
                    if (root == null)
                    {
                        return;
                    } 
                    if (root.Left != null)
                    {
                        root.Left.Neighbour = root.Right;
                    }

                    if (root.Right != null)
                    {
                        root.Right.Neighbour = (root.Neighbour != null) ? root.Neighbour.Left : null;
                    }


                    FindRightSibling(root.Left);
                    FindRightSibling(root.Right);
                }*/


        #region tree

        static Tree<T> FromFile<T>(string path, char separator = ';')
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            Tree<T> root = null;

            Dictionary<string, object[]> trees = new Dictionary<string, object[]>();

            // чтение и сбор
            using (StreamReader reader = File.OpenText(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splits = line.Split(separator);
                    string node = splits[0].Trim();
                    string left = splits.Length > 1 ? splits[1].Trim() : null;
                    string right = splits.Length > 2 ? splits[2].Trim() : null;
                    string parent = "1";//define parent by parent.left || parent.right contains node
                    foreach (KeyValuePair<string, object[]> kvp in trees)
                    {
                        if (kvp.Value[0] as string == node || kvp.Value[1] as string == node) 
                        parent = kvp.Key as string;
                    }
                        
                    if (trees.ContainsKey(node))
                    {
                        throw new Exception(string.Format("Узел '{0}' описан дважды.", node));
                    }
                    //trees.Add(node, new object[] { left, right, new Tree<T> ((T)Convert(node, typeof(T)), (Tree<T>)Convert(parent, typeof(Tree<T>)))});
                    trees.Add(node, new object[] { left, right, parent, new Tree<T>((T)Convert(node, typeof(T)))});
                }
            }

            // установка left/right
            foreach (KeyValuePair<string, object[]> kvp in trees)
            {
                string key = kvp.Value[0] as string;
                object[] objs; 
                if (!string.IsNullOrEmpty(key))
                {
                    if (!trees.TryGetValue(key, out objs))
                    {
                        throw new Exception(string.Format("Для узла '{0}' не найден левый узел '{1}'.", kvp.Key, key));
                    }
                     
                    ((Tree<T>)kvp.Value[3]).Left = (Tree<T>)objs[3];
                }
                key = kvp.Value[1] as string;
                if (!string.IsNullOrEmpty(key))
                {
                    if (!trees.TryGetValue(key, out objs))
                    {
                        throw new Exception(string.Format("Для узла '{0}' не найден правый узел '{1}'.", kvp.Key, key));
                    }
                    ((Tree<T>)kvp.Value[3]).Right = (Tree<T>)objs[3];
                }  
                key = kvp.Value[2] as string;
                if (!string.IsNullOrEmpty(key))
                {
                    if (!trees.TryGetValue(key, out objs))
                    {
                        throw new Exception(string.Format("Для узла '{0}' не найден родитель '{1}'.", kvp.Key, key));
                    }
                    ((Tree<T>)kvp.Value[3]).Parent = (Tree<T>)objs[3];
                }
                 

                if (root == null)
                {
                    root = (Tree<T>)kvp.Value[3];
                }
            }


             return root;
        }

        static object Convert(string value, Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }
            else if (type == typeof(Int32))
            {
                return Int32.Parse(value);
            }
            else if (type ==typeof(Tree<string>))
            {
                return value;
            }
            throw new Exception(string.Format("Тип {0} не найден!", type));
        }

        static void PrintTree<T>(Tree<T> node)
        {
            Console.WriteLine(node.Display());
            if (node.Left != null)
            {
                PrintTree(node.Left);
            }
            if (node.Right != null)
            {
                PrintTree(node.Right);
            }
        }

        #endregion

        [System.Diagnostics.DebuggerDisplay("{Display()}")]
        class Tree<T>
        {
            public T Data { get;  set; }
            public Tree<T> Left { get; set; }
            public Tree<T> Right { get; set; }
            public Tree<T> Neighbour { get; set; }
            public Tree<T> Parent { get; set; }

            public Tree(T data)
            {
                this.Data = data;
            }


            public Tree(T data, Tree<T> parent)
            {
                this.Data = data;
                this.Parent = parent;
            }

            public string Display()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.Data);
                sb.Append(", left->");
                if (this.Left != null)
                {
                    sb.Append(this.Left.Data);
                    sb.Append(", right->");
                }
                else
                {
                    sb.Append(" , right->");
                }
                if (this.Right != null)
                {
                    sb.Append(this.Right.Data);
                    sb.Append(", neighbour->");
                }
                else
                {
                    sb.Append(" , neighbour->");
                }
                if (this.Neighbour != null)
                {
                    sb.Append(this.Neighbour.Data);
                }
                else
                {
                    sb.Append(" ");
                }
                if (this.Parent != null)
                {
                    sb.Append("  ,parent->"+this.Parent.Data);
                }
                else
                {
                    sb.Append(" ");
                }

                return sb.ToString();
            }

            public static implicit operator Tree<T>(string v)
            {
                throw new NotImplementedException();
            }
        }
    }
}
