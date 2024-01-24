using Godot;
using System;

public partial class BuildingTemplate : Node
{
    private string name;
    private string description;
    private bool active;
    private int jobs;
    private string type;
    private int resource;
    private int techUnlock;

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
        this.resource = 0;
        this.techUnlock = 0;
    }

    /// <summary>
    /// Template for buildings
    /// </summary>
    /// <param name="name">Name of building</param>
    /// <param name="description">Short description of building</param>
    /// <param name="active">Building active state</param>
    /// <param name="jobs">Number of jobs</param>
    /// <param name="type">Type of building</param>
    /// <param name="resource">Resource value</param>
    /// <param name="techUnlock">Unlocking tech ID</param>
    public BuildingTemplate(string name, string description, bool active, int jobs, string type, int resource, int techUnlock)
    {
        this.name = name;
        this.description = description;
        this.active = active;
        this.jobs = jobs;
        this.type = type;
        this.resource = resource;
        this.techUnlock = techUnlock;
    }

    public string Name1 { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Active { get => active; set => active = value; }
    public int Jobs { get => jobs; set => jobs = value; }
    public string Type { get => type; set => type = value; }
    public int Resource { get => resource; set => resource = value; }
    public int TechUnlock { get => techUnlock; set => techUnlock = value; }
}
