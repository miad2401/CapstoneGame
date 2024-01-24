using Godot;
using System;

public partial class BuildButtonHandler : Button
{
	[Export] PanelContainer BuildPanel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent mevent)
	{
		if (mevent.GetType() == typeof(InputEventMouseButton) && mevent.IsPressed())
		{
			handleButtonPress();
		}
	}

	public void handleButtonPress()
	{
		BuildPanel.Visible = !BuildPanel.Visible;
		if(BuildPanel.Visible)
		{
			BuildPanel.MouseFilter = MouseFilterEnum.Ignore;
		} else
		{
			BuildPanel.MouseFilter = MouseFilterEnum.Pass;
		}
	}
}
