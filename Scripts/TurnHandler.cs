using Godot;
using System;
using System.Collections.Generic;

public partial class TurnHandler : Button
{
    Main main;
    BuildItemList buildItemList;
    Dictionary<Vector2I, BuildingTemplate> placedBuildingList;
    int mainFoodVal;
    int mainTotalPopVal;
    double mainGrowthThresholdVal;
    List<Main.ResourceNode> resourceNodes;
    GovernmentTemplate currGov;
    public List<PartyTemplate> parties;

    // Get all needed values and such
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        main = GetNode<Main>("/root/Main");
        buildItemList = GetNode<BuildItemList>("/root/Main/UI/Panels/BuildPanel/BuildItemList");
        placedBuildingList = buildItemList.placedBuildingList;
        mainFoodVal = main.FoodVal;
        mainTotalPopVal= main.TotalPopVal;
        mainGrowthThresholdVal= main.GrowthThresholdVal;
        resourceNodes = main.resourceNodes;
        currGov = main.currGov;
        parties = main.parties;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }

    public void pressedTurnButton()
    {
        //Update local vals
        mainFoodVal = main.FoodVal;
        mainTotalPopVal = main.TotalPopVal;
        mainGrowthThresholdVal = main.GrowthThresholdVal;
        placedBuildingList = buildItemList.placedBuildingList;
        resourceNodes = main.resourceNodes;
        currGov = main.currGov;
        parties = main.parties;

        main.UpdateLabel("turn", 1);// Increase turn counter by 1
        // Next update resource values, including growth (Handle calculating growth in seperate function)
        int totalWoodBonus = 0;
        int totalStoneBonus = 0;
        int totalCopperBonus = 0;
        int totalSteelBonus = 0;
        int totalFuelBonus = 0;
        int totalFoodBonus = 0; // Used in growth calculations
        int totalWaterBonus = 0;
        int totalLeisure = 0;
        double turnsToGrow;
        int popGrowth = 0;
        int totalfarmNum = 0;
        int duelDeathChance = 0;
        int totalWeaponBonus = 0;
        int totalResearchBonus = 0;

        foreach (var item in placedBuildingList) //Iterate through placed buildings, totaling bonuses. (Check if building is active before adding)
        {
            BuildingTemplate temp = item.Value;
            if (temp.Active == true)
            {
                Dictionary<int, int> bonusDict = temp.Bonus;
                foreach (var bonus in bonusDict)
                {
                    switch (bonus.Key)
                    {
                        case 0:
                            totalWoodBonus += bonus.Value;

                            break;
                        case 1:
                            totalStoneBonus += bonus.Value;

                            break;
                        case 2:
                            totalCopperBonus += bonus.Value;

                            break;
                        case 3:
                            totalSteelBonus += bonus.Value;

                            break;
                        case 4:
                            totalFuelBonus += bonus.Value;

                            break;
                        case 5:
                            totalFoodBonus += bonus.Value;
                            if (temp.Name1 == "Farm")
                            {
                                totalfarmNum++;
                            }
                            break;
                        case 6:
                            totalWaterBonus += bonus.Value;

                            break;
                        case 8:
                            totalLeisure += bonus.Value;

                            break;
                        case 9:
                            totalResearchBonus += bonus.Value;

                            break;
                    }
                }
            }
        }
        // Add bonuses for working resource nodes
        for(int i = 0; i < resourceNodes.Count; i++)
        {
            Main.ResourceNode res = resourceNodes[i];
            if(res.worked && res.activated)
            {
                switch (res.type)
                {
                    case "Wood":
                        totalWoodBonus += 2;
                        break;
                    case "Stone":
                        totalStoneBonus += 2;
                        break;
                    case "Copper":
                        totalCopperBonus += 2;
                        break;
                    case "Steel":
                        totalSteelBonus += 2;
                        break;  
                }
            }
        }

        //Handle Government bonuses/costs
        if (currGov.GovName == "monarchy")
        {
            //Goto function for calculating monarchy bonuses/costs
            if (totalFoodBonus > 0)
            {
                totalLeisure += 7; // Base is 5, 2 is the bonus from monarchy
            }
            totalStoneBonus *= 2;
            totalWoodBonus *= 2;
            totalCopperBonus *= 2;
        } else if (currGov.GovName == "oligarchy")
        {
            //Goto function for calculating oligarchy bonuses/costs
            totalStoneBonus *= 3;
            totalWoodBonus *= 3;
            totalCopperBonus *= 3;
            totalLeisure += 9;
            totalFuelBonus -= 3;
        }

        //Handle Law Bonuses/costs
        List<LawTemplate> enactedLaws = main.enactedLaws;
        for (int i = 0; i < enactedLaws.Count; i++)
        {
            switch (enactedLaws[i].LawID)
            {
                case 0:

                    break;
                case 1:
                    totalWoodBonus *= 2;
                    totalStoneBonus *= 2;
                    totalSteelBonus *= 2;
                    totalCopperBonus *= 2;

                    totalLeisure -= 2;
                    totalFoodBonus -= 2;
                    break;
                case 2:
                    totalFuelBonus += 2;
                    totalLeisure -= 1;

                    break;
                case 3:
                    totalFoodBonus += 2;
                    totalWaterBonus -= 1;
                    break;
                case 4:
                    totalWeaponBonus -= 1;
                    totalLeisure += 2;
                    duelDeathChance = 5; // Duels give 5 percent chance of pop death
                    //Now check if pop dies
                    Random rng = new Random();
                    int roll = rng.Next(101);
                    if (roll < duelDeathChance)
                    {
                        popGrowth--;
                        GD.Print("Pop dies from duel.");
                    }
                    break;

            }
        }

        // If more food than current needs increase growth, needs are calculated by current pop and stored food
        double neededFood = mainTotalPopVal * 0.2;
        if ((totalFoodBonus - neededFood) > 0)
        {
            if (mainGrowthThresholdVal > mainFoodVal)
            {
                //Calculate new amount of turns
                turnsToGrow = (mainGrowthThresholdVal - mainFoodVal) / (totalFoodBonus - neededFood);
                //GD.Print("Turns to grow: " + turnsToGrow);
                main.UpdateLabel("growth", Convert.ToInt32(turnsToGrow));
            }
            else
            {
                //Add one more pop and increase growth threshold
                //GD.Print("Growing pop.");
                popGrowth++;
                mainGrowthThresholdVal += 2 * mainTotalPopVal;
                main.GrowthThresholdVal = mainGrowthThresholdVal;
                //GD.Print("New growth threshold: " + mainGrowthThresholdVal);
                main.UpdateLabel("totalPop", popGrowth);
                //Calculate new amount of turns
                turnsToGrow = (mainGrowthThresholdVal - mainFoodVal) / (totalFoodBonus - neededFood);
                main.UpdateLabel("growth", Convert.ToInt32(turnsToGrow));
            }
        }
        else
        {
            GD.Print("Not enough food to grow. Needed food: " + neededFood + "|Provided Food: " + totalFoodBonus);
            // Put starvation logic here
            if (mainFoodVal <= 0)
            {
                popGrowth--;
                mainFoodVal = Convert.ToInt32(mainGrowthThresholdVal);
                GD.Print("Pop died due to starvation.");
            }
        }
        // Pops now eat the food they need
        totalFoodBonus -= Convert.ToInt32(neededFood);

        //Handle party calculations
        foreach (PartyTemplate party in parties)
        {
            if (party.PartyName == "Survivor's Nexus")
            {
                //Check if needs are met
                bool foodProd = (totalFoodBonus - neededFood) > 0;
                bool employment = main.EmployedPopVal == main.TotalPopVal;
                if (foodProd && employment)
                {
                    GD.Print("Party " + party.PartyName + " need's met, giving bonuses.");
                    List<string> bonuses = new List<string>() { "+2 happiness", "+5 farm output" };
                    totalLeisure += 2;
                    int totalFarmBonus = totalfarmNum * 5;
                    totalFoodBonus += totalFarmBonus;

                    //Update status to happy to match needs being met
                    party.Aproval = "Happy";
                    party.Bonuses = bonuses;
                } else
                {
                    //If needs are not met, change status and then calculate costs
                    if (foodProd || employment)
                    {
                        party.Aproval = "Content";
                    } else
                    {
                        party.Aproval = "Unhappy";
                        totalFoodBonus -= party.PartySize;
                        List<string> bonuses = new List<string>() { "-" + party.PartySize + " food per turn." };
                        party.Bonuses = bonuses;
                    }
                }
            }
        }
        //Now update main list with updated values.
        main.parties = parties;

        //Update values with bonuses
        main.UpdateLabel("Wood", totalWoodBonus);
        main.UpdateLabel("Stone", totalStoneBonus);
        main.UpdateLabel("Copper", totalCopperBonus);
        main.UpdateLabel("steel", totalSteelBonus);
        main.UpdateLabel("fuel", totalFuelBonus);
        main.UpdateLabel("food", totalFoodBonus);
        main.UpdateLabel("water", totalWaterBonus);
        main.UpdateLabel("leisure", totalLeisure);
        main.UpdateLabel("weapon", totalWeaponBonus);
        main.ResearchVal += totalResearchBonus;

        if (main.ResearchVal > 100 && main.ResearchVal < 200)
        {
            main.Tier = 1;
            main.createBuildingList();
        } else if (main.ResearchVal > 200)
        {
            main.Tier = 2;
            main.createBuildingList();
        }
    }

    

}
