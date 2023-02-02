using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BBG.PictureColoring
{
    public class HintButton : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private Text hintAmountText = null;

        #endregion

        #region Unity Methods

        private void Start()
        {
            UpdateUI();
            StartCoroutine(Getanimation());
            CurrencyManager.Instance.OnCurrencyChanged += (string obj) => { UpdateUI(); };
        }

        #endregion

        #region Private Methods

        private void UpdateUI()
        {
            hintAmountText.text = CurrencyManager.Instance.GetAmount("hints").ToString();
        }

        #endregion

        private IEnumerator Getanimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                gameObject.transform.DOScale(new Vector3(1.35f, 1.35f, 1.35f), 0.5f);
                yield return new WaitForSeconds(0.5f);
                gameObject.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f);
            }
        }
    }
}
