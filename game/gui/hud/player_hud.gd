extends Control
class_name PlayerHud

var initial_size = Vector2()

func set_health(current: int, maximum: int):
	var proportion: float = max(float(current) / maximum, 0)
	var updated_size: Vector2 = initial_size
	updated_size.x *= proportion
	$LifeBar/FullBar.rect_size = updated_size

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	initial_size = $LifeBar/FullBar.rect_size
