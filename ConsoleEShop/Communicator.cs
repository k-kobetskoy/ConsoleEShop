using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEShop
{
    public class Communicator
    {
        private readonly IIOService ioService;

        public Communicator(IIOService ioService)
        {
            this.ioService = ioService;
        }


        public string AskForString(string message, string paramName, int minLength = 0)
        {
            while (true)
            {
                ioService.Write(message);
                var result = ioService.ReadOrAbort();
                if (string.IsNullOrWhiteSpace(result)) return null;

                if (minLength > 0 && result.Length < minLength)
                {
                    ioService.Write($"{paramName} must contain at least {minLength} symbols");
                    message += "\nOr press Esc to abort operation";
                }
                else
                    return result;
            }
        }


        public int AskForNumber(string message, int upperBound = 0)
        {
            while (true)
            {
                ioService.Write(message);
                var response = ioService.ReadOrAbort();
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

                    ioService.Write($"Value can't be greater than {upperBound}");
                }

                message = "please enter correct number or pres Escape to abort operation";
            }
        }
    }
}
