using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

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

		//Path of tech 0 buildings
		Dictionary<string, Object> buildingsT0 = readBuildingListFile("res://Assets/data/buildings_tier0.xml");

		//Parse dictionary and then create building objects from said dictionary
		createBuildings(buildingsT0);

		//Tech 0
		List<int> genericValidTerrain = new List<int>();
		genericValidTerrain.Add(0);

		//Resource Costs for the farm
		ArrayList farmCosts = new ArrayList();
		farmCosts.Add(10);//10 wood
		farmCosts.Add(10);//10 stone



		//BuildingTemplate Farm = new BuildingTemplate("Farm", "Gives a set amount of food, varied on placement", false, 2, "Gatherer", 6, 0, genericValidTerrain, 2, farmCosts);
		BuildingTemplate Pasture = new BuildingTemplate("Pasture", "Gives a set amount of food, varied on placement");
		BuildingTemplate Lumbermill = new BuildingTemplate("Lumbermill", "Produces a set amount of lumber");
		BuildingTemplate Mine = new BuildingTemplate("Mine", "Produces a set amount of either stone or ore, depending on placement");
		BuildingTemplate Quarry = new BuildingTemplate("Quarry", "Produces a set amount of stone");
		BuildingTemplate Library = new BuildingTemplate("Library", "The first building that produces science and entertainment");

		//buildingList.Add(Farm);
		buildingList.Add(Pasture);
		buildingList.Add(Lumbermill);
		buildingList.Add(Mine);	
		buildingList.Add(Quarry);
		buildingList.Add(Library);

		//Tech 1

		//Tech 2

		//Tech 3
	}

	private void createBuildings(Dictionary<string, Object> buildingDict)
	{
		string name;
		string description;
		int jobs;
		string type;
		int resource;
		int techUnlock;
		List<int> validTerrain;
		ArrayList bonus;
		ArrayList costs;

		//Get list of buildings as a list
		List <Object> buildingList= (List<Object>)buildingDict["children"];
		
		//Iterate through all buildings
		for(int i = 0; i < buildingList.Count; i++)
		{
            foreach (KeyValuePair<string, Object> ele in (Dictionary<string, Object>)buildingList[i])
            {
				//Go further if there is a building to parse
				if (ele.Key == "children")
				{
                    List<Object> building = (List<Object>)ele.Value;
					for (int j = 0; j < building.Count; j++)
					{
						Dictionary<string, Object> buildingEles = (Dictionary<string, Object>)building[j];
						List<Object> buildingEle = (List<Object>)buildingEles["children"];
						switch (buildingEles["name"])
						{
							case "name":
								name = (string)buildingEle[0];
								GD.Print(name);
								break;
							case "description":
								description = (string)buildingEle[0];
                                GD.Print(description); 
								break;
							case "jobs":
								string jobsString = (string)buildingEle[0];
								jobs = jobsString.ToInt();
								GD.Print(jobs);
								break;
							case "resources":

								break;
							case "techUnlock":
								string techUnlockString = (string)buildingEle[0];
								techUnlock = techUnlockString.ToInt();
								GD.Print(techUnlock);
								break;
							case "validTerrain":

								break;
							case "bonuses":

								break;
							case "buildingCosts":

								break;
						}
					}
				}
                

            }
        }

		
	}

	private Dictionary<string, Object> readBuildingListFile(string filepath)
	{
		//Read file into plain text
		var file = FileAccess.Open(filepath, FileAccess.ModeFlags.Read);
		string XmlText = file.GetAsText();
		file.Close();

		if (XmlText == null) 
		{
			GD.Print("Failed to load building file.");
		}
		//Create xml document from plain text
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(XmlText);

		XmlNode rootElement = xmlDoc.DocumentElement;
		if (rootElement == null)
		{
			GD.Print("Failed to get root element of building T0 file.");
			return null;
		}
		
		Dictionary<string, Object> xmlDict = parseXmlElement(rootElement);
		return xmlDict;

	}

	private Dictionary<string, Object> parseXmlElement(XmlNode xmlNode)
	{
        Dictionary<string, object> resultDict = new Dictionary<string, object>();
        resultDict["name"] = xmlNode.Name;

        //Parse child elements recursively
        if (xmlNode.HasChildNodes)
        {
            List<object> childrenList = new List<object>();
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
					childrenList.Add(parseXmlElement(node));

                } else if (node.NodeType == XmlNodeType.Text)
				{
					childrenList.Add(node.InnerText);
				}
            }
			if (childrenList.Count > 0)
			{
				resultDict["children"] = childrenList;
			}
        }
		return resultDict;
    }
}
