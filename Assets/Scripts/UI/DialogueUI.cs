using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Text;
using System.Linq;

public class DialogueUI : MonoBehaviour
{
    [Header("Player Input")]
    public InputActionAsset inputActions;
    private InputAction fireAction;
    [Header("UI Elements")]
    public GameObject dialogueUIRoot;
    public RectTransform backgroundImg;

    public Image npcProfilePicture;
    public TextMeshProUGUI dialogueText;
    public GameObject nextPageButton;
    Button nextPageButtonComponent;

    InteractiveObject npcData;

    private TypeWriter typeWriter;

    Coroutine currentTyping;

    public Sprite nextPageIcon;
    public Sprite closeDialogIcon;

    [Space(10)]
    [Header("Scale Dialog Background")]
    [SerializeField] AnimationCurve scaleXCurve;
    [SerializeField] AnimationCurve scaleYCurve;
    [Header("Scale NPC Image")]
    [SerializeField] AnimationCurve scaleCurve;

    Coroutine currentScalingAnimation;
    Vector3 targetScale;
    Vector3 startScale;


    private void Awake()
    {
        fireAction = inputActions.FindActionMap("Player").FindAction("Fire");
    }

    private void Start()
    {
        typeWriter = new TypeWriter(dialogueText);
        typeWriter.Reset();
        nextPageButton.SetActive(false);
        nextPageButtonComponent = nextPageButton.GetComponent<Button>();
        backgroundImg.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        EventRepository.OnInteractionStart += ShowInteractionText;
        EventRepository.OnInteractionEnd += HideInteractionText;
        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;
    }

    private void OnDisable()
    {
        EventRepository.OnInteractionStart -= ShowInteractionText;
        EventRepository.OnInteractionEnd -= HideInteractionText;
        LocalizationSettings.SelectedLocaleChanged -= OnLanguageChanged;
    }


    void ShowInteractionText(object sender, InteractionEventArgs e)
    {
        ResetUIElements();
        dialogueUIRoot.SetActive(true);

        npcData = e.NPCData;
        npcProfilePicture.sprite = npcData.Icon;


        npcData.dialogue.RefreshString();

        string txt = npcData.dialogue.GetLocalizedString();
        typeWriter.BreakTextToPages(txt);


        //fireAction.Disable();

        bool hasMultiplePages = typeWriter.PagesCount > 1;
        nextPageButton.SetActive(hasMultiplePages);
        HandleNextPageButtonIcon();

        if (hasMultiplePages)
        {
            fireAction?.Disable();
        }

        //string test = typeWriter.TestPageBreak();
        //Debug.Log(test);
        StartTyping(true);
        StartScaleUI(true);
    }

    void HideInteractionText()
    {
        StartScaleUI(false);
        //dialogueUIRoot.SetActive(false);
        typeWriter.Reset();
    
        if (!fireAction.enabled)
            fireAction.Enable();
    }

    void OnLanguageChanged(UnityEngine.Localization.Locale locale)
    {
        typeWriter.Reset();
        HandleNextPageButtonIcon();
        npcData.dialogue.RefreshString();
        string txt = npcData.dialogue.GetLocalizedString();
        typeWriter.BreakTextToPages(txt);
        //string test = typeWriter.TestPageBreak();
        //Debug.Log(test);
        StartTyping(false);
    }

    public void StartTyping(bool startDelayed)
    {
        if (typeWriter.IsTyping)
        {
            StopCoroutine(currentTyping);
            //typeWriter.InteruptTyping();
            //currentTyping = null;
        }
        TextPage currentPageData = typeWriter.GetCurrentPage();
        currentTyping = StartCoroutine(typeWriter.TypeText(currentPageData, startDelayed));
    }

    public void NextPage()
    {
        if (typeWriter.IsTyping)
        {
            StopCoroutine(currentTyping);
            typeWriter.InteruptTyping();
            return;
        }


        if (typeWriter.IsLastPage())
        {
            HideInteractionText();
            typeWriter.Reset();
            return;
        }

        typeWriter.SetNextPage();
        HandleNextPageButtonIcon();

        StartTyping(false);
    }

    void HandleNextPageButtonIcon()
    {
        if (typeWriter.IsLastPage())
        {
            nextPageButtonComponent.image.sprite = closeDialogIcon;
            return;
        }

        if (nextPageButtonComponent.image.sprite != nextPageIcon)
            nextPageButtonComponent.image.sprite = nextPageIcon;
    }

    void ResetUIElements()
    {
        nextPageButtonComponent.image.sprite = nextPageIcon;
        npcProfilePicture.sprite = null;
    }

    void StartScaleUI(bool scaleUp)
    {
        if (currentScalingAnimation != null)
            StopCoroutine(currentScalingAnimation);

        startScale = backgroundImg.localScale;

        if (scaleUp)
            targetScale = Vector3.one;
        else
            targetScale = Vector3.zero;

        currentScalingAnimation = StartCoroutine(ScaleUI());
    }

    IEnumerator ScaleUI()
    {
        if (targetScale == Vector3.one)
            backgroundImg.gameObject.SetActive(true);

        float timer = 0f;

        float duration = 0.25f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            float curveXT = scaleXCurve.Evaluate(t);
            float curveYT = scaleYCurve.Evaluate(t);
            float cruveScale = scaleCurve.Evaluate(t);


            var amtX = Mathf.Lerp(startScale.x, targetScale.x, curveXT);
            var amtY = Mathf.Lerp(startScale.y, targetScale.y, curveYT);
            //var profilePictureScale = Mathf.Lerp(startScale.x, targetScale.x, curveYT);

            backgroundImg.localScale = new Vector3(amtX, amtY, amtY);
            //npcProfilePicture.rectTransform.localScale = new Vector3(profilePictureScale, profilePictureScale, profilePictureScale);
            yield return null;
        }

        backgroundImg.localScale = targetScale;
        //npcProfilePicture.rectTransform.localScale = targetScale;
        currentScalingAnimation = null;

        if (targetScale == Vector3.zero)
            backgroundImg.gameObject.SetActive(false);

    }

}

// typewriter treba da se brine samo o ispisu teksta, a ne da resava sve ostale probleme

public class TypeWriter
{
    public int PagesCount { get; private set; }
    public int CurrentPageIndex { get; private set; }
    public bool IsTyping { get; private set; }

    // typing speed
    const float maxTypeTime = 0.1f;
    const float typeSpeed = 5f;
    float _typingSpeed = maxTypeTime / typeSpeed;

    List<TextPage> textPages;
    TextMeshProUGUI _displayText;


    // metoda koja konvertuje text u stranice
    public TypeWriter(TextMeshProUGUI displayText)
    {
        if (displayText == null)
        {
            throw new System.ArgumentNullException(nameof(displayText));
        }
        _displayText = displayText;
        textPages = new List<TextPage>();
    }

    public bool IsLastPage() => PagesCount - 1 == CurrentPageIndex;

    public void Reset()
    {
        textPages.Clear();

        PagesCount = 0;
        CurrentPageIndex = 0;
        _displayText.text = string.Empty;
    }

    public void BreakTextToPages(string inputText)
    {
        if (_displayText == null)
            return;

        if (textPages == null)
        {
            textPages = new List<TextPage>();
        }

        _displayText.text = inputText; // ovo mi se ne svidja sto je ovde

        Canvas.ForceUpdateCanvases();
        _displayText.ForceMeshUpdate();

        var textInfo = _displayText.textInfo;
        PagesCount = textInfo.pageCount;

        for (int i = 0; i < PagesCount; i++)
        {
            var pageInfo = textInfo.pageInfo[i];

            int _startIndex = pageInfo.firstCharacterIndex;
            int _endIndex = pageInfo.lastCharacterIndex;
            int _lenght = _endIndex - _startIndex + 1;

            textPages.Add(new TextPage
            {
                startIndex = _startIndex,
                endIndex = _endIndex,
                pageText = _displayText.text.Substring(_startIndex, _lenght),
                //pageCount = pagesCount,
                //pageIndex = i
            });
        }
    }

    public TextPage GetPage(int index)
    {
        return textPages.ElementAtOrDefault(index);
    }

    public TextPage GetCurrentPage()
    {
        return textPages.ElementAtOrDefault(CurrentPageIndex);
    }

    public void InteruptTyping()
    {
        // prekini korutinu
        IsTyping = false;
        _displayText.maxVisibleCharacters = GetCurrentPage().endIndex;
    }

    public void SetNextPage()
    {
        CurrentPageIndex++;
        CurrentPageIndex = Mathf.Min(CurrentPageIndex, PagesCount - 1);
    }

    public IEnumerator TypeText(TextPage textPage, bool startDelayed) // zameni za enum
    {
        if (startDelayed)
        {
            _displayText.maxVisibleCharacters = 0;
            yield return new WaitForSeconds(0.25f); // hardkodovano!
        }

        IsTyping = true;

        // treba uraditi test da li korutina radi nesto ili ne

        _displayText.pageToDisplay = CurrentPageIndex + 1;
        int start = textPage.startIndex;
        int end = textPage.endIndex;

        _displayText.maxVisibleCharacters = start;

        float typeSpeedTime = maxTypeTime / typeSpeed;

        for (int i = start; i <= end; i++)
        {
            _displayText.maxVisibleCharacters = i;

            yield return new WaitForSeconds(typeSpeedTime);
        }

        _displayText.ForceMeshUpdate();
        IsTyping = false;
    }


    public string TestPageBreak()
    {
        if (textPages == null && textPages.Count == 0)
            return null;


        StringBuilder sb = new StringBuilder();
        int pageIndex = 0;

        foreach (var textPage in textPages)
        {
            sb.AppendLine($"PagesCount {textPages.Count}");
            sb.AppendLine($"Page {pageIndex++}; Start index {textPage.startIndex}; End index {textPage.endIndex}");
            sb.AppendLine($"Text: {textPage.pageText}");
        }

        return sb.ToString().Trim();

    }
    // PRE KUCANJA TREBA DA SE UTVRDI
    // string koji ce da otkuca
    // broj strana

    // šta se dešava kada se kucanje prekine -> ispise tekst do kraja odmah
    // prebacivanje na sledecu stranu
    // šta se dešava kada se promeni jezik -> krece ispocetka, resetuje current page na start i kuca ponovo
}

public struct TextPage
{
    public int startIndex;
    public int endIndex;
    public string pageText;
    // public int pageCount;
    // public int pageIndex;

}
