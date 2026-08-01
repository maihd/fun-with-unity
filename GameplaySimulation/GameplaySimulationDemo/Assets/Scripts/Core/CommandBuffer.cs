using UnityEngine;

using Unity.Burst;
using Unity.Collections;

public enum CommandKind
{
    None,

}


public struct Command
{
    public CommandKind kind;
}


public struct CommandBuffer
{
    public NativeRingQueue<Command> commands;

    public int count => commands.Length;

    
}
