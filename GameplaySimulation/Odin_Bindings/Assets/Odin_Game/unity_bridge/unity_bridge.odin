package unity_bridge

import "base:runtime"

foreign import unity_bridge "../../Odin_Bridge/Out/Odin_Bridge.lib"

foreign unity_bridge {
	runtime_context :: proc "c" () -> runtime.Context ---
}
