using Godot;
using System;

public partial class RegionMapHandler : TileMap
{

	Main main;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		main = GetNode<Main>("/root/Main");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _Input(InputEvent mevent)
	{
		base._Input(mevent);
		TileMap map = this;
		if (mevent.GetType() == typeof(InputEventMouseButton) && mevent.IsPressed())
		{
			//Get cell position from mouse position and then check what type of type was clicked.
			InputEventMouseButton mouseEvent = (InputEventMouseButton)mevent;
			Vector2 pos = mouseEvent.Position;
			Vector2I cell = map.LocalToMap(pos);
			GD.Print("Mouse pos: " + pos + "\nCell pos:" + cell);

			//TODO: Add regular terrain handling
			Vector2I resourceAtlasLocation = new Vector2I(4, 0);
			if (map.GetCellAtlasCoords(1, cell) == resourceAtlasLocation)
			{
				GD.Print("\nClicked a resource.\n");
				main.UpdateTileData("Resource", cell);
			}
		}
	}
}
