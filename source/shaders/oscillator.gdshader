shader_type canvas_item;

uniform float      speed = 1.0;
uniform float  amplitude = 10.0;
uniform vec2  multiplier = vec2(1.0);

void vertex() {
	float offset = cos(TIME * speed) - sin(TIME * speed) * amplitude;
	VERTEX = VERTEX + (multiplier * offset);
}