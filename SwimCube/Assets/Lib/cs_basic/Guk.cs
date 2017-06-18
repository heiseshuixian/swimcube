using System;
using System.Text;

namespace Giu.Basic {
    public class Guk {
        #region properties and constructor

        protected string type = "", name = "";
        protected string value = "";
        protected EventList<Guk> assistNodes = new EventList<Guk>();
        protected Map<string, string> attributes = new Map<string, string>();
        protected EventList<Guk> subNodes = new EventList<Guk>();
        protected char startQuote = '<', endQuote = '>';
        public bool IsAssist { get { return startQuote != '<'; } }


        protected Seq<Guk> toDontRemoveSub = new Seq<Guk>();
        protected Seq<Guk> toDontRemoveAssist = new Seq<Guk>();

        public Map<string, object> tempProperties;

        public Guk() {
            RegisterEvent();
        }

        public Guk(string str, AssignMode assignMode = AssignMode.Add, char _startQuote = '<', char _endQuote = '>')
            : this() {
            RegisterEvent();
            Parse(str, assignMode, _startQuote, _endQuote);
        }

        public Guk(string str, ref int i, AssignMode assignMode = AssignMode.Add, char _startQuote = '<', char _endQuote = '>') {
            RegisterEvent();
            Parse(str, ref i, assignMode, _startQuote, _endQuote);
        }

        protected void RegisterEvent() {
            attributes.OnChange += AttrChange;
            subNodes.OnChange += SubNodesChange;
            assistNodes.OnChange += AssistNodesChange;
        }

        #endregion

        #region event and handlers

        public event Action<Guk, string> OnTypeChange = null;
        public event Action<Guk, string> OnNameChange = null;
        public event Action<Guk, string> OnAttrChange = null;

        public event Action<Guk, EventList<Guk>.SeqEventType, int, object> OnAssistNodesChange = null;
        public event Action<Guk, EventList<Guk>.SeqEventType, int, object> OnSubNodesChange = null;

        public event Action<Guk, Guk> OnCreateSubNodeBegin = null;
        public event Action<Guk, Guk> OnCreateSubNodeEnd = null;
        public event Action<Guk> OnToRemoveSubAfterParse = null;
        public event Action<Guk> OnParseDone = null;

        public void AttrChange(string key) {
            if (OnAttrChange != null && !inParse)
                OnAttrChange(this, key);
        }

        public void AssistNodesChange(EventList<Guk>.SeqEventType type, int index, object obj) {
            if (OnAssistNodesChange != null && !inParse)
                OnAssistNodesChange(this, type, index, obj);
        }
        public void SubNodesChange(EventList<Guk>.SeqEventType type, int index, object obj) { if (OnSubNodesChange != null && !inParse) OnSubNodesChange(this, type, index, obj); }

        #endregion

        #region interface / PublicMethods

        public string Type {
            get { return type; }
            private set { type = value; }
        }

        public Guk SetType(string value, bool canTriggerCallBack = true) {
            if (OnTypeChange != null && type != value && !inParse && canTriggerCallBack) OnTypeChange(this, value);
            type = value;
            return this;
        }

        public string Name {
            get { return name; }
            private set { name = value; }
        }

        public Guk SetName(string value, bool canTriggerCallBack = true) {
            if (OnNameChange != null && name != value && !inParse && canTriggerCallBack) OnNameChange(this, value);
            name = value;
            return this;
        }

        private StringBuilder __publicBuilder = new StringBuilder();
        public string TypeAndNameToken {
            get {
                if (name.Exist()) {
                    __publicBuilder.Remove(0, __publicBuilder.Length);
                    __publicBuilder.Append(type).Append(':').Append(name);
                    return __publicBuilder.ToString();
                }
                else return type;
            }
            private set {
                if (!value.Exist()) return;
                string[] typeAndName = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (typeAndName.Length > 0) type = typeAndName[0];
                if (typeAndName.Length > 1) name = typeAndName[1];
            }
        }

        public Guk SetTypeAndNameToken(string value, bool canTriggerCallBack = true) {
            string[] typeAndName = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (typeAndName.Length > 0) SetType(typeAndName[0], canTriggerCallBack);
            if (typeAndName.Length > 1) SetName(typeAndName[1], canTriggerCallBack);
            return this;
        }

        public bool IsEmpty {
            get { return subNodes.IsEmpty && assistNodes.IsEmpty && attributes.Count <= 0 && !Value.Exist(); }
        }

        public string Value {
            get {
                if (HasAttr(".val")) return attributes.GetIfExist(".val", "");
                else if (value.Exist()) return value;
                return "";
            }
            private set {
                SetAttr(".val", value);
            }
        }

        public Guk SetValue(string value, bool canTriggerCallBack = true) {
            return SetAttr(".val", value, canTriggerCallBack); 
        }

        public bool HasTemp(string key) {
            if (!key.Exist()) return false;
            if (tempProperties == null) return false;
            return tempProperties.ContainsKey(key);
        }

        public object GetTemp(string key) {
            if (!key.Exist()) return null;
            if (tempProperties == null) return null;
            return tempProperties.GetIfExist(key, null);
        }

        public object SetTemp(string key, object value = null) {
            if (!key.Exist()) return null;
            if (tempProperties == null) tempProperties = new Map<string, object>();
            return tempProperties[key] = value;
        }

        public void RemoveTemp(string key) {
            if (!key.Exist()) return;
            if (tempProperties.ContainsKey(key)) tempProperties.Remove(key);
        }

        public void ClearTemp() {
            tempProperties.Clear();
        }

        public Seq<Guk> DontRemoveSubForGenerate { get { return toDontRemoveSub; } }


        public EventList<Guk> AssistNodes { get { return assistNodes; } }

        public Map<string, string> Attributes { get { return attributes; } }

        public EventList<Guk> SubNodes { get { return subNodes; } }



        #region Functions of Attributes

        public bool HasAttr(string key) { return attributes.ContainsKey(key); }

        public bool HasSub(string type) { return subNodes.Exists(g => g.Type == type); }

        public string this[string key] { get { return GetAttr(key); } private set { SetAttr(key, value, false); } }

        public string TryGetAttr(string attributeName, string defaultVal = "") { return attributes.GetIfExist(attributeName, defaultVal); }
        public string GetAttr(string attributeName) { return attributes.GetIfExist(attributeName, null); }

        public bool CheckTag(string key) {
            return HasAttr(key) && GetAttrBool(key) != false;
        }

        public Guk RemoveAttr(string attributeName, bool canTriggerCallBack = true) {
            bool p = inParse;
            inParse = !canTriggerCallBack;
            attributes.Remove(attributeName);
            inParse = p;
            return this;
        } 

        public Guk SetAttr(string attributeName, string value, bool canTriggerCallBack = true, bool force = false) { //force : 不管是否在 parse 中, 都要触发回调
            bool p = inParse;
            inParse = (inParse || !canTriggerCallBack) && !force;
            attributes[attributeName] = value;
            inParse = p;
            return this;
        }

        public Guk SetAttrTag(string attributeName, bool value, bool canTriggerCallBack = true) {
            if (value) SetAttr(attributeName, null, false);
            else RemoveAttr(attributeName, false);
            return this;
        }

        /// <summary>
        /// value存在时存入值，否则删除key
        /// </summary>
        public bool SetAttrAsyncExist(string key, string _value, bool canTriggerCallBack = true) {
             
            if (_value.Exist()) {
                SetAttr(key, _value, canTriggerCallBack); return true;
            } else { if (HasAttr(key)) attributes.Remove(key); return false; }
        }

        public string GetAttrString(string attributeName, string defaultValue = "") {
            string value = GetAttr(attributeName);
            if (value.Exist()) return value;
            return defaultValue;
        }
        public int GetAttrInt(string attributeName, int defaultValue = 0) {
            string value = GetAttr(attributeName);
            if (value.Exist()) int.TryParse(value, out defaultValue);
            return defaultValue;
        }

        public bool GetAttrBool(string attributeName, bool defaultValue = false) {
            string value = GetAttr(attributeName);
            if (value.Exist()) bool.TryParse(value, out defaultValue);
            return defaultValue;
        }

        public float GetAttrFloat(string attributeName, float defaultValue = 0) {
            string value = GetAttr(attributeName);
            if (value.Exist()) float.TryParse(value, out defaultValue);
            return defaultValue;
        }

        public Seq<string> GetAttrSeq(string attributeName, params char[] splitor) {
            string value = GetAttr(attributeName);
            return value.Exist() ? value.Split(splitor) : new Seq<string>();
        }

        public Seq<int> GetAttrIntVector(string attributeName, int defaultValue = 0) {
            return GetAttrSeq(attributeName, ',', '|', '_').Map<int>(s => {
                int i = defaultValue;
                if (s.Exist()) int.TryParse(s, out i);
                return i;
            });
        }

        public Seq<float> GetAttrVector(string attributeName, float defaultValue = 0) {
            return GetAttrSeq(attributeName, ',', '|', '_').Map<float>(s => {
                float f = defaultValue;
                if (s.Exist()) float.TryParse(s, out f);
                return f;
            });
        }

        #endregion

        #region Functions of SubNode

        public Guk GetSub(string __type, string __name) {
            return (__name != null ? //判断==null而不用Exist，因为空串也匹配，只有为null时才作为忽视条件
                                    (__type != null ? subNodes.FirstMatch(guk => guk.type == __type && guk.name == __name)
                                                    : subNodes.FirstMatch(guk => guk.name == __name)
                                    )
                                    : subNodes.FirstMatch(guk => guk.type == __type)
                    );
        }

        public Guk GetPosterity(string __type, string __name) {
            return (__name != null ? //判断==null而不用Exist，因为空串也匹配，只有为null时才作为忽视条件
                                    (__type != null ? FirstMatch(guk => guk.type == __type && guk.name == __name)
                                                    : FirstMatch(guk => guk.name == __name)
                                    )
                                    : FirstMatch(guk => guk.type == __type)
                    );
        }

        public Guk GetPosterity(string __typeAndname) {
            return FirstMatch(guk => guk.TypeAndNameToken == __typeAndname);
        }

        public Seq<Guk> GetAllPosterity(Predicate<Guk> match, int depth = 0) //depth = 0 时返回所有
        {
            Seq<Guk> PosteritynList = new Seq<Guk>();
            DoGuk((n, d) => { if ((depth == 0 || d < depth) && match(n)) PosteritynList.Add(n); });
            return PosteritynList;
        }

        public Seq<Guk> GetAllPosterityByType(string type, int depth = 0) {
            return GetAllPosterity(n => n.Type == type, depth);
        }

        public Seq<Guk> GetAllPosterityByName(string __name, int depth = 0) {
            return GetAllPosterity(n => n.name == __name, depth);
        }

        public Seq<Guk> GetAllPosterityByTypeAndName(string __type, string __name, int depth = 0) {
            return GetAllPosterity(n => n.Type == __type && n.name == __name, depth);
        }

        public Guk TryGetSubNode(string __type = "", string __name = "") {
            if (__name.Exist()) return subNodes.FirstMatch(n => n.Type == __type && n.Name == __name);
            else return subNodes.FirstMatch(n => n.TypeAndNameToken == __type);
        }

        public Guk CreateSubNode(string gukCode = null, AssignMode assignMode = AssignMode.Add) {
            Guk tempSub = new Guk();
            if (OnCreateSubNodeBegin != null) OnCreateSubNodeBegin(this, tempSub);
            subNodes.Add(tempSub);
            if (OnCreateSubNodeEnd != null) OnCreateSubNodeEnd(this, tempSub);

            if (gukCode.Exist()) {
                tempSub.Parse(gukCode);
            }
            return tempSub;
        }

        public Guk Traversal(Action<Guk, Guk> nodeFunc, bool parentFirst = true) { // 注意根节点并不会执行
            if (nodeFunc != null) {
                for (int i = 0; i < SubNodes.Count; i++) {
                    if (parentFirst) nodeFunc(this, SubNodes[i]);
                    SubNodes[i].Traversal(nodeFunc);
                    if (!parentFirst) nodeFunc(this, SubNodes[i]);
                }
            }
            return this;
        }

        public Guk DoGuk(Action<Guk, int> nodeFunc, int startDepth = 0, bool parentFirst = true) {
            if (nodeFunc != null) {
                if (parentFirst) nodeFunc(this, startDepth);
                for (int i = 0; i < SubNodes.Count; i++) SubNodes[i].DoGuk(nodeFunc, startDepth + 1);
                if (!parentFirst) nodeFunc(this, startDepth);
            }
            return this;
        }

        public Guk DoGukWhenConditionMeetForMeAndAnsistor(Predicate<Guk> condition, Action<Guk, int> nodeFunc, int startDepth = 0) {
            if (nodeFunc != null && condition != null && condition(this)) {
                for (int i = 0; i < SubNodes.Count; i++) SubNodes[i].DoGukWhenConditionMeetForMeAndAnsistor(condition, nodeFunc, startDepth + 1);
            }
            return this;
        }

        public Guk FirstMatch(Predicate<Guk> predFunc, int startDepth = 0, bool parentFirst = true) {
            if (predFunc != null) {
                if (parentFirst && predFunc(this)) return this;
                for (int i = 0; i < SubNodes.Count; i++) {
                    Guk match = SubNodes[i].FirstMatch(predFunc, startDepth + 1);
                    if (match != null) return match;
                }
                if (!parentFirst && predFunc(this)) return this;
            }
            return null;
        }



        #endregion

        #region Functions of Assist

        public Guk TryGetAssistNode(string __type = "", string __name = "") {
            for (int i = 0; i < assistNodes.Count; i++) {
                if ((assistNodes[i].Type == __type && assistNodes[i].Name == __name) || (!__name.Exist() && assistNodes[i].Type == __type)) return assistNodes[i];
            }
            return null;
        }

        public Guk TryGetAssistNodeByTypeAndName(string typeandname) {
            return assistNodes.FirstMatch(n => n.TypeAndNameToken == typeandname);
        }


        public Guk CreateAssistNode(string gukCode = null, AssignMode assignMode = AssignMode.Add) {
            Guk tempAssist = new Guk("[/]", assignMode, '[', ']');
            int index = 0;

            assistNodes.Add(tempAssist);

            if (gukCode.Exist()) {
                tempAssist.Parse(gukCode, ref index, assignMode, '[', ']');
            }
            //CreateUIForSubNode(this);
            //if (OnCreateSubNodeDone != null) OnCreateSubNodeDone(this, tempSub);
            return tempAssist;
        }

        #endregion

        #region Functions of Clone And Translate

        public void Clear() {
            subNodes.Clear();
            attributes.Clear();
        }

        public Guk ClearEvents() { 
            OnTypeChange = null;
            OnNameChange = null;
            OnAttrChange = null;
            OnAssistNodesChange = null;
            OnSubNodesChange = null;
            OnCreateSubNodeBegin = null;
            OnCreateSubNodeEnd = null;
            OnToRemoveSubAfterParse = null; 
            return this; 
        }

        public Guk Clone { get { return new Guk().PasteAll(this); } }
        #region Paste
        
        public Guk PasteBasic(Guk origin) {
            type = origin.type; name = origin.name; value = origin.value;
            startQuote = origin.startQuote; endQuote = origin.endQuote;
            return this;
        }

        //TODO : Paste很容易混淆，要好好想想
        //public Guk PasteAttributes(Guk origin) {
        //    attributes = origin.attributes.Clone; return this; }
        //public Guk PasteTemps(Guk origin) { if(origin.tempProperties != null) { tempProperties = origin.tempProperties.Clone; } return this; }
        public Guk PasteSubNodes(Guk origin) {
            for (int i = 0; i < origin.SubNodes.Count; i++) {
                Guk orgsub = origin.SubNodes[i];
                Guk matchsub = null;
                for (int subind = 0; subind < SubNodes.Count; subind++) {
                    if (subNodes[subind].Type == orgsub.Type && subNodes[subind].Name == orgsub.Name) {
                        matchsub = subNodes[subind];
                        matchsub.PasteAll(orgsub);
                        break;
                    }
                }
                if (matchsub == null) {
                    matchsub = orgsub.Clone;
                    subNodes.Append(matchsub);
                }
                
            };
            return this;
        }
        public Guk PasteAssistNodes(Guk origin) {
            for (int i = 0; i < origin.AssistNodes.Count; i++) {
                Guk orgassist = origin.AssistNodes[i];
                Guk matchassist = null;
                for (int assistind = 0; assistind < AssistNodes.Count; assistind++) {
                    if (AssistNodes[assistind].Type == orgassist.Type && AssistNodes[assistind].Name == orgassist.Name) {
                        matchassist = AssistNodes[assistind];
                        matchassist.PasteAll(orgassist);
                        break;
                    }
                }
                if (matchassist == null) {
                    matchassist = orgassist.Clone;
                    AssistNodes.Append(matchassist);
                } 
            };
            return this;  
        }

        public Guk PasteEvents(Guk origin) {
            if (origin.OnTypeChange != null) OnTypeChange = origin.OnTypeChange.Clone() as Action<Guk, string>;
            if (origin.OnNameChange != null) OnNameChange = origin.OnNameChange.Clone() as Action<Guk, string>;
            if (origin.OnAttrChange != null) OnAttrChange = origin.OnAttrChange.Clone() as Action<Guk, string>;

            if (origin.OnAssistNodesChange != null) OnAssistNodesChange = origin.OnAssistNodesChange.Clone() as Action<Guk, EventList<Guk>.SeqEventType, int, object>;
            if (origin.OnSubNodesChange != null) OnSubNodesChange = origin.OnSubNodesChange.Clone() as Action<Guk, EventList<Guk>.SeqEventType, int, object>;

            if (origin.OnCreateSubNodeBegin != null) OnCreateSubNodeBegin = origin.OnCreateSubNodeBegin.Clone() as Action<Guk, Guk>;
            if (origin.OnCreateSubNodeEnd != null) OnCreateSubNodeEnd = origin.OnCreateSubNodeEnd.Clone() as Action<Guk, Guk>;

            if (origin.OnToRemoveSubAfterParse != null) OnToRemoveSubAfterParse = origin.OnToRemoveSubAfterParse.Clone() as Action<Guk>;
            return this;
        }

        public Guk PasteInfoFromGuk(Guk from) {
            PasteBasic(from).PasteSubNodes(from).SyncTemps(from).PasteEvents(from).SyncAttributes(from).PasteAssistNodes(from);

            return this;
        }

        public Guk PasteAll(Guk from) {
            PasteBasic(from).SyncAttributes(from).PasteSubNodes(from).PasteAssistNodes(from).PasteEvents(from).SyncTemps(from);
            return this;
        }



        #endregion

        #region sync 

        

        public Guk SyncAttributes(Guk origin) {
            //attributes = new Map<string, string>(origin.attributes); //todo
            origin.attributes.ForEachPairs((k, v) => {
                attributes[k] = v;
            });
            return this;
        }
        public Guk SyncTemps(Guk origin) {
            if (origin.tempProperties != null) {
                if (tempProperties == null) tempProperties = new Map<string, object>(origin.tempProperties.Count);
                origin.tempProperties.ForEachPairs((k, v) => { tempProperties[k] = v; });
            }
            return this;
        }
        public Guk SyncSubNodes(Guk origin) { origin.subNodes.SeqDo((k, i) => subNodes.Append(k)); return this; } //注意这里是引用
        public Guk SyncAssistNodes(Guk origin) { origin.assistNodes.SeqDo((n, i) => { if (!assistNodes.Exists(a => a.Equals(n))) assistNodes.Add(n.Clone); }); return this; } //这边还要比较值
        public Guk SyncInfoFromGuk(Guk from) {
            PasteBasic(from).SyncAttributes(from).SyncSubNodes(from).SyncAssistNodes(from).SyncTemps(from);
            return this;
        }


        #endregion

        public bool BasicEqual(Guk compare) {
            return type == compare.type && name == compare.name && Value == compare.Value;//用大写Value表示如果被覆盖则不对比较结果产生影响
        }

        public bool AttributesEqual(Guk compare) {
            return attributes.Count == compare.attributes.Count && attributes.HasEvery((k, v) => compare.attributes.ContainsKey(k) && compare.attributes[k] == v);
        }

        public bool SubnodesEqual(Guk compare) {
            return subNodes.Count == compare.subNodes.Count && subNodes.HasEvery(s => compare.subNodes.Exists(n => s.InfoEqual(n))); //递归
        }

        public bool AssistsEqual(Guk compare) {
            return assistNodes.Count == compare.assistNodes.Count && assistNodes.HasEvery(s => compare.assistNodes.Exists(n => s.InfoEqual(n))); //递归
        }

        public bool InfoEqual(Guk compare) {
            return BasicEqual(compare) &&
                    AttributesEqual(compare) &&
                    SubnodesEqual(compare) &&
                    AssistsEqual(compare);
        }


        //将节点中的Assist赋值给同TN的所有Posterity，-[并移除该Assist]- (不移除了)
        public Guk SetPosterityAttributesByAssist(int depth = 0, bool canTriggerCallBack = true) {
            assistNodes.SeqDo((assist, i) => {
                if (assist.Attributes.Count > 0 || assist.Value.Exist()) {
                    Seq<Guk> nodesGot = new Seq<Guk>();
                    Traversal((p, _n) => {
                        if (_n.name == assist.name && _n.type == assist.type) nodesGot.Add(_n);

                        _n.assistNodes.SeqDo((__an, __i) => { if (__an.name == assist.name && __an.type == assist.type) nodesGot.Add(__an); });  //注意替换attr
                    });
                    //DevLog.Default.Debug("",nodesGot);
                    assist.Attributes.ForEachPairs((k, v) => nodesGot.DoSeq(nodeGot => { nodeGot.SetAttr(k, v, canTriggerCallBack); }));
                    if (assist.Value.Exist()) nodesGot.DoSeq(nodeGot => { nodeGot.SetValue(assist.Value, canTriggerCallBack); });

                }
            });
            return this;
        }

        public Guk AppendBy(Guk tempNode) {
            tempNode.attributes.ForEachPairs((k, v) => { if (!HasAttr(k)) attributes[k] = v; });
            tempNode.assistNodes.SeqDo((g, i) => {
                Guk gmatch = assistNodes.FirstMatch(n => n.Name == g.Name && n.Name == g.Name);
                if (gmatch == null) assistNodes.Add(g);
                else gmatch.attributes.ForEachPairs((k, v) => { if (!gmatch.HasAttr(k)) gmatch[k] = v; });
            });
            return this;
        }

        public Guk ApplyStyles(Func<string, Guk> styleNameToStyle) {
            return this;//Abort in this version
            //Seq<string> styleToRemove = new Seq<string>();

            //attributes.ForEachPairs((k, v) => {
            //    if (k[0] == '#') {
            //        Guk style = styleNameToStyle(k);
            //        if (style != null) {
            //            AppendBy(style);
            //            styleToRemove.Add(k);
            //        }
            //    }
            //});
            //attributes.RemoveAll((k, v) => styleToRemove.Contains(k));

            //return this;
        }

        public Guk ReplaceExistSub(Guk sub, Guk newSub) {
            if (sub == null || sub == null) return this;
            bool p = inParse;
            inParse = true;
            subNodes.Replace(sub, newSub);
            inParse = p;
            return this;
        }

        public Guk TranslateGuk(Guk Template, bool replaceExist = false) { //回调的赋值要在Translate之前，因为要调用到事件的Begin和End. 解析要在Translate之后,因为要解析的是编译后的guk

            Guk gukTempInstance = Template.Clone.PasteBasic(this);
            gukTempInstance.inParse = true;
            //Log.Default.Debug("","= TranslateGuk ===== after clone ============= \n Template : \n"gukTempInstance + "\n  --*---------------*--\n Self : \n" + this);
            gukTempInstance.SyncAttributes(this);
            //Log.Default.Debug("","= TranslateGuk ===== after sync attr ========= \n Template : \n"gukTempInstance + "\n  --*---------------*--\n Self : \n" + this);
            gukTempInstance.PasteAssistNodes(this).PasteEvents(this).SetPosterityAttributesByAssist(0, false);
            //Log.Default.Debug("","= TranslateGuk ===== after sync all stuff ==== \n Template : \n"gukTempInstance + "\n  --*---------------*--\n Self : \n" + this);
            gukTempInstance.ExcuteGenerateEvents(); //因为Template肯定不会有事件，要对Clone的子节点执行生成时生命周期 
                                                    //Log.Default.Debug("","= TranslateGuk ===== after traversal events == \n Template : \n"gukTempInstance + "\n  --*---------------*--\n Self : \n" + this);
                                                    //Travsal后事件会生成 TODO: 这边机制有些问题，很可能改到不该修改的节点（问题还是出现在ButtonText上）,需要想办法（用Text: type+ ?）
            gukTempInstance.PasteSubNodes(this).SyncTemps(this);


            gukTempInstance.inParse = false;
            //Log.Default.Debug("","= TranslateGuk ===== after paste subnodes ==== \n Template : \n"gukTempInstance + "\n  --*---------------*--\n Self : \n" + this);
            return gukTempInstance;
        }

        public Guk ExcuteGenerateEvents() { //执行生成时生命周期, 当对节点重设生命周期回调时，调用这个方法来覆盖到所有子节点
            Traversal((parent, child) => {
                if (parent.OnCreateSubNodeBegin != null) parent.OnCreateSubNodeBegin(parent, child);
                if (parent.OnCreateSubNodeEnd != null) parent.OnCreateSubNodeEnd(parent, child);
                if (child.OnParseDone != null) child.OnParseDone(child);
            });
            if (OnParseDone != null) OnParseDone(this);
            return this;
        }

        public Guk ExtractTemplate(Guk Template) //immutable
        {
            if (Template == null) return this;

            inParse = true;
            Guk __temp = Clone;

            for (int i = 0; i < Template.subNodes.Count; i++) {
                Guk subN = __temp.TryGetSubNode(Template.subNodes[i].type, Template.subNodes[i].name);
                if (subN != null) {
                    Guk translateSubN = subN.ExtractTemplate(Template.subNodes[i]);

                    if (translateSubN == subN) {
                        inParse = false;
                        return this;
                    } //子节点的Extract返回为自己说明子节点向下查询不通过

                    //迁移子节点下所有Assist
                    __temp.assistNodes.Add(translateSubN.assistNodes);
                    translateSubN.assistNodes.Clear();

                    //如果子节点有内容，则将该子节点也转变为Assist
                    if (translateSubN.attributes.Count > 0) {
                        translateSubN.startQuote = '[';
                        translateSubN.endQuote = ']';
                        __temp.assistNodes.Add(translateSubN);
                    }

                    __temp.subNodes.Remove(subN);
                }
            }

            for (int i = 0; i < __temp.assistNodes.Count; i++) {
                Guk assistN = __temp.assistNodes[i];
                Guk translateAssistN = Template.TryGetAssistNode(assistN.type, assistN.name);
                if (translateAssistN != null) {
                    assistN.attributes.RemoveAll((k, v) => {
                        return (translateAssistN.HasAttr(k) && translateAssistN.GetAttr(k) == v) || (k == ".val" && assistN.Value == translateAssistN.Value); //这里没改好容易造成文字不生成的Bug,已修复，之前是用了小写的value（这种做接口的方式是否有问题）
                    });
                }
            }

            Template.attributes.ForEachPairs((k, c) => { if (!__temp.attributes.ContainsKey(k)) __temp.attributes[k] = ""; });
            __temp.attributes.RemoveAll((k, v) => Template.HasAttr(k) && Template.attributes.GetIfExist(k, null) == v);

            __temp.assistNodes.Remove(aMe => aMe.IsEmpty);// || Template.assistNodes.HasAny(aTemp => aMe.InfoEqual(aTemp)));


            //__temp.subNodes.Remove(sub => Template.subNodes.Exists(subT => subT.name == sub.name && subT.type == sub.type));
            inParse = false;
            return __temp;
        }

        public bool HasSameStructuresWith(Guk compare) //compare可以多于我, 还需要测试
        {
            bool isEqual = true;

            this.subNodes.SeqDo((subN, i) => {
                Guk compSub = compare.TryGetSubNode(subN.type, subN.name);
                if (compSub == null) isEqual = false;
                else if (!subN.HasSameStructuresWith(compSub)) isEqual = false;
            });
            return isEqual;
        }



        #endregion


        #endregion

        #region Serialize

        public override string ToString() { return TypeAndNameToken; }

        public string Serialize(int lineOffset = 0) {
            bool usingOffset = lineOffset >= 0;

            StringBuilder stringBuilder = new StringBuilder(128);
            stringBuilder.Remove(0, stringBuilder.Length);

            if (usingOffset) stringBuilder.Append(' ', lineOffset * 4);
            stringBuilder.Append(startQuote).Append(type);
            if (name.Exist()) stringBuilder.Append(":").Append(name);

            if (attributes.Count > 0 || value.Exist()) stringBuilder.Append(' ');

            attributes.ForEachPairs((_key, _value) => {
                bool hasSpace = _value.Exist() && _value.Contains(" ");
                if (_key != ".val")
                    if (_value.Exist()) stringBuilder.Append(_key).Append(hasSpace ? "=\"" : "=").Append(SerializeEscapedValue(_value)).Append(hasSpace ? "\" " : " ");
                    else stringBuilder.Append(_key).Append(" ");
            });

            if (Value.Exist()) {
                bool hasSpace = Value.Exist() && Value.Contains(" ");
                stringBuilder.Append(".val").Append(hasSpace ? "=\"" : "=").Append(SerializeEscapedValue(Value)).Append(hasSpace ? "\" " : " ");
            }

            if (attributes.Count > 0) stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(' ');
            foreach (var node in assistNodes) stringBuilder.Append(node.Serialize(-1)).Append(' ');
            if (assistNodes.Count > 0) stringBuilder.Remove(stringBuilder.Length - 1, 1);

            if (subNodes.Count <= 0) {
                stringBuilder.Append('/').Append(endQuote);
                return stringBuilder.ToString();
            }

            stringBuilder.Append(endQuote);
            if (usingOffset) stringBuilder.Append('\n');

            foreach (var node in subNodes) stringBuilder.Append(node.Serialize((usingOffset) ? (lineOffset + 1) : lineOffset) + ((lineOffset >= 0) ? "\n" : ""));

            if (usingOffset) stringBuilder.Append(' ', lineOffset * 4);
            stringBuilder.Append(startQuote).Append('/').Append(type).Append(endQuote);
            return stringBuilder.ToString();
        }

        public string SerializeEscapedValue(string value) {
            return value.BatchReplace(
                new string[] { "\n", "\r", "\t", "\b", "[", "<", ">", "]", "\"", "\'", "\\" },
                new string[] { "\\n", "\\r", "\\t", "\\b", "\\[", "\\<", "\\>", "\\]", "\\\"", "\\\'", "\\" }
                );
        }

        #endregion

        #region Deserialize

        public enum AssignMode {
            Add,
            Or,
            Xor
        }

        protected static StringBuilder stringBuilderPublic = new StringBuilder(100);
        public bool inParse { get; protected set; }
        public Guk Parse(string str, AssignMode assignMode = AssignMode.Add, char _startQuote = '<', char _endQuote = '>') {
            int index = 0;
            Parse(str, ref index, assignMode, _startQuote, _endQuote);
            return this;
        }

        public static Seq<Guk> ParseFile(string str) {
            int index = 0;
            Seq<Guk> ret = new Seq<Guk>();
            ToNext(str, ref index);
            while (index < str.Length) {
                Guk g = new Guk();
                g.Parse(str, ref index);
                ret.Add(g);
                ToNext(str, ref index);
            }
            return ret;
        }


        public Guk Parse(string str, ref int i, AssignMode assignMode = AssignMode.Add, char _startQuote = '<', char _endQuote = '>') {
            inParse = true;
            startQuote = _startQuote;
            endQuote = _endQuote;
            ParseLabel(str, ref i, assignMode);


            if (assignMode == AssignMode.Xor) {
                if (OnToRemoveSubAfterParse != null)
                    OnToRemoveSubAfterParse(this);
                subNodes.Exclude(toDontRemoveSub, false); // attr ?
                assistNodes.Exclude(toDontRemoveAssist, false);
                toDontRemoveSub.Clear();
                toDontRemoveAssist.Clear();
            }
            inParse = false;
            if (OnParseDone != null) OnParseDone(this);
            return this;
        }

        protected static bool IsSpace(char c) { return c == ' ' || c == '\t' || c == '\n' || c == '\r'; }

        protected static bool IsQuote(char c) { return c == '"' || c == '\''; }

        protected static bool IsColon(char c) { return c == ':'; }

        protected static bool IsSharp(char c) { return c == '#'; }

        protected static bool IsAt(char c) { return c == '@'; }

        protected static void ToNext(string str, ref int i) {
            while (i < str.Length) {
                if (!IsSpace(str[i])) {
                    if (str[i] == '/' && str[i + 1] == '/')
                        ToNextLine(str, ref i);
                    else break;
                } else {
                    i++;
                }
            }
        }

        protected static void ToNextLine(string str, ref int i) { while (i < str.Length) { if (str[i] == '\n' || str[i] == '\r') break; i++; } }

        protected static void SkipStartQuote(string str, ref int i, char _startQuote) {
            ToNext(str, ref i);
            if (str[i] == _startQuote) i++; 
            ToNext(str, ref i);
        }

        protected static string GetWordUntilCharOrSpace(string str, ref int i, char c1, char c2) {
            int start = i;
            while (!IsSpace(str[i]) && c1 != str[i] && c2 != str[i]) i++;
            return str.Substring(start, i - start);
        }

        protected static string GetWordUntilChar(string str, ref int i, char c1, char c2) {
            int start = i;
            while (c1 != str[i] && c2 != str[i]) i++;
            return str.Substring(start, i - start);
        }

        protected static string GetWordUntilCharEscapes(string str, ref int i, char c1, char c2 = '\0') { //不跳空格且接受转义
                                                                                                          //int start = i;
            stringBuilderPublic.Remove(0, stringBuilderPublic.Length);
            while (c1 != str[i] && c2 != str[i]) {
                CatchCharWithEscapes(stringBuilderPublic, str, ref i); i++;
            }
            return stringBuilderPublic.ToString();
        }

        protected static string GetAttrValue(string str, ref int i, char _endQuote) { //不跳空格且接受转义
                                                                                      //int start = i;
            stringBuilderPublic.Remove(0, stringBuilderPublic.Length);
            while (!IsSpace(str[i]) && _endQuote != str[i] && '[' != str[i] && ('/' != str[i] || _endQuote != str[i + 1])) {
                CatchCharWithEscapes(stringBuilderPublic, str, ref i); i++;
            }
            return stringBuilderPublic.ToString();
        }

        public static void CatchCharWithEscapes(StringBuilder stringBuilder, string str, ref int i) {
            if ('\\' == str[i]) {
                i++;
                switch (str[i]) {
                    case 'n': stringBuilderPublic.Append('\n'); break;
                    case 'r': stringBuilderPublic.Append('\r'); break;
                    case 't': stringBuilderPublic.Append('\t'); break;
                    case 'b': stringBuilderPublic.Append('\b'); break;
                    default: stringBuilderPublic.Append(str[i]); break;
                }
            } else {
                stringBuilder.Append(str[i]);
            }
        }

        protected static string GetLabelType(string str, ref int i, char _endQuote) {
            ToNext(str, ref i);
            int start = i;
            while (!IsSpace(str[i]) && !IsColon(str[i]) && str[i] != '/' && str[i] != _endQuote) i++;
            return str.Substring(start, i - start);
        }

        protected static string GetLabelName(string str, ref int i, char _endQuote) {
            ToNext(str, ref i);
            if (!IsColon(str[i])) return "";
            else {
                i++; // pass colon
                int start = i;
                while (!IsSpace(str[i]) && str[i] != '/' && str[i] != _endQuote) i++;
                return str.Substring(start, i - start);
            }
        }

        protected void GetTypeAndNameBeforeParse(string str, int index, out string type, out string name, char startQuote = '<') {
            SkipStartQuote(str, ref index, startQuote);
            type = GetLabelType(str, ref index, endQuote);
            name = GetLabelName(str, ref index, endQuote);
        }

        protected void ParseLabel(string str, ref int i, AssignMode assignMode = AssignMode.Add) {
            SkipStartQuote(str, ref i, startQuote);

            type = GetLabelType(str, ref i, endQuote);
            name = GetLabelName(str, ref i, endQuote);
            ToNext(str, ref i);
            while (str[i] != '/' && str[i] != endQuote) ParseAttributes(str, ref i, assignMode);

            if (str[i] == '/') { // if this node has nothing inside
                i += 2; // skip />  
                return;
            }

            i++; // skip >

            // temporary. to include all whitespaces into value, if any
            int startPos = i;

            ToNext(str, ref startPos);

            if (str[startPos] == startQuote) {
                i = startPos;

                while (str[i + 1] != '/') {// parse subnodes 
                    i++; // skip <

                    Guk tempSub = null;
                    if (assignMode == AssignMode.Or || assignMode == AssignMode.Xor) {
                        string __type = null, __name = null;
                        GetTypeAndNameBeforeParse(str, i, out __type, out __name);
                        tempSub = subNodes.FirstMatch(n => n.type == __type && n.name == __name && !toDontRemoveSub.Contains(n));
                    }

                    bool isNewCreate = false;
                    if (tempSub == null) {
                        isNewCreate = true;
                        tempSub = new Guk();
                        if (OnCreateSubNodeBegin != null) OnCreateSubNodeBegin(this, tempSub);
                        if (assignMode == AssignMode.Xor) subNodes.Insert(toDontRemoveSub.Count, tempSub);
                        else subNodes.Add(tempSub);
                    }

                    if (tempSub != null && assignMode == AssignMode.Xor) {
                        toDontRemoveSub.Append(tempSub);
                    }
                    tempSub.Parse(str, ref i, assignMode);
                    if (isNewCreate && OnCreateSubNodeEnd != null) OnCreateSubNodeEnd(this, tempSub);

                    //if (OnCreateSubNodeDone != null) OnCreateSubNodeDone(this, tempSub);

                    ToNext(str, ref i);
                    if (i >= str.Length) return; // EOF 
                    if (str[i] != startQuote) throw new GukParsingException(string.Format("Unexpected token({0}): {1}",i, str));
                }
                i++; // skip <
            } else { // parse value 
                value = GetWordUntilCharEscapes(str, ref i, startQuote);
                i++; // skip < 
                if (str[i] != '/') throw new GukParsingException("Ending Error " + type + " length:" + str.Length + " i:" + i + " code:" + str);
            }

            i++; // skip /
            ToNext(str, ref i);

            string endType = GetWordUntilCharOrSpace(str, ref i, endQuote, '\0');
            if (endType != type) throw new GukParsingException("label type mismatch : " + type + " | " + endType);
            ToNext(str, ref i);
            if (str[i] != endQuote) throw new GukParsingException("Ending Error " + type + " " + i);
            i++; // skip >  
        }

        protected void ParseAttributes(string str, ref int i, AssignMode assignMode = AssignMode.Add) {
            ToNext(str, ref i);
            switch (str[i]) {
                case '[': ParseBlock(str, ref i, assignMode); break; //Block
                default: ParseAttr(str, ref i); break; //Value
            }
            ToNext(str, ref i);
        }

        private void ParseAttr(string str, ref int i) {
            ToNext(str, ref i);
            string attrName = GetWordUntilCharOrSpace(str, ref i, '=', endQuote);
            ToNext(str, ref i);
            if ('=' != str[i]) { //Attributes 为 Tag 的情况
                if (attrName.Exist()) attributes[attrName] = null;
                ToNext(str, ref i);
            } else {
                i++; // skip '='
                ToNext(str, ref i);

                string attrValue = null;
                char quote = str[i];
                if (!IsQuote(quote)) {
                    quote = ' ';
                    attrValue = GetAttrValue(str, ref i, endQuote);
                    //throw new GukParsingException("Empty can not appear after (" + attrName + ") ");
                } else {
                    i++; // skip quote
                    attrValue = GetWordUntilCharEscapes(str, ref i, quote);
                    i++; // skip quote
                }

                attributes[attrName] = attrValue;
                ToNext(str, ref i);
            }
        }

        private void ParseBlock(string str, ref int i, AssignMode assignMode = AssignMode.Add) {
            ToNext(str, ref i);
            Guk tempSub = null;
            if (assignMode == AssignMode.Or || assignMode == AssignMode.Xor) {
                string __type = null, __name = null;
                GetTypeAndNameBeforeParse(str, i, out __type, out __name, '[');
                tempSub = assistNodes.FirstMatch(n => n.type == __type && n.name == __name && !assistNodes.Contains(n));
            }

            if (tempSub == null) {
                tempSub = new Guk();
                assistNodes.Add(tempSub);
            }

            if (tempSub != null && assignMode == AssignMode.Xor) {
                toDontRemoveAssist.Append(tempSub);
            }

            tempSub.Parse(str, ref i, assignMode, '[', ']');

            ToNext(str, ref i);
        }


        #endregion Deserialize

        public class GukParsingException : Exception { public GukParsingException(string message) : base(message) { } }
    }


}
