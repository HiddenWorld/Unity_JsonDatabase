# Unity_JsonDatabase

Provides a means of creating and easily accessing database information via JSON files.  

Use the example files to see how you can create your own database.  
Make sure to use the Load function from wherever you want to load the database (eg. within a GameController script's Awake or Start function), for example:  
```
private void Awake()
{
	NameDatabase.Load("Databases/ExampleNameDatabase.json");
}
```

You can override the 'OnLoadedInfo' function in your custom database class to do some form of initialisation for each info loaded (like storing item info to a dictionary based on item type), for example:  
```
Dictionary<string, List<ItemInfo>> itemInfoByType;

protected override void OnLoadedInfo(ItemInfo _info)
{
	List<ItemInfo> typeItemInfo;
	if (itemInfoByType.TryGetValue(_info.Type, out typeItemInfo) == false)
	{
		typeItemInfo = new List<ItemInfo>();
		itemInfoByType.Add(_info.Type, typeItemInfo);
	}
  
	typeItemInfo.Add(_info);
}
```
