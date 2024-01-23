using Godot;
using System;
using System.Collections;

public partial class Main : Node2D
{
	//Labels
	[Export] Label Wood;
	[Export] Label Stone;
	[Export] Label Copper;
	[Export] Label Steel;
	[Export] Label Fuel;
	[Export] Label Food;
	[Export] Label Water;
	[Export] Label Weapons;
	[Export] Label Leisure;

	[Export] Label TotalPop;
	[Export] Label EmployedPop;
	[Export] Label Growth;

	//Values
	int woodVal;
	int stoneVal;
	int copperVal;
	int steelVal;
	int fuelVal;
	int foodVal;
	int waterVal;
	int weaponVal;
	int leisureVal;
	int totalPopVal;
	int employedPopVal;
	double growthVal;

	[Export] TileMap regionMap;
	struct ResourceNode { int xPos; int yPos; bool activated; bool worked;
		public ResourceNode(int x, int y, bool active, bool working)
		{
			xPos = x; yPos = y; activated = active; worked = working;
		}
	};
	ArrayList resourceNodes = new ArrayList();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Here we would set our inital values for our resources, based on difficulty selected.

		//Also populate our list of resource nodes with all current resource nodes
		Vector2I resourceAtlasLocation = new Vector2I(4, 0);
		Godot.Collections.Array<Vector2I> resourceArray = regionMap.GetUsedCellsById(0, 0, resourceAtlasLocation);

		foreach(Vector2I cell in resourceArray)
		{
			ResourceNode currNode = new ResourceNode(cell.X, cell.Y, false, false);
			resourceNodes.Add(currNode);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//TODO: Finish Logic
		//Check for enabled resource nodes and then check to see if they are being worked

		//If both are a go then update resource accordingly

	}

	//Change Label Data
	public void UpdateLabel(String labelName, int value)
	{
		switch (labelName)
		{
			case null:
				break;
			case "Wood":
				Wood.Text = value.ToString();
				break;
			case "Stone":
				Stone.Text = value.ToString();
				break;
				//TODO: Finsh this switch
		}
	}
}
