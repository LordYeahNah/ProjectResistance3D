using System.Collections.Generic;
using Godot;

namespace NexusExtensions;

public class StateTransition
{
    // State that we will transition to
    public State NextState;
    // Properties that are required for a successful transition
    public List<StateProperty> _requiredProps = new List<StateProperty>();

    public bool CanTransition(List<StateProperty> currentProps)
    {
        bool hasCheckedProperty = false;
        foreach (var prop in _requiredProps)
        {
            foreach (var curProp in currentProps)
            {
                if (curProp.PropertyName == prop.PropertyName)
                {
                    hasCheckedProperty = true;
                    switch (curProp.PropertyType)
                    {
                        case EPropertyType.PROP_Bool:
                            if(curProp is StateValue<bool> curPropB && prop is StateValue<bool> propB)
                                if (curPropB.Value != propB.Value)
                                    return false;
                            break;
                        case EPropertyType.PROP_Float:
                            if(curProp is StateValue<float> curPropFl && prop is StateValue<float> propFl)
                                if (curPropFl.Value != propFl.Value)
                                    return false;

                            break;
                        case EPropertyType.PROP_Int:
                            if(curProp is StateValue<int> curPropInt && prop is StateValue<int> propInt)
                                if (curPropInt.Value != propInt.Value)
                                    return false;
                            break;
                        case EPropertyType.PROP_Vector2:
                            if(curProp is StateValue<Vector2> curPropVec && prop is StateValue<Vector2> propVec)
                                if (curPropVec.Value != propVec.Value)
                                    return false;
                            break;
                        case EPropertyType.PROP_Node2D:
                            if(curProp is StateValue<Node2D> curPropNode && prop is StateValue<Node2D> propNode)
                                if (curPropNode.Value != propNode.Value)
                                    return false;
                            break;
                        case EPropertyType.PROP_Vector3:
                            if(curProp is StateValue<Vector3> curPropValue && prop is StateValue<Vector3> propVec3)
                                if (curPropValue.Value != propVec3.Value)
                                    return false;
                            break;
                        case EPropertyType.PROP_Node3D:
                            if (curProp is StateValue<Node3D> curPropNode3D && prop is StateValue<Node3D> propNode3D)
                                if (curPropNode3D.Value != propNode3D.Value)
                                    return false;
                            break;
                    }
                }
            }
        }

        return hasCheckedProperty;
    }
}