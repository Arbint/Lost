using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWidget : MonoBehaviour
{
    [SerializeField] Image mTransitionImage;

    void Awake()
    {
        mTransitionImage.gameObject.SetActive(false);
    }

    public void DipToBlack(float dipInAndOutDuration, float dipStayDuration, Action dippedToBlackCallback)
    {
        StartCoroutine(StartDipToBlack(dipInAndOutDuration, dipStayDuration, dippedToBlackCallback));
    }

    IEnumerator StartDipToBlack(float dipInAndOutDuration, float dipStayDuration, Action dippedToBlackCallback)
    {
        float timeCounter = 0;
        mTransitionImage.gameObject.SetActive(true);
        Color trasitionImageColor = Color.black;
        trasitionImageColor.a = 0;

        while (timeCounter < dipInAndOutDuration)
        {
            trasitionImageColor.a = timeCounter / dipInAndOutDuration;
            mTransitionImage.color = trasitionImageColor;
            timeCounter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        trasitionImageColor.a = 1;
        mTransitionImage.color = trasitionImageColor;
        dippedToBlackCallback();

        //wait for dipStayDuraiton
        yield return new WaitForSeconds(dipStayDuration);

        //Dip out from black
        while (trasitionImageColor.a > 0)
        {
            trasitionImageColor.a -= Time.deltaTime;
            mTransitionImage.color = trasitionImageColor;
            yield return new WaitForEndOfFrame();
        }

        mTransitionImage.gameObject.SetActive(false);
    }
}
