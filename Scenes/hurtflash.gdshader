shader_type canvas_item;

uniform float flashState : hint_range(0,1) = 1;
const vec4 flashColor = vec4(1, 1, 1, 1);

void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	COLOR = mix(pixelColor, flashColor, 1);
	COLOR.a *= pixelColor.a;
}