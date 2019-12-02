using ProjetoGrafos.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoGrafos.DataStructure
{
    class SortedList2
    {
        List<Node> lista = new List<Node>();


        public void Add(Node n)
        {
            lista.Add(n);
            lista.Sort((a, b) => a.h - b.h);
        }
        public Node RemoveFirst()
        {
            Node aux = lista[0];
            lista.RemoveAt(0);
            return aux;
        }
        public Node Topo()
        {
            return lista[0];
        }
    }
}
