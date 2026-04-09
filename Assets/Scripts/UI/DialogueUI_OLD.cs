using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Text;

public class DialogueUI_OLD : MonoBehaviour
{
    public InputActionAsset inputActions;
    private InputAction fireAction;


    public GameObject dialogueUIRoot;
    public GameObject nextPageButton;

    public Image npcProfilePicture;
    public TextMeshProUGUI dialogueText;

    InteractiveObject npcData;

    bool isTyping;
    bool isAnimationInterupted;

    Coroutine typeText;

    public PlayableDirector playableDirector;
    public PlayableAsset dialogOn;
    public PlayableAsset dialogOff;

    //string fullText;
    int currentPage;
    bool isDialogClosedByButton;

    int currentPageCount;

    public Sprite nextPageIcon;
    public Sprite closeDialogIcon;

    int dialogSessionId;
    bool restartTypingAfterUpdate;

    TypeWriter typeWriter;
    //bool shouldRestartTyping;

    //AudioSource audioSource;
    //public AudioClip typingLettersSound;

    // treba nam event koji ce kao parametar da prosledi npc data skriptable objekat
    private void Awake()
    {
        fireAction = inputActions.FindActionMap("Player").FindAction("Fire");
    }
    private void Start()
    {
        nextPageButton.SetActive(false);
        dialogueUIRoot.SetActive(false);
        ResetText();

        typeWriter = new TypeWriter(dialogueText);
    }

    private void OnEnable()
    {
        //currentPage = 1;
        //currentPageCount = 1;
        //isTimelineStart = true;

        EventRepository.OnInteractionStart += ShowInteractionText;
        EventRepository.OnInteractionEnd += HideInteractionText;
        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        EventRepository.OnInteractionStart -= ShowInteractionText;
        EventRepository.OnInteractionEnd -= HideInteractionText;
        LocalizationSettings.SelectedLocaleChanged -= OnLanguageChanged;

        if (npcData != null)
        {
            npcData.dialogue.StringChanged -= UpdateText;
        }
    }

    void OnLanguageChanged(UnityEngine.Localization.Locale locale)
    {
        if (npcData == null || !dialogueUIRoot.activeInHierarchy)
            return;

        restartTypingAfterUpdate = true;
    }

    void ShowInteractionText(object sender, InteractionEventArgs e)
    {
        // 1. OBAVEZNO: Ako je ostala neka stara pretplata, ubij je odmah!
        if (npcData != null)
        {
            npcData.dialogue.StringChanged -= UpdateText;
        }

        if (typeText != null)
        {
            StopCoroutine(typeText);
            typeText = null;
        }





        dialogSessionId++;
        int mySession = dialogSessionId;

        isAnimationInterupted = true;
        currentPage = 0;
        //currentPageCount = 1;
        npcData = e.NPCData;
        playableDirector.playableAsset = dialogOn;
        playableDirector.time = 0f;

        dialogueUIRoot.SetActive(true);

        nextPageButton.GetComponent<Button>().image.sprite = nextPageIcon;
        nextPageButton.SetActive(false);


        playableDirector.Play();

        npcProfilePicture.sprite = npcData.Icon;

        npcData.dialogue.StringChanged += UpdateText;
        //shouldRestartTyping = true;
        npcData.dialogue.RefreshString();

        //fireAction.Disable();
    }



    void HideInteractionText()
    {
        if (isAnimationInterupted)
        {
            TurnOffDialogUI();
            playableDirector.time = 0f;

            return;
        }

        playableDirector.playableAsset = dialogOff;
        playableDirector.time = 0f;
        playableDirector.Play();
    }


    void UpdateText(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;


        dialogueText.text = value;
        //typeWriter.BreakTextToPages(value); // za sada zavisi da je vec stavljen tekst u tmpro
        //string test = typeWriter.TestPageBreak();
        //Debug.Log(test);
        //Canvas.ForceUpdateCanvases();
        //currentPageCount = dialogueText.textInfo.pageCount;
        dialogueText.maxVisibleCharacters = 0; // dialogueText.text.Length;
        dialogueText.ForceMeshUpdate();

        if (restartTypingAfterUpdate && dialogueUIRoot.activeInHierarchy)
        {
            //shouldRestartTyping = false;
            currentPage = 0;
            StartTypingText(currentPage); // neki bolji sistem da ovo ne krene svakako
            restartTypingAfterUpdate = false;
            EventSystem.current.SetSelectedGameObject(nextPageButton);
        }

    }

    public void EndOfScaleUpAnimationReached()
    {
        isAnimationInterupted = false; // ovo se okida iz timelinea, sluzi da ne baguje kad se brzo ulazi i izlazi iz collidera
    }

    public void StartTypingText(int pageIndex)
    {
        // ova metoda se prvi put okida iz unity timeline
        // svaki naredni put se okdia preko dugmeta za next page, metoda dole. Tekst mesh pro iz nekog razloga ne prikazuje drugu stranu

        if (typeText != null)
            StopCoroutine(typeText);

        dialogueText.maxVisibleCharacters = 0;

        nextPageButton.SetActive(false);

        typeText = StartCoroutine(TypeDialogText(pageIndex));

    }

    public void NextPage()
    {
        Debug.Log($"NextPageActivated. Current page {currentPage}");
        if (isTyping)
        {
            Debug.Log($"Typing interupted. Current page {currentPage}");
            StopCoroutine(typeText);

            var page = dialogueText.textInfo.pageInfo[currentPage];
            dialogueText.maxVisibleCharacters = page.lastCharacterIndex;
            isTyping = false;

            return;
        }

        currentPage++;
        dialogueText.ForceMeshUpdate();

        if (currentPage >= dialogueText.textInfo.pageCount)
        {
            isDialogClosedByButton = true;
            HideInteractionText();
            //TurnOffDialogUI();
            return;
            //nextPageButton.SetActive(false);
        }

        Debug.Log($"---- trebalo bi da kucam stranu sada");
        StartTypingText(currentPage);
    }

    IEnumerator TypeDialogText(int pageIndex)
    {
        int mySession = dialogSessionId;

        yield return null;

        if (mySession != dialogSessionId)
            yield break;


        Canvas.ForceUpdateCanvases();
        dialogueText.ForceMeshUpdate();
        currentPageCount = dialogueText.textInfo.pageCount;

        bool hasMorePages = currentPageCount > 1; // && pageIndex < (currentPageCount - 1);

        if (hasMorePages)
        {
            nextPageButton.GetComponent<Button>().image.sprite = nextPageIcon;
            fireAction?.Disable();
        }

        if (pageIndex == (currentPageCount - 1))
        {
            nextPageButton.GetComponent<Button>().image.sprite = closeDialogIcon;
        }

        nextPageButton.SetActive(hasMorePages);

        //if (pageIndex < currentPageCount - 1)
        //    nextPageButton.SetActive(false);

        dialogueText.pageToDisplay = pageIndex + 1;

        // Debug.Log($"Usao sam u korutinu za ispis teksta {pageIndex}");


        var txtInfo = dialogueText.textInfo;
        var page = txtInfo.pageInfo[pageIndex];

        float maxTypeTime = 0.1f;
        float typeSpeed = 5f;
        isTyping = true;

        //int maxVisibleChars = 0;
        //dialogueText.maxVisibleCharacters = maxVisibleChars;


        int start = page.firstCharacterIndex;
        int end = page.lastCharacterIndex;

        //  Debug.Log($"Start: {start}, end {end}");
        //char[] chars = dialogueDisplayText.text.ToCharArray();
        dialogueText.maxVisibleCharacters = start;

        float typeSpeedTime = maxTypeTime / typeSpeed;

        for (int i = start; i <= end; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(typeSpeedTime);
        }

        dialogueText.ForceMeshUpdate();
        isTyping = false;
    }


    public void TurnOffDialogUI()
    {
        Debug.Log("TurnOffDialogUI Triggered");

        if (npcData != null)
        {
            npcData.dialogue.StringChanged -= UpdateText;
            npcData = null;
        }

        if (typeText != null)
        {
            StopCoroutine(typeText);
            typeText = null;
        }

        //npcData.dialogue.StringChanged -= UpdateText;
        ResetText();

        npcProfilePicture.sprite = null;
        npcData = null;
        //nextPageButton.SetActive(false);
        dialogueUIRoot.SetActive(false);
        isAnimationInterupted = true;
        currentPageCount = 1;
        isDialogClosedByButton = false;

        if (!fireAction.enabled)
            fireAction.Enable();
    }


    void ResetText()
    {
        dialogueText.text = string.Empty;
        dialogueText.maxVisibleCharacters = 0;
    }
}


