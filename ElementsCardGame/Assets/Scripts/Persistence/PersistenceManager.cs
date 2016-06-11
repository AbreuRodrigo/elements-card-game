using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistenceManager {
	private const string DECK_IN_USE = "deckInUse";

	private static PersistenceManager instance;
	public static PersistenceManager Instance {
		get { 
			if(instance == null) {
				instance = new PersistenceManager ();
			}
			return instance;
		}		
	}

	private PersistenceManager() {
	}

	public void SaveData(string filePath, object data) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = File.Open (Application.persistentDataPath + "/" + filePath + ".dat", FileMode.OpenOrCreate);

		bf.Serialize (fs, data);
		fs.Close ();
	}

	public object LoadData(string filePath) {
		string path = Application.persistentDataPath + "/" + filePath + ".dat";

		if(FileExists(filePath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fs = File.Open (path, FileMode.Open);
			object data = bf.Deserialize (fs);
			fs.Close ();

			return data;
		}

		return null;
	}

	public bool FileExists(string filePath) {
		return File.Exists (Application.persistentDataPath + "/" + filePath + ".dat");
	}

	public void CreateFile(string filePath) {
		File.Create (Application.persistentDataPath + "/" + filePath + ".dat");
	}

	public int GetPlayerDeckInUse() {
		return PlayerPrefs.GetInt (DECK_IN_USE, 0);
	}

	public void SavePlayerDeckInUse(int deckInUse) {
		PlayerPrefs.SetInt (DECK_IN_USE, deckInUse);
		PlayerPrefs.Save ();
	}
}