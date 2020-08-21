extends KinematicBody2D
class_name Player

# int, int
signal health_changed(current, maximum)

export (int) var health_max = 100
export (int) var health     = health_max
export (int) var speed      = 200

var velocity = Vector2()

func damage(points: int) -> void:
	health -= points
	emit_signal("health_changed", health, health_max)

# Called when the node enters the scene tree for the first time.
#func _ready() -> void:
#	pass

# Handles discrete inputs such as jump or attack
#func _input(event: InputEvent) -> void:
#	pass

# Handles persistent inputs such as movement
func _physics_process(delta: float) -> void:
	_handle_move_input_()
	velocity = move_and_slide(velocity)

func _handle_move_input_() -> void:
	velocity = Vector2()
	if Input.is_action_pressed("ui_left"):
		velocity.x -= 1
	if Input.is_action_pressed("ui_right"):
		velocity.x += 1
	if Input.is_action_pressed("ui_up"):
		velocity.y -= 1
	if Input.is_action_pressed("ui_down"):
		velocity.y += 1
	velocity = velocity.normalized() * speed

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass
