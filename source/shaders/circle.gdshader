shader_type canvas_item;

uniform float radius = 1.0;

void vertex() {
	UV = UV - 0.5;
}

void fragment() {
	COLOR = vec4(float(length(UV) < radius * 0.5)) * COLOR;
}