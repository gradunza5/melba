extends TileMap

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var Dimensions : Vector2
var wallChance = 0.5 #percent
var deathLimit = 3
var birthLimit = 3
var canSimulate = false
var oldMap : TileMap

const SPACE = 0
const WALL = 1

# Called when the node enters the scene tree for the first time.
func _ready():
	randomize()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if (canSimulate):
		oldMap = self
		simulation_step()

func init_map():
	for x in range(Dimensions.x):
		for y in range(Dimensions.y):
			if randf() < wallChance:
				set_cell(x, y, WALL)
	canSimulate = true

func simulation_step():
	for x in range (Dimensions.x):
		for y in range (Dimensions.y):
			var nbs = count_alive_neighbors(oldMap, x, y)
			if (oldMap.get_cell(x, y) == WALL):
				if (nbs < deathLimit):
					set_cell(x, y, SPACE)
				else:
					set_cell(x, y, WALL)
			else:
				if (nbs > birthLimit):
					set_cell(x, y, SPACE)
				else:
					set_cell(x, y, WALL)

func count_alive_neighbors(map, x, y):
	var count = 0
	
	for i in range(-1, 1):
		for j in range (-1, 1):
			var neighbor_x = x + i
			var neighbor_y = y + j
			
			if (i == 0 && j == 0):
				continue
			elif (neighbor_x < 0 || neighbor_y < 0 || neighbor_x > Dimensions.x || neighbor_y > Dimensions.y):
				count = count + 1
			elif (map.get_cell(neighbor_x, neighbor_y) == WALL):
				count = count + 1
	return count