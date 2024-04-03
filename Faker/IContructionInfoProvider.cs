using System.Diagnostics.CodeAnalysis;
namespace Faker;

public interface IContructionInfoProvider
{
    bool TryGet(Type type, [MaybeNullWhen(false)] out ConstructionInfo info);
}
