using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Manages main UI
//----------------------------------------------------------------------------------------

public class nUIManager : MonoBehaviour
{
    [System.Serializable]
    public class ImageGroup
    {
        public Image[] images;
    }

    public static nUIManager instance { get; private set; } // GameController instance

    public HealthBar healthBar;             // reference to player health bar
    public Image[] weaponImageUI;           // reference to weapon one image
    public ImageGroup[] weaponOverlays;     // references to weapon overlays
    public Image consumableImageUI;         // reference to consumable image
    public Text consumableAmountUI;         // reference to consumable amount text
    public Text starFruitAmountUI;          // reference to star fruit amount text
    public Text keyAmountUI;                // reference to key amount text
    public Text objectivesText;             // reference to objectives text
    public Text[] weaponUseAmountUI;        // reference to weapon use amount (ex. ketchup shots left)
    public Text livesAmountUI;
    public Slider loadingSlider;            // reference to loading slider for loading screen
    public Sprite[] commonIcons;            // reference to icons to commonly used icons
    public PostProcessVolume PPV;           // reference to post proccessing

    private Vignette healthVignette;        // ppv vignette settings
    private Coroutine vigTimer = null;      // vignette fade effect coroutine

    // do singleton stuff
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        PPV.profile.TryGetSettings(out healthVignette);
        healthVignette.intensity.value = 0.0f;
    }

    // update health bar
    public void updateHealthBar(float amount)
    {
        healthBar?.updateHealthBar(amount);
    }

    // set heath bar max
    public void setHealthBarMax(float max)
    {
        healthBar?.setHealthBarMax(max);
    }

    // sets selected weapon overlays on
    public void setWeaponOverlayActive(int weaponIndex)
    {
        // set new weapon icon overlays active
        foreach (Image i in weaponOverlays[weaponIndex].images)
        {
            i.gameObject.SetActive(true);
        }
    }

    // sets selected weapon overlays off
    public void setWeaponOverlayInactive(int weaponIndex)
    {
        // set new weapon icon overlays active
        foreach (Image i in weaponOverlays[weaponIndex].images)
        {
            i.gameObject.SetActive(false);
        }
    }

    // set selected weapon UI
    public void setWeaponUI(int weaponIndex, nItemType type)
    {
        if (type == nItemType.None)
        {
            weaponImageUI[weaponIndex].gameObject.SetActive(false);
        }
        else
        {
            weaponImageUI[weaponIndex].sprite = commonIcons[getIconFromType(type)];
            weaponImageUI[weaponIndex].gameObject.SetActive(true);
        }
    }

    // set the consumable UI
    public void setConsumablesUI(nItemType type, float amount)
    {
        if (type == nItemType.None)
        {
            consumableImageUI.gameObject.SetActive(false);
            consumableAmountUI.text = "x" + amount;
            return;
        }

        consumableImageUI.gameObject.SetActive(true);
        consumableImageUI.sprite = commonIcons[getIconFromType(type)];
        consumableAmountUI.text = "x" + amount;
    }

    // set the star fruit UI
    public void setStarFruitUI(int amount)
    {
        starFruitAmountUI.text = "" + amount;
    }

    public void updateLivesUI(int lives)
    {
        // find with tag is evil
        //PlayerManager lifeAmount = GameObject.FindWithTag("Player").GetComponentInParent<PlayerManager>();

        livesAmountUI.text = "" + lives;
    }

    //update the key UI
    public void setKeyUI(int keys)
    {
        keyAmountUI.text = "" + keys;
    }

    // sets objective text
    public void setObjectiveText(string text)
    {
        objectivesText.text = text;
    }

    // set weapon usage amount UI
    public void setWeaponUseUI(int weaponIndex, float amount, bool active = true)
    {
        weaponUseAmountUI[weaponIndex].text = "x" + amount;
        weaponUseAmountUI[weaponIndex].transform.parent.gameObject.SetActive(active);
    }

    public void setLoadingProgress(float value)
    {
        if (loadingSlider != null)
        {
            //print("setting loading slider value: " + value);
            loadingSlider.transform.parent.gameObject.SetActive(true);
            loadingSlider.value = value;
        }
        else
        {
            print("no loading slider is present");
        }
    }

    public void setVignetteIntensity(float intensity)
    {
        if (vigTimer != null)
        {
            StopCoroutine(vigTimer);
        }

        healthVignette.intensity.value = intensity;
    }

    public void doVignetteFadeEffect(float duration, float intensity)
    {
        if (vigTimer != null)
        {
            StopCoroutine(vigTimer);
        }

        vigTimer = StartCoroutine(vignetteFadeEffect(duration, intensity));
    }

    // returns index for commonIcons from type
    private int getIconFromType(nItemType type)
    {
        switch (type)
        {
            case nItemType.HealthPickup:
                return 0;
            case nItemType.OnionWeapon:
                return 1;
            case nItemType.KetchupWeapon:
                return 2;
            default:
                return -1;
        }
    }

    // timer for vignette effect
    private IEnumerator vignetteFadeEffect(float duration, float toIntensity)
    {
        float passed = 0.0f;
        float original = healthVignette.intensity.value;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            healthVignette.intensity.value = Mathf.Lerp(original, toIntensity, passed / duration);
            yield return null;
        }
    }
}