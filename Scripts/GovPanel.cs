using Godot;
using System;
using System.Collections.Generic;

public partial class GovPanel : PanelContainer
{
	[Export] Main main;

	List<GovernmentTemplate> govs = new List<GovernmentTemplate>();
	Label currGovLabel;
	Label GovBonusLabel;
	Label PartyDataLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{   
        //First get/make list of governments
        //In the future, read this from file like with buildings
        currGovLabel = GetNode<Label>("/root/Main/UI/Panels/GovPanel/HBoxContainer/GovernmentTypeContainer/BoxContainer/CurrGovLabel");
		GovBonusLabel = GetNode<Label>("/root/Main/UI/Panels/GovPanel/HBoxContainer/GovernmentTypeContainer/GovBonusLabel");
		PartyDataLabel = GetNode<Label>("/root/Main/UI/Panels/GovPanel/HBoxContainer/PartyContainer/VBoxContainer/PartyData");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void visibilityChanged()
	{
        govs = main.govs;
        
		if (Visible)
		{
			MouseFilter = MouseFilterEnum.Stop;
			updateGovText();
			updatePartyText();
		} else
		{
            MouseFilter = MouseFilterEnum.Ignore;
        }
	}

	public void changeGovernment()
	{
		//Change gov then update text
		GD.Print("Changing Government");
        govs = main.govs;

		for (int i = 1; i < govs.Count; i++)
		{
			if (govs[i - 1].GovName == main.currGov.GovName)
			{
				main.currGov = govs[i];
				break;
			}
		}
		updateGovText();
    }

	public void updateGovText()
	{
        string curGovText = "Current Government:\n";
        string govBonusText = "Stats:\n";

        foreach (GovernmentTemplate gov in govs)
        {
            if (gov.GovName == main.currGov.GovName)
            {
                curGovText += gov.GovName;
                for (int i = 0; i < gov.Stats.Count; i++)
                {
                    govBonusText += gov.Stats[i].ToString() + "\n";
                }
            }
        }
        currGovLabel.Text = curGovText;
        GovBonusLabel.Text = govBonusText;
    }

	public void updatePartyText()
	{
		string partyText = "";
		foreach (PartyTemplate party in main.parties)
		{
			partyText += party.PartyName + "\n";
			partyText += "\t - " + party.PartySize + " pop\n";
			partyText += "\t - ";
			for (int i = 0; i < party.Wants.Count; i++)
			{
				partyText += party.Wants[i] + ", ";
			}
			partyText += "\n";
			partyText += "\t - Current Status: " + party.Aproval + "\n";
			for (int j = 0; j < party.Bonuses.Count; j++)
			{
                partyText += "\t\t - " + party.Bonuses[j] + "\n";
            }
			
		}
		PartyDataLabel.Text = partyText;
	}
}