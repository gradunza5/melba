class_name Entity
extends KinematicBody2D

# int, int
signal health_changed(current, maximum)

export (int) var health_max = 1
export (int) var health     = health_max
export (int) var speed      = 1
