--[[
int value

]]

CS.UnityEngine.Debug.Log(LuaScriptCaller)
CS.UnityEngine.Debug.Log(Global)
CS.UnityEngine.Debug.Log(Self)

local x = 1

function start()
    LuaScriptCaller:Greeting()
end

function update()
    -- CS.UnityEngine.Debug.Log("Updating")

    if x <= count then
        CS.UnityEngine.Debug.Log(message .. " " .. x)
        x = x + 1
    end
end