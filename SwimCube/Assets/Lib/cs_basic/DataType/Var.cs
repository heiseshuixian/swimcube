﻿using System;

namespace Giu.Basic.DataType {

    public enum VType {
        None = 0,
        Bool = 1,
        Int = 2,
        Long = 3,
        UInt = 4,
        ULong = 5,
        Float = 6,
        Double = 7,
        String = 8,
        ID = 9
    }

    public static class VarMethod {
        public static Var DefaultValFormType(this VType type) {
            switch (type) {
                case VType.Bool: return new VBool();
                case VType.Int: return new VInt();
                case VType.Long: return new VLong();
                case VType.UInt: return new VUInt();
                case VType.ULong: return new VULong();
                case VType.Float: return new VFloat();
                case VType.Double: return new VDouble();
                case VType.String: return new VString();
                case VType.ID: return new VID();
                case VType.None:
                default: return null;
            }
        }
         
        public static Var ConverToValByType(object val, VType type) {
            switch (type) {
                case VType.Bool: return (bool)val;
                case VType.Int: return (int)val;
                case VType.Long: return (long)val;
                case VType.UInt: return (uint)val;
                case VType.ULong: return (ulong)val;
                case VType.Float: return (float)val;
                case VType.Double: return (double)val;
                case VType.String: return (string)val;
                case VType.ID: return (string)val;
                case VType.None:
                default: return null;
            }
        }

        public static string ToProtoTypeString(this VType type) {
            switch (type) {
                case VType.Bool: return "bool";
                case VType.Int: return "int32";
                case VType.Long: return "int64";
                case VType.UInt: return "uint32";
                case VType.ULong: return "uint64";
                case VType.Float: return "float";
                case VType.Double: return "double";
                case VType.String: return "string";
                case VType.ID: return "Uuid";
                case VType.None:
                default: return null;
            }
        }

    }

    public abstract class Var : IComparable<Var>, IEquatable<Var> {
         
        public VType Type { get; set; }
        public abstract object Obj { get; }

        public abstract string Format(string format);

        public virtual bool Equals(Var other) { return CompareTo(other) == 0; }

        public virtual int CompareTo(Var other) { return Type - other.Type; }
         
        public static implicit operator Var(bool val)   { return new VBool(val); }
        public static implicit operator Var(int val)    { return new VInt(val); }
        public static implicit operator Var(long val)   { return new VLong(val); }
        public static implicit operator Var(uint val)   { return new VUInt(val); }
        public static implicit operator Var(ulong val)  { return new VULong(val); }
        public static implicit operator Var(float val)  { return new VFloat(val); }
        public static implicit operator Var(double val) { return new VDouble(val); }
        public static implicit operator Var(string val) { return new VString(val); }
        public static implicit operator Var(ID val)     { return new VID(val); }
    }

    public class VBool : Var {
        public bool Val { get; set; }
        public override object Obj { get { return Val; } }
        public VBool() { Type = VType.Bool; }
        public VBool(bool val) : this() { Val = val; }
        public static implicit operator VBool(bool val) { return new VBool(val); }
        public static implicit operator bool(VBool vval) { return vval == null ? false : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VBool)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VInt : Var {
        public int Val { get; set; }
        public override object Obj { get { return Val; } }
        public VInt() { Type = VType.Int; }
        public VInt(int val) : this() { Val = val; }
        public static implicit operator VInt(int val) { return new VInt(val); }
        public static implicit operator int(VInt vval) { return vval == null ? 0 : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VInt)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VLong : Var {
        public long Val { get; set; }
        public override object Obj { get { return Val; } }
        public VLong() { Type = VType.Long; }
        public VLong(long val) : this() { Val = val; }
        public static implicit operator VLong(long val) { return new VLong(val); }
        public static implicit operator long(VLong vval) { return vval == null ? 0 : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VLong)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VUInt : Var {
        public uint Val { get; set; }
        public override object Obj { get { return Val; } }
        public VUInt() { Type = VType.UInt; }
        public VUInt(uint val) : this() { Val = val; }
        public static implicit operator VUInt(uint val) { return new VUInt(val); }
        public static implicit operator uint(VUInt vval) { return vval == null ? 0 : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VUInt)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VULong : Var {
        public ulong Val { get; set; }
        public override object Obj { get { return Val; } }
        public VULong() { Type = VType.ULong; }
        public VULong(ulong val) : this() { Val = val; }
        public static implicit operator VULong(ulong val) { return new VULong(val); }
        public static implicit operator ulong(VULong vval) { return vval == null ? 0 : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VULong)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VFloat : Var {
        public float Val { get; set; }
        public override object Obj { get { return Val; } }
        public VFloat() { Type = VType.Float; }
        public VFloat(float val) : this() { Val = val; }
        public static implicit operator VFloat(float val) { return new VFloat(val); }
        public static implicit operator float(VFloat vval) { return vval == null ? 0 : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VFloat)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VDouble : Var {
        public double Val { get; set; }
        public override object Obj { get { return Val; } }
        public VDouble() { Type = VType.Double; }
        public VDouble(double val) : this() { Val = val; }
        public static implicit operator VDouble(double val) { return new VDouble(val); }
        public static implicit operator double(VDouble vval) { return vval == null ? 0 : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VDouble)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VString : Var {
        public string Val { get; set; }
        public override object Obj { get { return Val; } }
        public VString() { Type = VType.String; }
        public VString(string val) : this() { Val = val; }
        public static implicit operator VString(string val) { return new VString(val); }
        public static implicit operator string(VString vval) { return vval == null ? null : vval.Val; }
        public override string ToString() { return Val; }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VString)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }

    public class VID : Var {
        public ID Val { get; set; }
        public override object Obj { get { return Val; } }
        public VID() { Type = VType.ID; }
        public VID(ID val) : this() { Val = val; }
        public static implicit operator VID(ID val) { return new VID(val); }
        public static implicit operator ID(VID vval) { return vval == null ? ID.Empty : vval.Val; }
        public override string ToString() { return Val.ToString(); }
        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, Val);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : Val.CompareTo(((VID)other).Val); }
        public override int GetHashCode() { return Val.GetHashCode(); }
    }


    public class VarMap<T> : Map<T, Var> {
        public VarMap() : base(true) { }


        public bool SetAttr(T key, Var val) {
            if (!ContainsKey(key)) return false;
            if (this[key].Type != val.Type) return false;
            this[key] = val;
            return true;
        }
        public bool SetAttrBool(T key, bool val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.Bool) return false;
            ((VBool)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrInt(T key, int val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.Int) return false;
            ((VInt)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrLong(T key, long val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.Long) return false;
            ((VLong)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrUint(T key, uint val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.UInt) return false;
            ((VUInt)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrULong(T key, ulong val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.ULong) return false;
            ((VULong)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrFloat(T key, float val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.Float) return false;
            ((VFloat)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrDouble(T key, double val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.Double) return false;
            ((VDouble)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrString(T key, string val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.String) return false;
            ((VString)var).Val = val;
            this[key] = var;
            return true;
        }
        public bool SetAttrId(T key, ID val) {
            if (!ContainsKey(key)) return false;
            Var var = this[key];
            if (var.Type != VType.ID) return false;
            ((VID)var).Val = val;
            this[key] = var;
            return true;
        }
    }

    public class VarSeq : GapSeq<Var> {
        public VarSeq(VType type, string typeName) : base() {
            itemType = type;
            itemTypeName = typeName;
        }
        public VType itemType { get; private set; }
        public string itemTypeName { get; private set; }

        public BiMap<Var, int> keyIndexMap = new BiMap<Var, int>();

        public Var GetAttr(Var key) {
            if (keyIndexMap.ContainsKey(key)) {
                return this[keyIndexMap[key]];
            }
            return null;
        }

        public bool SetAttr(Var key, Var val) {
            if (keyIndexMap.ContainsKey(key)) {
                throw new EntityExpection(string.Format("key {0} of the list is already exist"));
                Insert(keyIndexMap[key], val);
            } else {
                int free = FreeRow;
                keyIndexMap[key] = free;
                Insert(free, val);
            }
            return true;
        }

        public bool DelAttr(Var key) {
            int index = keyIndexMap[key];
            return RemoveByInd(index);
        }

        public override void Clear() {
            base.Clear();
            keyIndexMap.Clear();
        }

        public override bool RemoveByInd(int index) {
            base.RemoveByInd(index);
            return keyIndexMap.RemoveByValue(index);
        }

        public bool SetAttrBool(Var key, bool val) {
            if (itemType != VType.Bool) return false;
            return SetAttr(key, (VBool)val);
        }
        public bool SetAttrInt(Var key, int val) {
            if (itemType != VType.Int) return false;
            return SetAttr(key, (VInt)val);
        }
        public bool SetAttrLong(Var key, long val) {
            if (itemType != VType.Long) return false;
            return SetAttr(key, (VLong)val);
        }
        public bool SetAttrUInt(Var key, uint val) {
            if (itemType != VType.UInt) return false;
            return SetAttr(key, (VUInt)val);
        }
        public bool SetAttrULong(Var key, ulong val) {
            if (itemType != VType.ULong) return false;
            return SetAttr(key, (VULong)val);
        }
        public bool SetAttrFloat(Var key, float val) {
            if (itemType != VType.Float) return false;
            return SetAttr(key, (VFloat)val);
        }
        public bool SetAttrDouble(Var key, double val) {
            if (itemType != VType.Double) return false;
            return SetAttr(key, (VDouble)val);
        }
        public bool SetAttrString(Var key, string val) {
            if (itemType != VType.String) return false;
            return SetAttr(key, (VString)val);
        }
        public bool SetAttrID(Var key, ID val) {
            if (itemType != VType.ID) return false;
            return SetAttr(key, (VID)val);
        }

    }

}
