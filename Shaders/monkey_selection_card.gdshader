shader_type canvas_item;
uniform vec4 selectionHighlightColor = vec4(0,0,0, 0.4);
void fragment() {
	// Called for every pixel the material is visible on.
	COLOR =selectionHighlightColor;
}
