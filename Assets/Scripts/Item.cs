using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    private string _name = "unassigned";
    private string _description = "unassigned";

    public string Name => _name;

    public string Description => _description;
}
