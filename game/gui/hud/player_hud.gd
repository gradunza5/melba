extends Control
class_name PlayerHud

var initial_size = Vector2()

func set_hp(current: int, maximum: int):
	var percent: float = float(current) / maximum
	var proportion: float = min(percent, 100.0) / 100.0
	var updated_size: Vector2 = initial_size
	updated_size.x *= proportion
	$LifeBar/FullBar.rect_size = updated_size

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	initial_size = $LifeBar/FullBar.rect_size
