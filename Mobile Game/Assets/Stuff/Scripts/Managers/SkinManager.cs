using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkinManager : MonoBehaviour
{
    [Header("References")]
    public Skin[] skins;
    private GameManager gm;

    [Header("Skin Display")]
    public TMP_Text priceText;
    public Animator skinDisplay;
    public Button selectButton;
    public int currentSelectedSkin;
    public int currentSkinIndex;
    public Button buyWithCoinButton;
    public Button buyWithGemsButton;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void NextSkin()
    {
        currentSkinIndex += 1;
        if (currentSkinIndex >= skins.Length)
        {
            currentSkinIndex = 0;
        }

        OnNewSkin();
    }

    public void PreviousSkin()
    {
        currentSkinIndex -= 1;
        if (currentSkinIndex < 0)
        {
            currentSkinIndex = skins.Length - 1;
        }

        OnNewSkin();
    }

    public void SelectSkin()
    {
        currentSelectedSkin = currentSkinIndex;
        gm.playerSkin = skins[currentSkinIndex].animator;
        selectButton.interactable = skins[currentSkinIndex].owned && currentSelectedSkin != currentSkinIndex;
    }

    void OnNewSkin()
    {
        // Set buttons
        selectButton.interactable = skins[currentSkinIndex].owned && currentSelectedSkin != currentSkinIndex;
        buyWithCoinButton.interactable = gm.gems >= skins[currentSkinIndex].gemCost && !skins[currentSkinIndex].owned;
        priceText.text = skins[currentSkinIndex].gemCost.ToString();
        // buyWithGemsButton.enabled = gm.gems >= skins[currentSkinIndex].gemCost;

        // Set skin display
        skinDisplay.runtimeAnimatorController = skins[currentSkinIndex].animator;
    }

    public void BuySkin()
    {
        skins[currentSkinIndex].owned = true;
        gm.gems -= skins[currentSkinIndex].gemCost;

        OnNewSkin();
    }
    public void BuySkinWithGems(int skin)
    {

    }
}
