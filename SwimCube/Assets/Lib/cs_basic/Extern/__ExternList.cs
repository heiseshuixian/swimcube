using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Giu.Basic{

    public static class __ExternList { 

        public static string ConcatToString<T>(this List<T> list, string split = ",") {
            if (list == null) return null;
            System.Text.StringBuilder stringbuilder = new System.Text.StringBuilder();
            for (int i = 0; i < list.Count; i++) { 
                    stringbuilder.Append(list[i] == null ? "null" : list[i].ToString()).Append(split); 
            }
            if (stringbuilder.Length > 0) stringbuilder.Remove(stringbuilder.Length - 1, 1);
            return stringbuilder.ToString();
        }
    }
}
