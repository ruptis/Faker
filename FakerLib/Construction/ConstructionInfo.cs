﻿using System.Reflection;
using System.Text;
namespace FakerLib.Construction;

internal record ConstructionInfo(
    Type Type,
    ConstructorInfo Constructor,
    List<ContructorParameterInfo> Parameters,
    List<PropertyInfo> Properties,
    List<FieldInfo> Fields)
{
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Type.Name);
        builder.Append('(');
        var count = Parameters.Count;
        foreach (ContructorParameterInfo parameter in Parameters)
        {
            builder.Append(parameter.Type.Name);
            builder.Append(' ');
            builder.Append(parameter.ParameterName);
            if (--count > 0)
                builder.Append(", ");
        }
        builder.Append(')');
        
        if (Properties.Count > 0)
        {
            builder.Append(" { ");
            count = Properties.Count;
            foreach (PropertyInfo property in Properties)
            {
                builder.Append(property.PropertyType.Name);
                builder.Append(' ');
                builder.Append(property.Name);
                if (--count > 0)
                    builder.Append(", ");
            }
            builder.Append(" }");
        }
        
        if (Fields.Count > 0)
        {
            builder.Append(" { ");
            count = Fields.Count;
            foreach (FieldInfo field in Fields)
            {
                builder.Append(field.FieldType.Name);
                builder.Append(' ');
                builder.Append(field.Name);
                if (--count > 0)
                    builder.Append(", ");
            }
            builder.Append(" }");
        }
        
        return builder.ToString();
    }
}