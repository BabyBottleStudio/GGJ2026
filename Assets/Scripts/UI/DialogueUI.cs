using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUIRoot;
    public GameObject nextPageButton;

    public Image npcProfilePicture;
    public TextMeshProUGUI dialogueText;

    //Animator animator;

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

    //AudioSource audioSource;
    //public AudioClip typingLettersSound;

    // treba nam event koji ce kao parametar da prosledi npc data skriptable objekat

    private void Start()
    {

        nextPageButton.SetActive(false);
        dialogueUIRoot.SetActive(false);
        //audioSource = GetComponent<AudioSource>();
        //animator = GetComponent<Animator>();
        //playableDirector = dialogueUIRoot.GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        currentPage = 0;
        //isTimelineStart = true;
        
        EventRepository.OnInteractionStart += ShowInteractionText;
        EventRepository.OnInteractionEnd += HideInteractionText;
    }

    private void OnDisable()
    {
        EventRepository.OnInteractionStart -= ShowInteractionText;
        EventRepository.OnInteractionEnd -= HideInteractionText;
        //npcData.dialogue.StringChanged -= UpdateText; // ovo baguje kada se interaptuje meni

    }



    void ShowInteractionText(object sender, InteractionEventArgs e)
    {
        isAnimationInterupted = true;
        currentPage = 0;
        npcData = e.NPCData;
        playableDirector.playableAsset = dialogOn;
        playableDirector.time = 0f;
        dialogueUIRoot.SetActive(true);



        playableDirector.Play();
        //animator.Play();
        //npcProfilePicture.gameObject.SetActive(true);
        //dialogueDisplayText.gameObject.SetActive(true);

        npcProfilePicture.sprite = npcData.Icon;

        npcData.dialogue.StringChanged += UpdateText;
        npcData.dialogue.RefreshString();

        //currentPageCount = GetPageCount();
        dialogueText.ForceMeshUpdate();
        if (currentPageCount > 1)
        {
            Debug.Log(GetPageCount());
            nextPageButton.SetActive(true);
        }
        else
        {
            nextPageButton.SetActive(false);
        }

        //StartTypingText();
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
        //playableDirector.cli
        //npcData.dialogue.StringChanged -= UpdateText;
        //UpdateText(string.Empty);
        //npcProfilePicture.sprite = null;
        //npcData = null;
        //dialogueUIRoot.SetActive(false);
    }


    void UpdateText(string value)
    {
        //if (isTyping)
        //    StopCoroutine(typeDialogText);

        dialogueText.text = value;
        dialogueText.ForceMeshUpdate();
        currentPageCount = dialogueText.textInfo.pageCount;
        dialogueText.maxVisibleCharacters = 0; // dialogueText.text.Length;


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

        // if (!isTyping)
        //{
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();
        //Debug.Log($"Start typing reached. Current page {currentPage}, pageIndex = {pageIndex}");
        typeText = StartCoroutine(TypeDialogText(pageIndex));
        //    }
    }

    public void NextPage()
    {
       // Debug.Log($"NextPageActivated. Current page {currentPage}");
        if (isTyping)
        {
            //Debug.Log($"Typing interupted. Current page {currentPage}");
            StopCoroutine(typeText);

            var page = dialogueText.textInfo.pageInfo[currentPage];
            dialogueText.maxVisibleCharacters = page.lastCharacterIndex;
            isTyping = false;


            return;
        }

        currentPage++;
       // Debug.Log($"Current page {currentPage}");
       // Debug.Log($"Broj strana {dialogueText.textInfo.pageCount}");
        dialogueText.ForceMeshUpdate();

        if (currentPage >= dialogueText.textInfo.pageCount)
        {
            // Debug.Log($"Index {currentPage} je veci od broja strana {dialogueText.textInfo.pageCount}");
            // hide interation text
            HideInteractionText();
            TurnOffDialogUI();
            isDialogClosedByButton = true;
            return;
        }

      //  Debug.Log($"---- trebalo bi da kucam stranu sada");
        StartTypingText(currentPage);
    }

    IEnumerator TypeDialogText(int pageIndex)
    {
        dialogueText.pageToDisplay = pageIndex + 1;

       // Debug.Log($"Usao sam u korutinu za ispis teksta {pageIndex}");
        dialogueText.ForceMeshUpdate();



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

        for (int i = start; i <= end; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(maxTypeTime / typeSpeed);
        }

        /*
        foreach (char c in chars)
        {
            maxVisibleChars++;
            dialogueText.maxVisibleCharacters = maxVisibleChars;
            //audioSource.PlayOneShot(typingLettersSound, 0.05f);
            yield return new WaitForSeconds(maxTypeTime / typeSpeed);

        }*/
        dialogueText.ForceMeshUpdate();
        isTyping = false;
    }

    int GetPageCount()
    {
        dialogueText.ForceMeshUpdate();

        return dialogueText.textInfo.pageCount;
    }


    public void TurnOffDialogUI()
    {
        if (isDialogClosedByButton)
        {
            isDialogClosedByButton = false;
            return;
        }

        npcData.dialogue.StringChanged -= UpdateText;
        UpdateText(string.Empty);
        npcProfilePicture.sprite = null;
        npcData = null;
        dialogueUIRoot.SetActive(false);
        isAnimationInterupted = true;
    }

}
