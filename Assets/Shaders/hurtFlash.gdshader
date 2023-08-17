shader_type canvas_item;

uniform float state :hint_range(0.0, 1.0, 0.1);
const vec4 flashColor = vec4(1, 1, 1, 1);

void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	COLOR = mix(pixelColor, flashColor, state);
	COLOR.a *= pixelColor.a;
}
