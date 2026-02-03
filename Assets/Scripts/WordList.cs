using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWordList", menuName = "Word List")]
public class WordList : ScriptableObject
{
    public List<WordData> words = new List<WordData>();
}
