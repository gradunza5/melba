extends Node2D

var accumulator: float = 0.0

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	print("onload!")
	$DebugMap/Player.connect("health_changed", $CanvasLayer/PlayerHud, "set_health")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	accumulator += delta
	if accumulator > 0.50:
		accumulator -= 0.50
		$DebugMap/Player.damage(10)
