# Introduction
C# In Unity does not follow newest version of C# and DotNet.
Unity have its own Net runtime which is called Mono (forked).
Currently Unity6 supported C# version upto minimal C# 10.

## Setup requirement
- Create a `csc.rsp` file in scripts or .asmdef folder

## Ergonomics features
- Tuple
- Record: both record struct and record class
- Global Usings
- Tuple deconstruction
- Record deconstruction
- Be careful with record, Unity serialization does not support record

## Performance features
- Function pointer: should only use UnmanagedFunctionPointerAttribute, avoid function pointer syntax
- Mono internal call: MonoPInvokeCallbackAttribute
- Object pointer: Do some tricks to get object address, and convert to IntPtr. Aware of pinning object, if object is UnityObject, they already pinned for you.
- Export function to native side: static function with MonoPInvokeCallbackAttribute

## Unique in Unity
- Burst Compiler
- Job System
- DOTS in common

## Compiler
- C# Roslyn
- You can do C# code generation in Unity