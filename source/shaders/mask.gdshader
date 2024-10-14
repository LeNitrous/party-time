shader_type canvas_item;

uniform vec4  mask_color : source_color = vec4(1, 1, 1, 1);
uniform float mask_range                = 0.1;
uniform sampler2D screen : hint_screen_texture;

void fragment() {
	vec4 screenPixelColor = texture(screen, SCREEN_UV);

	if (length(abs(mask_color - screenPixelColor)) >= mask_range)
	{
		COLOR.a = 0.0;
	}
}
