shader_type canvas_item;

uniform float flashState : hint_range(0,1) = 0;
const vec4 flashColor = vec4(1, 1, 1, 1);

void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	COLOR = mix(pixelColor, flashColor, flashState);
	COLOR.a *= pixelColor.a;
}