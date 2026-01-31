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

    private void Awake()
    {
        ChangeStateMaskUI(false);
    }



    // Start is called before the first frame update
    private void OnEnable()
    {
        EventRepository.OnPickupCollected += UpdateScore;
        EventRepository.OnKeyCollected += ActivateMaskUI;
    }

    private void OnDisable()
    {
        EventRepository.OnPickupCollected -= UpdateScore;
        EventRepository.OnActionKeyPressed -= ChangePlayerImage;
    }

    private void UpdateScore(object sender, PickupCollectedEventArgs e)
    {
        coinsCollected += e.Value;
        scoreText.text = coinsCollected.ToString();
    }

    void ActivateMaskUI(object sender, PickupCollectedEventArgs e)
    {
        foreach (var obj in arrowsIcons)
        {
            obj.SetActive(true);
        }
        EventRepository.OnKeyCollected -= ActivateMaskUI; // self odjava
        EventRepository.OnActionKeyPressed += ChangePlayerImage;
    }

    private void ChangeStateMaskUI(bool isActive)
    {
        foreach (var obj in arrowsIcons)
        {
            obj.SetActive(isActive);
        }
    }

    void ChangePlayerImage(object sender, ActionPressedEventArgs e)
    {
        if (e.Value)
        {
            playerImage.sprite = playerData.playerIconWithMask;
        }
        else
        {
            playerImage.sprite = playerData.playerIcon;

        }
    }


}
