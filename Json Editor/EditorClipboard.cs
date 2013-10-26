using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTn.Json.Editor.Forms;

namespace ZTn.Json.Editor
{
    class EditorClipboard<T> where T : class
    {
        static T data = null;
        static bool persistentState = false;

        public static void Clear()
        {
            data = null;
        }

        public static T Get()
        {
            if (!persistentState)
            {
                T source = data;
                Clear();
                return source;
            }
            else
            {
                return data;
            }
        }

        public static bool IsEmpty()
        {
            return data == null;
        }

        public static void Set(T data)
        {
            Set(data, true);
        }

        public static void Set(T data, bool persistentState)
        {
            EditorClipboard<T>.data = data;
            EditorClipboard<T>.persistentState = persistentState;
        }
    }
}
