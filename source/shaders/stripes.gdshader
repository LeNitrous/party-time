shader_type canvas_item;

uniform vec4 stripe_color : source_color = vec4(0.25);
uniform float stripe_bias                = 2.5;
uniform float       speed                = 0.0;
uniform float       angle                = 0.0;
uniform float     density                = 8.0;

void fragment() {
	float r = radians(angle);
	float w = cos(r) + UV.x + sin(r) * UV.y - speed * TIME;
	if (floor(mod(w * density, stripe_bias)) > 0.0001) {
		COLOR = mix(COLOR, stripe_color, stripe_color.a);
	}
}