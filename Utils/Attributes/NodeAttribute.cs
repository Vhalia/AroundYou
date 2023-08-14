using System;

namespace AroundYou.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NodeAttribute : Attribute
    {
        public string nodePath;

        public NodeAttribute(string nodePath)
        {
            this.nodePath = nodePath;
        }
    }
}
