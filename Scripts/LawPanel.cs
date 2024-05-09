using Godot;
using System;
using System.Collections.Generic;

public partial class LawPanel : PanelContainer
{
    [Export] Main main;
	ItemList availableLaws;
	ItemList enactedLaws;
	List<LawTemplate> availableLawsList;
	List<LawTemplate> enactedLawsList;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		availableLaws = GetNode<ItemList>("/root/Main/UI/Panels/LawPanel/VBoxContainer/HBoxContainer/VBoxContainer/availableLawList");
		enactedLaws = GetNode<ItemList>("/root/Main/UI/Panels/LawPanel/VBoxContainer/HBoxContainer/VBoxContainer2/pastLaws");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void visibilityChanged()
	{
		availableLawsList = main.availableLaws;
		enactedLawsList = main.availableLaws;
		if (Visible)
		{
            MouseFilter = MouseFilterEnum.Stop;
			UpdateAvailableLaws();
			UpdateEnactedLaws();
        } else
		{
            MouseFilter = MouseFilterEnum.Ignore;
        }
	}

	public void UpdateAvailableLaws()
	{
		availableLaws.Clear();
		availableLawsList = main.availableLaws;
		GD.Print("Num of available laws: " + availableLawsList.Count);
		foreach(LawTemplate law in availableLawsList)
		{
			string lawText = law.LawName + "|";
			lawText += law.LawDescription;
			availableLaws.AddItem(lawText);
		}
	}

	public void UpdateEnactedLaws()
	{
		enactedLaws.Clear();
		enactedLawsList = main.enactedLaws;
		GD.Print("Num of enacted laws: " + enactedLawsList.Count);
		foreach(LawTemplate law in enactedLawsList)
		{
			string lawText = law.LawName + "|";
			lawText += law.LawDescription;
			enactedLaws.AddItem(lawText);
		}
	}

	public void EnactLaws()
	{
		int[] selectedItemIdx =  availableLaws.GetSelectedItems();
		for (int i = 0; i < selectedItemIdx.Length; i++)
		{
			string lawText = availableLaws.GetItemText(selectedItemIdx[i]);
			enactedLaws.AddItem(lawText, selectable:false);
			LawTemplate law = availableLawsList[selectedItemIdx[i]];
			enactedLawsList.Add(law);
			availableLawsList.RemoveAt(selectedItemIdx[i]);
            availableLaws.RemoveItem(selectedItemIdx[i]);

			//Update lists in main now
			main.availableLaws = availableLawsList;
			main.enactedLaws = enactedLawsList;
        }
        UpdateAvailableLaws();
		UpdateEnactedLaws();
	}
}
