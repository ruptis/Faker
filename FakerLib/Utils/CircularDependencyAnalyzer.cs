using System.Text;
namespace FakerLib.Utils;

internal sealed class CircularDependencyAnalyzer(int depthLimit = 10)
{
    private readonly Dictionary<Type, int> _visitedCounters = new();
    private readonly Stack<Type> _path = new();
    
    private readonly StringBuilder _builder = new();
    
    public bool Validate(Type type)
    {
        _path.Push(type);
        if (!_visitedCounters.TryGetValue(type, out var depth))
        {
            _visitedCounters[type] = 1;
            return true;
        }
        
        if (depth >= depthLimit)
            return false;
        
        _visitedCounters[type] = depth + 1;
        return true;
    }
    
    public void Remove(Type type)
    {
        if (_visitedCounters.TryGetValue(type, out var depth))
        {
            if (depth == 1)
                _visitedCounters.Remove(type);
            else
                _visitedCounters[type] = depth - 1;
        }
        _path.Pop();
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
