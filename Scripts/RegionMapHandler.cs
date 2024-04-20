using Godot;
using System;

public partial class RegionMapHandler : TileMap
{

	Main main;
	Vector2I grassCell = new Vector2I(0, 0);
	Vector2I mountainCell = new Vector2I(1, 0);
	Vector2I riverCell = new Vector2I(2, 0);
	Vector2I plainsCell = new Vector2I(3, 0);
	Vector2I desertCell = new Vector2I(4, 0);
	Vector2I marshCell = new Vector2I(5, 0);

	Vector2I woodResource = new Vector2I(0, 2);
	Vector2I stoneResource = new Vector2I(1, 2);
	Vector2I copperResource = new Vector2I(2, 2);
	Vector2I steelResource = new Vector2I(3, 2);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		main = GetNode<Main>("/root/Main");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void _UnhandledInput(InputEvent mevent)
	{
		base._Input(mevent);
		TileMap map = this;
		GetViewport().SetInputAsHandled();
		if (mevent.GetType() == typeof(InputEventMouseButton) && mevent.IsPressed())
		{
			//Get cell position from mouse position and then check what type of type was clicked.
			InputEventMouseButton mouseEvent = (InputEventMouseButton)mevent;
			Vector2 pos = mouseEvent.Position;
			Vector2I cell = map.LocalToMap(pos);
			
			//Update tiledata label with terrain data
			Vector2I currTerrainCell = map.GetCellAtlasCoords(0, cell);
            GD.Print("Mouse pos: " + pos + "\nCell pos:" + cell + "\nTerrain ID:" + currTerrainCell);
            if ( currTerrainCell == grassCell)
			{
				main.UpdateTileData("Grass", cell);
			} else if (currTerrainCell == mountainCell)
			{
				main.UpdateTileData("Mountain", cell);
			}
			else if (currTerrainCell == riverCell)
			{
				main.UpdateTileData("River", cell);
			}
			else if (currTerrainCell == plainsCell)
			{
				main.UpdateTileData("Plains", cell);
			}
			else if (currTerrainCell == desertCell)
			{
				main.UpdateTileData("Desert", cell);
			}
			else if (currTerrainCell == marshCell)
			{
				main.UpdateTileData("Marsh", cell);
			}

			Vector2I currResourceCell = map.GetCellAtlasCoords(1, cell);
			if (currResourceCell == woodResource)
			{
				main.UpdateTileResourceData("Wood", cell);
			} else if (currResourceCell == stoneResource)
			{
				main.UpdateTileResourceData("Stone", cell);
			}
			else if (currResourceCell == copperResource)
			{
				main.UpdateTileResourceData("Copper", cell);
			}
			else if (currResourceCell == steelResource)
			{
				main.UpdateTileResourceData("Steel", cell);
			} else
			{
				main.UpdateTileResourceData("", cell);
			}
		}
	}
}
