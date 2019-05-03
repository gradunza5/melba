extends Node2D

var Room = preload("res://Room.tscn")
onready var Map = $TileMap
onready var Camera = $Camera2D

func _ready(): 
	randomize()
	Map.Init(Vector2(128,256))

func _input(event):
	if event.is_action_pressed('ui_accept'):
		Map.NextStep()
