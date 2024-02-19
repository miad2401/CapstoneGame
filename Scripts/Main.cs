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
	[Export] Label TileDataResource;

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
	struct ResourceNode { public int xPos; public int yPos; public bool activated; public bool worked; public string type;
		public ResourceNode(int x, int y, bool active, bool working, string resType)
		{
			xPos = x; yPos = y; activated = active; worked = working; type = resType;
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
		Vector2I woodResource = new Vector2I(0, 2);
		Vector2I stoneResource = new Vector2I(1, 2);
		Vector2I copperResource = new Vector2I(2, 2);
		Vector2I steelResource = new Vector2I(3, 2);
		Godot.Collections.Array<Vector2I> resourceWoodArray = regionMap.GetUsedCellsById(1, 0, woodResource);
		Godot.Collections.Array<Vector2I> resourceStoneArray = regionMap.GetUsedCellsById(1, 0, stoneResource);
		Godot.Collections.Array<Vector2I> resourceCopperArray = regionMap.GetUsedCellsById(1, 0, copperResource);
		Godot.Collections.Array<Vector2I> resourceSteelArray = regionMap.GetUsedCellsById(1, 0, steelResource);

		foreach (Vector2I cell in resourceWoodArray)
		{
			ResourceNode currNode = new ResourceNode(cell.X, cell.Y, false, false, "Wood");
			resourceNodes.Add(currNode);
		}
		foreach (Vector2I cell in resourceStoneArray)
		{
			ResourceNode currNode = new ResourceNode(cell.X, cell.Y, false, false, "Stone");
			resourceNodes.Add(currNode);
		}
		foreach (Vector2I cell in resourceCopperArray)
		{
			ResourceNode currNode = new ResourceNode(cell.X, cell.Y, false, false, "Copper");
			resourceNodes.Add(currNode);
		}
		foreach (Vector2I cell in resourceSteelArray)
		{
			ResourceNode currNode = new ResourceNode(cell.X, cell.Y, false, false, "Steel");
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

	//Update tile resource data label
	public void UpdateTileResourceData(string data, Vector2I cellCoords)
	{
		TileDataResource.Text = data;
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
		//TODO: Change building list to read from building list file which would have all building data rather than hard code
		//TODO: Add rest of building data

		//TODO: Insert file path of tech 0 buildings
		readBuildingListFile("res://Assets/data/buildings_tier0.xml");

		//Tech 0
		List<int> genericValidTerrain = new List<int>();
		genericValidTerrain.Add(0);

		//Resource Costs for the farm
		ArrayList farmCosts = new ArrayList();
		farmCosts.Add(10);//10 wood
		farmCosts.Add(10);//10 stone



		BuildingTemplate Farm = new BuildingTemplate("Farm", "Gives a set amount of food, varied on placement", false, 2, "Gatherer", 6, 0, genericValidTerrain, 2, farmCosts);
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

	private void readBuildingListFile(string filepath)
	{
		/*var jsonAsText = FileAccess.GetFileAsString(filepath);

		var jsonAsDict = Json.ParseString(jsonAsText);
		GD.Print(jsonAsDict);*/

		var parser = new XmlParser();
		parser.Open(filepath);
		while (parser.Read() != Error.FileEof)
		{
			if (parser.GetNodeType() == XmlParser.NodeType.Element)
			{
				var nodeName = parser.GetNodeName();
				var attributesDict = new Godot.Collections.Dictionary();
				for (int idx = 0; idx < parser.GetAttributeCount(); idx++)
				{
					attributesDict[parser.GetAttributeName(idx)] = parser.GetAttributeValue(idx);
				}
				GD.Print($"The {nodeName} element has the following attributes: {attributesDict}");
			}
		}
	}
}
