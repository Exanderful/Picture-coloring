using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;


public class AdsInit : MonoBehaviour
{
    private void Awake()
    {
       MobileAds.Initialize(initStatus => { });
    }

    void Start()
    {
        StartCoroutine(next_scene());
    }

    IEnumerator next_scene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
    }
}
