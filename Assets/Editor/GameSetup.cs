using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameSetup
{
    [MenuItem("Tools/Setup Game")]
    public static void Setup()
    {
        // 1. Create Word List
        WordList wordList = CreateWordList();

        // 2. Setup Scene
        GameObject canvasObj = SetupCanvas();
        GameObject eventSystem = SetupEventSystem();

        // 3. Create Prefabs
        GameObject cardPrefab = CreateCardPrefab(canvasObj);
        GameObject slotPrefab = CreateSlotPrefab(canvasObj);

        // 4. Setup Grid
        GameObject gridObj = SetupGrid(canvasObj);

        // 5. Setup UI
        Text timerText = CreateText(canvasObj, "TimerText", "Time: 10", new Vector2(0, 200));
        Text statusText = CreateText(canvasObj, "StatusText", "Memorize...", new Vector2(0, 250));
        Button checkButton = CreateButton(canvasObj, "CheckButton", "Check Answer", new Vector2(0, -200));

        // 6. Setup GameManager
        SetupGameManager(canvasObj, wordList, cardPrefab, slotPrefab, gridObj, timerText, statusText, checkButton);

        Debug.Log("Game Setup Complete!");
    }

    private static WordList CreateWordList()
    {
        string path = "Assets/SampleWordList.asset";
        WordList asset = AssetDatabase.LoadAssetAtPath<WordList>(path);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<WordList>();
            asset.words = new List<WordData>
            {
                new WordData { japanese = "りんご", english = "Apple" },
                new WordData { japanese = "犬", english = "Dog" },
                new WordData { japanese = "猫", english = "Cat" },
                new WordData { japanese = "本", english = "Book" },
                new WordData { japanese = "学校", english = "School" },
                new WordData { japanese = "水", english = "Water" },
                new WordData { japanese = "車", english = "Car" },
                new WordData { japanese = "電車", english = "Train" },
                new WordData { japanese = "飛行機", english = "Airplane" },
                new WordData { japanese = "コンピュータ", english = "Computer" },
                new WordData { japanese = "先生", english = "Teacher" },
                new WordData { japanese = "学生", english = "Student" },
                new WordData { japanese = "友達", english = "Friend" },
                new WordData { japanese = "家族", english = "Family" },
                new WordData { japanese = "家", english = "House" }
            };
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        }
        return asset;
    }

    private static GameObject SetupCanvas()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        return canvasObj;
    }

    private static GameObject SetupEventSystem()
    {
        GameObject es = GameObject.Find("EventSystem");
        if (es == null)
        {
            es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        return es;
    }

    private static GameObject CreateCardPrefab(GameObject canvas)
    {
        if (!Directory.Exists("Assets/Resources")) Directory.CreateDirectory("Assets/Resources");

        // Check if prefab already exists to avoid overwriting blindly or error
        string path = "Assets/Resources/Card.prefab";
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        // Create temporary object in scene
        GameObject cardObj = new GameObject("Card");
        cardObj.transform.SetParent(canvas.transform, false);

        Image img = cardObj.AddComponent<Image>();
        img.color = Color.white;
        cardObj.AddComponent<CanvasGroup>();
        DraggableCard draggable = cardObj.AddComponent<DraggableCard>();

        // Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(cardObj.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = "Word";
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.rectTransform.sizeDelta = new Vector2(100, 100);
        // Default font
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        draggable.wordText = text;

        // Save as prefab
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(cardObj, path);

        // Destroy temporary
        Object.DestroyImmediate(cardObj);

        return prefab;
    }

    private static GameObject CreateSlotPrefab(GameObject canvas)
    {
        if (!Directory.Exists("Assets/Resources")) Directory.CreateDirectory("Assets/Resources");

        string path = "Assets/Resources/Slot.prefab";
        GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        GameObject slotObj = new GameObject("Slot");
        slotObj.transform.SetParent(canvas.transform, false);

        Image img = slotObj.AddComponent<Image>();
        img.color = new Color(0.8f, 0.8f, 0.8f, 1f);

        slotObj.AddComponent<CardSlot>();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(slotObj, path);

        Object.DestroyImmediate(slotObj);

        return prefab;
    }

    private static GameObject SetupGrid(GameObject canvas)
    {
        GameObject grid = GameObject.Find("Grid");
        if (grid == null)
        {
            grid = new GameObject("Grid");
            grid.transform.SetParent(canvas.transform, false);
            GridLayoutGroup glg = grid.AddComponent<GridLayoutGroup>();
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = 3;
            glg.cellSize = new Vector2(100, 100);
            glg.spacing = new Vector2(10, 10);
            glg.childAlignment = TextAnchor.MiddleCenter;

            RectTransform rt = grid.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(400, 600);
        }
        return grid;
    }

    private static Text CreateText(GameObject canvas, string name, string content, Vector2 pos)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            obj = new GameObject(name);
            obj.transform.SetParent(canvas.transform, false);
            Text t = obj.AddComponent<Text>();
            t.text = content;
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.alignment = TextAnchor.MiddleCenter;
            t.color = Color.black;
            t.fontSize = 24;

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(300, 50);
        }
        return obj.GetComponent<Text>();
    }

    private static Button CreateButton(GameObject canvas, string name, string content, Vector2 pos)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            obj = new GameObject(name);
            obj.transform.SetParent(canvas.transform, false);
            Image img = obj.AddComponent<Image>();
            img.color = Color.cyan;

            Button btn = obj.AddComponent<Button>();

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(obj.transform, false);
            Text t = textObj.AddComponent<Text>();
            t.text = content;
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.alignment = TextAnchor.MiddleCenter;
            t.color = Color.black;
            t.fontSize = 20;

             RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(160, 50);
        }
        return obj.GetComponent<Button>();
    }

    private static void SetupGameManager(GameObject canvas, WordList wordList, GameObject cardPrefab, GameObject slotPrefab, GameObject grid, Text timer, Text status, Button button)
    {
        GameObject gmObj = GameObject.Find("GameManager");
        if (gmObj == null)
        {
            gmObj = new GameObject("GameManager");
        }

        GameManager gm = gmObj.GetComponent<GameManager>();
        if (gm == null) gm = gmObj.AddComponent<GameManager>();

        gm.wordList = wordList;
        gm.cardPrefab = cardPrefab;
        gm.slotPrefab = slotPrefab;
        gm.gridParent = grid.transform;
        gm.timerText = timer;
        gm.statusText = status;
        gm.checkButton = button;
    }
}
