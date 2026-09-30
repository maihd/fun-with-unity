using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using System.Runtime.InteropServices;

public enum CommandKind
{
    None,

}


public struct Command
{
    public CommandKind kind;
    public CommandData data;
}


[StructLayout(LayoutKind.Explicit)]
public struct CommandData
{

}


public struct CommandBuffer
{
    public Command[] commands;

    public int count;
    public int capacity => commands.Length;

    public CommandBuffer(int capacity)
    {
        commands = new Command[capacity];
        count = 0;
    }


    public Command this[int index] => commands[index];

    public void Add(Command command)
    {
        commands[count] = command;
        count += 1;
    }

    public void Clear()
    {
        count = 0;
    }
}


public static class CommandRegistry
{
    private static CommandBuffer[] buffers = new CommandBuffer[] { new CommandBuffer(4096), new CommandBuffer(4096) };
    static int currentIndex = 0;

    public static ref CommandBuffer Current => ref buffers[currentIndex];

    public static void SwapBuffers()
    {
        currentIndex = (currentIndex + 1) % buffers.Length;
    }

    public static void QueueEvent(Command command)
    {
        Current.Add(command);
    }
}