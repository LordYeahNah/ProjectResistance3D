using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public enum EPropertyType
{
    PROP_NONE,
    PROP_Node2D,
    PROP_Vector2,
    PROP_Bool,
    PROP_Float,
    PROP_Int,
    PROP_Vector3,
}

public class StateProperty
{
    public string PropertyName;
    private EPropertyType _propertyType;
    public EPropertyType PropertyType => _propertyType;

    public StateProperty(string name, EPropertyType propType)
    {
        PropertyName = name;
        _propertyType = propType;
    }
}

public class StateValue<T> : StateProperty
{
    public T Value;

    public StateValue(string name, T value, EPropertyType propType) : base(name, propType)
    {
        Value = value;
    }
}