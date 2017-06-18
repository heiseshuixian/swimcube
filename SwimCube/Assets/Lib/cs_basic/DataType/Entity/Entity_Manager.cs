using System;
using Giu.Basic.DataType;

namespace Giu.Basic.DataType { 

    public partial class Entity : Var {

        public static Map<ID, Entity> Entities = new Map<ID, Entity>(); //Id => Entity

        public delegate void DelHandler(ID entityID);
        public static event DelHandler OnDel = null;

        public delegate void RegAliasHandler(string Alias, Entity oldEntity, Entity newEntity);
        public static event RegAliasHandler OnRegAlias = null;

        public delegate void DelAliasHandler(string Alias, Entity oldEntity);
        public static event DelAliasHandler OnDelAlias = null;

        public static Entity Create(string type, ID entityID, ID root) {
            Entity ret = null; 
            if (!ID.Exist(entityID)) {
                ret = new Entity(type, ID.Empty, root);
            } 
            else if (Entities.ContainsKey(entityID))
                throw new EntityExpection("Entity Create Failed, The Uuid Is Already Exist. ");
            else {
                ret = new Entity(type, entityID, root);
                Entities.Add(entityID, ret);
            }
            return ret;
        }

        public static bool Has(ID entityID) { return Entities.ContainsKey(entityID); }

        public static Entity Get(ID entityID) { return Entities.GetIfExist(entityID, null); }
         
        public static bool Del(ID entityID) {
            try {
                if (Has(entityID)) { 
                    DelAliasesOfEntity(Get(entityID));
                    bool ret = Entities.Remove(entityID);
                    if (ret && OnDel != null) OnDel(entityID);
                    return ret;
                }
            } catch (Exception ex) { throw new EntityExpection("EntityExpection because ", ex); }
            return false;
        }

        private static OTMMap<Entity, string> entityAlias = new OTMMap<Entity, string>(true); // EntityID => EntityAlias

        public static OTMMap<Entity, string> Alias { get { return entityAlias; } }

        public static Entity GetByAlias(string aliasID) {
            Entity ret = null;
            entityAlias.TryGetKey(aliasID, out ret);
            return ret;
        }

        public static void RegAlias(Entity entity, string alias) {
            if (alias.Exist()) {
                Entity oldEntity = null;
                entityAlias.TryGetKey(alias, out oldEntity);
                entityAlias[entity] = alias;
                if (OnRegAlias != null) OnRegAlias(alias, oldEntity, entity);
            }
        }

        public static void DelAlias(string alias) {
            if (alias.Exist()) {
                Entity oldEntity = null;
                entityAlias.TryGetKey(alias, out oldEntity);
                if (entityAlias.RemoveByValue(alias) && OnDelAlias != null) {
                    OnDelAlias(alias, oldEntity);
                }
            }
        }

        public static void DelAliasesOfEntity(Entity entity) {
            if (entity != null && entityAlias.ContainsKey(entity)) {
                Seq<string> aliases = entityAlias.GetValueIfExist(entity, null);
                for(int i = 0; i < aliases.Count; i++) DelAlias(aliases[i]); 
                entityAlias.RemoveByKey(entity);
            }
        }

        public static int AliedEntityCount { get { return entityAlias.KeyCount; } }
        public static int AliesCount { get { return entityAlias.ValueCount; } }

    }
}
