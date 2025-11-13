# Simplicity Coding Style in C# for Unity

## Simple rules
- Avoid deep hierarchy, multi-connected classes
- Prefer dumb data, simple struct when doing logic processing
- Prefer simple and works functions when doing computings
- Avoid metaprogramming, reflection when you dont need it
- Make the code the problem at hands, so study your problems before do coding
- Use concrete solutions first, only use abstractions when you need it
- Use and understand your platforms, avoid create new abstractions over your platform
- C# maybe a powerful language, but it has flaws, wrong use cause payback
- In common use of C#, we only need the scripting side of C#.

## Formatting
Any formatting use in your projects are accepted.

## Naming conventions
For simplicity and reduce cognition loads, you should use common naming conventions. Use existing conventions from your projects is a must.

## Features must be used with care
- Exceptions: even exceptions is common in C# code, but it's not always a good things.
- Metaprogramming: reflection works, generic works, but not always a good things.
- Code generation: its generated code can read and understood, avoid.

## Same techniques, ideas
- Orthodox C++
- Compression-oriented Programming
- Grug-brained Developer
- Odin and other modern better C languages
- Beef Programming Language (C# syntaxes with Orthodox C++ semantics)

## Topics need deep dive
- Delegates, lambdas, closures
- Avoid GC: structs, pooling, avoid lambda bindings if available, native collections
- Data oriented in C#
- When common "Clean Code" approachs failed
- Unity GameObjects, Components, how to use its in simplicity way

## Conclusion
This docs is only for remind us to keep C# code as simple as possible, keep scripting, keep making game instead deep dive programming in a dogma way.
That why this docs is short.