using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class Translator
{
	private static string[] DefaultNameTextAsset = new string[2] { "loc", "loc_user" };

	private static Dictionary<string, SystemLanguage> tagLanguage;

	private static Dictionary<SystemLanguage, Dictionary<string, string>> LanguageDictionary;

	private static SystemLanguage _language;

	private static bool customLanguage = false;

	private static bool inited = false;

	public static SystemLanguage Language
	{
		get
		{
			if (customLanguage)
			{
				return _language;
			}
			return Application.systemLanguage;
		}
		set
		{
			customLanguage = true;
			_language = value;
		}
	}

	public static string Translate(this string Word)
	{
		return Word.Translate(Language);
	}

	public static string Translate(this string Word, SystemLanguage Language)
	{
		if (!inited)
		{
			Init();
		}
		if (LanguageDictionary[Language].ContainsKey(Word))
		{
			return LanguageDictionary[Language][Word];
		}
		return Word;
	}

	private static void Init()
	{
		tagLanguage = new Dictionary<string, SystemLanguage>();
		LanguageDictionary = new Dictionary<SystemLanguage, Dictionary<string, string>>();
		for (int i = 0; i < DefaultNameTextAsset.Length; i++)
		{
			AddDictionary(DefaultNameTextAsset[i]);
		}
		inited = true;
	}

	public static void AddDictionary(string NameTextAsset)
	{
		TextAsset textAsset = Resources.Load<TextAsset>(NameTextAsset);
		XmlDocument xmlDocument = new XmlDocument();
		if (!textAsset)
		{
			return;
		}
		xmlDocument.LoadXml(textAsset.text);
		XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Languages");
		if (elementsByTagName.Count > 0)
		{
			foreach (XmlNode item in elementsByTagName)
			{
				XmlNodeList childNodes = item.ChildNodes;
				if (childNodes.Count <= 0)
				{
					continue;
				}
				foreach (XmlNode item2 in childNodes)
				{
					SystemLanguage systemLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), item2.Name);
					string value = item2.Attributes["tag"].Value;
					if (!tagLanguage.ContainsKey(value))
					{
						tagLanguage.Add(value, systemLanguage);
						LanguageDictionary.Add(systemLanguage, new Dictionary<string, string>());
					}
				}
			}
		}
		XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("Text");
		if (elementsByTagName2.Count <= 0)
		{
			return;
		}
		foreach (XmlNode item3 in elementsByTagName2)
		{
			XmlNodeList childNodes2 = item3.ChildNodes;
			if (childNodes2.Count <= 0)
			{
				continue;
			}
			foreach (XmlNode item4 in childNodes2)
			{
				if (!(item4.Name == "Line"))
				{
					continue;
				}
				XmlNodeList childNodes3 = item4.ChildNodes;
				foreach (XmlNode item5 in childNodes3)
				{
					if (tagLanguage.ContainsKey(item5.Name) && !LanguageDictionary[tagLanguage[item5.Name]].ContainsKey(item4.Attributes["text"].Value))
					{
						LanguageDictionary[tagLanguage[item5.Name]].Add(item4.Attributes["text"].Value, item5.InnerText);
					}
				}
			}
		}
	}
}
