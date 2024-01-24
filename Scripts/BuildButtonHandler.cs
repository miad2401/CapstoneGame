using Godot;
using System;

public partial class BuildButtonHandler : Button
{
	[Export] PanelContainer BuildPanel;
	bool isMouseOverButton = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Callable mouseEnter = new Callable(this, MethodName.OnMouseEntered);
		Callable mouseLeave = new Callable(this, MethodName.OnMouseExited);
		this.Connect("mouse_entered", mouseEnter);
		this.Connect("mouse_exited", mouseLeave);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent mevent)
	{
		if (mevent.GetType() == typeof(InputEventMouseButton) && mevent.IsPressed())
		{
			if (isMouseOverButton)
			{
				handleButtonPress();
			}
			
		}
	}

	public void handleButtonPress()
	{
		BuildPanel.Visible = !BuildPanel.Visible;
		if(BuildPanel.Visible)
		{
			BuildPanel.MouseFilter = MouseFilterEnum.Stop;
		} else
		{
			BuildPanel.MouseFilter = MouseFilterEnum.Ignore;
		}
	}

	public void OnMouseEntered()
	{
		isMouseOverButton = true;
	}

	public void OnMouseExited()
	{
		isMouseOverButton = false;
	}
}
