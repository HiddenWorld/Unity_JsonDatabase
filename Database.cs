using System;
using System.Collections.Generic;
using UnityEngine;

namespace YourProjectName
{
	[Serializable]
	public class DatabaseInfo
	{
		[SerializeField]
		protected int id;

		public int Id { get { return id; } }
	}

	[Serializable]
	public class Database<TDerivedType, TInfoType> where TDerivedType : Database<TDerivedType, TInfoType> where TInfoType : DatabaseInfo
	{
		public static TDerivedType Instance { get; private set; }

		[SerializeField]
		protected List<TInfoType> info;

		private Dictionary<int, TInfoType> infoById;

		protected virtual void OnLoaded()
		{
			if (info == null)
			{
				Debug.LogWarningFormat("No info loaded for {0}.", typeof(TDerivedType).Name);
			}

			// Initialise and store each info object in the Info by ID dictionary:
			infoById = new Dictionary<int, TInfoType>();
			foreach (TInfoType currentInfo in info)
			{
				OnLoadedInfo(currentInfo);
				infoById.Add(currentInfo.Id, currentInfo);
			}

			Debug.LogFormat("Loaded {0}.", typeof(TDerivedType).Name);
		}

		protected virtual void OnLoadedInfo(TInfoType _info) { }

		public static void Load(string _databaseFilePath)
		{
			StreamedAssetText.Read(_databaseFilePath, LoadFromBuffer);
		}

		private static void LoadFromBuffer(string _buffer)
		{
			if (Instance != null)
			{
				Debug.LogWarningFormat("Loading {0} multiple times.", typeof(TDerivedType).Name);
			}

			// Parse the buffer to the database object:
			if (_buffer != string.Empty)
			{
				Instance = JsonUtility.FromJson<TDerivedType>(_buffer);
				Instance.OnLoaded();
			}

			if (Instance == null)
			{
				Debug.LogErrorFormat("Failed to load {0}!", typeof(TDerivedType).Name);
			}
		}

		public TInfoType GetInfoById(int _id)
		{
			TInfoType retrievedInfo;
			infoById.TryGetValue(_id, out retrievedInfo);

			if (retrievedInfo == null)
			{
				Debug.LogErrorFormat("Invalid info (ID: {0}) retrieved from {1}.", _id, typeof(TDerivedType).Name);
			}

			return (retrievedInfo);
		}

		public static TInfoType GetInfo(int _id)
		{
			return (Instance.GetInfoById(_id));
		}
	}
}
