using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

#if WINDOWS_UWP
using Windows.Storage;
using Windows.System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
#endif

public class FileWriter : MonoBehaviour
{
	//define filePath
#if WINDOWS_UWP
    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
    Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
#endif

	//private string saved line;
	private string saveInformation;
	private string timeStamp;
	private string fileName;

	//private save counter
	private bool firstSave = true;
	private bool IsRecording = false;

	//Hashtable declaration
	//private static Dictionary<string, IconProperty> iconCollection = new Dictionary<string, IconProperty>();

#if WINDOWS_UWP
	async void WriteData()
	{
        if (firstSave)
		{
			fileName = System.DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "") + ".txt";

			StorageFile sampleFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
			await FileIO.AppendTextAsync(sampleFile, saveInformation + "\r\n");
			firstSave = false;
        }
		else
		{
			StorageFile sampleFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			await FileIO.AppendTextAsync(sampleFile, saveInformation + "\r\n");
		}

	}
#endif

	public void SaveInformationToFile(string information)
	{
		if (IsRecording)
		{
			saveInformation = timeStamp + " " + information;
#if WINDOWS_UWP
		WriteData();
#endif
		}
	}

	public void InitRecording()
	{
		IsRecording = true;
		firstSave = true;
	}

	void Update()
	{
		timeStamp = System.DateTime.Now.ToString();
	}
}
