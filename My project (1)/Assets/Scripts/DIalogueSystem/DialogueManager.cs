using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{

    [Header("UI 요소 - 인스펙터 창에서 연결")]

    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defaltCharacterImage;

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    private DialogueDataSO currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyUp(KeyCode.Space))
        {
            HandleNextInput();
        }
    }
    IEnumerator TypeText(string textToTtpe)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < textToTtpe.Length; i++)
        {
            dialogueText.text += textToTtpe[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void CompleteTyping()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        isTyping=false;

        if(currentDialogue  != null && currentLineIndex < currentDialogue.dialogeLines.Count)
        {
            dialogueText.text = currentDialogue.dialogeLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogeLines.Count) 
        {
            if(typingCoroutine != null)
            {
                StopCoroutine (typingCoroutine);
            }

        }

        string currentText = currentDialogue.dialogeLines [currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));

    }

    public void ShowNextLine()
    {
        currentLineIndex++;

        if(currentLineIndex >= currentDialogue.dialogeLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }


    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentLineIndex = 0;
    }

    public void HandleNextInput()
    {
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if (!isTyping)
        {
            ShowNextLine();
        }
    }

    public void SkipDialoge()
    {
        EndDialogue ();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StatDialogue(DialogueDataSO dialogue)
    {

        if (dialogue == null || dialogue.dialogeLines.Count == 0) return;
        
            currentDialogue= dialogue;
            currentLineIndex= 0;
            isDialogueActive= true;


            DialoguePanel.SetActive (true);
            characterNameText.text = dialogue.characterName;

            if(characterImage != null)
            {
                if(dialogue.characterImage != null)
                {
                    characterImage.sprite = dialogue.characterImage;
                }
                else
                {
                    characterImage.sprite = defaltCharacterImage;
                }
            }

            ShowCurrentLine();

        
    }



}


