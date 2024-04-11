using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class BuildingTemplate : Node
{
    private string name;
    private string description;
    private bool active;
    private int jobs;
    private string type;
    private int techUnlock;
    private List<int> validTerrainID;
    private Dictionary<int, int> bonus; //Resource id is key, number of resource is value
    private Dictionary<int, int> costs;

    /// <summary>
    /// Template for buildings
    /// </summary>
    /// <param name="name">Name of building</param>
    /// <param name="description">Short description of building</param>
    public BuildingTemplate(string name, string description)
    {
        this.name = name;
        this.description = description;
        this.active = false;
        this.jobs = 0;
        this.type = "General";
        this.techUnlock = 0;
        this.validTerrainID = new List<int>();
        this.bonus = new Dictionary<int, int>();
        this.costs = new Dictionary<int, int>();

        validTerrainID.Add(0);
    }

    /// <summary>
    /// Template for buildings
    /// </summary>
    /// <param name="name">Name of building</param>
    /// <param name="description">Short description of building</param>
    /// <param name="active">Building active state</param>
    /// <param name="jobs">Number of jobs</param>
    /// <param name="type">Type of building</param>
    /// <param name="resource">Type of resource</param>
    /// <param name="techUnlock">Unlocking tech ID</param>
    /// <param name="validTerrain">List of valid terrain types</param>
    /// <param name="bonus">Amount of bonus to resource</param>
    /// <param name="costs">List of costs to build</param>
    public BuildingTemplate(string name, string description, bool active, int jobs, string type, int techUnlock, List<int> validTerrain, Dictionary<int, int> bonus, Dictionary<int, int> costs)
    {
        this.name = name;
        this.description = description;
        this.active = active;
        this.jobs = jobs;
        this.type = type;
        this.techUnlock = techUnlock;
        this.validTerrainID = validTerrain;
        this.bonus = bonus;
        this.costs = costs;
    }

    public string Name1 { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Active { get => active; set => active = value; }
    public int Jobs { get => jobs; set => jobs = value; }
    public string Type { get => type; set => type = value; }
    public int TechUnlock { get => techUnlock; set => techUnlock = value; }
    public List<int> ValidTerrainID { get => validTerrainID; set => validTerrainID = value; }
    public Dictionary<int, int> Costs { get => costs; set => costs = value; }
    public Dictionary<int, int> Bonus { get => bonus; set => bonus = value; }

    public override string ToString()
    {
        return name + " \n" + description; 
    }
}
