shader_type spatial;
render_mode unshaded;

void fragment() {
	vec4 world_pos = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0));
	world_pos.z = - world_pos.z;
	ALBEDO = clamp(world_pos.xyz * 0.5 + 0.5, 0, 1);
}
