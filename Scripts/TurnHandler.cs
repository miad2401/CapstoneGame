using Godot;
using System;
using System.Collections.Generic;

public partial class TurnHandler : Button
{

    Main main;
    BuildItemList buildItemList;
    Dictionary<Vector2I, BuildingTemplate> placedBuildingList;

    // Get all needed values and such
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        main = GetNode<Main>("/root/Main");
        buildItemList = GetNode<BuildItemList>("/root/Main/UI/Panels/BuildPanel/BuildItemList");
        placedBuildingList = buildItemList.placedBuildingList;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void pressedTurnButton()
    {
        main.UpdateLabel("turn", 1);// Increase turn counter by 1
        // Next update resource values, including growth (Handle calculating growth in seperate function)
        int totalWoodBonus;
        int totalStoneBonus;
        int totalCopperBonus;
        int totalSteelBonus;
        int totalFuelBonus;
        int totalFoodBonus; // Used in growth calculations
        int totalWaterBonus;
        foreach (var item in placedBuildingList) //Iterate through placed buildings, totaling bonuses. (Check if building is active before adding)
        {
            // TODO: Finish bonus calculations
        }
    }

    // TODO: Finish growth calculation function
}
