using System;
using UnityEngine;

[Serializable]
public struct SerializableLuaValue
{
    public enum ValueType
    {
        Nil,
        Bool,
        Number,
        String,
        Table,
    }

    public ValueType            Type;
    public bool                 AsBool;
    public double               AsNumber;
    public string               AsString;

    [SerializeReference]
    public SerializableLuaTable AsTable;

    public override string ToString()
    {
        var result = "[" + Type;
        
        switch (Type)
        {
            case SerializableLuaValue.ValueType.Nil:
                break;

            case SerializableLuaValue.ValueType.Bool:
                    result += ", " + AsBool;
                break;

            case SerializableLuaValue.ValueType.Number:
                    result += ", " + AsNumber;
                break;

            case SerializableLuaValue.ValueType.String:
                    result += ", \"" + AsString + "\"";
                break;

            case SerializableLuaValue.ValueType.Table:
                break;
        }

        result += "]";
        return result;
    }
}

[Serializable]
public class SerializableLuaTable
{
    public string[]                 Keys;

    public SerializableLuaValue[]   Values;
}