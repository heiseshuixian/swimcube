using UnityEngine;
using System.Collections;
using Giu.Basic;
using System;

public enum E_TriggerType
{
    on_level_selected,
    on_game_start,
    on_game_over,
    on_get_gift,
}

public class TriggerManager {
    public delegate void TriggerCallback(object obj);

    public Map<E_TriggerType, Seq<TriggerCallback>> m_triggerList;
    static TriggerManager m_instance;
    private TriggerManager()
    {
        m_triggerList = new Map<E_TriggerType, Seq<TriggerCallback>>();
        foreach (int myCode in Enum.GetValues(typeof(E_TriggerType)))
        {
            //string strName = Enum.GetName(typeof(E_TriggerType), myCode);//获取名称
            //string strVaule = myCode.ToString();
            //HHUtil.LogS("enum...", strName, strVaule);
            m_triggerList[(E_TriggerType)myCode] = new Seq<TriggerCallback>();
        }
    }
    public static TriggerManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new TriggerManager();
        }
        return m_instance;
    }

    public TriggerCallback AddTrigger(E_TriggerType type, TriggerCallback callback)
    {
        m_triggerList[type].Add(callback);
        return callback;
    }

    public void RemoveTrigger(E_TriggerType type, TriggerCallback callback)
    {
        m_triggerList[type].RemoveItem(callback);
    }

    public void Trigger(E_TriggerType type, object obj = null)
    {
        foreach(TriggerCallback callback in m_triggerList[type])
        {
            callback(obj);
        }
    }
}
