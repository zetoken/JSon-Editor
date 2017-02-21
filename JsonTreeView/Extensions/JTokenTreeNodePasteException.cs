using System;

namespace ZTn.Json.JsonTreeView.Extensions
{
    public class JTokenTreeNodePasteException : AggregateException
    {
        public JTokenTreeNodePasteException(Exception sourceException)
            : base(sourceException)
        {
        }
    }
}
