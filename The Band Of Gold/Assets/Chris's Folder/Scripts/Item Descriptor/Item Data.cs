using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite icon;
    [TextArea]
    public string description;
}