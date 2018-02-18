using System.Collections;
using System.IO;
using UnityEngine;

namespace YourProjectName
{
	public class StreamedAssetText : MonoBehaviour
	{
		public delegate void OnTextReadDelegate(string _buffer);

		private static StreamedAssetText Instance { get; set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Multiple instances of StreamedAssetText exist!");
			}

			Instance = this;
		}

		public IEnumerator ReadText(string _filePath, OnTextReadDelegate _onTextReadDelegate)
		{
			// Get the Platform's file path:
			string filePath = GenerateFilePath((Application.streamingAssetsPath), _filePath);

			// Buffer the file:
			string buffer = string.Empty;
			if (filePath.Contains("://") || filePath.Contains(":///"))
			{
				// Handle WebGL/Android path:
				WWW www = new WWW(filePath);
				yield return www;
				if (www != null)
				{
					buffer = www.text;
				}
			}
			else
			{
				// Handle local path:
				if (File.Exists(filePath))
				{
					buffer = File.ReadAllText(filePath);
				}
			}

			if (buffer != string.Empty)
			{
				_onTextReadDelegate(buffer);
			}
			else
			{
				Debug.LogWarningFormat("Failed to read text from {0}!", filePath);
			}
		}

		public static void Read(string _fileName, OnTextReadDelegate _onTextReadDelegate)
		{
			Instance.StartCoroutine(Instance.ReadText(_fileName, _onTextReadDelegate));
		}

		private string GenerateFilePath(string _path1, string _path2)
		{
			// Ensure only one '/' exists in-between the paths:
			string path1 = (_path1[_path1.Length - 1] == '/') ? _path1.TrimEnd('/') : _path1;
			string path2 = (_path2[0] == '/') ? _path2.TrimStart('/') : _path2;
			string generatedFilePath = path1 + "/" + path2;

			return (generatedFilePath);
		}
	}
}
