using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    public TMP_Text scoreText;
    public Animator coinIconAnim;
    string coinIconAnimTrigger = "ThrowEmpty";
    
    private int coinsCollected;

    public PlayerData playerData;

    public Image playerImage;
    public Image maskImage;

    public GameObject[] arrowsIcons;

    public GameObject gameOverCanvas;

    //public GameObject maskImageAnimation;

    //CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        ChangeStateMaskUI(false);
        gameOverCanvas.SetActive(false);
        //coinIconAnim.SetInteger("State", 0);

        //maskImageAnimation.SetActive(false);
        //gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
    }



    // Start is called before the first frame update
    private void OnEnable()
    {
        EventRepository.OnPickupCollected += UpdateScore;
        EventRepository.OnPickupCollected += CoinPickupIconAnimation;
        //EventRepository.OnCutsceneEnd += ActivateMaskUI;
        EventRepository.OnLevelFinished += ActivateLevelCompleteCanvas;
        EventRepository.OnThrowPressed += UpdateScore;
        EventRepository.OnThrowPressed += CoinThrowIconAnimation;
    }

    private void OnDisable()
    {
        EventRepository.OnPickupCollected -= UpdateScore;
        EventRepository.OnPickupCollected -= CoinPickupIconAnimation;
        EventRepository.OnActionKeyPressed -= ChangePlayerImage;
        EventRepository.OnLevelFinished -= ActivateLevelCompleteCanvas;
        EventRepository.OnThrowPressed -= UpdateScore;
        EventRepository.OnThrowPressed -= CoinThrowIconAnimation;
    }


    void CoinPickupIconAnimation(object sender, PickupCollectedEventArgs e)
    {
        coinIconAnim.SetTrigger("Collect");
        //coinIconAnim.SetInteger("State", 1);
        coinIconAnimTrigger = "Throw";
    }

    void CoinThrowIconAnimation()
    {
        coinIconAnim.SetTrigger(coinIconAnimTrigger);
        coinIconAnimTrigger = coinsCollected <= 0 ? "ThrowEmpty" : "Throw";
    }

    private void UpdateScore(object sender, PickupCollectedEventArgs e)
    {
        coinsCollected += e.Value;
        scoreText.text = coinsCollected.ToString();
    }

    private void UpdateScore()
    {
        if (coinsCollected == 0)
            return;

        coinsCollected --;
        scoreText.text = coinsCollected.ToString();
    }

    public void ActivateMaskUI()
    {
        //StartCoroutine(Wait(10));

        foreach (var obj in arrowsIcons)
        {
            obj.SetActive(true);
        }

        //maskImageAnimation.SetActive(true);


        //EventRepository.OnCutsceneEnd -= ActivateMaskUI; // self odjava
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

    /*
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
    */

    void ActivateLevelCompleteCanvas()
    {
        gameOverCanvas.SetActive(true);
    }

    //IEnumerator Wait(float secondsToWait)
    //{
    //    yield return new WaitForSeconds(secondsToWait);
        
    //}
}
