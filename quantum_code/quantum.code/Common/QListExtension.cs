using Quantum.Collections;
using System;
using System.Collections.Generic;

namespace Quantum
{
    public delegate T TAction<T>(T item);

    public delegate bool BoolAction<T>(T item);

    public static class QListExtensions
    {
        public static unsafe void Dispose<T>(this QList<T> qList, Frame f) where T : unmanaged
        {
            qList.Clear();
            f.TryFreeList<T>(qList);
        }

        public static unsafe void Dispose<T>(this QListPtr<T> qListPtr, Frame f) where T : unmanaged
        {
            f.TryFreeList(qListPtr);
        }

        public static unsafe void Dispose<T>(this QListPtr<T> qListPtr, Frame f, ref Ptr ptr) where T : unmanaged
        {
            f.TryFreeList(qListPtr);
            ptr = default;
        }

        public static unsafe bool TryDispose<T>(this QListPtr<T> qListPtr, Frame f, ref Ptr ptr) where T : unmanaged
        {
            if (f.TryFreeList(qListPtr))
            {
                ptr = default;
                return true;
            }
            return false;
        }

        public static unsafe bool TryDispose<T>(this QList<T> qListPtr, Frame f) where T : unmanaged
        {
            if (qListPtr.Count > 0)
                return false;
            qListPtr.Clear();
            qListPtr.Dispose(f);
            return true;
        }

        public static unsafe bool TryDispose<T>(this QList<T> qListPtr, Frame f, ref Ptr ptr) where T : unmanaged
        {
            if (qListPtr.Count > 0)
                return false;
            qListPtr.Clear();
            qListPtr.Dispose(f);
            ptr = default;
            return true;
        }

        //public static unsafe void RemoveIndex<T>(this QListPtr<T> qListPtr, Frame f, ref QList<T> qlist, ref Ptr ptr) where T : unmanaged
        //{
        //    if (qlist.Count > 0)
        //    {
        //        return;
        //    }
        //    qlist.Clear();
        //    qListPtr.Dispose(f, ref ptr);
        //}

        public static unsafe bool TryGetListNotEmpty<T>(this QListPtr<T> qListPtr, Frame f, out QList<T> qList) where T : unmanaged
        {
            qList = f.ResolveList(qListPtr);
            if (qList.Count == 0)
                return false;
            return true;
        }

        public static unsafe bool TryGetListNotEmpty<T>(this QListPtr<T> qListPtr, Frame f, out QList<T> qList, out int count) where T : unmanaged
        {
            count = 0;
            if (!f.TryResolveList(qListPtr, out qList))
                return false;
            count = qList.Count;
            if (count == 0)
                return false;

            return true;
        }

        public static unsafe bool TryResloveOrCreateQList<T>(this QListPtr<T> qListPtr, Frame f, out QList<T> qList) where T : unmanaged
        {
            if (!f.TryResolveList(qListPtr, out qList))
            {
                qList = f.AllocateList<T>(2);
                return false;
            }
            return true;
        }

        #region LamdaExtention
        public static unsafe void Foreach<T>(this QList<T> qList, Action<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = 0; i < count; i++)
                action(qList[i]);
        }
        public static unsafe void SetElementData<T>(this QList<T> qList, TAction<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = 0; i < count; i++)
                qList[i] = action(qList[i]);
        }

        public static unsafe T Find<T>(this QList<T> qList, BoolAction<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = 0; i < count; i++)
            {
                var element = qList[i];
                if (action(element))
                    return element;
            }
            return default;
        }

        public static unsafe bool TryFind<T>(this QList<T> qList, BoolAction<T> action, out T element) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = 0; i < count; i++)
            {
                var member = qList[i];
                if (action(member))
                {
                    element = member;
                    return true;
                }
            }
            element = default;
            return false;
        }

        public static unsafe bool IsContain<T>(this QList<T> qList, BoolAction<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = 0; i < count; i++)
            {
                var member = qList[i];
                if (action(member))
                    return true;
            }
            return false;
        }

        public static unsafe List<T> FindAll<T>(this QList<T> qList, BoolAction<T> action) where T : unmanaged
        {
            var list = new List<T>();
            int count = qList.Count;
            for (int i = 0; i < count; i++)
            {
                var element = qList[i];
                if (action(element))
                    list.Add(element);
            }
            return list;
        }

        /// <summary>
        /// Super NOTE: You MUST FREE the QList after use
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="qList"></param>
        /// <param name="f"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static unsafe QList<T> FindAll<T>(this QList<T> qList, Frame f, BoolAction<T> action) where T : unmanaged
        {
            var list = f.AllocateList<T>(); ;
            int count = qList.Count;
            for (int i = 0; i < count; i++)
            {
                var element = qList[i];
                if (action(element))
                    list.Add(element);
            }
            return list;
        }

        public static unsafe int IndexOf<T>(this QList<T> qList, BoolAction<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = 0; i < count; i++)
            {
                var element = qList[i];
                if (action(element))
                    return qList.IndexOf(element);
            }
            return -1;
        }

        public static unsafe void RemoveOnce<T>(this QList<T> qList, BoolAction<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = count - 1; i >= 0; i--)
                if (action(qList[i]))
                {
                    qList.RemoveAt(i);
                    break;
                }
        }

        public static unsafe void RemoveOnce<T>(this QList<T> qList, BoolAction<T> action, Action<T> SubActionafterRemoveElement) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var element = qList[i];
                if (action(element))
                {
                    qList.RemoveAt(i);
                    SubActionafterRemoveElement?.Invoke(element);
                    break;
                }
            }
        }

        public static unsafe void RemoveAll<T>(this QList<T> qList, BoolAction<T> action) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = count - 1; i >= 0; i--)
                if (action(qList[i]))
                    qList.RemoveAt(i);
        }

        public static unsafe void RemoveAll<T>(this QList<T> qList, BoolAction<T> action, Action<T> SubActionafterRemoveElement) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var element = qList[i];
                if (action(element))
                    qList.RemoveAt(i);
                SubActionafterRemoveElement?.Invoke(element);
            }
        }

        public static unsafe void CleanWithAction<T>(this QList<T> qList, Action<T> actionAfterRemoveElement) where T : unmanaged
        {
            int count = qList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                var element = qList[i];
                qList.RemoveAt(i);
                actionAfterRemoveElement?.Invoke(element);
            }
        }
        #endregion
    }
}
