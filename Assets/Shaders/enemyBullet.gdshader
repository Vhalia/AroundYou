shader_type canvas_item;

uniform vec4 color : source_color;


void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	vec2 pixelSize = vec2(1.0/float(textureSize(TEXTURE, 0).x), 1.0/float(textureSize(TEXTURE, 0).y));
	
	float alpha = -4.0 * pixelColor.a;
	alpha += texture(TEXTURE, UV + vec2(pixelSize.x, 0)).a;//right offset
	alpha += texture(TEXTURE, UV + vec2(-pixelSize.x, 0)).a;//left offset
	alpha += texture(TEXTURE, UV + vec2(0, pixelSize.y)).a;//top offset
	alpha += texture(TEXTURE, UV + vec2(0, -pixelSize.y)).a;//bottom offset
	
	vec4 finalColor = mix(pixelColor, color, clamp(alpha, 0.0, 1.0));
	COLOR = vec4(finalColor.rgb, clamp(abs(alpha)+pixelColor.a, 0.0, 1.0));
}
