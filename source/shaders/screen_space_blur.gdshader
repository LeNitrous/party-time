shader_type canvas_item;

uniform float amount: hint_range(0.0, 5.0);
uniform sampler2D screen : hint_screen_texture, repeat_disable, filter_linear_mipmap;

void fragment() {
	COLOR = textureLod(screen, SCREEN_UV, amount);
}