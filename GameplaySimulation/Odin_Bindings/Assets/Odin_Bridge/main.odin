package unity_odin_bridge


import "base:runtime"
import "core:log"
import "core:mem"
import "core:strings"

ENABLE_UNITY_COLLECTIONS_CHECKS :: #config(ENABLE_UNITY_COLLECTIONS_CHECKS, true)
UNITY_DOTS_DEBUG :: #config(UNITY_DOTS_DEBUG, false)


IUnityInterfaces :: struct {
	GetInterface:           proc "system" (guid: UnityInterfaceGUID) -> ^IUnityInterface,
	RegisterInterface:      proc "system" (guid: UnityInterfaceGUID, ptr: ^IUnityInterface),
	GetInterfaceSplit:      proc "system" (guidHigh: u64, guidLow: u64) -> ^IUnityInterface,
	RegisterInterfaceSplit: proc "system" (guidHigh: u64, guidLow: u64, ptr: ^IUnityInterface),
}

IUnityInterface :: struct {}

UnityInterfaceGUID :: struct {
	m_GUIDHigh, mGUIDLow: u64,
}


// IUnityLog
IUnityLog_GUID :: UnityInterfaceGUID{0x9E7507fA5B444D5D, 0x92FB979515EA83FC}

IUnityLog :: struct {
	Log: proc "system" (type: UnityLogType, message, filename: cstring, fileLine: i32),
}

UnityLogType :: enum {
	Error,
	Warning = 2,
	Log = 3,
	Exception = 4,
}


// IUnityMemoryManager
IUnityMemoryManager_GUID :: UnityInterfaceGUID{0xBAF9E57C61A811EC, 0xC5A7CC7861A811EC}

IUnityMemoryManager :: struct {
	CreateAllocator:  proc "std" (areaName: cstring, objectName: cstring) -> ^UnityAllocator,
	DestroyAllocator: proc "std" (allocator: ^UnityAllocator),
	Allocate:         proc "std" (
		allocator: ^UnityAllocator,
		size, align: u32,
		file: cstring,
		line: i32,
	) -> rawptr,
	Deallocate:       proc "std" (
		allocator: ^UnityAllocator,
		ptr: rawptr,
		file: cstring,
		line: i32,
	),
	Reallocate:       proc "std" (
		allocator: ^UnityAllocator,
		ptr: rawptr,
		size, align: u32,
		file: cstring,
		line: i32,
	) -> rawptr,
}

UnityAllocator :: struct {}


// Unity DLL Entry

state: struct {
	ctx:       runtime.Context,
	log:       ^IUnityLog,
	mem:       ^IUnityMemoryManager,
	allocator: ^UnityAllocator,
}

@(export)
UnityPluginLoad :: proc "system" (unityInterfaces: ^IUnityInterfaces) {
	state.log = cast(^IUnityLog)unityInterfaces.GetInterface(IUnityLog_GUID)
	state.mem = cast(^IUnityMemoryManager)unityInterfaces.GetInterface(IUnityMemoryManager_GUID)

	state.allocator = state.mem.CreateAllocator("Odin Memory", "Main Context")

	state.ctx = runtime.default_context()

	// Use Unity Memory Allocator for Odin code
	// state.ctx.allocator = {
	// 	procedure = proc(
	// 		allocator_data: rawptr,
	// 		mode: runtime.Allocator_Mode,
	// 		size, alignment: int,
	// 		old_memory: rawptr,
	// 		old_size: int,
	// 		location := #caller_location,
	// 	) -> (
	// 		[]byte,
	// 		runtime.Allocator_Error,
	// 	) {
	// 		runtime.DEFAULT_TEMP_ALLOCATOR_TEMP_GUARD()

	// 		path_name := strings.clone_to_cstring(location.file_path)
	// 		// defer delete(path_name)

	// 		#partial switch mode {
	// 		case .Alloc, .Alloc_Non_Zeroed:
	// 			return mem.byte_slice(
	// 					state.mem.Allocate(
	// 						state.allocator,
	// 						u32(size),
	// 						u32(alignment),
	// 						path_name,
	// 						location.line,
	// 					),
	// 					size,
	// 				),
	// 				.None

	// 		case .Free:
	// 			state.mem.Deallocate(state.allocator, old_memory, path_name, location.line)

	// 		case .Free_All:
	// 			return nil, .Mode_Not_Implemented

	// 		case .Resize:
	// 			if old_memory == nil {
	// 				return mem.byte_slice(
	// 						state.mem.Allocate(
	// 							state.allocator,
	// 							u32(size),
	// 							u32(alignment),
	// 							path_name,
	// 							location.line,
	// 						),
	// 						size,
	// 					),
	// 					.None
	// 			}

	// 			return mem.byte_slice(
	// 					state.mem.Reallocate(
	// 						state.allocator,
	// 						old_memory,
	// 						u32(size),
	// 						u32(alignment),
	// 						path_name,
	// 						location.line,
	// 					),
	// 					size,
	// 				),
	// 				.None

	// 		case .Query_Features:
	// 			set := (^mem.Allocator_Mode_Set)(old_memory)
	// 			if set != nil {
	// 				set^ = {.Alloc, .Alloc_Non_Zeroed, .Free, .Resize, .Query_Features}
	// 			}

	// 			return nil, nil

	// 		case .Query_Info:
	// 			return nil, .Mode_Not_Implemented
	// 		}

	// 		return nil, nil
	// 	},
	// }

	// Create working logger from Unity Logger
	state.ctx.logger = {
		options = {.Short_File_Path, .Line},
		procedure = proc(
			data: rawptr,
			level: runtime.Logger_Level,
			text_raw: string,
			options: runtime.Logger_Options,
			location := #caller_location,
		) {
			runtime.DEFAULT_TEMP_ALLOCATOR_TEMP_GUARD()

			logType: UnityLogType
			switch level {
			case .Debug, .Info:
				logType = .Log
			case .Warning:
				logType = .Warning
			case .Error:
				logType = .Error
			case .Fatal:
				logType = .Exception
			}

			text := strings.clone_to_cstring(text_raw, context.temp_allocator)
			path_name := strings.clone_to_cstring(location.file_path, context.temp_allocator)
			state.log.Log(logType, text, path_name, location.line)
		},
	}

	// Finsished
	context = state.ctx
	log.infof("Odin_Bridge UnityPluginLoad")
}


@(export)
UnityPluginUnload :: proc "system" () {
	context = state.ctx
	log.infof("Odin_Bridge UnityPluginUnload")

	state.mem.DestroyAllocator(state.allocator)

	state = {}
}


@(export)
runtime_context :: proc "c" () -> runtime.Context {
	return state.ctx
}
