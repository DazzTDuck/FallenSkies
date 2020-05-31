using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationUI : MonoBehaviour
{
    public GameObject Menu;

    public bool activeBG;
    public GameObject settingsBG;

    public float AnimationSpeed = 0.5f;

    public bool AnimaitonOnStart;

    [Header("Animation type")]
    public bool scale;
    public bool scaleX;
    public bool scaleY;
    public bool dropLogo;

    [Header("On Close Actions")]
    public bool closeScale;
    public bool closeLogo;

    [Header("Curve types")]
    public LeanTweenType tweenTypeIn;
    public LeanTweenType tweenTypeOut;

    [Header("Don't touch!")]
    public bool isTweening = false;

    public void TweenScale()
    {
        LeanTween.scale(gameObject, Vector3.one, AnimationSpeed).setEase(tweenTypeIn).setOnStart(ShowMenu);
    }

    public void TweenScaleX()
    {
        LeanTween.scaleX(gameObject, 1f, AnimationSpeed).setEase(tweenTypeIn).setOnStart(ShowMenu);
    }

    public void TweenScaleY()
    {
        LeanTween.scaleY(gameObject, 1f, AnimationSpeed).setEase(tweenTypeIn).setOnStart(ShowMenu);
    }
    public void DropLogo()
    {
        LeanTween.moveLocalY(gameObject, 650f, AnimationSpeed).setEase(tweenTypeIn).setOnStart(ShowMenu);
    }

    //for buttons
    public void OnCloseScaleX()
    {
        LeanTween.scaleX(gameObject, 0f, AnimationSpeed).setEase(tweenTypeOut).setOnComplete(HideMenu);
    }
    public void OnCloseLogoUp()
    {
        LeanTween.moveLocalY(gameObject, 1500f, AnimationSpeed).setEase(tweenTypeOut).setOnComplete(HideMenu);
    }

    public void OnCloseAnimation()
    {
        if (closeScale)
        {
            OnCloseScaleX();
        }
        if (closeLogo)
        {
            OnCloseLogoUp();
        }
    }

    public void OnButtonPressPlayAnimation()
    {
        if (scale)
        {
            TweenScale();
        }
        if (scaleX)
        {
            TweenScaleX();
        }
        if (scaleY)
        {
            TweenScaleY();
        }
        if (dropLogo)
        {
            DropLogo();
        }
    }

    private void Start()
    {        
        if (AnimaitonOnStart)
        {
            if (scale)
            {
                TweenScale();
            }
            if (scaleX)
            {
                TweenScaleX();
            }
            if (scaleY)
            {
                TweenScaleY();
            }
            if (dropLogo)
            {
                DropLogo();
            }
        }     
    }

    public void HideMenu()
    {
        Menu.SetActive(false);
        isTweening = false;
    }
    public void ShowMenu()
    {
        Menu.SetActive(true);
        isTweening = true;     
    }
}
