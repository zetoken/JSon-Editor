using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTn.Json.Editor.Forms
{
    /// <summary>
    /// Exception thrown when a <see cref="JToken"/> instance is of an unattended type.
    /// </summary>
    public sealed class UnattendedJTokenTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the class with the faulty <see cref="JToken"/> instance.
        /// </summary>
        /// <param name="jToken"></param>
        public UnattendedJTokenTypeException(JToken jToken)
            : base("Unattended JToken type encountered: " + jToken.GetType().FullName)
        {
        }
    }
}
