using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText; // Reference to the Text component
    public string[] dialogues; // Array of dialogues
    public bool isFinalDialogue = false;
    private int currentDialogueIndex = 0; // Current index in the dialogue array

    void Start()
    {
        if (dialogues.Length > 0)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            AdvanceDialogue();
        }
    }

    void AdvanceDialogue()
    {
        if (currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++;
            dialogueText.text = dialogues[currentDialogueIndex];
        }
        else
        {
            if (isFinalDialogue)
            {
                SceneManager.LoadScene("Scenes/Thanks for playing");
            }
            else
            {
                SceneManager.LoadScene("Scenes/Battlefield");
            }
        }
    }
}
