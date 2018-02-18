using System;
using UnityEngine;

namespace YourProjectName
{
	[Serializable]
	public class NameInfo : DatabaseInfo
	{
		[SerializeField]
		private string name;
		
		public string Name { get { return name; } }
	}

	[Serializable]
	public class NameDatabase : Database<NameDatabase, NameInfo>
	{

	}
}
