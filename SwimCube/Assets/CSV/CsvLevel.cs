using System;
using System.Collections.Generic;
using UnityEngine;


public class CsvLevelItem
{
	public int ID;
	public string name;
	public string desc;
	public int type;
	public int enableArmyType;
	public int atkDistance;
	public int castTime; // 技能释放的时机类型

	public int target1;
	public int target2;

	//public int targetType; // 目标类型
	//本方1 敌方2 所有3 自身4 - ArmsTypeNum - ArmsTypeEle
	//0没有目标
	public int[] targetsParam;

	public int[] effectTypes;
	public int[] effectParams1;
}

public class CsvLevel
{
	public static Dictionary<int, CsvLevelItem> items;
	public static bool Init()
	{
		items = new Dictionary<int, CsvLevelItem>();
		Objv objv = CsvUtil.GetObjvByName("Skill");

		for (int i = 0; i < objv.rowCount; i++)
		{
			CsvSkillItem item = new CsvSkillItem();
			item.ID = (int)objv.QueryByIndex(i, "ID");
			item.name = (string)objv.QueryByIndex(i, "Name");
			item.desc = (string)objv.QueryByIndex(i, "Txt");
			item.type = (int)(objv.QueryByIndex(i, "Type"));
			item.enableArmyType = (int)(objv.QueryByIndex(i, "EnableType"));
			item.atkDistance = (int)(objv.QueryByIndex(i, "Range"));

			item.target1 = (int)(objv.QueryByIndex(i, "Target"));
			item.target2 = (int)(objv.QueryByIndex(i, "Target2"));

			item.castTime = (int)(objv.QueryByIndex(i, "CastTime"));

			item.effectTypes = new int[2];
			item.effectParams1 = new int[2];
			//item.effectParams2 = new int[2];

			for (int j = 1; j <= 2; j++) {
				item.effectTypes[j - 1] = (int)(objv.QueryByIndex(i, "NumType" + j));
				item.effectParams1[j - 1] = (int)(objv.QueryByIndex(i, "Param" + j + "-1"));
			}

			items[item.ID] = item;
		}
		return true;
	}

	public static CsvSkillItem GetItem(int id) {
		if (items.ContainsKey(id)) {
			return items[id];
		}
		return null;
	}
}
