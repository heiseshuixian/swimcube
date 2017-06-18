using System;
using Giu.Basic.DataType;
using System.Collections.Generic;

namespace Giu.Basic.DataType {

    public partial class Entity : Var { // Var


        public Map<int, VarSeq> _lists;
        private void InitialLists() { _lists = new Map<int, VarSeq>(); }

        public void InitialList(int index, VType type, Def.Entry entry) {
            string name = entry.name;
            VarSeq newList = new VarSeq(type, entry.type);
            _lists[index] = newList;
            newList.ClearEvents();
            newList.OnAdd += (rowId, value) => {
                if (OnListAdd != null) OnListAdd(entityId, name, newList.keyIndexMap.GetKey(rowId));
                if (OnListRowAdd != null) OnListRowAdd(entityId, name, rowId);
                if(value is Entity) {
                    Entity item = value as Entity;
                    item._OnAttrChange_Private_List = delegate (ID item_entityID, string item_attrName, Var item_val) { // 关于 '=', 一个entity最多只能属于一个列表
                        if (OnListItemChange != null) OnListItemChange(entityId, name, rowId, item_attrName, item_val);
                        if (OnListRowItemChange != null) OnListItemChange(entityId, name, rowId, item_attrName, item_val);
                    };
                }
                //_indexes[index].Add(rowId);
            }; 
            newList.OnDel += (rowId, value) => {
                if (OnListDel != null) OnListDel(entityId, name, newList.keyIndexMap.GetKey(rowId));
                if (OnListRowDel != null) OnListRowDel(entityId, name, rowId);
                if (value is Entity) {
                    Entity item = value as Entity;
                    item._OnAttrChange_Private_List = null;
                }
            };
            newList.OnClear += () => { //其实并不支持Clear
                if (OnListClr != null) OnListClr(entityId, name);
            };
        }

        // ============================ Hooker =============================

        public delegate void ListAddHandler(ID entityID, string listName, Var rowID); 
        public delegate void ListDelHandler(ID entityID, string listName, Var rowID);
        public delegate void ListClrHandler(ID entityID, string listName);
        public delegate void ListRowAddHandler(ID entityID, string listName, int row); 
        public delegate void ListRowDelHandler(ID entityID, string listName, int row);
        public static event ListAddHandler OnListAdd = null; 
        public static event ListDelHandler OnListDel = null;
        private static event ListClrHandler OnListClr = null;
        public static event ListRowAddHandler OnListRowAdd = null; 
        public static event ListRowDelHandler OnListRowDel = null;

        public delegate void ListItemChangeHandler(ID entityID, string listName, Var rowId, string colName, Var val);
        public delegate void ListRowItemChangeHandler(ID entityID, string listName, int row, string colName, Var val);
        public static event ListItemChangeHandler OnListItemChange = null;
        public static event ListRowItemChangeHandler OnListRowItemChange = null;
        private bool TriggerOnItemListChange(int listInd, string listName, Var rowID, string colName) {
            VarSeq list = _lists[listInd];
            if (OnListItemChange != null) OnListItemChange(entityId, listName, rowID, colName, GetListItem(listName, rowID, colName));
            if (OnListRowItemChange != null) OnListRowItemChange(entityId, listName, list.keyIndexMap.GetValue(rowID), colName, GetListItem(listName, rowID, colName));
            return true;
        }


        // ============================ Setter ============================= 

        public bool SetList(string name, List<object> input) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            Def.Entry entry = def.GetEntry(index);

            Seq<int> originKeys = _lists[index].UsedKeys.Map(imd => (int)(VInt)_lists[index].keyIndexMap.GetKey(imd));
            for (int i = 0; i < originKeys.Count; i++) if (originKeys[i] > input.Count) _lists[index].DelAttr(originKeys[i]);
            for (int i = 0; i < input.Count; i++) {
                object val = input[i];
                switch (entry.type.TryConvertToEnum(VType.None)) {
                    case VType.Bool: return _lists[index].SetAttrBool(name, (bool)val);
                    case VType.Int: return _lists[index].SetAttrInt(name, (int)val);
                    case VType.Long: return _lists[index].SetAttrLong(name, (long)val);
                    case VType.UInt: return _lists[index].SetAttrUInt(name, (uint)val);
                    case VType.ULong: return _lists[index].SetAttrULong(name, (ulong)val);
                    case VType.Float: return _lists[index].SetAttrFloat(name, (float)val);
                    case VType.Double: return _lists[index].SetAttrDouble(name, (double)val);
                    case VType.String: return _lists[index].SetAttrString(name, (string)val);
                    case VType.ID: return _lists[index].SetAttrID(name, (ID)val);
                    case VType.None:
                    default: break;
                } 
            } 
            return true;
        }

        public bool SetListInt(string name, Var rowId, int value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrInt(rowId, value);
            return true;
        }
        public bool SetListLong(string name, Var rowId, long value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrLong(rowId, value);
            return true;
        }
        public bool SetListUInt(string name, Var rowId, uint value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrUInt(rowId, value);
            return true;
        }
        public bool SetListULong(string name, Var rowId, ulong value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrULong(rowId, value);
            return true;
        }
        public bool SetListFloat(string name, Var rowId, float value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrFloat(rowId, value);
            return true;
        }
        public bool SetListDouble(string name, Var rowId, double value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrDouble(rowId, value);
            return true;
        }
        public bool SetListString(string name, Var rowId, string value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrString(rowId, value);
            return true;
        }
        public bool SetListID(string name, Var rowId, ID value) {
            int index = def.GetInd(name);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].SetAttrID(rowId, value);
            return true;
        }
        public bool SetListEntity(string name, Entity value) {
            int index = def.GetInd(name);
            Def.Entry entry = def.GetEntry(index);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType == VType.ID) _lists[index].SetAttrID(value.GetAttrByInd(entry.indexing), value.entityId);
            else _lists[index].SetAttr(value.GetAttrByInd(entry.indexing), value);
            return true;
        }

        public bool SetListNewRow(string listName, Var rowID) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType == VType.None) {
                Entity entity = Create(_lists[index].itemTypeName, entityId.Append(listName).Append(rowID.Format(null)), root);
                Def.Entry entry = def.GetEntry(index);
                entity.SetAttrByInd(entry.indexing, rowID);
                _lists[index].SetAttr(rowID, entity);
            }
            return true;
        }

        public bool DelListRow(string listName, Var rowID) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            _lists[index].DelAttr(rowID);
            return true;
        }



        public bool SetListItemBool(string listName, Var rowId, string colName, bool value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrBool(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemInt(string listName, Var rowId, string colName, int value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrInt(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemLong(string listName, Var rowId, string colName, long value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrLong(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemUInt(string listName, Var rowId, string colName, uint value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrUInt(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemULong(string listName, Var rowId, string colName, ulong value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrULong(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemFloat(string listName, Var rowId, string colName, float value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrFloat(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemDouble(string listName, Var rowId, string colName, double value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrDouble(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemString(string listName, Var rowId, string colName, string value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrString(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemID(string listName, Var rowId, string colName, ID value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrID(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }
        public bool SetListItemObj(string listName, Var rowId, string colName, object value) {
            int index = def.GetInd(listName);
            if (!_lists.ContainsKey(index)) return false;
            if (_lists[index].itemType != VType.None) return false;
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            if (row == null) return false;
            row.SetAttrObj(colName, value);
            TriggerOnItemListChange(index, listName, rowId, colName);
            return true;
        }



        // ============================ Getter =============================
        public bool ListHasKey(string name, Var indexingID) {
            int index = def.GetInd(name);
            return _lists[index].GetAttr(indexingID) != null;
        }
        public bool GetListBool(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VBool)_lists[index].GetAttr(indexingID);
        }
        public int GetListInt(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VInt)_lists[index].GetAttr(indexingID);
        }
        public long GetListLong(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VLong)_lists[index].GetAttr(indexingID);
        }
        public uint GetListUInt(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VUInt)_lists[index].GetAttr(indexingID);
        }
        public ulong GetListULong(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VULong)_lists[index].GetAttr(indexingID);
        }
        public float GetListFloat(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VFloat)_lists[index].GetAttr(indexingID);
        }
        public double GetListDouble(string name, Var indexingID) { 
            int index = def.GetInd(name);
            return (VDouble)_lists[index].GetAttr(indexingID);
        }
        public string GetListString(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VString)_lists[index].GetAttr(indexingID);
        }
        public ID GetListID(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (VID)_lists[index].GetAttr(indexingID);
        }
        public Entity GetListEntity(string name, Var indexingID) {
            int index = def.GetInd(name);
            return (Entity)_lists[index].GetAttr(indexingID);
        }

        public object GetListObj(string name, Var indexingID) {
            int index = def.GetInd(name);
            return _lists[index].GetAttr(indexingID).Obj;
        }

        public Seq<Var> GetListUsedKeys(string name) {
            int index = def.GetInd(name);
            return _lists[index].UsedKeys.Map(i => _lists[index].keyIndexMap.GetKey(i));
        }

        public Seq<int> GetListUsedRows(string name) {
            int index = def.GetInd(name);
            return _lists[index].UsedKeys;
        }

        public VType GetListType(string name) {
            int index = def.GetInd(name);
            return _lists[index].itemType;
        }

        public int GetListCount(string name) {
            int index = def.GetInd(name);
            return _lists[index].Count;
        }
        public Var GetListItem(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttr(colName);
        }
        public bool GetListItemBool(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrBool(colName);
        }

        public int GetListItemInt(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrInt(colName);
        }
        public long GetListItemLong(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrLong(colName);
        }
        public uint GetListItemUInt(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrUInt(colName);
        }
        public ulong GetListItemULong(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrULong(colName);
        }
        public float GetListItemFloat(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrFloat(colName);
        }
        public double GetListItemDouble(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrDouble(colName);
        }
        public string GetListItemString(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrString(colName);
        }

        public ID GetListItemID(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttrID(colName);
        }

        public Entity GetListItemEntity(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return Get(row.GetAttrID(colName));
        }
        public object GetListItemObj(string listName, Var rowId, string colName) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttr(colName).Obj;
        }
        public object GetListItemFromat(string listName, Var rowId, string colName, string format = null) {
            int index = def.GetInd(listName);
            Entity row = (Entity)_lists[index].GetAttr(rowId);
            return row.GetAttr(colName).Format(format);
        }
    }
}
