using System.Reflection;
using System.Text;
namespace Faker;

public record ConstructionInfo(
    Type Type,
    ConstructorInfo Constructor,
    List<ContructorParameterInfo> Parameters,
    List<PropertyInfo> Properties,
    List<FieldInfo> Fields)
{
    public ConstructionInfo(Type type, ConstructorInfo constructor) : this(type, constructor, [], [], [])
    {}

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Type.Name);
        builder.Append("(");
        var count = Parameters.Count;
        foreach (var parameter in Parameters)
        {
            builder.Append(parameter.Type.Name);
            builder.Append(" ");
            builder.Append(parameter.ParameterName);
            if (--count > 0)
                builder.Append(", ");
        }
        builder.Append(")");
        
        if (Properties.Count > 0)
        {
            builder.Append(" { ");
            count = Properties.Count;
            foreach (var property in Properties)
            {
                builder.Append(property.PropertyType.Name);
                builder.Append(" ");
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
            foreach (var field in Fields)
            {
                builder.Append(field.FieldType.Name);
                builder.Append(" ");
                builder.Append(field.Name);
                if (--count > 0)
                    builder.Append(", ");
            }
            builder.Append(" }");
        }
        
        return builder.ToString();
    }
}