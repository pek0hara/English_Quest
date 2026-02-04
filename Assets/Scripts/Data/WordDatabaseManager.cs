using System.Collections.Generic;

/// <summary>
/// 単語データベースの統括管理クラス
/// 各レベルの単語データにアクセスするためのインターフェースを提供
/// </summary>
public static class WordDatabaseManager
{
    /// <summary>
    /// 指定レベルの単語リストを取得
    /// </summary>
    /// <param name="level">TOEICレベル（400, 600, 800）</param>
    /// <returns>該当レベルの単語リスト</returns>
    public static List<WordData> GetWordsByLevel(WordLevel level)
    {
        switch (level)
        {
            case WordLevel.Level400:
                return WordDatabase400.GetWords();
            case WordLevel.Level600:
                return WordDatabase600.GetWords();
            case WordLevel.Level800:
                return WordDatabase800.GetWords();
            default:
                return WordDatabase400.GetWords();
        }
    }

    /// <summary>
    /// 全レベルの単語を取得（300語）
    /// </summary>
    /// <returns>全単語リスト</returns>
    public static List<WordData> GetAllWords()
    {
        var allWords = new List<WordData>();
        allWords.AddRange(WordDatabase400.GetWords());
        allWords.AddRange(WordDatabase600.GetWords());
        allWords.AddRange(WordDatabase800.GetWords());
        return allWords;
    }

    /// <summary>
    /// 指定レベルからランダムに単語を取得
    /// </summary>
    /// <param name="level">TOEICレベル</param>
    /// <param name="count">取得する単語数</param>
    /// <returns>ランダムに選ばれた単語リスト</returns>
    public static List<WordData> GetRandomWords(WordLevel level, int count)
    {
        var words = GetWordsByLevel(level);
        return GetRandomFromList(words, count);
    }

    /// <summary>
    /// 全レベルからランダムに単語を取得
    /// </summary>
    /// <param name="count">取得する単語数</param>
    /// <returns>ランダムに選ばれた単語リスト</returns>
    public static List<WordData> GetRandomWordsFromAll(int count)
    {
        var allWords = GetAllWords();
        return GetRandomFromList(allWords, count);
    }

    /// <summary>
    /// 複数レベルからランダムに単語を取得
    /// </summary>
    /// <param name="levels">取得するレベルの配列</param>
    /// <param name="count">取得する単語数</param>
    /// <returns>ランダムに選ばれた単語リスト</returns>
    public static List<WordData> GetRandomWordsFromLevels(WordLevel[] levels, int count)
    {
        var words = new List<WordData>();
        foreach (var level in levels)
        {
            words.AddRange(GetWordsByLevel(level));
        }
        return GetRandomFromList(words, count);
    }

    /// <summary>
    /// 指定レベルの単語数を取得
    /// </summary>
    /// <param name="level">TOEICレベル</param>
    /// <returns>単語数</returns>
    public static int GetWordCount(WordLevel level)
    {
        return GetWordsByLevel(level).Count;
    }

    /// <summary>
    /// 全単語数を取得
    /// </summary>
    /// <returns>全単語数</returns>
    public static int GetTotalWordCount()
    {
        return GetAllWords().Count;
    }

    /// <summary>
    /// リストからランダムに要素を取得
    /// </summary>
    private static List<WordData> GetRandomFromList(List<WordData> source, int count)
    {
        if (count >= source.Count)
        {
            var result = new List<WordData>(source);
            ShuffleList(result);
            return result;
        }

        var shuffled = new List<WordData>(source);
        ShuffleList(shuffled);
        return shuffled.GetRange(0, count);
    }

    /// <summary>
    /// Fisher-Yatesアルゴリズムでリストをシャッフル
    /// </summary>
    private static void ShuffleList<T>(List<T> list)
    {
        var rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
}
