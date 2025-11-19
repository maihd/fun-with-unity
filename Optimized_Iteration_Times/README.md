# Introduction
How to optimized Unity Editor build, domain reload, compilation, asset processings.

## Coding iterations
- Avoid domain reload, turn off when play mode
- Use Hot Reload for C# (Assume working with C# is slow)
- If code does not need speed, use dynamic scripting language, or Unity Visual Scripting
- When domain reload, Unity Editor need to re-do serializations, must a deep look about your scripts. Even private fields does have serialized, use NonSerialized
- Careful when turn domain reload: Your statics isnt reset after quit play mode, you need handle reset statics by yourself. 

## Optimize file processing
- Windows Defender may affects files processing performance
- Use better SSD

## Optimize Unity Editor
- Unity Editor support compile Editor code in Release Mode
- Avoid unnecessary asset importer
- When windows does nothings, its should be closed

## Assets and Project Interations
- When open project, Unity Editor must ensure your assets is up to date and valid. So its will do some importing progresses.
- Even we have incremental assets processing, when you share project to other members, they will need importing agains, make its slow, use Unity Accelerator to avoid that.
- When you have no knowledges about where/which make Unity Editor slow, use Project Auditor to profiling (and others).

## Resources
- https://johnaustin.io/articles/2020/domain-reloads-in-unity
- https://blog.s-schoener.com/2023-08-16-why-your-unity-project-is-slow/
- https://www.reddit.com/r/Unity3D/comments/1gjcp9l/how_we_cut_down_our_domain_reload_from_25s_6s/