using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
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

	[Export] Label Turn;

    //Values
    private int woodVal;
    private int stoneVal;
    private int copperVal;
    private int steelVal;
    private int fuelVal;
    private int foodVal;
    private int waterVal;
    private int weaponVal;
    private int leisureVal;
    private int totalPopVal;
    private int employedPopVal;
    private double growthVal;
    private double growthThresholdVal;
    private int turnVal;
	private int researchVal;

    [Export] TileMap regionMap;
	public struct ResourceNode { public int xPos; public int yPos; public bool activated; public bool worked; public string type;
		public ResourceNode(int x, int y, bool active, bool working, string resType)
		{
			xPos = x; yPos = y; activated = active; worked = working; type = resType;
		}
	};
	public List<ResourceNode> resourceNodes = new List<ResourceNode>();

	private Vector2I currCell = new Vector2I();

	public List<BuildingTemplate> buildingList = new List<BuildingTemplate>();

	public List<GovernmentTemplate> govs = new List<GovernmentTemplate>();
	public GovernmentTemplate currGov;

	public List<PartyTemplate> parties = new List<PartyTemplate>();
	public List<LawTemplate> availableLaws;
	public List<LawTemplate> enactedLaws;

    public int WoodVal { get => woodVal; set => woodVal = value; }
    public int StoneVal { get => stoneVal; set => stoneVal = value; }
    public int CopperVal { get => copperVal; set => copperVal = value; }
    public int SteelVal { get => steelVal; set => steelVal = value; }
    public int FuelVal { get => fuelVal; set => fuelVal = value; }
    public int FoodVal { get => foodVal; set => foodVal = value; }
    public int WaterVal { get => waterVal; set => waterVal = value; }
    public int WeaponVal { get => weaponVal; set => weaponVal = value; }
    public int LeisureVal { get => leisureVal; set => leisureVal = value; }
    public int TotalPopVal { get => totalPopVal; set => totalPopVal = value; }
    public int EmployedPopVal { get => employedPopVal; set => employedPopVal = value; }
    public double GrowthVal { get => growthVal; set => growthVal = value; }
    public double GrowthThresholdVal { get => growthThresholdVal; set => growthThresholdVal = value; }
    public int TurnVal { get => turnVal; set => turnVal = value; }
	public int ResearchVal { get => researchVal; set => researchVal = value; }

    #endregion

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		//Here we would set our inital values for our resources, based on difficulty selected.
		woodVal = 40;
		stoneVal = 40;
		steelVal = 20;
		fuelVal = 20;
		foodVal = 20;
		waterVal = 100;
		weaponVal = 10;
		leisureVal = 5;
		totalPopVal = 20;
		employedPopVal = 0;
		growthVal = 5;
		growthThresholdVal = 40;
		turnVal = 1;
		researchVal = 0;

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

		//Create government list
		GovernmentTemplate monarchy = new GovernmentTemplate("monarchy", (Texture2D)GD.Load("res://Assets/Art/MonarchyIcon.png"), new List<string>() {"-Increased happiness when food production is positive", "-Increased resource output" }, new List<string>());
		govs.Add(monarchy);
		currGov = monarchy;
		GD.Print("Government added: " + monarchy.GovName);

		GovernmentTemplate oligarchy = new GovernmentTemplate("oligarchy", (Texture2D)GD.Load("res://Assets/Art/OligarchyIcon.png"), new List<string>() {"-Large increase to resource output", "-Increased happiness due to abundance of resources", "-Slight decrease in fuel per turn"}, new List<string>());
		govs.Add(oligarchy);
        GD.Print("Government added: " + oligarchy.GovName);

		//Create party list
		PartyTemplate survivors = new PartyTemplate("Survivor's Nexus", 10, new List<string>() {"Stable food production", "Full employment"}, "Content", new List<string>() {});
		parties.Add(survivors);
		GD.Print("Party added: " + survivors.PartyName);

		//Add Laws
		availableLaws = new List<LawTemplate>();
		enactedLaws = new List<LawTemplate>();
		LawTemplate law1 = new LawTemplate("No rest for the weary", "Requires all population to work twice as hard, at the cost of happiness and extra food consumption", 1);
		LawTemplate law2 = new LawTemplate("Fuel Rations", "Ration out fuel, lessening fuel consumption but costing happiness", 2);
		LawTemplate law3 = new LawTemplate("Food Rations", "Ration out food, lessening food consumption but costing water", 3);
		LawTemplate law4 = new LawTemplate("Public duels", "Allow dueling, distributing weapons and increasing happiness but has a chance to lose a pop", 4);
		availableLaws.Add(law1);
		availableLaws.Add(law2);
		availableLaws.Add(law3);
		availableLaws.Add(law4);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	//Change Label Data
	public void UpdateLabel(String labelName, int value)
	{
		switch (labelName)
		{
			case null:
				break;
			case "Wood":
                woodVal += value;
                Wood.Text = "Wood: " + woodVal.ToString();
                break;
			case "Stone":
                stoneVal += value;
                Stone.Text = "Stone: " + stoneVal.ToString();
				
				break;
			case "Copper":
                copperVal += value;
                Copper.Text = "Copper: " + copperVal.ToString();
				
				break;
			case "steel":
                steelVal += value;
                Steel.Text = "Steel: " + steelVal.ToString();
				
				break;
			case "fuel":
                fuelVal += value;
                Fuel.Text = "Fuel: " + fuelVal.ToString();
				
				break;
			case "food":
                foodVal += value;
                Food.Text = "Food: " + foodVal.ToString();
				
				break;
			case "water":
                waterVal += value;
                Water.Text = "Water: " + waterVal.ToString();
				
				break;
			case "weapon":
                weaponVal += value;
                Weapons.Text = "Weapons: " + weaponVal.ToString();
				
				break;
			case "leisure":
                leisureVal = value;
                Leisure.Text = "Leisure: " + leisureVal.ToString();
				
				break;
			case "totalPop":
                totalPopVal += value;
                TotalPop.Text = "Total Population: " + totalPopVal.ToString();
				
				break;
			case "employedPop":
                employedPopVal += value;
                EmployedPop.Text = "Population Employed: " + employedPopVal.ToString();
				
				break;
			case "growth":
                growthVal = value;
                Growth.Text = "New pop in: " + growthVal.ToString();
				
				break;
			case "turn":
				turnVal += value;
				Turn.Text = "Turn: " + turnVal.ToString();
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

		//Path of tech 0 buildings
		Dictionary<string, Object> buildingsT0 = readBuildingListFile("res://Assets/data/buildings_tier0.xml");

		//Parse dictionary and then create building objects from said dictionary
		createBuildings(buildingsT0);
		//Tech 1

		//Tech 2

		//Tech 3
	}

	private void createBuildings(Dictionary<string, Object> buildingDict)
	{
		string name = "";
		string description = "";
		int jobs = 0;
		string type = "";
		int techUnlock = 0;
		List<int> validTerrain = new List<int>();
		Dictionary<int, int> bonus = new Dictionary<int, int>();
		Dictionary<int, int> costs = new Dictionary<int, int>();

		//Get list of buildings as a list
		List <Object> fileBuildingList= (List<Object>)buildingDict["children"];
		
		//Iterate through all buildings
		for(int i = 0; i < fileBuildingList.Count; i++)
		{
            foreach (KeyValuePair<string, Object> ele in (Dictionary<string, Object>)fileBuildingList[i])
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
								//GD.Print("Building name: " + name);
								break;
							case "description":
								description = (string)buildingEle[0];
                                //GD.Print("Building description: " + description); 
								break;
							case "jobs":
								string jobsString = (string)buildingEle[0];
								jobs = jobsString.ToInt();
								//GD.Print("Num of jobs: " + jobs);
								break;
							case "techUnlock":
								string techUnlockString = (string)buildingEle[0];
								techUnlock = techUnlockString.ToInt();
								//GD.Print("Tech unlock num: " + techUnlock);
								break;
							case "validTerrain":
								//iterate through subelement
								//reset list
								validTerrain.Clear();
								foreach(object t in buildingEle)
								{
                                    Dictionary<string, Object> buildingSubEles = (Dictionary<string, Object>)t;
                                    List<Object> buildingSubEle = (List<Object>)buildingSubEles["children"];
                                    foreach (object vt in buildingSubEle)
                                    {
                                        string vTerrain = (string)vt;
                                        validTerrain.Add(vTerrain.ToInt());
                                        //GD.Print("Valid terrain: " + vTerrain + " |Total valid terrain: " + validTerrain.Count);
                                    }
                                }
								break;
							case "bonuses":
								bonus.Clear();
								foreach(object b in buildingEle)
								{
									Dictionary<string, Object> buildingSubEles2 = (Dictionary<string, Object>)b;
									List<Object> bonusList = (List<Object>)buildingSubEles2["children"];
									int resourceId = 0;
									int numOfresource = 0;
									for (int k = 0; k < bonusList.Count; k++) //bb is a dictionary of either resource id or numOfResource
									{
										Dictionary<string, Object> bonusItem = (Dictionary<string, Object>)bonusList[k];
										if (k == 0) // 0 is resourceID, 1 is num
										{
											List<Object> bonusItemTxtL = (List<Object>)bonusItem["children"];
											string bonusItemTxt = (string)bonusItemTxtL[0];
											resourceId = bonusItemTxt.ToInt();
                                        } else
										{
                                            List<Object> bonusItemTxtL = (List<Object>)bonusItem["children"];
                                            string bonusItemTxt = (string)bonusItemTxtL[0];
                                            numOfresource = bonusItemTxt.ToInt();
                                        }
									}
									bonus.Add(resourceId, numOfresource);
									//GD.Print("Bonuses: ID: " + resourceId + " | Num: " + numOfresource);
                                }
                                break;
							case "buildingCosts":
								costs.Clear();
                                foreach (object c in buildingEle)
                                {
                                    Dictionary<string, Object> buildingSubEles2 = (Dictionary<string, Object>)c;
                                    List<Object> costList = (List<Object>)buildingSubEles2["children"];
                                    int resourceId = 0;
                                    int numOfresource = 0;
                                    for (int k = 0; k < costList.Count; k++) //bb is a dictionary of either resource id or numOfResource
                                    {
                                        Dictionary<string, Object> costItem = (Dictionary<string, Object>)costList[k];
                                        if (k == 0) // 0 is resourceID, 1 is num
                                        {
                                            List<Object> costItemTxtL = (List<Object>)costItem["children"];
                                            string costItemTxt = (string)costItemTxtL[0];
                                            resourceId = costItemTxt.ToInt();
                                        }
                                        else
                                        {
                                            List<Object> costItemTxtL = (List<Object>)costItem["children"];
                                            string costItemTxt = (string)costItemTxtL[0];
                                            numOfresource = costItemTxt.ToInt();
                                        }
                                    }
                                    costs.Add(resourceId, numOfresource);
                                    //GD.Print("Costs: ID: " + resourceId + " | Num: " + numOfresource);
                                }
                                break;
							case "type":
								type = (string)buildingEle[0];
								//GD.Print("Building type: " + type);
								break;
						}
					}
                    BuildingTemplate buildingFromFile = new BuildingTemplate(name, description, false, jobs, type, techUnlock, new List<int>(validTerrain), new Dictionary<int, int>(bonus), new Dictionary<int, int>(costs));
                    buildingList.Add(buildingFromFile);
                    GD.Print("Building: " + name + " Added successfully.");
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
