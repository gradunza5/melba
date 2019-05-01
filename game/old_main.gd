extends Node2D

var Room = preload("res://Room.tscn")
onready var Map = $TileMap

var tile_size = 32
var num_rooms = 50
var min_size = 4
var max_size = 10
var vspread = 200
var hspread = 400
var cull = 0.75 # percent

var path #AStar pathfinding object

func _ready(): 
	randomize()
	make_rooms()
	
func make_rooms(): 
	for i in range(num_rooms):
		var pos = Vector2(rand_range(-hspread, hspread), rand_range(-vspread, vspread))
		var r = Room.instance()
		var w = min_size + randi() % (max_size - min_size)
		var h = min_size + randi() % (max_size - min_size)
		r.make_room(pos, Vector2(w, h) * tile_size)
		$Rooms.add_child(r)
	# wait for movement to stop
	yield(get_tree().create_timer(1.1), 'timeout')
	# cull rooms
	var room_positions = []
	for room in $Rooms.get_children():
		if randf() < cull:
			room.queue_free()
		else: 
			room.mode = RigidBody2D.MODE_STATIC
			var rpos = room.position
			room_positions.append(Vector3(rpos.x, rpos.y, 0))
	yield(get_tree(), 'idle_frame')
	
	#generate MST connecting rooms
	path = find_mst(room_positions)

func _draw():
	for room in $Rooms.get_children():
		draw_rect(Rect2(room.position - room.size, room.size * 2), Color(32, 228, 0), false)
	
#	if path:
#		for p in path.get_points():
#			for c in path.get_point_connections(p):
#				var pp = path.get_point_position(p)
#				var cp = path.get_point_position(c)
#				draw_line(Vector2(pp.x, pp.y), Vector2(cp.x, cp.y), Color(1, 1, 0), 15, true)

func _process(delta):
	update()

func _input(event):
	if event.is_action_pressed('ui_select'):
		Map.clear()
		for n in $Rooms.get_children():
			n.queue_free()
		path = null
		make_rooms()
	if event.is_action_pressed('ui_focus_next'):
		make_map()
			
func find_mst(nodes) :
	# Prim's Algorithm
	var path = AStar.new()
	path.add_point(path.get_available_point_id(), nodes.pop_front())
	
	# repeat for all nodes
	while nodes:
		var min_dist = INF #minimum distance so far
		var min_pos = null # position of closest node
		var p = null # current position we're looking at
		
		for p1 in path.get_points():
			p1 = path.get_point_position(p1)
			
			# loop through remaining nodes
			for p2 in nodes:
				if p1.distance_to(p2) < min_dist:
					min_dist = p1.distance_to(p2)
					min_pos = p2
					p = p1
		var n = path.get_available_point_id()
		path.add_point(n, min_pos)
		path.connect_points(path.get_closest_point(p), n)
		nodes.erase(min_pos)
	return path
	
func make_map():
	# Create tile map from generated rooms and path.
	Map.clear()
	
	# Fill tilemap with walls, then carve out rooms.
	var full_rect = Rect2()
	for room in $Rooms.get_children():
		var r = Rect2(room.position - room.size,
					 room.get_node("CollisionShape2D").shape.extents*2)
		full_rect = full_rect.merge(r)
		
	var top_left = Map.world_to_map(full_rect.position)
	var bot_right = Map.world_to_map(full_rect.end)
	
	# It would be more efficient to start with a map filled with walls.
	# This fills the map with walls.
	for x in range(top_left.x, bot_right.x):
		for y in range(top_left.y, bot_right.y):
			Map.set_cell(x, y, 1)
			
	# Carve rooms
	var corridors = [] # One corridor per connection
	for room in $Rooms.get_children():
		var s = (room.size / tile_size).floor()
		#var pos = Map.world_to_map(room.position)
		var ul = (room.position / tile_size).floor() - s
		for x in range (1, s.x * 2):
			for y in range (1, s.y * 2):
				Map.set_cell(ul.x + x, ul.y + y, 0)
		
		#Carve connection
		var p = path.get_closest_point(Vector3(room.position.x, room.position.y, 0))
		for conn in path.get_point_connections(p):
			if not conn in corridors:
				var start = Map.world_to_map(Vector2(path.get_point_position(p).x, path.get_point_position(p).y))
				var end = Map.world_to_map(Vector2(path.get_point_position(conn).x, path.get_point_position(conn).y))
				carve_path(start, end)
		corridors.append(p)
		
func carve_path(pos1, pos2):
	var x_diff = sign(pos2.x - pos1.x)
	var y_diff = sign(pos2.y - pos1.y)
	if x_diff == 0: x_diff = pow(-1.0, randi() % 2) # Random direction if horizontally aligned
	if y_diff == 0: y_diff = pow(-1.0, randi() % 2) # Random direction if vertically aligned
	
	# choose to either go up or over first
	var x_y = pos1
	var y_x = pos2
	if (randi() % 2) > 0:
		x_y = pos2
		y_x = pos1
	
	for x in range(pos1.x, pos2.x, x_diff):
		Map.set_cell(x, x_y.y, 0)
		Map.set_cell(x, x_y.y + y_diff, 0) # Widen the corridor
	for y in range(pos1.y, pos2.y, y_diff):
		Map.set_cell(y_x.x, y, 0)
		Map.set_cell(y_x.x + x_diff, y, 0)
	
	
	
	
	
	