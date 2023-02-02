using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBG.PictureColoring
{
	[System.Serializable]
	public class LevelData
	{
		#region Inspector Variables

		public TextAsset	levelFile;
		public int			coinsToAward;
		public bool			locked;
		public int			coinsToUnlock;

		#endregion

		#region Member Variables

		private bool	levelFileParsed;
		private string	id;
		private string	assetPath;

		#endregion

		#region Properties

		public string Id
		{
			get
			{
				if (!levelFileParsed)
				{
					ParseLevelFile();
				}

				return id;
			}
		}

		public string AssetPath
		{
			get
			{
				if (!levelFileParsed)
				{
					ParseLevelFile();
				}

				return assetPath;
			}
		}

		public LevelSaveData LevelSaveData
		{
			get
			{
				return GameManager.Instance.GetLevelSaveData(Id);
			}
		}

		/// <summary>
		/// Gets the level file data, should only call this if you know the level has been loaded
		/// </summary>
		public LevelFileData LevelFileData { get { return LoadManager.Instance.GetLevelFileData(Id); } }

		#endregion

		#region Public Methods

		public bool IsColorComplete(int colorIndex)
		{
			if (LevelFileData == null)
			{
				Debug.LogError("[LevelData] IsColorRegionComplete | LevelFileData has not been loaded.");

				return false;
			}

			if (colorIndex < 0 || colorIndex >= LevelFileData.regions.Count)
			{
				Debug.LogErrorFormat("[LevelData] IsColorComplete | Given colorIndex ({0}) is out of bounds for the regions list of size {1}.", colorIndex, LevelFileData.regions.Count);

				return false;
			}

			LevelSaveData	levelSaveData	= LevelSaveData;
			List<Region>	regions			= LevelFileData.regions;

			for (int i = 0; i < regions.Count; i++)
			{
				Region region = regions[i];

				if (region.colorIndex == colorIndex && !levelSaveData.coloredRegions.Contains(region.id))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Checks if all regions have been colored
		/// </summary>
		public bool AllRegionsColored()
		{
			if (LevelFileData == null)
			{
				Debug.LogError("[LevelData] AllRegionsColored | LevelFileData has not been loaded.");

				return false;
			}

			LevelSaveData	levelSaveData	= LevelSaveData;
			List<Region>	regions			= LevelFileData.regions;

			for (int i = 0; i < regions.Count; i++)
			{
				Region region = regions[i];

				if (region.colorIndex > -1 && !levelSaveData.coloredRegions.Contains(region.id))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Gets a random region index in the given ColorRegion that has not been colored in
		/// </summary>
		public int GetSmallestUncoloredRegion(int colorIndex)
		{
			if (LevelFileData == null)
			{
				Debug.LogError("[LevelData] GetRandomUncoloredRegion | LevelFileData has not been loaded.");

				return -1;
			}

			if (colorIndex < 0 || colorIndex >= LevelFileData.regions.Count)
			{
				Debug.LogErrorFormat("[LevelData] GetRandomUncoloredRegion | Given colorRegionIndex ({0}) is out of bounds for the colorRegions list of size {1}.", colorIndex, LevelFileData.regions.Count);

				return -1;
			}

			LevelSaveData	levelSaveData	= LevelSaveData;
			List<Region>	regions			= LevelFileData.regions;

			int minRegionSize	= int.MaxValue;
			int index			= -1;

			for (int i = 0; i < regions.Count; i++)
			{
				Region region = regions[i];

				if (colorIndex == region.colorIndex && !levelSaveData.coloredRegions.Contains(region.id))
				{
					if (minRegionSize > region.numberSize)
					{
						minRegionSize	= region.numberSize;
						index			= i;
					}
				}
			}

			return index;
		}

		#endregion

		#region Private Methods

		private void ParseLevelFile()
		{
			string[] fileContents = levelFile.text.Split('\n');

			if (fileContents.Length != 2)
			{
				Debug.LogError(levelFile.name);
			}

			id			= fileContents[0].Trim();
			assetPath	= fileContents[1].Trim();

			levelFileParsed = true;
		}

		#endregion
	}
}
