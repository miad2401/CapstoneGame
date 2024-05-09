using Godot;
using System;

public partial class LawTemplate : Node
{
    private string lawName;
    private string lawDescription;
    private int lawID;

    /// <summary>
    /// Template for laws
    /// </summary>
    /// <param name="lawName">Name of the law</param>
    /// <param name="lawDescription">Description of what the law does</param>
    /// <param name="ID">ID of law</param>
    public LawTemplate(string lawName, string lawDescription, int ID) 
    {
        this.lawName = lawName;
        this.lawDescription = lawDescription;
        this.lawID = ID;
    }

    public string LawName { get => lawName; set => lawName = value; }
    public string LawDescription { get => lawDescription;set => lawDescription = value; }
    public int LawID { get => lawID;set => lawID = value; }
}
