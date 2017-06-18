using Giu.Basic.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Giu.Basic.DataType {
    [Serializable]
    public struct ID : IFormattable, IComparable, IComparable<ID>, IEquatable<ID> {
        public const string INVALID_OPEN_ID = "";
        public static readonly ID Empty = new ID(INVALID_OPEN_ID, 0);
         
        private string open_id;

        private long mark_id;

        private static string default_open_id = "";
         
        public static string Default_open_id {
            get { return default_open_id; } 
            set { default_open_id = value; }
        }

        public string OpenID { get { return open_id.Exist() ? open_id : default_open_id; } }

        public static bool Exist(ID id) { return id != null && id != Empty; }

        public ID(string _open_id, long _mark_id) {
            if (_open_id != null) open_id = _open_id;
            else open_id = INVALID_OPEN_ID;
            mark_id = _mark_id;
        }

        public int CompareTo(object obj) {
            if (obj is ID) return CompareTo((ID)obj);
            return OpenID.CompareTo(obj);
        }

        public int CompareTo(ID other) { return open_id.CompareTo(other.open_id) == 0 ? 0 : mark_id.CompareTo(other.mark_id); }

        public bool Equals(ID other) { return open_id.Equals(other.open_id); }

        public string ToString(string format, IFormatProvider formatProvider) { return string.Format(formatProvider, format, ToString()); }

        public string ToBraceString() {
            char[] chars = new char[26]; chars[0] = '{'; chars[25] = '}';
            Convert.ToBase64CharArray(ToByteArray(), 0, 16, chars, 1);
            return new string(chars);
        }

        public byte[] ToByteArray() { return Encoding.UTF8.GetBytes(OpenID); }

        public override string ToString() { return mark_id != 0 ? OpenID + mark_id : OpenID; }
         
        public override bool Equals(object obj) { return base.Equals(obj); }

        public override int GetHashCode() { return ToString().GetHashCode(); }

        public static ID New { get { return new ID(Guid.NewGuid().ToString(), 0); } }


        public static bool operator ==(ID uuid1, ID uuid2) {
            return Compare(uuid1, uuid2);
        }

        public static bool operator !=(ID sid1, ID sid2) {
            return !Compare(sid1, sid2);
        }

        private static bool Compare(ID uuid1, ID uuid2) { return uuid1.open_id == uuid2.open_id; }

        public static ID Parse(string open_id, long mark_id) { return new ID(open_id, mark_id); } 

        public ID Append(string subId) { open_id = StrGen.Start(open_id).Append('.').Append(subId).End; return this; }
         
        public static implicit operator string(ID id) { return id.ToString(); }
    }
}
