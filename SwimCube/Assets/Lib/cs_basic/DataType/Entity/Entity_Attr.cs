namespace Giu.Basic.DataType {

    public partial class Entity : Var { //Var

        public VarMap<int> _attrs;
        private void InitialAttrs() {
            _attrs = new VarMap<int>();
            _attrs.OnSet -= TriggerOnAttrChange;
            _attrs.OnSet += TriggerOnAttrChange;
        }

        public void InitialAttr(int index, VType type, Def.Entry entry) {
            _attrs[index] = type.DefaultValFormType() ?? Create(entry.type, ID.Empty, root);
            if (_attrs[index].Type == VType.None) {
                Entity sub = ((Entity)_attrs[index]);
                sub._OnAttrChange_Private_Attr += delegate (ID subEntityID, string subAttrName, Var value) {
                    string attrName = entry.name + "." + subAttrName;
                    if (OnAttrChange != null) TriggerOnAttrChange(attrName, value); //sub的property事件会触发自己的sub.property事件
                };
            }
        }

        // ============================ Hooker =============================

        public delegate void AttrChangeHandler(ID entityID, string attrName, Var value);
        public static event AttrChangeHandler OnAttrChange = null;
        private event AttrChangeHandler _OnAttrChange_Private_Attr = null;
        private event AttrChangeHandler _OnAttrChange_Private_List = null;

        private void TriggerOnAttrChange(int index, Var value) {
            Def.Entry entry = def.GetEntry(index);
            if (entry == null) return;
            string attrName = entry.name;
            TriggerOnAttrChange(attrName, value);
        }
        private void TriggerOnAttrChange(string attrName, Var value) {
            if (OnAttrChange != null) OnAttrChange(entityId, attrName, value);
            if (_OnAttrChange_Private_Attr != null) _OnAttrChange_Private_Attr(entityId, attrName, value);
            if (_OnAttrChange_Private_List != null) _OnAttrChange_Private_List(entityId, attrName, value);
        }


        // ============================ Setter =============================

        public bool SetAttrByInd(int ind, Var val) { return _attrs.SetAttr(ind, val); }
        public bool SetAttr(string name, Var val) { return _attrs.SetAttr(def.GetInd(name), val); }
        public bool SetAttrByDef(string name, object val) {
            Def.Entry entry = def.GetEntry(name);
            return SetAttrByType(name, entry.type.TryConvertToEnum(VType.None), val);
        }
        public bool SetAttrByType(string name, VType type, object val) {
            Def.Entry entry = def.GetEntry(name);
            switch (type) { 
                case VType.Bool: return SetAttrBool(name, (bool)val);
                case VType.Int: return SetAttrInt(name, (int)val);
                case VType.Long: return SetAttrLong(name, (long)val);
                case VType.UInt: return SetAttrUInt(name, (uint)val);
                case VType.ULong: return SetAttrULong(name, (ulong)val);
                case VType.Float: return SetAttrFloat(name, (float)val);
                case VType.Double: return SetAttrDouble(name, (double)val);
                case VType.String: return SetAttrString(name, (string)val);
                case VType.ID: return SetAttrID(name, (ID)val);
                case VType.None: 
                default: return SetAttrEntity(name, val as Entity); 
            }
        }

        public bool SetAttrBool(string name, bool value) { return _attrs.SetAttrBool(def.GetInd(name), value); }
        public bool SetAttrInt(string name, int value) { return _attrs.SetAttrInt(def.GetInd(name), value); }
        public bool SetAttrLong(string name, long value) { return _attrs.SetAttrLong(def.GetInd(name), value); }
        public bool SetAttrUInt(string name, uint value) { return _attrs.SetAttrUint(def.GetInd(name), value); }
        public bool SetAttrULong(string name, ulong value) { return _attrs.SetAttrFloat(def.GetInd(name), value); }
        public bool SetAttrFloat(string name, float value) { return _attrs.SetAttrFloat(def.GetInd(name), value); }
        public bool SetAttrDouble(string name, double value) { return _attrs.SetAttrDouble(def.GetInd(name), value); }
        public bool SetAttrString(string name, string value) { return _attrs.SetAttrString(def.GetInd(name), value); }
        public bool SetAttrID(string name, ID value) { return _attrs.SetAttrId(def.GetInd(name), value); }
        public bool SetAttrEntity(string name, Entity value) { //列表中是ID, 也可直接塞进Entity
            int _id = def.GetInd(name);
            Def.Entry entry = def.GetEntry(_id);
            if (entry == null) return false;
            if (entry.type.TryConvertToEnum(VType.None) == VType.ID) {
                return SetAttrID(name, value.entityId);
            } else {
                _attrs.SetAttr(def.GetInd(name), value);
            }
            return true;
        }


        public bool SetAttrObj(string attrName, object value) {
            Var attr = GetAttr(attrName);
            if (attr == null) return false;
            try {
                switch (attr.Type) {
                    case VType.Bool: SetAttrBool(attrName, (bool)value); break;
                    case VType.Int: SetAttrInt(attrName, (int)value); break;
                    case VType.UInt: SetAttrUInt(attrName, (uint)value); break;
                    case VType.Long: SetAttrLong(attrName, (long)value); break;
                    case VType.ULong: SetAttrULong(attrName, (ulong)value); break;
                    case VType.Float: SetAttrFloat(attrName, (float)value); break;
                    case VType.Double: SetAttrDouble(attrName, (double)value); break;
                    case VType.String: SetAttrString(attrName, (string)value); break;
                    case VType.ID: SetAttrID(attrName, (ID)value); break;
                    case VType.None: SetAttrEntity(attrName, (Entity)value); break;
                }
            } catch {
                return false;
            }
            return true;
        }
        // ============================ Getter =============================

        public bool GetAttrBool(string name) { return (VBool)_attrs.GetIfExist(def.GetInd(name), new VBool()); }
        public int GetAttrInt(string name) { return (VInt)_attrs.GetIfExist(def.GetInd(name), new VInt()); }
        public long GetAttrLong(string name) { return (VLong)_attrs.GetIfExist(def.GetInd(name), new VLong()); }
        public uint GetAttrUInt(string name) { return (VUInt)_attrs.GetIfExist(def.GetInd(name), new VUInt()); }
        public ulong GetAttrULong(string name) { return (VULong)_attrs.GetIfExist(def.GetInd(name), new VULong()); }
        public float GetAttrFloat(string name) { return (VFloat)_attrs.GetIfExist(def.GetInd(name), new VFloat()); }
        public double GetAttrDouble(string name) { return (VDouble)_attrs.GetIfExist(def.GetInd(name), new VDouble()); }
        public string GetAttrString(string name) { return (VString)_attrs.GetIfExist(def.GetInd(name), new VString()); }
        public ID GetAttrID(string name) { return (VID)_attrs.GetIfExist(def.GetInd(name), new VID()); }
        public Entity GetAttrEntity(string name) { //列表中是ID, 也可直接取出Entity
            Def.Entry entry = def.GetEntry(def.GetInd(name));
            if (entry == null) return null;
            if (entry.type.TryConvertToEnum(VType.None) == VType.ID) {
                return Get(GetAttrID(name));
            } else {
                return (Entity)_attrs[def.GetInd(name)];
            }
        }
        public Var GetAttr(string name) { return _attrs[def.GetInd(name)]; }
        private Var GetAttrByInd(int ind) { return _attrs[ind]; }

        public string GetAttrFormat(string name, string format = null) { return _attrs[def.GetInd(name)].Format(format); }
        public object GetAttrObj(string name) { return _attrs[def.GetInd(name)].Obj; }
    }
}
