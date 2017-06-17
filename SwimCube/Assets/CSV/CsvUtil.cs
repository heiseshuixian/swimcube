
using UnityEngine;
class CsvUtil
{
    public static Objv GetObjvByName(string csvName)
    {
#if SERVER
        string fileData = FileUtil.ReadFile(FileUtil.ConfigPath + csvName + ".csv");
#else
        TextAsset ta = Resources.Load<TextAsset>("Config/" + csvName);
        string fileData = ta.text;
#endif      
        Objv objv = new Objv(csvName, fileData);
        return objv;
    }

}
