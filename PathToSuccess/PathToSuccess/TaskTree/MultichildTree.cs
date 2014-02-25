using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.TaskTree
{
    public class MultichildTree<T>//структура данных
    {
        public class Node<T>
        {
            public T Data;
            public Node<T> Parent;
            public List<Node<T>> Children;

            public Node(Node<T> parent, T data)
            {
                Parent = parent;
                Data = data;
            }
        }

        private Node<T> Root;
        private int Count = 0;

        private Node<T> LastReturned;

        public MultichildTree()
        {
            //...//
        }

        public MultichildTree(T data)
        {
            Root = new Node<T>(null, data);
            Count++;
        }

        private Node<T> FindNode(T data)
        {
            Node<T> res = null;
            FNode(Root, data, ref res);
            return res;
        }

        private void FNode(Node<T> where, T data, ref Node<T> result)
        {
            if (where != null)
            {
                if (where.Data.Equals(data))
                {
                    result = where;
                    return;
                }
                else
                {
                    foreach(var child in where.Children)
                    {
                        FNode(child, data, ref result);
                    }
                }
            }
        }

        public void Add(T data, T parent)
        {

        }
    }
}
