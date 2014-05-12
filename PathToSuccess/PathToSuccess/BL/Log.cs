using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.BL
{
    // USAGE: to get messages: while(Log.MessagesLeft()) var message = Log.NextMessage(); ...
    public static class Log
    {
        public class ListQueue<T>
        {
            private List<T> items;

            public ListQueue()
            {
                items = new List<T>();
            }

            public T Pop()
            {
                if (items.Count > 0)
                {
                    var x = items.First();
                    items.Remove(x);
                    return x;
                }
                return default(T);
            }

            public T Peek()
            {
                return items.Count > 0 ? items.First() : default(T);
            }

            public void Push(T item)
            {
                items.Add(item);
            }

            public void Clean()
            {
                items.Clear();
            }
        }

        public static ListQueue<String> Messages { get; private set; }

        public static void Initialize()
        {
            Messages = new ListQueue<string>();
        }

        public static bool MessagesLeft()
        {
            return Messages.Peek() != null;
        }

        public static String NextMessage()
        {
            return MessagesLeft() ? Messages.Pop() : "";
        }

        public static void Add(String message)
        {
            Messages.Push(message);
        }

        public static void Clean()
        {
            Messages.Clean();
        }

        public static void SaveToFile()
        {
            var file = System.IO.File.Create("log.txt");
            while (MessagesLeft())
            {
                var s = NextMessage();
                file.Write(System.Text.Encoding.UTF8.GetBytes(s), 0, System.Text.Encoding.UTF8.GetByteCount(s));
            }
        }
    }
}
