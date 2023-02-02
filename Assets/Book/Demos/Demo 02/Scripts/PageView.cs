namespace echo17.EndlessBook.Demo02
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;
    using BBG.PictureColoring;
    using BBG;
    using UnityEngine.Advertisements;

    /// <summary>
    /// Class to handle the mini scene that is used to render some pages to a render texture
    /// </summary>
    public class PageView : MonoBehaviour
    {
        /// <summary>
        /// The camera to render the texture
        /// </summary>
        protected Camera pageViewCamera;

        /// <summary>
        /// Ray casting mask and distance values in case there
        /// is page interaction
        /// </summary>
        public LayerMask raycastLayerMask;
        public float maxRayCastDistance = 1000f;

        public Canvas canvas;

        public GameObject cam;
        public GameObject cam2;
        public GameObject transitionImage;

        private bool pictureClicked = false;

        void Awake()
        {
            pictureClicked = false;
            // cache the page camera
            pageViewCamera = GetComponentInChildren<Camera>();
        }

        public virtual void Activate()
        {
            // turn on. Useful for when a page is now being shown in the book
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            // turn off. Useful for when a page is no longer visible.
            // No need to render the page scene any longer
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when the touch down event occurs
        /// </summary>
        public virtual void TouchDown()
        {
            /*if (!pictureClicked)
            {
                pictureClicked = true;
                StartCoroutine(transition());
            }*/
        }

        public void StartTransition()
        {
            if (!pictureClicked)
            {
                pictureClicked = true;
                StartCoroutine(transition());
            }
        }

        IEnumerator loadAds()
        {
            if (PlayerPrefs.GetInt("adsRemoved") == 0)
            {
                GameObject.Find("Interstitial").GetComponent<Interstitial>().LoadAds();
                yield return new WaitForSeconds(1.5f);
                GameObject.Find("Interstitial").GetComponent<Interstitial>().ShowInterstitialAd();
                yield return new WaitForSeconds(1f);
                GameObject.Find("Interstitial").GetComponent<Interstitial>().DestroyInterstitialAd();
            }
        }

        IEnumerator transition()
        {
            cam.transform.DOMoveY(3f, 2f).SetEase(Ease.OutCirc);
            transitionImage.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 2f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(2f);
            StartCoroutine(loadAds());
            cam.transform.position = new Vector3(1f, 5.71f, 0f);
            cam2.GetComponent<Camera>().targetTexture = null;
            transitionImage.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.InSine);
            GameManager.Instance.StartLevel(GameManager.Instance.Categories[0].levels[Demo02.CurrentLvl]);
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            pictureClicked = false;
            if (GameManager.Instance.Categories[0].levels[Demo02.CurrentLvl].LevelSaveData.isCompleted)
            {
                GameObject.Find("GameScreen").GetComponent<GameScreen>().RestartButton.SetActive(true);
            }
            else
            {
                GameObject.Find("GameScreen").GetComponent<GameScreen>().RestartButton.SetActive(false);
            }
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                if (Demo02.CurrentLvl == 1)
                {
                    SoundManager.Instance.Stop("bkg-music");
                    SoundManager.Instance.Play("galaxy");
                }
                else if (Demo02.CurrentLvl == 2)
                {
                    SoundManager.Instance.Stop("bkg-music");
                    SoundManager.Instance.Play("lvl3");
                }
                else if (Demo02.CurrentLvl == 3)
                {
                    SoundManager.Instance.Stop("bkg-music");
                    SoundManager.Instance.Play("lvl4");
                }
                else if (Demo02.CurrentLvl == 4)
                {
                    SoundManager.Instance.Stop("bkg-music");
                    SoundManager.Instance.Play("lvl5");
                }
            }
        }

        /// <summary> 
        /// Casts a ray into the page scene, calling a handler if a hit is found
        /// </summary>
        /// <param name="hitPointNormalized">The hit point of the page normalized between 0 and 1 on both axis</param>
        /// <param name="action">The handler to perform if a hit is found</param>
        /// <returns></returns>
        public virtual bool RayCast(Vector2 hitPointNormalized, BookActionDelegate action)
        {
            // no camera available, exit
            if (pageViewCamera == null) return false;
            // cast a ray
            RaycastHit hit;
            if (Physics.Raycast(pageViewCamera.ViewportPointToRay(hitPointNormalized), out hit, maxRayCastDistance, raycastLayerMask))
            {
                // call virtual hit method
                return HandleHit(hit, action);
            }
            return false;
        }

        /// <summary>
        /// Called when a drag event occurs on a page
        /// </summary>
        /// <param name="increment">The amount dragged since last frame</param>
        /// <param name="useInertia">Whether to use inertia or not</param>
        public virtual void Drag(Vector2 increment, bool useInertia)
        {
        }

        /// <summary>
        /// Virtual handler to take care of hits for each page view
        /// </summary>
        /// <param name="hit">The hit point information</param>
        /// <param name="action">Handler to call</param>
        /// <returns></returns>
        protected virtual bool HandleHit(RaycastHit hit, BookActionDelegate action)
        {
            return false;
        }
    }
}
