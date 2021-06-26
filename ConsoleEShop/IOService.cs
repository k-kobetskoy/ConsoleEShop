using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleEShop
{
    public class IOService : IIOService
    {
        public List<string> TestInput { get; set; }
        public List<string> TestOutput { get; set; }

        public IOService()
        {
            TestOutput = new List<string>();
            TestInput = new List<string>();
        }
        public string Read()
        {
            var result = Console.ReadLine();
            TestInput.Add(result);
            return result;
        }

        public void Write(string s)
        {
            TestOutput.Add(s);
            Console.WriteLine(s);
        }

        public void WriteInLine(string s) => Console.Write(s);

        public string ReadOrAbort()
        {
            var command = new StringBuilder("");
            var key = Console.ReadKey(true);

            while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape)
            {
                if (key.Key == ConsoleKey.Backspace && Console.CursorLeft > 0)
                {
                    var cli = --Console.CursorLeft;
                    command.Remove(cli, 1);
                    Console.CursorLeft = 0;
                    Console.Write(new String(Enumerable.Range(0, command.Length + 1).Select(o => ' ').ToArray()));
                    Console.CursorLeft = 0;
                    Console.Write(command.ToString());
                    Console.CursorLeft = cli;
                    key = Console.ReadKey(true);
                }
                else if (char.IsLetterOrDigit(key.KeyChar) || char.IsWhiteSpace(key.KeyChar))
                {
                    var cli = Console.CursorLeft;
                    command.Insert(cli, key.KeyChar);
                    Console.CursorLeft = 0;
                    Console.Write(command.ToString());
                    Console.CursorLeft = cli + 1;
                    key = Console.ReadKey(true);
                }
                else if (key.Key == ConsoleKey.LeftArrow && Console.CursorLeft > 0)
                {
                    Console.CursorLeft--;
                    key = Console.ReadKey(true);
                }
                else if (key.Key == ConsoleKey.RightArrow && Console.CursorLeft < command.Length)
                {
                    Console.CursorLeft++;
                    key = Console.ReadKey(true);
                }
                else
                {
                    key = Console.ReadKey(true);
                }
            }

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return command.ToString();
            }
            return null;
        }


        public void Clear()
        {
            Console.Clear();
        }


        public void Highlight(string s)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }



}
