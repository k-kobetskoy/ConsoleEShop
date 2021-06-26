using System.Collections.Generic;

namespace ConsoleEShop
{
    public interface IIOService
    {
        List<string> TestOutput { get; set; }
        List<string> TestInput { get; set; }
        void Clear();
        
        void Highlight(string s);
        string Read();
        string ReadOrAbort();
        void Write(string s);
        void WriteInLine(string s);
    }
}