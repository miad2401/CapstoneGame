using Godot;
using System;

public partial class TopBarHandler : TabBar
{
	[Export] Panel GovernmentPanel;
	[Export] Panel LawPanel;
	[Export] Panel ResearchPanel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (CurrentTab)
		{
			case 0:
				GovernmentPanel.Visible = false;
				LawPanel.Visible = false;
				ResearchPanel.Visible = false;
				break;
			case 1:
				GovernmentPanel.Visible = true;
				LawPanel.Visible = false;
				ResearchPanel.Visible = false;
				break;
			case 2:
				GovernmentPanel.Visible = false;
				LawPanel.Visible = true;
				ResearchPanel.Visible = false;
				break;
			case 3:
				GovernmentPanel.Visible = false;
				LawPanel.Visible = false;
				ResearchPanel.Visible = true;
				break;
		}
	}
}
