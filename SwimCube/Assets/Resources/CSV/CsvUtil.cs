using Giu.Basic;
using UnityEngine;
class CsvUtil
{
    public static Objv GetObjvByName(string csvName)
    {

        TextAsset ta = Resources.Load<TextAsset>("CSV/csvconfig/" + csvName);
        string fileData = ta.text;   
        Objv objv = new Objv(csvName, fileData);
        return objv;
    }

}
