using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    public TMP_Text scoreText;
    private int coinsCollected;

    public PlayerData playerData;

    public Image playerImage;
    public Image maskImage;

    public GameObject[] arrowsIcons;

    public GameObject gameOverCanvas;

    public GameObject maskImageAnimation;

    //CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        ChangeStateMaskUI(false);
        gameOverCanvas.SetActive(false);
        maskImageAnimation.SetActive(false);
        //gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
    }



    // Start is called before the first frame update
    private void OnEnable()
    {
        EventRepository.OnPickupCollected += UpdateScore;
        EventRepository.OnCutsceneEnd += ActivateMaskUI;
        EventRepository.OnLevelFinished += ActivateLevelCompleteCanvas;
    }

    private void OnDisable()
    {
        EventRepository.OnPickupCollected -= UpdateScore;
        EventRepository.OnActionKeyPressed -= ChangePlayerImage;
        EventRepository.OnLevelFinished -= ActivateLevelCompleteCanvas;
    }

    private void UpdateScore(object sender, PickupCollectedEventArgs e)
    {
        coinsCollected += e.Value;
        scoreText.text = coinsCollected.ToString();
    }

    void ActivateMaskUI()
    {
        //StartCoroutine(Wait(10));

        foreach (var obj in arrowsIcons)
        {
            obj.SetActive(true);
        }

        maskImageAnimation.SetActive(true);


        EventRepository.OnCutsceneEnd -= ActivateMaskUI; // self odjava
        EventRepository.OnActionKeyPressed += ChangePlayerImage;
    }

    private void ChangeStateMaskUI(bool isActive)
    {
        foreach (var obj in arrowsIcons)
        {
            obj.SetActive(isActive);
        }
    }

    void ChangePlayerImage(bool maskOn)
    {
        if (maskOn)
        {
            playerImage.sprite = playerData.playerIconWithMask;
        }
        else
        {
            playerImage.sprite = playerData.playerIcon;

        }
    }

    void ChangePlayerImage(object sender, ActionPressedEventArgs e)
    {
        if (e.isMaskOn)
        {
            playerImage.sprite = playerData.playerIconWithMask;
        }
        else
        {
            playerImage.sprite = playerData.playerIcon;

        }
    }

    void ActivateLevelCompleteCanvas()
    {
        gameOverCanvas.SetActive(true);
    }

    //IEnumerator Wait(float secondsToWait)
    //{
    //    yield return new WaitForSeconds(secondsToWait);
        
    //}
}
