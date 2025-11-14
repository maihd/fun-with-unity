# Introduction
C# In Unity does not follow newest version of C# and DotNet.
Unity have its own Net runtime which is called Mono (forked).
Currently Unity6 supported C# version upto minimal C# 10.

## Setup requirement
- Create a `csc.rsp` file in Assets folder

## Ergonomics features
- Tuple
- Record: both record struct and record class
- Global Usings
- Tuple deconstruction
- Record deconstruction

## Performance features
- Function pointer
- Mono internal call
- Object pointer

## Unique in Unity
- Burst Compiler
- Job System
- DOTS in common