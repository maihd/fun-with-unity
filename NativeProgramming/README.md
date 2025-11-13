# Introduction
This document provide helpful information to doing native programming (mainly for Unity plugins).

## The first principles
- Preferred simplicity, even it's needed to using C++.
- Preferred simple language that works: Odin, C, Orthodox C++, Beef.
- C# is purely worked with C ABI for FFI (not C++ or Rust).
- If your library or plugins have C API, just use it.
- If you need write some bindings, preferred written its in C. And C can easily do codegen for bindings.
- Avoid native scripting if you can, Unity have good performance for many use cases (but not the best). If you just want do some native stuffs to explore new way of making things, just do it.

## Doing scripting in native
- Choose any languages if you can
- Provide a bindings from Unity C# through unsafe features:
    - Function pointer
    - Object to pointer and UnityObject internal native pointer
    - Static functions that call method from Unity Object
    - If you use vanilla C# object in your native code, you need to pin C# object (Google this)
- Even you use OOP language like C++ or Beef, you still need a framework (existed or custom) to doing things in fast ways. So may be native languages depend on your favorites.
- If you can do complex and hard works, try combine Unity ECS with native code.
- Roll your own ECS is also a choice.
- Still, you should keep things simple, only for gameplay code, library is another story.

## Services
- Mostly supported by creator, because Unity is widely used.
- If they do not support Unity, there is a chance that they have support C#.
- If still not, find another options.
- Finally, services is big, roll your own bindings is not a good choices.

## Platform native
- Not the same concept with computer native (language that compile to machine code, usually system languages).
- Widely languages in used are: C/C++, Java/Kotlin, ObjC/Swift
- Unity have mostly done it for us
- Maybe have existed bindings
- You create bindings, call the native function export to C# side. This is old style in mobile gamedev (gameplay code in C++, handle services logic in platform native languages, both side have callback to communicating).

## Other topics 
Not common in gamedev in my knowledges. Tell me your story are welcome.