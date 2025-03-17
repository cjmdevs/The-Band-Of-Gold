using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialugue/DialogueObect")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] Dialogue;
}
