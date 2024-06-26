﻿using System.Diagnostics.CodeAnalysis;
using System.Reflection;
namespace FakerLib.Construction;

internal sealed class ContructionInfoProvider
{
    public bool TryGet(Type type, [MaybeNullWhen(false)] out ConstructionInfo info)
    {
        info = null;

        ConstructorInfo? constructor = GetConstructor(type);
        if (constructor is null)
            return false;

        var parameters = GetConstructorParametersInfos(constructor);
        var properties = type.GetProperties().Where(p => p.CanWrite).ToList();
        var fields = type.GetFields().ToList();

        foreach (ContructorParameterInfo parameter in parameters)
        {
            PropertyInfo? property = properties.FirstOrDefault(p => p.Name == parameter.MemberName);
            if (property is not null)
                properties.Remove(property);
            else
            {
                FieldInfo? field = fields.FirstOrDefault(f => f.Name == parameter.MemberName);
                if (field is not null)
                    fields.Remove(field);
            }
        }

        info = new ConstructionInfo(type, constructor, parameters, properties, fields);

        return true;
    }

    private static ConstructorInfo? GetConstructor(Type type)
    {
        var constructors = type.GetConstructors();
        return constructors.Length switch
        {
            0 => null,
            1 => constructors[0],
            _ => constructors.OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault(c => c.GetParameters().All(p => p.ParameterType != type)),
        };
    }

    private static List<ContructorParameterInfo> GetConstructorParametersInfos(MethodBase constructor) =>
        constructor.GetParameters().Select(p =>
                new ContructorParameterInfo(
                    p.ParameterType,
                    p.Name ?? throw new InvalidOperationException("Parameter name is null."),
                    p.Name?[0].ToString().ToUpper() + p.Name?[1..]))
            .ToList();

}
