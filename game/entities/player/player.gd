extends KinematicBody2D
class_name Player

# int, int
signal hp_changed(current, maximum)

export (int) var max_hp = 100
export (int) var hp = max_hp
export (int) var speed = 200

var velocity = Vector2()

# Called when the node enters the scene tree for the first time.
#func _ready() -> void:
#	pass

# Handles discrete inputs such as jump or attack
#func _input(event: InputEvent) -> void:
#	pass

# Handles persistent inputs such as movement
func _physics_process(delta: float) -> void:
	handle_move_input()
	velocity = move_and_slide(velocity)

func handle_move_input() -> void:
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

func damage(points: int) -> void:
	hp -= points
	emit_signal("hp_changed", hp, max_hp)

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta: float) -> void:
#	pass
