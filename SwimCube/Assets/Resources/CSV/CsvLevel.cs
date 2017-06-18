using System;
using System.Collections.Generic;
using UnityEngine;
using Giu.Basic;
public class EnemyType
{
    public int type;
    public int num;
}
public class ObstacleType
{
    public int type;
    public Vector3 pos;
}
public class CsvLevelItem
{
    public int index;//关卡序号
    public string name;//关卡名称
    public Seq<EnemyType> etype = new Seq<EnemyType>();//敌人类型
    public Seq<ObstacleType> obstacle = new Seq<ObstacleType>();//障碍物类型
    public string ornamental;//装饰物位置
    public int numtype1;//通关条件值类型1
    public int param1; // 通关条件参数1

    public int numtype2;//通关条件值类型1
    public int param2; // 通关条件参数1

    public string target;//通关条件的描述
    public string txt;//关卡描述
}

public class CsvLevel
{
    public static Map<int, CsvLevelItem> items;
    public static bool Init()
    {
        items = new Map<int, CsvLevelItem>();
        Objv objv = CsvUtil.GetObjvByName("Level");

        for (int i = 0; i < objv.rowCount; i++)
        {
            CsvLevelItem item = new CsvLevelItem();
            item.index = (int)objv.QueryByIndex(i, "Index");
            item.name = (string)objv.QueryByIndex(i, "Name");
            string type = (string)objv.QueryByIndex(i, "Type");
            item.etype.Clear();
            if (type != "")
            {
                string[] typeStr = type.Split('~');

                for (int index = 0; index < typeStr.Length; index++)
                {
                    EnemyType Etype = new EnemyType();
                    string[] etypeStr = typeStr[index].Split('+');
                    Etype.type = int.Parse(etypeStr[0]);
                    Etype.num = int.Parse(etypeStr[1]);
                    item.etype.Add(Etype);
                }
            }



            string obstacle = (string)(objv.QueryByIndex(i, "Obstacle"));
            if (obstacle != "")
            {
                string[] ObstacleStr = obstacle.Split('~');

                for (int index = 0; index < ObstacleStr.Length; index++)
                {
                    ObstacleType Etype = new ObstacleType();
                    string[] etypeStr = ObstacleStr[index].Split('+');
                    Etype.type = int.Parse(etypeStr[0]);
                    Etype.pos = ComUnitl.String2Vector3(etypeStr[1]);
                }
            }



            item.ornamental = (string)(objv.QueryByIndex(i, "Ornamental"));
            item.numtype1 = (int)(objv.QueryByIndex(i, "NumType1"));
            item.param1 = (int)(objv.QueryByIndex(i, "Param1-1"));
            item.numtype2 = (int)(objv.QueryByIndex(i, "NumType2"));
            item.param2 = (int)(objv.QueryByIndex(i, "Param2-1"));



            item.target = (string)(objv.QueryByIndex(i, "Target"));
            item.txt = (string)(objv.QueryByIndex(i, "Txt"));


            items[item.index] = item;
        }
        return true;
    }

    public static CsvLevelItem GetItem(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        return null;
    }
}
