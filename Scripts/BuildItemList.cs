using Godot;
using System;


public partial class BuildItemList : ItemList
{



    Main main;
    Vector2I currentCell;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //Get root node
        main = GetNode<Main>("/root/Main");
        
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        currentCell = main.GetCurrCell();
        if(GetParentControl() != null && GetParentControl().Visible)
        {
            //First check what terrain type the cell is

            //Then check if there is a resource on that cell as well


        }
	}
}
