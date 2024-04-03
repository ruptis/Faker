using System.Text;
namespace Faker;

internal sealed class CircularDependencyAnalyzer
{
    private readonly HashSet<Type> _visited = [];
    private readonly Stack<Type> _path = new();
    
    private readonly StringBuilder _builder = new();
    
    public bool Validate(Type type)
    {
        if (!_visited.Add(type))
            return false;

        _path.Push(type);
        return true;
    }
    
    public void Remove(Type type)
    {
        _visited.Remove(type);
        _path.Pop();
    }

    public void Reset()
    {
        _visited.Clear();
        _path.Clear();
    }

    public override string ToString()
    {
        _builder.Clear();
        var count = _path.Count;
        foreach (Type type in _path.Reverse())
        {
            _builder.Append(type.Name);
            if (--count > 0)
                _builder.Append(" -> ");
        }
        return _builder.ToString();
    }
}
