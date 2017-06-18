using Giu.Basic.Helper;
using System;
using System.Collections.Generic;

namespace Giu.Basic.DataType {

    [Serializable]
    internal class EntityDefExpection : Exception {
        public EntityDefExpection() { }
        public EntityDefExpection(string message) : base(message) { }
        public EntityDefExpection(string message, Exception innerException) : base(message, innerException) { }
    }

    public class Def {

        public enum IDType {
            None,
            Index = 1,
            Uuid = 2
        }

        public class Entry {
            public enum Species { Attr = 0, List = 1 }
            /// <summary>
            /// Entry的种类, Attr, List
            /// </summary>
            public Species species { get; set; }

            /// <summary>
            /// Entry在该Entity定义中的index序号
            /// </summary>
            public int index { get; set; }

            /// <summary>
            /// Entry的名字
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// Entry值的类型, 默认类型有
            /// Bool, Int, Long, UInt, ULong, Float, Double, String,ID
            /// 其他类型将被当做Entity处理
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 当Entry的种类为List时此项有效, 它标记了该列表以哪一个列作为唯一ID
            /// </summary>
            public int indexing { get; set; }

            /// <summary>
            /// Entry中的其他自定义标签
            /// </summary>
            public Map<string, string> Flags = new Map<string, string>(); 

            public Entry(Species _species, int _index, string _name, string _type, int _indexing) { species = _species; index = _index; name = _name; type = _type; indexing = _indexing; }
        }

        private static Map<string, Def> defs = new Map<string, Def>(true);

        public static Seq<string> AllDefsKey { get { return defs.KeySeq; } }

        public static void Clear() { defs.Clear(); }

        public static Def Get(string type) { return defs.GetIfExist(type, null); }

        public static Def Create(string text) { return Create(new Guk(text)); }

        public static Def Create(Guk entityData, string entityType = null, bool isSub = false) { return new Def().InitFormGuk(entityData, entityType, isSub); }
        //=====================================

        private Def() { }

        public string Type { get; set; }

        public IDType idType { get; set; }
         
        public bool isSub { get; set; }

        public List<int> Indexes { get; set; }
        private Map<int, Entry> Entries;
        private Map<string, int> EntryNameToInds;

        public Entry GetEntry(int ind) { return Entries.GetIfExist(ind, null); }

        public Entry GetEntry(string name) { return GetEntry(GetInd(name)); }

        public int GetInd(string name) { return EntryNameToInds.GetIfExist(name, -1); }

        private void Init(string entityType, int capacity = 0) {
            Type = entityType;
            Indexes = capacity == 0 ? new List<int>() : new List<int>(capacity);
            Entries = capacity == 0 ? new Map<int, Entry>() : new Map<int, Entry>(capacity);
            EntryNameToInds = capacity == 0 ? new Map<string, int>() : new Map<string, int>(capacity); ;
            defs.Add(entityType, this);
        }

        private Def InitFormGuk(Guk entityData, string entityType, bool isSub = false) {
            if (!entityType.Exist()) entityType = entityData.Type;
            if (!isSub) isSub = entityData.HasAttr("isSub");
            if (defs.ContainsKey(entityType)) throw new EntityDefExpection(string.Format("CreateDef Error: The type {0} is already exist. ", entityData.Type));

            Init(entityType, entityData.SubNodes.Count);

            //不是sub的情况下Entry第一位为id
            if(!isSub) AddEntry(Entry.Species.Attr, 1, "uuid", "ID", -1);

            for (int i = 0; i < entityData.SubNodes.Count; i++) {
                Guk entryData = entityData.SubNodes[i];

                Entry.Species species = entryData.Type.TryConvertToEnum(Entry.Species.Attr);

                int index = entryData.Name.TryConvertToInt();

                string name = entryData.GetAttr("name");
                string type = entryData.GetAttr("type");
                int indexing = entryData.GetAttr("indexing").TryConvertToInt(0);


                int limit = (species == Entry.Species.Attr ? 10 : 100);
                if (index < limit) throw new EntityDefExpection(string.Format("Entry Def {0} Parse Error: The Index {0} of entry <{1}:{2}> must be larger than {3} (1 for uuid, 2 for type id, 11+ for attr, 101+ for list). ", entityType, species, index, limit));
                if (Indexes.Contains(index)) throw new EntityDefExpection(string.Format("Entry Def {0} Parse Error: The index {1} of <{1}:{2}> is alreadyExist. ", entityType, species, index));
                if (!name.Exist()) throw new EntityDefExpection(string.Format("Entry Def {0} Parse Error: The entry <{1}:{2}> must have name. ", entityType, species, index));
                if (!type.Exist()) throw new EntityDefExpection(string.Format("Entry Def {0} Parse Error: The entry <{1}:{2}> must have type. ", entityType, species, index));
                if (species == Entry.Species.List && indexing <= 0) throw new EntityDefExpection(string.Format("Entry Def {0} Parse Error: The {1} <{1}:{2}> must be indexed('indexing'='colIndex'). ", entityType, species, index));

                Entry entry = AddEntry(species, index, name, type, indexing);
                if (entryData.SubNodes.Count > 0) Create(entryData, type, true);

                foreach (var key in entryData.Attributes.Keys) if (key != "name" && key != "type" && key != "indexing") entry.Flags.Add(key, entityData.GetAttr(key));
            }
            return this;
        }

        public Entry AddEntry(Entry.Species _species, int _index, string _name, string _type, int _indexing) {
            try {
                Entry e = new Entry(_species, _index, _name, _type, _indexing);
                if (EntryNameToInds.ContainsKey(_name)) throw new EntityExpection(StrGen.New["AddEntry Error : Name Existed"][_index][" "][_name][" "][_type].End);
                Indexes.Add(_index);
                EntryNameToInds.Add(_name, _index);
                Entries.Add(_index, e);
                return e;
            } catch (Exception ex) {
                throw new EntityExpection(StrGen.New[_index][" "][_name][" "][_type].End, ex);
            }
        }


        public Guk ToGuk() {
            Guk guk = new Guk();
            guk.SetType(Type, false);
            if (isSub) guk.SetAttr("isSub", null);
            Entries.ForEachPairs((index, entry) => {
                Guk sub = guk.CreateSubNode(StrGen.Start("<").Append(entry.species).Append(':').Append(index).Append(" />").End);
                sub.SetAttr("name", entry.name, false);
                sub.SetAttr("type", entry.type, false);
                entry.Flags.ForEachPairs((k, v) => sub.SetAttr(k, v, false));
            });
            return guk;
        }

        public Json ToJson() {
            Json json = Json.Create();
            json.Add("isSub", isSub);
            Entries.ForEachPairs((index, entry) => {
                Json entryi = json.CreateChild(index.ToString());
                entryi.Add(entry.species.ToString(), entry.name);
                entryi.Add("type", entry.type.ToString());
                entry.Flags.ForEachPairs((k, v) => entryi.Add(k, v));
            });
            return json;
        }

        public string ToProto() {
            StrGen.Builder _ = StrGen.GetBuilder();
            _ = _["message D"][Type]['{']['\n'];
            Entries.ForEachPairs((index, entry) => {
                string repeated = ""; 
                string annotate = ""; 
                if(entry.species == Entry.Species.List) {
                    repeated = "    repeated ";
                    Def def = Get(entry.type);
                    if (def == null) throw new EntityDefExpection(string.Format("The declared type {0} not exist. ", entry.type));
                    Entry sub_entry = def.GetEntry(entry.indexing);
                    if (sub_entry == null) throw new EntityDefExpection(string.Format("The declared indexing {0} of list {1}.{2} not met. ", entry.indexing, Type, entry.name));
                    annotate = entry.species == Entry.Species.List ? StrGen.New["    // This list is indexed by it's col "][entry.indexing][" ("][entry.type]['.'][sub_entry.name][")"].End : "";
                }
                string typeString = entry.type.TryConvertToEnum(VType.None).ToProtoTypeString() ?? "D" + entry.type;

                _ = _[repeated][typeString][" "][entry.name][" = "][index][';'][annotate]['\n'];
            });
            _ = _['}']['\n'];
            return _.End;
        }

        public override string ToString() {
            return ToGuk().Serialize(1);
        }
    }


}
