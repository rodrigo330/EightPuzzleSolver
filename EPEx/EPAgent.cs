using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using ProjetoGrafos.DataStructure;
using System.Drawing;
using System.Linq;

namespace EP
{
    /// <summary>
    /// EPAgent - searchs solution for the eight puzzle problem
    /// </summary>
    public class EightPuzzle : Graph
    {
        private int[] initState;
        private int[] target;
        SortedDictionary<int, List<Node>> sl = new SortedDictionary<int, List<Node>>();
        HashSet<string> test = new HashSet<string>();

        /// <summary>
        /// Creating the agent and setting the initialstate plus target
        /// </summary>
        /// <param name="InitialState"></param>
        public EightPuzzle(int[] InitialState, int[] Target)
        {
            initState = InitialState;
            target = Target;
        }

        /// <summary>
        /// Accessor for the solution
        /// </summary>
        public int[] GetSolution()
        {
            return FindSolution();
        }

        /// <summary>
        /// Função principal de busca
        /// </summary>
        /// <returns></returns>
        private int[] FindSolution()
        {
            Node node = new Node("", initState, 0);
            node.h = CalculateH(node);
            Add(node);
            test.Add(node.ToString());
            while (sl.Values.First()[0].h != 0)//sem nivel
            //while (sl.Values.First()[0].h - sl.Values.First()[0].Nivel != 0)//com nivel
            {
                node = sl.Values.First()[0];
                Remove();
                foreach (Node n in GetSucessors(node))
                {
                    if (!test.Contains(n.ToString()))
                    {
                        Add(n);
                        test.Add(n.ToString());
                    }
                }
                
            }


            
            return BuildAnswer(sl.Values.First()[0]);
        }




        private int CalculateH(Node n)
        {
            int[] lista = (int[])n.Info;
            int h = 0;
            int size = (int)Math.Sqrt(lista.Length);
            Node aux = new Node("", target, 0);
            Point atual;
            Point objetivo = new Point();
            for (int i = 0; i < lista.Length; i++)
            {
                if (lista[i] != 0)
                {
                    atual = GetPoint(n, i);
                    objetivo.X = lista[i] % size;
                    objetivo.Y = lista[i] / size;
                    h += Math.Abs(atual.X - objetivo.X);
                    h += Math.Abs(atual.Y - objetivo.Y);
                }
            }
            //h += n.Nivel;
            return h;
        }




        private void Add(Node n)
        {
            List<Node> l = new List<Node>();
            if (!sl.ContainsKey(n.h))
            {
                l.Add(n);
                sl.Add(n.h, l);
            }
            else
                sl[n.h].Add(n);
        }

        private void Remove()
        {
            List<Node> l = sl.Values.First();
            if (l.Count == 1)
                sl.Remove(l[0].h);
            else
                l.RemoveAt(0);
        }




        private List<Node> GetSucessors(Node n)
        {
            List<Node> lista = new List<Node>();
            int size = (int)Math.Sqrt(((int[])n.Info).Length);
            int indZero = FindZero(n);
            Point indPoint = GetPoint(n, indZero);


            if (indPoint.Y > 0)// && n.parent != ((int[])n.Info)[indPoint.X + size*(indPoint.Y-1)])
            {
                lista.Add(MakeMove(n,new Point(indPoint.X,indPoint.Y-1)));
            }
            if (indPoint.X < size - 1)// && n.parent != ((int[])n.Info)[indPoint.X + 1 + size * indPoint.Y])
            {
                lista.Add(MakeMove(n, new Point(indPoint.X + 1, indPoint.Y)));
            }
            if (indPoint.Y < size - 1)// && n.parent != ((int[])n.Info)[indPoint.X + size * (indPoint.Y + 1)])
            {
                lista.Add(MakeMove(n, new Point(indPoint.X, indPoint.Y + 1)));
            }
            if (indPoint.X > 0)// && n.parent != ((int[])n.Info)[indPoint.X - 1 + size * indPoint.Y])
            {
                lista.Add(MakeMove(n, new Point(indPoint.X - 1, indPoint.Y)));
            }

            return lista;
        }

        private Node MakeMove(Node n,Point p)
        {
            Node node;
            int indZero = FindZero(n);
            int[] list = (int[])((int[])n.Info).Clone();
            list[indZero] = list[p.X + p.Y * (int)Math.Sqrt(((int[])n.Info).Length)];
            list[p.X + p.Y * (int)Math.Sqrt(((int[])n.Info).Length)] = 0;
            node = new Node(n.Name + list[indZero] + ",", list, n.Nivel + 1);
            node.h = CalculateH(node);
            //node.parent = list[indZero];
            return node;
        }

        private int FindZero(Node n)
        {
            int i;
            for (i = 0; i < ((int[])n.Info).Length; i++)
            {
                if (((int[])n.Info)[i] == 0)
                {
                    break;
                }
            }
            return i;
        }


        private Point GetPoint(Node n,int indice)
        {
            int[] lista = (int[])n.Info;
            int size = (int)Math.Sqrt(lista.Length);
            return (new Point((int)indice%size,(int)indice/size));
        }
        
        private int[] BuildAnswer(Node n)
        {
            n.Name.TrimEnd(',');
            String[] aux = n.Name.Split(',');
            int[] resposta = new int[aux.Length - 1];

            for (int i = 0; i < aux.Length - 1; i++)
            {
                resposta[i] = Convert.ToInt32(aux[i]);
            }
            return resposta;
        }

        private bool TargetFound(Node n)
        {
            for (int i = 0; i < ((int[])n.Info).Length; i++)
                if (((int[])n.Info)[i] != target[i])
                    return false;
            return true;
        }
    }
}

