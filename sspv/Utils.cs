using System;

namespace sspv

{
    public static class Utils
    {
        public static T Consume<T>(T Obj, Action<T> Action) {
            Action.Invoke(Obj);
            return Obj;
        }
    }
}
