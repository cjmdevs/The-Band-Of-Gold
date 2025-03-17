using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;

    private void Start()
    {
        GetComponent<TypewriterEffect>().Run("The sunset is quite nice today, but currently, we can't enjoy it due to the many monsters that have been invading our village and pillaging us.\nIf only there was someone that could help us with this predicament.", textLabel);
    }
}
