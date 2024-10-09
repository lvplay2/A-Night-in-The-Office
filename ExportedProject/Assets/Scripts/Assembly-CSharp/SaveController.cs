using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveController : MonoBehaviour
{
	protected static ArrayList savableOjects = new ArrayList();

	protected static GameSave lastSave;

	protected static void registrateObject(SavableObject obj)
	{
		savableOjects.Add(obj);
	}

	public static void SaveGame()
	{
		string path = Application.persistentDataPath + "/save.dat";
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(path);
		GameSave gameSave = new GameSave();
		gameSave.s1 = "s1";
		gameSave.s2 = "s22";
		gameSave.arr = new ArrayList();
		gameSave.arr.Add("sdds");
		gameSave.arr.Add("s==s");
		gameSave.arr.Add(">.<");
		binaryFormatter.Serialize(fileStream, gameSave);
		Erunda erunda = new Erunda();
		erunda.i1 = 1;
		erunda.f1 = 0.5f;
		binaryFormatter.Serialize(fileStream, erunda);
		fileStream.Close();
	}

	public static void LoadGame()
	{
		string path = Application.persistentDataPath + "/save.dat";
		if (!File.Exists(path))
		{
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(path, FileMode.Open);
		GameSave gameSave = (GameSave)binaryFormatter.Deserialize(fileStream);
		Debug.Log(gameSave.s1);
		Debug.Log(gameSave.s2);
		foreach (string item in gameSave.arr)
		{
			Debug.Log(item);
		}
		Erunda erunda = (Erunda)binaryFormatter.Deserialize(fileStream);
		Debug.Log(erunda.i1);
		Debug.Log(erunda.f1);
		fileStream.Close();
	}

	public void TestSave()
	{
		SaveGame();
	}

	public void TestLoad()
	{
		LoadGame();
	}
}
