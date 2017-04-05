using System;

namespace ComicPresence.Common.Data
{
    /// <summary>
    /// Use with SqlOrm
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
