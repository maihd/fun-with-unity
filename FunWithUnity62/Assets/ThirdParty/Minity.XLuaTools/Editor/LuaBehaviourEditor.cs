using System;
using UnityEngine;
using UnityEditor;
using Minity.XLuaTools;
using System.Collections.Generic;

[CustomEditor(typeof(LuaBehaviour))]
public class LuaBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var behaviour = target as LuaBehaviour;
        if (behaviour.ReadMetaFromCode)
        {
            if (GUILayout.Button("Log"))
            {
                var code = behaviour.Code?.Code;
                if (code != null)
                {
                    var trimmedCode = code.Trim();
                    if (trimmedCode.StartsWith("--[["))
                    {
                        var endIndex = trimmedCode.IndexOf("]]");
                        if (endIndex < 0)
                        {
                            Debug.LogError("Your script is error! '--[[' must end with ']]'");
                            return;
                        }

                        var metaCode = trimmedCode.Substring(4, endIndex - 4).Trim();
                        Debug.Log(metaCode);

                        var metaLines = metaCode.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                        var keys = new List<string>();
                        var values = new List<SerializableLuaValue>();

                        foreach (var line in metaLines)
                        {
                            var token = Token.Parse(line);
                            Debug.Log(token.Type + " " + token.Name + " = " + token.DefaultValue);

                            keys.Add(token.Name);
                            values.Add(token.DefaultValue);
                        }

                        behaviour.Fields.Keys = keys.ToArray();
                        behaviour.Fields.Values = values.ToArray();
                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
        }
    }

    struct Token
    {
        public string               Type;
        public string               Name;
        public SerializableLuaValue DefaultValue;

        public static Token Parse(string line)
        {
            int index = 0;
            while (index < line.Length && char.IsWhiteSpace(line[index]))
            {
                index++;
            }
            
            string type = ParseType(line, ref index);

            while (index < line.Length && char.IsWhiteSpace(line[index]))
            {
                index++;
            }

            string name = ParseName(line, ref index);

            while (index < line.Length && char.IsWhiteSpace(line[index]))
            {
                index++;
            }

            return new()
            {
                Type = type,
                Name = name,
                DefaultValue = ParseValue(line, ref index)
            };
        }

        static string ParseType(string line, ref int index)
        {
            return ParseName(line, ref index);
        }

        static string ParseName(string line, ref int index)
        {
            char c = line[index];
            if (c != '_' && !char.IsLetter(c))
            {
                return null;
            }
            index++;

            string result = "" + c;
            while (index < line.Length && !char.IsWhiteSpace(line[index]))
            {
                c = line[index++];
                if (c == '_' || char.IsLetter(c) || char.IsNumber(c))
                {
                    result += c;
                }
                else
                {
                    return null;
                }
            }

            return result;
        }

        static SerializableLuaValue ParseValue(string line, ref int index)
        {
            char c = line[index];
            if (c == '\"')
            {
                index++;

                string value = "";
                while (index < line.Length && line[index] != '\"')
                {
                    value += line[index];
                    index++;
                }

                if (index >= line.Length && line[line.Length - 1] != '\"')
                {
                    Debug.LogError("String must be end with \'\"\'");
                    return new();
                }

                return new()
                {
                    Type = SerializableLuaValue.ValueType.String,
                    AsString = value,
                };
            }
            
            if (char.IsLetter(c))
            {
                string token = ParseName(line, ref index);
                if (token == "true")
                {
                    return new()
                    {
                        Type = SerializableLuaValue.ValueType.Bool,
                        AsBool = true,
                    };
                }

                if (token == "false")
                {
                    return new()
                    {
                        Type = SerializableLuaValue.ValueType.Bool,
                        AsBool = false,
                    };
                }

                if (token == "nil")
                {
                    return new()
                    {
                        Type = SerializableLuaValue.ValueType.Nil,
                    };
                }
            }

            if (char.IsNumber(c) || c == '.')
            {
                string token = line.Substring(index).Trim();
                double value = double.Parse(token);
                return new()
                {
                    Type = SerializableLuaValue.ValueType.Number,
                    AsNumber = value,
                };
            }

            return new();
        }
    }
}