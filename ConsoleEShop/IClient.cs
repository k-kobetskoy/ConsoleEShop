using System;
using ConsoleEShop.Views;

namespace ConsoleEShop
{
    public interface IClient
    {
        string Read();
        void Write(string s);
        void StartListen();
        event EventHandler<ClientRequestArgs> RequestRecieved;
        void Response(string response);
        string ReadOrAbort();
        void Response(IView responce);
        int AskForNumber(string message, int upperBound = 0);
        string AskForString(string message, string paramName, int minLength = 0);
    }
}