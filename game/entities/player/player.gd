class_name Player
extends Entity

var velocity = Vector2()
var invincible_seconds = 0.300
var seconds_since_last_hit = 0.0

func damage(points: int) -> void:
	health -= points
	emit_signal("health_changed", health, health_max)

# Handles persistent inputs such as movement
func _physics_process(delta: float) -> void:
	_handle_move_input_()
	velocity = move_and_slide(velocity)
	seconds_since_last_hit -= delta
	for i in get_slide_count():
		var collision: KinematicCollision2D = get_slide_collision(i)
		if collision.collider is Enemy and seconds_since_last_hit <= 0:
			var enemy: Enemy = collision.collider
			damage(enemy.collision_damage)
			seconds_since_last_hit = invincible_seconds

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
