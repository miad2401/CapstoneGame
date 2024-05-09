using Godot;
using System;

public partial class ResearchPanel : PanelContainer
{
    [Export] Main main;
	Label totalResearch;
	Label tierLabel;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		totalResearch = GetNode<Label>("/root/Main/UI/Panels/ResearchPanel/VBoxContainer/totalResearch");
		tierLabel = GetNode<Label>("/root/Main/UI/Panels/ResearchPanel/VBoxContainer/tierLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void visibilityChanged()
	{
		if (Visible)
		{
            MouseFilter = MouseFilterEnum.Stop;
			totalResearch.Text = "Total research: " + main.ResearchVal;
			tierLabel.Text = "Current Tier: " + main.Tier;
        } else
		{
            MouseFilter = MouseFilterEnum.Ignore;
        }
	}
}
