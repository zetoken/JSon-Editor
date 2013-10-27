using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTn.Json.Editor.Forms;

namespace ZTn.Json.Editor.Generic
{
    /// <summary>
    /// Generic clipboard for <typeparamref name="T"/> based instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class EditorClipboard<T> where T : class
    {
        #region >> Fields

        static T data = null;
        static bool persistentState = true;

        #endregion

        /// <summary>
        /// Clear all data stored in the clipboard.
        /// </summary>
        public static void Clear()
        {
            data = null;
            persistentState = true;
        }

        /// <summary>
        /// Get the data stored in the clipboard.
        /// If data is set as not persistant then clipboard is automatically cleared.
        /// </summary>
        /// <returns></returns>
        public static T Get()
        {
            if (persistentState)
            {
                return data;
            }
            else
            {
                T source = data;
                Clear();
                return source;
            }
        }

        /// <summary>
        /// Indicates if a data is stored in the clipboard.
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty()
        {
            return data == null;
        }

        /// <summary>
        /// Insert a persistent data in the clipboard.
        /// </summary>
        /// <param name="data"></param>
        public static void Set(T data)
        {
            Set(data, true);
        }

        /// <summary>
        /// Insert a data in the clipboard by specifying the persistent state.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="persistentState">
        ///     <list>
        ///         <item><c>true</c> means the data will not be removed from clipboard afert a <see cref="Get()"/>.</item>
        ///         <item><c>false</c> means the data will be removed from clipboard afert a <see cref="Get()"/>.</item>
        ///     </list>
        /// </param>
        public static void Set(T data, bool persistentState)
        {
            EditorClipboard<T>.data = data;
            EditorClipboard<T>.persistentState = persistentState;
        }
    }
}
