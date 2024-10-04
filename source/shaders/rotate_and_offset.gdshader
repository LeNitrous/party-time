shader_type canvas_item;

uniform vec2   offset   = vec2(0.0);
uniform float  rotation = 0.0;

void vertex() {
	float cosa = cos(rotation);
	float sina = sin(rotation);
	vec2 pivot = 0.5 / TEXTURE_PIXEL_SIZE;
	vec2 uv = VERTEX - pivot;
	     uv = vec2
		(
			cosa * uv.x - sina * uv.y,
			cosa * uv.y + sina * uv.x
		);
		uv = uv + pivot;

	VERTEX = uv + offset;
}