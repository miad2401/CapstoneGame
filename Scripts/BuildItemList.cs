using Godot;
using System;
using System.Collections;
using System.Collections.Generic;


public partial class BuildItemList : ItemList
{
	Main main;
	Vector2I currentCell;
	List<BuildingTemplate> buildingList;
	Vector2I farmCell = new Vector2I(0, 4);
	Vector2I pastureCell = new Vector2I(1, 4);
    Vector2I lumberCell = new Vector2I(2, 4);
    Vector2I mineCell = new Vector2I(3, 4);
    Vector2I quarryCell = new Vector2I(4, 4);
    Vector2I libraryCell = new Vector2I(5, 4);

    int currentCellTerrainID;
	TileMap map;
	Vector2I grassCell = new Vector2I(0,0);
	Vector2I mountainCell = new Vector2I(1,0);
	Vector2I riverCell = new Vector2I(2,0);
	Vector2I plainsCell = new Vector2I(3,0);

	Vector2I woodCell = new Vector2I(0,2);
	Vector2I stoneCell = new Vector2I(1,2);
	Vector2I copperCell = new Vector2I(2,2);
	Vector2I steelCell = new Vector2I(3,2);
	Vector2I fuelCell = new Vector2I(4,2);
	Vector2I foodCell = new Vector2I(5,2);
	Vector2I waterCell = new Vector2I(6,2);
	Vector2I weaponsCell = new Vector2I(7,2);
	Vector2I leisureCell = new Vector2I(8,2);

	bool listPopulated;

	int currBuildingSelected;

	public Dictionary<Vector2I, BuildingTemplate> placedBuildingList; // Key = tile coords, val = building
	List<BuildingTemplate> currentValidBuildingList;
	
	BuildingTemplate pickedBuilding;
	[Export] Button confirmButton;

	List<Main.ResourceNode> resourceNodes;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		//Get root node
		main = GetNode<Main>("/root/Main");
		map = GetNode<TileMap>("/root/Main/Map/TileMap");
		buildingList = main.buildingList;
		listPopulated = false;
		placedBuildingList = new Dictionary<Vector2I, BuildingTemplate>();
		currentValidBuildingList= new List<BuildingTemplate>();
		resourceNodes = main.resourceNodes;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(buildingList.Count < 1)
		{
			buildingList = main.buildingList;
		}
		if (currentCell != main.GetCurrCell())
		{
			listPopulated = false;
			Clear();
		}
		currentCell = main.GetCurrCell();
		Vector2I currAtlasCoords = map.GetCellAtlasCoords(0, currentCell);
		Vector2I currAtlasResourceCoords = map.GetCellAtlasCoords(1, currentCell);

		if (GetParentControl() != null && GetParentControl().Visible)
		{
			if (!listPopulated)
			{
				//First check what terrain type the cell is
				if (currAtlasCoords == grassCell)
				{
					PopulateListForTerrain(grassCell);
				}
				else if (currAtlasCoords == mountainCell)
				{
					PopulateListForTerrain(mountainCell);
				}
				else if (currAtlasCoords == riverCell)
				{
					PopulateListForTerrain(riverCell);
				}
				else if (currAtlasCoords == plainsCell)
				{
					PopulateListForTerrain(plainsCell);
				}//Then check if there is a resource on that cell as well

				//TODO: Add resource checking
/*				if (currAtlasResourceCoords == new Vector2I(-1, -1))
				{

				}*/
				listPopulated = true;
			}
		}
	}

	private void PopulateListForTerrain(Vector2I atlasCoords)
	{
        buildingList = main.buildingList;
		currentValidBuildingList.Clear();

        foreach (BuildingTemplate building in buildingList)
		{
			List<int> validterrain = building.ValidTerrainID;
			if (validterrain.Contains(atlasCoords.X))
			{
				//TODO: Add icons
				AddItem(building.ToString(), null, true);
				//Add item to valid building list
				currentValidBuildingList.Add(building);
			}
		}
	}

	private void onItemSelected(int idx)
	{
		GD.Print("Build Item Selected: " + idx);
		currBuildingSelected = idx;
		confirmButton.Disabled = false; //Enable the confirm button
		pickedBuilding = currentValidBuildingList[currBuildingSelected];
	}

	private void confirmButtonPressed()
	{
		//Get current tile, then place a building on it
		placedBuildingList.Add(currentCell, pickedBuilding);
		if (pickedBuilding.Name1 == "Farm")
		{
			GD.Print("Farm placed @ tile: " + currentCell.ToString());
            map.SetCell(2, currentCell, 0, farmCell);
        } else if (pickedBuilding.Name1 == "Pasture")
		{
            GD.Print("Pasture placed @ tile: " + currentCell.ToString());
            map.SetCell(2, currentCell, 0, pastureCell);
        }
        else if (pickedBuilding.Name1 == "Lumbermill")
        {
            GD.Print("Lumbermill placed @ tile: " + currentCell.ToString());
            map.SetCell(2, currentCell, 0, lumberCell);

        }
        else if (pickedBuilding.Name1 == "Mine")
        {
            GD.Print("Mine placed @ tile: " + currentCell.ToString());
            map.SetCell(2, currentCell, 0, mineCell);
        }
        else if (pickedBuilding.Name1 == "Quarry")
        {
            GD.Print("Quarry placed @ tile: " + currentCell.ToString());
            map.SetCell(2, currentCell, 0, quarryCell);
        }
        else if (pickedBuilding.Name1 == "Library")
        {
            GD.Print("Library placed @ tile: " + currentCell.ToString());
            map.SetCell(2, currentCell, 0, libraryCell);
        }
		//Now check if building is placed over resource tile and set the tile to worked.
		for(int i = 0; i < resourceNodes.Count; i++)
		{
			Main.ResourceNode res = resourceNodes[i];
			if (res.yPos == currentCell.Y && res.xPos == currentCell.X)
			{
				GD.Print("Resource node is now being worked.");
                res.activated = true;
				res.worked = true;
			}
			resourceNodes[i] = res;
		}
		//Update the main class's list of resource nodes
		main.resourceNodes = resourceNodes;

		//Now subtract resources
		Dictionary<int, int> buildingCosts = pickedBuilding.Costs;
		
		if (buildingCosts.ContainsKey(0)) // Wood
		{
			main.UpdateLabel("Wood", -buildingCosts[0]);
		}
		if (buildingCosts.ContainsKey(1)) // Stone
		{
            main.UpdateLabel("Stone", -buildingCosts[1]);
        }
        if (buildingCosts.ContainsKey(2)) // Copper
        {
            main.UpdateLabel("Copper", -buildingCosts[2]);
        }
        if (buildingCosts.ContainsKey(3)) // Steel
        {
            main.UpdateLabel("steel", -buildingCosts[3]);
        }

		//Update Jobs
		main.UpdateLabel("employedPop", pickedBuilding.Jobs);
		pickedBuilding.Active = true;
    }
}
