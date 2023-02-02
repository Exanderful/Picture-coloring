using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using echo17.EndlessBook.Demo02;

namespace BBG.PictureColoring
{
    public class ColorList : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private ColorListItem colorListItemPrefab = null;
        [SerializeField] private Transform colorListContainer = null;

        #endregion

        #region Member Variables

        private ObjectPool colorListItemPool;
        private List<ColorListItem> colorListItems;

        #endregion

        #region Properties

        public int SelectedColorIndex { get; set; }
        public System.Action<int> OnColorSelected { get; set; }
        public bool AllActive = true;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            colorListItemPool = new ObjectPool(colorListItemPrefab.gameObject, 1, colorListContainer);
            colorListItems = new List<ColorListItem>();
            AllActive = true;
        }

        public void Setup(int selectedColorIndex)
        {
            Clear();

            LevelData activeLevelData = GameManager.Instance.ActiveLevelData;

            if (activeLevelData != null)
            {
                // Setup each color list item
                for (int i = 0; i < activeLevelData.LevelFileData.colors.Count; i++)
                {
                    Color color = activeLevelData.LevelFileData.colors[i];
                    ColorListItem colorListItem = colorListItemPool.GetObject<ColorListItem>();

                    colorListItems.Add(colorListItem);

                    colorListItem.Setup(color, i + 1);
                    colorListItem.SetSelected(i == selectedColorIndex);

                    CheckCompleted(i);
                    DeactivateSelected(i);

                    colorListItem.Index = i;
                    colorListItem.OnListItemClicked = OnColorListItemClicked;
                }
            }

            SelectedColorIndex = selectedColorIndex;
        }

        /*public void AllDisabled()
        {
			var activeStatus = new List<bool>();
			bool allActive = false;
			bool allInactive = false;
			activeStatus = colorListContainer.Cast<Transform>().Select(child => child.gameObject.activeInHierarchy).ToList();
			allActive = activeStatus.TrueForAll(status => status == true);
            if (!allActive)
            {
				allInactive = activeStatus.TrueForAll(status => status == true);
				Debug.Log("all inactive");
            }
            else
            {
				Debug.Log("all active");
			}


			bool active = colorListContainer.Cast<Transform>().All(child => child.gameObject.activeInHierarchy);
            if (!active)
            {
				Debug.Log("all inactive");
            }
            else
            {
				Debug.Log("all active");
            }
		}*/

        public void Clear()
        {
            // Clear the list
            colorListItemPool.ReturnAllObjectsToPool();
            colorListItems.Clear();
        }

        /// <summary>
        /// Checks if the color region is completed and if so sets the ColorListItem as completed
        /// </summary>
        public void CheckCompleted(int colorIndex)
        {
            LevelData activeLevelData = GameManager.Instance.ActiveLevelData;

            if (activeLevelData != null && colorIndex < colorListItems.Count && activeLevelData.IsColorComplete(colorIndex))
            {
                colorListItems[colorIndex].SetCompleted();
            }
        }

        public void DeactivateSelected(int colorIndex)
        {
            LevelData activeLevelData = GameManager.Instance.ActiveLevelData;

            if (activeLevelData != null && colorIndex < colorListItems.Count && activeLevelData.IsColorComplete(colorIndex))
            {
                colorListItems[colorIndex].gameObject.SetActive(false);
            }
        }

        #endregion

        #region Private Methods

        private void OnColorListItemClicked(int index, object data)
        {
            if (index != SelectedColorIndex)
            {
                // Set the current selected ColorListItem to un-selected and select the new one
                colorListItems[SelectedColorIndex].SetSelected(false);
                colorListItems[index].SetSelected(true);

                SelectedColorIndex = index;

                OnColorSelected(index);
            }
        }

        #endregion
    }
}
