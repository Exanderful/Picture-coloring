using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public GameObject rewardButton;

    private void Start()
    {
        if(PlayerPrefs.GetInt("adsRemoved") == 1)
        {
            rewardButton.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    public void OnRemoveAdsPurchaseComplete()
    {
        Debug.Log("adsremoved set 1");
        PlayerPrefs.SetInt("adsRemoved", 1);
        rewardButton.transform.localScale = new Vector3(0f, 0f, 0f);
    }
}
