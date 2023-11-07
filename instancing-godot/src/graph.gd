extends MultiMeshInstance3D

@export var resolution : int = 1000


func _ready():
	self.multimesh.instance_count = resolution * resolution
	
	for x in range(resolution):
		for y in range(resolution):
			var uv : Vector2 = get_uv(Vector2(x,y))
			var pos = wave(uv.x, uv.y, 0)
			pos.z = - pos.z
			update_point(x, y, pos)

func get_uv(xy : Vector2) -> Vector2:
	var step = 2.0 / resolution
	return (xy + Vector2(0.5, 0.5)) * step + Vector2(-1, -1)

func update_point(x : int, y : int, pos : Vector3):
	var step = 2.0 / resolution
	
	if (x < resolution && y < resolution):
		var id = x + y * resolution
		var point_transform = Transform3D()
		point_transform.origin = pos
		point_transform = point_transform.scaled_local(Vector3(step, step, step))
		self.multimesh.set_instance_transform(id, point_transform)

func wave(u : float, v : float, t : float) -> Vector3:
	var p = Vector3()
	p.x = u
	p.y = sin(PI * (u + v + t))
	p.z = v
	return p
