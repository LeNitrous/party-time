shader_type canvas_item;

uniform float m_scale  = 1.0f;
uniform vec2  m_speed  = vec2(0.0);
uniform vec2  m_offset = vec2(0.0);

void vertex() {
	UV = (VERTEX + m_offset + TIME * m_speed)
		* TEXTURE_PIXEL_SIZE * (1.0f / m_scale);
}