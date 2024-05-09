using Godot;
using System;
using System.Collections.Generic;

public partial class PartyTemplate : Node
{
    private string partyName;
    private int partySize;
    private List<string> wants;
    private string aproval;
    private List<string> bonuses;

    /// <summary>
    /// Template for political parties
    /// </summary>
    /// <param name="partyName">Name of the Party</param>
    /// <param name="partySize">Pop size of the party</param>
    /// <param name="wants">List of wants for the party</param>
    /// <param name="aproval">Current party opinion of player</param>
    /// <param name="bonuses">List of bonuses</param>
    public PartyTemplate(string partyName, int partySize, List<string> wants, string aproval, List<string> bonuses) 
    { 
        this.PartyName = partyName;
        this.PartySize = partySize;
        this.Wants = wants;
        this.Aproval = aproval;
        this.Bonuses = bonuses;
    }

    public string PartyName { get => partyName; set => partyName = value; }
    public int PartySize { get => partySize; set => partySize = value; }
    public List<string> Wants { get => wants; set => wants = value; }
    public string Aproval { get => aproval; set => aproval = value; }
    public List<string> Bonuses { get => bonuses; set => bonuses = value; }
}
