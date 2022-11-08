using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HirataMainControl
{
    public class WT_Library
    {
        public static string StringArrayCombine(string[] Array, string key)
        {
            return (StringArrayCombine(Array, key, 0, Array.Length));
        }
        public static string StringArrayCombine(string[] Array, string key, int Start)
        {
            return (StringArrayCombine(Array, key, Start, Array.Length - Start));
        }
        public static string StringArrayCombine(string[] Array, string key, int Start, int count)
        {
            string ReturnString = "";
            for (int i = 0; i < (count-1); i++)
                ReturnString = ReturnString + Array[Start + i] + key;

            return (ReturnString + Array[Start + count-1]);
        }
        public static string[] SplitByCount(string Text, int Count)
        { 
            List<string> Return_Array = new List<string>();

            while (Text.Length >= 16)
            {
                Return_Array.Add(Text.Substring(0, 16));
                Text = Text.Substring(16);
            }
            if(Text!= "")
                Return_Array.Add(Text);
            return Return_Array.ToArray();
        }
    }
}
