using UnityEngine;

[CreateAssetMenu(menuName = "Dialugue/DialogueObect")]
public class DialogueObject : ScriptableObject
{
    [field: SerializeField]
    [field: TextArea]
    public string[] Dialogue { get; }
}
