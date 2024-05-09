using Godot;
using System;
using System.Collections.Generic;

public partial class GovernmentTemplate : Node
{
    private string govName;
    private Texture2D icon;
    private List<string> stats;
    private List<string> requirements;


    /// <summary>
    /// Template for goverment forms
    /// </summary>
    /// <param name="govName">Name</param>
    /// <param name="icon">Icon for gov</param>
    /// <param name="stats">List of abstract stats associated with the government</param>
    /// <param name="requirements">List of abstract requirements needed to unlock the government</param>
    public GovernmentTemplate(string govName, Texture2D icon, List<string> stats, List<string> requirements)
    {
        this.govName = govName;
        this.icon = icon;
        this.stats = stats;
        this.requirements = requirements;
    }

    public string GovName { get => govName; set => govName = value; }
    public Texture2D Icon { get => icon; set => icon = value; }
    public List<string> Stats { get => stats; set => stats = value; }
    public List<string> Requirements { get => requirements; set => requirements = value; }
}
