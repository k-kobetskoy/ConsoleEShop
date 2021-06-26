using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleEShop.Views;

namespace ConsoleEShop
{

    public interface IClient
    {
        void StartListen();
        event EventHandler<ClientRequestArgs> RequestRecieved;
        void Response(string response);
        string ReadOrAbort();
        void Response(IView responce);
    }

    public class ClientRequestArgs : EventArgs
    {
        public string Command { get; set; }
    }


    public class Client:IClient
    {
        private readonly IIOService ioService;
        public event EventHandler<ClientRequestArgs> RequestRecieved;

        public Client(IIOService ioService)
        {
            this.ioService = ioService;
        }
        public void StartListen()
        {
            while (true)
            {
                var command = ioService.Read();

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


        public void Response(IView responce)
        {
           ioService.Clear();
            ioService.Write(responce.ShowViewData());
        }

        public void Response(string response)
        {
            ioService.Clear();
            ioService.Write(response);
        }

    }
}
