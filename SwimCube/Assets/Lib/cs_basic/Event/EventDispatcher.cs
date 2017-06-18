using Giu.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Giu.Basic.Event {
    /*
        Type Safe EventDispatcher
        Only Enum can be used for EventID
        argType specify the arg type passed to EventHandler
        See Tests\Giu.EventDispatcherTest for usage
        */
    public class EventDispatcher<Enum, argType> {
        [Serializable]
        public class EventTypeNotMatch : Exception {
            Type expected;
            Enum given;
            public EventTypeNotMatch(Type _expected, Enum _given) : base(ConstructMessage(_expected, _given)) {
                expected = _expected;
                given = _given;
            }
            private static string ConstructMessage(Type _expected, Enum _given) {
                return string.Format("Expected [{0}], Given [{1}] of Type [{2}]", _expected.FullName, System.Enum.GetName(_given.GetType(), _given), _given.GetType().FullName);
            }
        }

        [Serializable]
        public class NoListenerForEvent : Exception {
            public Enum eventID;
            public NoListenerForEvent(Enum _eventEnum) : base(ConstructMessage(_eventEnum)) {
                eventID = _eventEnum;
            }
            private static string ConstructMessage(Enum _eventEnum) {
                return string.Format("No Listener Found For EventID [{0}]", System.Enum.GetName(_eventEnum.GetType(), _eventEnum));
            }
        }

        [Serializable]
        public class DuplicateListenerForEvent : Exception {
            public Enum eventID;
            public EventHandler handler;
            public DuplicateListenerForEvent(Enum _eventEnum, EventHandler _handler) : base(ConstructMessage(_eventEnum, _handler)) {
                eventID = _eventEnum;
                handler = _handler;
            }
            private static string ConstructMessage(Enum _eventEnum, EventHandler _handler) {
                return string.Format("Duplicate Listener [Target:{0} FullName:{1}.{2}] For EventID [{3}]", _handler.Target, _handler.Method.DeclaringType.FullName, _handler.Method.Name, System.Enum.GetName(_eventEnum.GetType(), _eventEnum));
            }
        }

        public delegate void EventHandler(Enum _eventEnum, argType arg);
        private Hash<List<EventHandler>> Listeners = new Hash<List<EventHandler>>();
        public bool HasListener<TEnum>(TEnum _eventEnum) {
            int _eventInt = (int)Convert.ChangeType(_eventEnum, TypeCode.Int32);
            return Listeners.ContainsKey(_eventInt);
        }
        // Dispatch msgs to modules
        public void Dispatch<TEnum>(TEnum _eventEnum, argType msgs) where TEnum : Enum {
            //CheckType(_eventEnum);
            int _eventInt = (int)Convert.ChangeType(_eventEnum, TypeCode.Int32);
            if (Listeners.ContainsKey(_eventInt)) {
                for (int i = 0; i < Listeners[_eventInt].Count; i++) {
                    Listeners[_eventInt][i](_eventEnum, msgs);
                }
            } else {
                // TODO: Temp code
                throw new NoListenerForEvent(_eventEnum);
            }
        }
        public void AddListener<TEnum>(TEnum _eventEnum, EventHandler handler) where TEnum : Enum {
            int _eventInt = (int)Convert.ChangeType(_eventEnum, TypeCode.Int32);
            if (!Listeners.ContainsKey(_eventInt)) {
                var tmp = new List<EventHandler>();
                Listeners[_eventInt] = tmp;
            }

            if (Listeners[_eventInt].Contains(handler)) {
                throw new DuplicateListenerForEvent(_eventEnum, handler);
            } else {
                if (!Listeners[_eventInt].Contains(handler)) Listeners[_eventInt].Add(handler);
            }
        }

        public void DelListener<TEnum>(TEnum _eventEnum, EventHandler handler) where TEnum : Enum {
            int _eventInt = (int)Convert.ChangeType(_eventEnum, TypeCode.Int32);
            if (Listeners.ContainsKey(_eventInt) && Listeners[_eventInt].Contains(handler)) {
                Listeners[_eventInt].Remove(handler);
            }
        }

    }
}
