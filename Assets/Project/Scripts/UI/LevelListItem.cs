using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BBG.PictureColoring
{
    public class LevelListItem : RecyclableListItem<LevelData>
    {
        #region Inspector Variables

        [SerializeField] private PictureCreator pictureCreator = null;
        [SerializeField] private GameObject loadingIndicator = null;
        [SerializeField] private GameObject completedIndicator = null;
        [SerializeField] private GameObject playedIndicator = null;
        [SerializeField] private GameObject lockedIndicator = null;
        [SerializeField] private GameObject coinCostContainer = null;
        [SerializeField] private Text coinCostText = null;
        [SerializeField] private Material imageMaterial = null;

        #endregion

        #region Member Variables

        private string levelId;

        #endregion

        #region Public Methods

        public override void Initialize(LevelData dataObject)
        {
        }

        public override void Removed()
        {
            ReleaseLevel();
        }

        public override void Setup(LevelData levelData)
        {
            ReleaseLevel();

            UpdateUI(levelData);

            levelId = levelData.Id;

            bool loading = LoadManager.Instance.LoadLevel(levelData, OnLoadManagerFinished);

            if (loading)
            {
                pictureCreator.Clear();

                loadingIndicator.SetActive(true);
            }
            else
            {
                // Level already loaded
                SetImages(levelId);

                loadingIndicator.SetActive(false);
            }
        }

        public override void Refresh(LevelData levelData)
        {
            if (levelData.Id != levelId)
            {
                Setup(levelData);
            }
            else
            {
                pictureCreator.RefreshImages();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoked when the LevelLoadManager finishes loading everything needed to display the levels thumbnail
        /// </summary>
        private void OnLoadManagerFinished(string levelId, bool success)
        {
            if (success && this.levelId == levelId)
            {
                loadingIndicator.SetActive(false);

                SetImages(levelId);
            }
        }

        /// <summary>
        /// Sets the images
        /// </summary>
        private void SetImages(string levelId)
        {
            LevelFileData levelFileData = LoadManager.Instance.GetLevelFileData(levelId);

            float containerWidth = (pictureCreator.transform.parent as RectTransform).rect.width;
            float containerHeight = (pictureCreator.transform.parent as RectTransform).rect.height;
            float contentWidth = levelFileData.imageWidth;
            float contentHeight = levelFileData.imageHeight;
            float scale = Mathf.Min(containerWidth / contentWidth, containerHeight / contentHeight, 1f);

            pictureCreator.RectT.sizeDelta = new Vector2(contentWidth, contentHeight);
            pictureCreator.RectT.localScale = new Vector3(scale, scale, 1f);

            pictureCreator.Setup(levelId, imageMaterial, padding: 2);
        }

        /// <summary>
        /// Updates the UI of the list item
        /// </summary>
        private void UpdateUI(LevelData levelData)
        {
            bool isLocked = levelData.locked && !levelData.LevelSaveData.isUnlocked;
            bool isPlaying = GameManager.Instance.IsLevelPlaying(levelData.Id);
            bool isCompleted = levelData.LevelSaveData.isCompleted;

            //completedIndicator.SetActive(isCompleted);
            //playedIndicator.SetActive(!isCompleted && isPlaying);
            lockedIndicator.SetActive(isLocked);
            coinCostContainer.SetActive(isLocked);

            coinCostText.text = levelData.coinsToUnlock.ToString();
        }

        private void ReleaseLevel()
        {
            if (!string.IsNullOrEmpty(levelId))
            {
                LoadManager.Instance.ReleaseLevel(levelId);
                levelId = null;
            }
        }

        #endregion
    }
}
