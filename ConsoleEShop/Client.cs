using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Views;

namespace ConsoleEShop
{
    public class ClientRequestArgs : EventArgs
    {
        public string Command { get; set; }
    }


    public class Client:IClient
    {
        
        public event EventHandler<ClientRequestArgs> RequestRecieved;

        public void StartListen()
        {
            while (true)
            {
                var command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                    continue;

                RequestRecieved?.Invoke(this, new ClientRequestArgs(){Command = command.Trim()});
            }
        }
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

        public string Read()
        {
            return Console.ReadLine();


        }

        public void Write(string s)
        {

            Console.WriteLine(s);
        }

        public void Response(IView responce)
        {
           Console.Clear();
            Console.WriteLine(responce.ShowViewData());
        }

        public void Response(string response)
        {
            Console.Clear();
            Console.WriteLine(response);
        }
        public int AskForNumber(string message, int upperBound = 0)
        {
            while (true)
            {
                Console.WriteLine(message);
                var response = ReadOrAbort();
                if (string.IsNullOrWhiteSpace(response))
                    return -1;

                var parse = int.TryParse(response, out var result);



                if (parse && result >= 1)
                {
                    if (upperBound > 0)
                    {
                        if (result <= upperBound)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        return result;
                    }

                    Console.WriteLine($"Value can't be greater than {upperBound}");
                }

                message = "please enter correct number or pres Escape to abort operation";
            }
        }
        public string AskForString(string message, string paramName, int minLength = 0)
        {
            while (true)
            {
                Console.WriteLine(message);
                var result = ReadOrAbort();
                if (string.IsNullOrWhiteSpace(result)) return null;

                if (minLength > 0 && result.Length < minLength)
                {
                    Console.WriteLine($"{paramName} must contain at least {minLength} symbols");
                    message += "\nOr press Esc to abort operation";
                }
                else
                    return result;
            }
        }
    }
}
