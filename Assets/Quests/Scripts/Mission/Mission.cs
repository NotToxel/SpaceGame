using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    public string[] titles;
    
    [TextArea(3,10)]
    public string[] rewards;
}
