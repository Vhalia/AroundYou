using Godot;
using System.Reflection;
using System;
using AroundYou.Utils.Attributes;
using System.Linq;

namespace AroundYou.Utils.Extensions
{
    public static class NodeExtensions
    {
        public static T GetNodeOrDefault<T>(this Node node) where T : class
        {
            return node.GetNodeOrNull<T>($"{nameof(T)}");
        }

        public static void WireNodes(this Node node)
        {
            // Get all fields
            FieldInfo[] info = node
                .GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var f in info)
            {
                // Search for Node attribute
                NodeAttribute attr = (NodeAttribute)Attribute.GetCustomAttribute(f, typeof(NodeAttribute));
                if (attr != null)
                {
                    // Get the node using NodePath and set the field value
                    var nodeFound = node.GetNode(attr.nodePath);
                    if (nodeFound != null)
                        f.SetValue(node, nodeFound);

                }
            }
        }

        public static bool HasNode<T>(this Node node) where T : class
        {
            foreach(Node child in node.GetChildren())
            {
                if (child is T)
                    return true;
            }
            return false;
        }

        public static bool TryGetNode<T>(this Node node, out T result) where T : class
        {
            T found = GetNodeOrDefault<T>(node);
            result = null;
            if (found != null)
                return true;
            return false;
        }

        public static bool IsInAnyGroup(this Node node, params string[] groups)
        {
            foreach(var nodeGroup in groups)
            {
                if (groups.Contains(nodeGroup)) return true;
            }

            return false;
        }

        public static T GetAutoLoad<T>(this Node node) where T : class
        {
            return node.GetNode($"/root/{typeof(T).Name}") as T;
        }

        public static Node GetMain(this Node node)
        {
            return node.GetNode("/root/Main");
        }
    }
}
