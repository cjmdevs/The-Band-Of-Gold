using UnityEngine;

[CreateAssetMenu(menuName = "Dialugue/DialogueObect")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;

    public string[] Dialogue => dialogue;
}
