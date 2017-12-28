using System;
using System.Collections.Generic;
using System.Text;

namespace Unre
{
    public static class LinkedListExtension
    {
        public static void RemoveLast<T>(this LinkedList<T> list, bool isRemovedDispose) where T : class
        {
            //TODO:check null and throw exception

            //TODO:check any and throw exception

            if (isRemovedDispose)
            {
                var requireDisposeT = list.Last.Value;
                list.RemoveLast();
                if (requireDisposeT is IDisposable)
                {
                    ((IDisposable)(requireDisposeT)).Dispose();
                }
                else
                {
                    requireDisposeT = null;
                }
            }
            else
            {
                list.RemoveLast();
            }
        }

    }
}
