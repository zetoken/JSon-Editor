using System;

namespace ZTn.Json.JsonTreeView.Extensions
{
    public class JTokenTreeNodeDeleteException : AggregateException
    {
        public JTokenTreeNodeDeleteException(Exception sourceException)
            : base(sourceException)
        {
        }
    }
}
