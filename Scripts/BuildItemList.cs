using Godot;
using System;
using System.Collections.Generic;


public partial class BuildItemList : ItemList
{
	Main main;
	Vector2I currentCell;
	List<BuildingTemplate> buildingList;
	int currentCellTerrainID;
	TileMap map;
	Vector2I grassCell = new Vector2I(0,0);
	Vector2I mountainCell = new Vector2I(1,0);
	Vector2I riverCell = new Vector2I(2,0);
	Vector2I plainsCell = new Vector2I(3,0);

	Vector2I resourceCell = new Vector2I(4,0);
	bool listPopulated;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get root node
		main = GetNode<Main>("/root/Main");
		map = GetNode<TileMap>("/root/Main/Map/TileMap");
		buildingList = main.buildingList;
		listPopulated = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (currentCell != main.GetCurrCell())
		{
			listPopulated = false;
			Clear();
		}
		currentCell = main.GetCurrCell();
		if(GetParentControl() != null && GetParentControl().Visible)
		{
			if (!listPopulated)
			{
				//First check what terrain type the cell is
				if (map.GetCellAtlasCoords(0, currentCell) == grassCell)
				{
					foreach (BuildingTemplate building in buildingList)
					{
						List<int> validterrain = building.ValidTerrainID;
						if (validterrain.Contains(grassCell.X))
						{
							//TODO: Add icons
							AddItem(building.Name1, null, true);
						}
					}
				}
				else if (map.GetCellAtlasCoords(1, currentCell) == mountainCell)
				{
					foreach (BuildingTemplate building in buildingList)
					{
						List<int> validterrain = building.ValidTerrainID;
						if (validterrain.Contains(mountainCell.X))
						{
							//TODO: Add icons
							AddItem(building.Name1, null, true);
						}
					}
				}
				else if (map.GetCellAtlasCoords(1, currentCell) == riverCell)
				{
					foreach (BuildingTemplate building in buildingList)
					{
						List<int> validterrain = building.ValidTerrainID;
						if (validterrain.Contains(riverCell.X))
						{
							//TODO: Add icons
							AddItem(building.Name1, null, true);
						}
					}
				}
				else if (map.GetCellAtlasCoords(1, currentCell) == plainsCell)
				{
					foreach (BuildingTemplate building in buildingList)
					{
						List<int> validterrain = building.ValidTerrainID;
						if (validterrain.Contains(plainsCell.X))
						{
							//TODO: Add icons
							AddItem(building.Name1, null, true);
						}
					}
				}//Then check if there is a resource on that cell as well

				if (map.GetCellAtlasCoords(1, currentCell) == resourceCell)
				{

				}
				listPopulated = true;
			}
		}
	}
}
