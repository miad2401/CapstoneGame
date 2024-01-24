using Godot;
using System;

public partial class TopBarHandler : TabBar
{
	[Export] PanelContainer GovernmentPanel;
	[Export] PanelContainer LawPanel;
	[Export] PanelContainer ResearchPanel;

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
				GovernmentPanel.MouseFilter = MouseFilterEnum.Ignore;
				LawPanel.MouseFilter = MouseFilterEnum.Ignore;
				ResearchPanel.MouseFilter = MouseFilterEnum.Ignore;
				break;
			case 1:
				GovernmentPanel.Visible = true;
				LawPanel.Visible = false;
				ResearchPanel.Visible = false;
				GovernmentPanel.MouseFilter = MouseFilterEnum.Stop;
				LawPanel.MouseFilter = MouseFilterEnum.Ignore;
				ResearchPanel.MouseFilter = MouseFilterEnum.Ignore;
				break;
			case 2:
				GovernmentPanel.Visible = false;
				LawPanel.Visible = true;
				ResearchPanel.Visible = false;
				GovernmentPanel.MouseFilter = MouseFilterEnum.Ignore;
				LawPanel.MouseFilter = MouseFilterEnum.Stop;
				ResearchPanel.MouseFilter = MouseFilterEnum.Ignore;
				break;
			case 3:
				GovernmentPanel.Visible = false;
				LawPanel.Visible = false;
				ResearchPanel.Visible = true;
				GovernmentPanel.MouseFilter = MouseFilterEnum.Ignore;
				LawPanel.MouseFilter = MouseFilterEnum.Ignore;
				ResearchPanel.MouseFilter = MouseFilterEnum.Stop;
				break;
		}
	}
}
