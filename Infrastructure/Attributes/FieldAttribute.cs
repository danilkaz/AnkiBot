using System;

namespace Infrastructure.Attributes
{
    public class FieldAttribute : Attribute
    {
        public FieldAttribute(string name, bool isUnique = false)
        {
            Name = name;
            IsUnique = isUnique;
        }
        
        public string Name { get; }
        public bool IsUnique { get; }
    }
}