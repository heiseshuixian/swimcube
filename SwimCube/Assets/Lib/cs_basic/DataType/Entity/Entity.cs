using System;
using Giu.Basic.DataType;

namespace Giu.Basic.DataType {

    [Serializable]
    internal class EntityExpection : Exception {
        public EntityExpection() { }
        public EntityExpection(string message) : base(message) { }
        public EntityExpection(string message, Exception innerException) : base(message, innerException) { }
    }

    [Serializable]
    public partial class Entity : Var {

        public Def def { get; private set; }
        public Seq<string> AllAttrNames { get; private set; }
        public Seq<string> AllListNames { get; private set; }

        public Seq<string> AllAlias { get { return entityAlias.GetValueIfExist(this, null); } }

        public ID entityId { get; private set; }
        public ID root { get; private set; }

        public override object Obj { get { return this; } }


        private Entity(string type, VID _id, ID _root) {
            def = Def.Get(type);
            if (def == null) throw new EntityExpection(string.Format("Entity Struct Failed, Type {0} is not exist.", type));
            entityId = _id;
            root = _root;
            Type = VType.None;
            ConstructEntityFromDef();

            AllAttrNames = new Seq<string>();
            AllListNames = new Seq<string>();
            for (int i = 0; i < def.Indexes.Count; i++) {
                Def.Entry entry = def.GetEntry(def.Indexes[i]);
                if (entry.species == Def.Entry.Species.Attr)
                    AllAttrNames.Add(entry.name);
                else AllListNames.Add(entry.name);
            }

        }

        public void ConstructEntityFromDef() {
            InitialAttrs();
            InitialLists();


            int indexCount = def.Indexes.Count;
            for (int i = 0; i < indexCount; i++) {
                int ind = def.Indexes[i];
                var entry = def.GetEntry(ind);
                VType type = entry.type.TryConvertToEnum(VType.None);
                switch (entry.species) {
                    case Def.Entry.Species.Attr: InitialAttr(ind, type, entry); break;
                    case Def.Entry.Species.List: InitialList(ind, type, entry); break;
                    default: break;
                }
            }
        }

        public Json ToJson(bool withDef = false) {

            Json json = Json.Create();
            json.SetValue("__type", def.Type);
            json.SetValue("__id", ID.Exist(entityId) ? entityId : "");
            json.SetValue("__root", root);
            int indexCount = def.Indexes.Count;
            if (withDef) json.Add("__def", def.ToJson());

            for (int i = 0; i < indexCount; i++) {
                int ind = def.Indexes[i];
                var entry = def.GetEntry(ind);
                VType type = entry.type.TryConvertToEnum(VType.None);
                switch (entry.species) {
                    case Def.Entry.Species.Attr:
                        Var attr = GetAttr(entry.name);
                        if (attr.Type != VType.None)
                            json.Add(ind.ToString(), attr);
                        else
                            json.Add(ind.ToString(), ((Entity)attr).ToJson(withDef));
                        break;
                    case Def.Entry.Species.List:
                        VarSeq list = _lists[ind];
                        Json listi = json.CreateChild(ind.ToString());
                        if (list.itemType != VType.None)
                            for (int j = 0; j < list.UsedKeys.Count; j++) {
                                Var key = list.keyIndexMap.GetKey(list.UsedKeys[j]);
                                listi.Add(key.Format(null), list[list.UsedKeys[j]]);
                            } else
                            for (int j = 0; j < list.UsedKeys.Count; j++) {
                                Var key = list.keyIndexMap.GetKey(list.UsedKeys[j]);
                                listi.Add(key.Format(null), ((Entity)list[list.UsedKeys[j]]).ToJson());
                            }
                        break;
                    default: break;
                }
            }

            return json;
        }

        public override string ToString() {

            return ToJson(false).Serialize();
        }

        public override string Format(string format) {
            if (!format.Exist()) return ToString();
            return string.Format(format, entityId);
        }

        public override int CompareTo(Var other) { return base.CompareTo(other) == 0 ? 0 : entityId.CompareTo(((Entity)other).entityId); }

    }
}
