using Godot;
using System;

public partial class RegionMapHandler : TileMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
    public override void _UnhandledInput(InputEvent mevent)
    {
		GetViewport().SetInputAsHandled();
        base._UnhandledInput(mevent);
		if(mevent.GetType() == typeof(InputEventMouseButton))
        {
			
        }
    }
}
