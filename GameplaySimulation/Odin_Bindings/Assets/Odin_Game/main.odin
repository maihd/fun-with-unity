package odin_game

import "base:runtime"
import "unity_bridge"

import "core:log"
import linalg "core:math/linalg"
import glm "core:math/linalg/glsl"


Entity :: struct {
	position: [3]f32,
	rotation: quaternion128,
	scale:    [3]f32,
}


Event :: struct {
	kind: Event_Kind,
	data: struct #raw_union {
		entity: Entity,
	},
}


Event_Kind :: enum {
	None,
	Spawn,
	Despawn,
}


state: struct {
	ctx:      runtime.Context,
	entities: [dynamic]Entity,
	events:   [dynamic]Event,
}


@(export)
hellope :: proc "c" () {
	context = unity_bridge.runtime_context()

	log.info("Hellope!")
	log.info("We are from Odin DLL that greeting to you!")

	ints := make([dynamic]int)
	append(&ints, 1)
	append(&ints, 2)
	append(&ints, 3)
	log.info(ints)

	delete(ints)
}


@(export)
game_init :: proc "c" () {
	state.ctx = unity_bridge.runtime_context()

	context = state.ctx
}


@(export)
game_deinit :: proc "c" () {
	context = state.ctx

	state = {}
}


@(export)
game_update :: proc "c" (dt: f32) {
	context = state.ctx

	free_all(context.temp_allocator)
	clear(&state.events)

	for &e in state.entities {
		// e.position += 10 * dt

		// e.rotation *= glm.quatAxisAngle({0, 1, 0}, dt * glm.PI)

		// linalg.matrix4_rotate()
		mat := glm.mat4FromQuat(e.rotation)
		mat *= glm.mat4Rotate({0, 1, 0}, dt * glm.PI)
		e.rotation = glm.quatFromMat4(mat)
	}
}


@(export)
spawn_entity :: proc "c" (e: Entity) {
	context = state.ctx

	append(&state.entities, e)
	append(&state.events, Event{kind = .Spawn, data = {entity = e}})
}


@(export)
register_entity :: proc "c" (e: Entity) {
	context = state.ctx

	append(&state.entities, e)
}


@(export)
get_game_entities :: proc "c" () -> []Entity {
	return state.entities[:]
}


@(export)
get_game_events :: proc "c" () -> []Event {
	return state.events[:]
}
