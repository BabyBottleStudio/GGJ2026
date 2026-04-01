using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class DialogueUI : MonoBehaviour
{
    public GameObject dialogueUIRoot;

    public Image npcProfilePicture;
    public TextMeshProUGUI dialogueDisplayText;

    //Animator animator;

    NonPlayableCharacter npcData;

    bool isTyping;

    Coroutine typeDialogText;

    public PlayableDirector playableDirector;
    public PlayableAsset dialogOn;
    public PlayableAsset dialogOff;

    //AudioSource audioSource;
    //public AudioClip typingLettersSound;

    // treba nam event koji ce kao parametar da prosledi npc data skriptable objekat

    private void Start()
    {

        dialogueUIRoot.SetActive(false);
        //audioSource = GetComponent<AudioSource>();
        //animator = GetComponent<Animator>();
        //playableDirector = dialogueUIRoot.GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        EventRepository.OnInteractionStart += ShowInteractionText;
        EventRepository.OnInteractionEnd += HideInteractionText;
    }

    private void OnDisable()
    {
        EventRepository.OnInteractionStart -= ShowInteractionText;
        EventRepository.OnInteractionEnd -= HideInteractionText;

    }

    void ShowInteractionText(object sender, InteractionEventArgs e)
    {
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

        //StartTypingText();
    }



    void HideInteractionText()
    {
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

        dialogueDisplayText.text = value;
        dialogueDisplayText.maxVisibleCharacters = dialogueDisplayText.text.Length - 1;
    }


    public void StartTypingText()
    {
        if (!isTyping)
            typeDialogText = StartCoroutine(TypeDialogText());
    }


    IEnumerator TypeDialogText()
    {
        float maxTypeTime = 0.1f;
        float typeSpeed = 5f;
        isTyping = true;

        int maxVisibleChars = 0;

        dialogueDisplayText.maxVisibleCharacters = maxVisibleChars;

        char[] chars = dialogueDisplayText.text.ToCharArray();

        foreach (char c in chars)
        {
            maxVisibleChars++;
            dialogueDisplayText.maxVisibleCharacters = maxVisibleChars;
            //audioSource.PlayOneShot(typingLettersSound, 0.05f);
            yield return new WaitForSeconds(maxTypeTime / typeSpeed);

        }

        isTyping = false;
    }




    public void TurnOffDialogUI()
    {
        npcData.dialogue.StringChanged -= UpdateText;
        UpdateText(string.Empty);
        npcProfilePicture.sprite = null;
        npcData = null;
        dialogueUIRoot.SetActive(false);
    }

}
