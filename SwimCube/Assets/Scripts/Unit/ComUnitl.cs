using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class  ComUnitl
{
    public static Vector3 String2Vector3(string source) {
        Vector3 result = Vector3.zero;
        string[] sourceStr = source.Split('|');
        result.x = float.Parse(sourceStr[0]);
        result.y = float.Parse(sourceStr[1]);
        result.z = float.Parse(sourceStr[2]);
        return result;
    }


}

