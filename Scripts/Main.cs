using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Main : Node2D
{
	#region fields
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

	[Export] Label TileData;

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
	//TODO: Change the resourceNode struct to include different types of resources.
	struct ResourceNode { public int xPos; public int yPos; public bool activated; public bool worked;
		public ResourceNode(int x, int y, bool active, bool working)
		{
			xPos = x; yPos = y; activated = active; worked = working;
		}
	};
	ArrayList resourceNodes = new ArrayList();

	private Vector2I currCell = new Vector2I();

	public List<BuildingTemplate> buildingList = new List<BuildingTemplate>();

	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Here we would set our inital values for our resources, based on difficulty selected.

		//Also populate our list of resource nodes with all current resource nodes
		Vector2I resourceAtlasLocation = new Vector2I(4, 0);
		Godot.Collections.Array<Vector2I> resourceArray = regionMap.GetUsedCellsById(1, 0, resourceAtlasLocation);

		foreach(Vector2I cell in resourceArray)
		{
			ResourceNode currNode = new ResourceNode(cell.X, cell.Y, false, false);
			resourceNodes.Add(currNode);
		}

		//Create Building directory
		createBuildingList();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//TODO: Finish Logic
		//Check for enabled resource nodes and then check to see if they are being worked
		foreach(ResourceNode node in resourceNodes)
		{
			if(node.activated == true && node.worked)
			{
				// Update according resource
			}
		}
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
			case "Copper":
				Copper.Text = value.ToString();
				break;
			case "steel":
				Steel.Text = value.ToString();
				break;
			case "fuel":
				Fuel.Text = value.ToString();
				break;
			case "food":
				Food.Text = value.ToString();
				break;
			case "water":
				Water.Text = value.ToString();
				break;
			case "weapon":
				Weapons.Text = value.ToString();
				break;
			case "leisure":
				Leisure.Text = value.ToString();
				break;
			case "totalPop":
				TotalPop.Text = value.ToString();
				break;
			case "employedPop":
				EmployedPop.Text = value.ToString();
				break;
			case "growth":
				Growth.Text = value.ToString();
				break;
		}
	}

	//Update tile data label
	public void UpdateTileData(string data, Vector2I cellCoords)
	{
		TileData.Text = data;
		SetCurrCell(cellCoords);
	}

	//Change resource node data
	public void UpdateResourceNode(int xPos, int yPos, String updatePrefix, bool data)
	{
		for(int i = 0; i < resourceNodes.Count; i++)
		{
			ResourceNode node = (ResourceNode) resourceNodes[i];
			if (node.xPos == xPos)
			{
				if (node.yPos == yPos)
				{
					//Updatedata will have 2 prefixes, A- and W- meaning active and worked
					if (updatePrefix.StartsWith("A-"))
					{
						node.activated = data;
					} else if (updatePrefix.StartsWith("W-"))
					{
						node.worked = data;
					}
				}
			}
		}
	}



	// Current cell getter and setter
	public Vector2I GetCurrCell()
	{
		return currCell;
	}

	public void SetCurrCell(Vector2I newCoords)
	{
		currCell = newCoords;
	}

	private void createBuildingList()
	{
		//Add list of buildings by tech

		//Tech 0
		BuildingTemplate Farm = new BuildingTemplate("Farm", "Gives a set amount of food, varied on placement");
		BuildingTemplate Pasture = new BuildingTemplate("Pasture", "Gives a set amount of food, varied on placement");
		BuildingTemplate Lumbermill = new BuildingTemplate("Lumbermill", "Produces a set amount of lumber");
		BuildingTemplate Mine = new BuildingTemplate("Mine", "Produces a set amount of either stone or ore, depending on placement");
		BuildingTemplate Quarry = new BuildingTemplate("Quarry", "Produces a set amount of stone");
		BuildingTemplate Library = new BuildingTemplate("Library", "The first building that produces science and entertainment");

		buildingList.Add(Farm);
		buildingList.Add(Pasture);
		buildingList.Add(Lumbermill);
		buildingList.Add(Mine);	
		buildingList.Add(Quarry);
		buildingList.Add(Library);

		//Tech 1

		//Tech 2

		//Tech 3
	}
}
