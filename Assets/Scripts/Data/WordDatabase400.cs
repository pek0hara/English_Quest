using System.Collections.Generic;

/// <summary>
/// TOEIC 400点レベル - 基礎単語（100語）
/// 日常会話・基本ビジネス用語
/// </summary>
public static class WordDatabase400
{
    public static List<WordData> GetWords()
    {
        return new List<WordData>
        {
            // 日常生活 (1-20)
            new WordData { japanese = "りんご", english = "apple" },
            new WordData { japanese = "犬", english = "dog" },
            new WordData { japanese = "猫", english = "cat" },
            new WordData { japanese = "本", english = "book" },
            new WordData { japanese = "水", english = "water" },
            new WordData { japanese = "食べ物", english = "food" },
            new WordData { japanese = "家", english = "house" },
            new WordData { japanese = "部屋", english = "room" },
            new WordData { japanese = "椅子", english = "chair" },
            new WordData { japanese = "机", english = "desk" },
            new WordData { japanese = "窓", english = "window" },
            new WordData { japanese = "ドア", english = "door" },
            new WordData { japanese = "時計", english = "clock" },
            new WordData { japanese = "電話", english = "phone" },
            new WordData { japanese = "鍵", english = "key" },
            new WordData { japanese = "財布", english = "wallet" },
            new WordData { japanese = "傘", english = "umbrella" },
            new WordData { japanese = "靴", english = "shoes" },
            new WordData { japanese = "服", english = "clothes" },
            new WordData { japanese = "帽子", english = "hat" },

            // 場所・建物 (21-40)
            new WordData { japanese = "学校", english = "school" },
            new WordData { japanese = "病院", english = "hospital" },
            new WordData { japanese = "駅", english = "station" },
            new WordData { japanese = "空港", english = "airport" },
            new WordData { japanese = "ホテル", english = "hotel" },
            new WordData { japanese = "レストラン", english = "restaurant" },
            new WordData { japanese = "店", english = "store" },
            new WordData { japanese = "銀行", english = "bank" },
            new WordData { japanese = "図書館", english = "library" },
            new WordData { japanese = "公園", english = "park" },
            new WordData { japanese = "映画館", english = "theater" },
            new WordData { japanese = "郵便局", english = "post office" },
            new WordData { japanese = "会社", english = "company" },
            new WordData { japanese = "工場", english = "factory" },
            new WordData { japanese = "市場", english = "market" },
            new WordData { japanese = "道", english = "road" },
            new WordData { japanese = "橋", english = "bridge" },
            new WordData { japanese = "海", english = "sea" },
            new WordData { japanese = "山", english = "mountain" },
            new WordData { japanese = "川", english = "river" },

            // 人物・関係 (41-55)
            new WordData { japanese = "先生", english = "teacher" },
            new WordData { japanese = "学生", english = "student" },
            new WordData { japanese = "友達", english = "friend" },
            new WordData { japanese = "家族", english = "family" },
            new WordData { japanese = "両親", english = "parents" },
            new WordData { japanese = "子供", english = "children" },
            new WordData { japanese = "医者", english = "doctor" },
            new WordData { japanese = "看護師", english = "nurse" },
            new WordData { japanese = "店員", english = "clerk" },
            new WordData { japanese = "運転手", english = "driver" },
            new WordData { japanese = "客", english = "customer" },
            new WordData { japanese = "上司", english = "boss" },
            new WordData { japanese = "同僚", english = "colleague" },
            new WordData { japanese = "隣人", english = "neighbor" },
            new WordData { japanese = "観光客", english = "tourist" },

            // 乗り物 (56-65)
            new WordData { japanese = "車", english = "car" },
            new WordData { japanese = "電車", english = "train" },
            new WordData { japanese = "バス", english = "bus" },
            new WordData { japanese = "飛行機", english = "airplane" },
            new WordData { japanese = "船", english = "ship" },
            new WordData { japanese = "自転車", english = "bicycle" },
            new WordData { japanese = "タクシー", english = "taxi" },
            new WordData { japanese = "地下鉄", english = "subway" },
            new WordData { japanese = "トラック", english = "truck" },
            new WordData { japanese = "バイク", english = "motorcycle" },

            // 時間・曜日 (66-75)
            new WordData { japanese = "今日", english = "today" },
            new WordData { japanese = "明日", english = "tomorrow" },
            new WordData { japanese = "昨日", english = "yesterday" },
            new WordData { japanese = "週末", english = "weekend" },
            new WordData { japanese = "朝", english = "morning" },
            new WordData { japanese = "午後", english = "afternoon" },
            new WordData { japanese = "夕方", english = "evening" },
            new WordData { japanese = "夜", english = "night" },
            new WordData { japanese = "分", english = "minute" },
            new WordData { japanese = "時間", english = "hour" },

            // 基本動詞・形容詞 (76-90)
            new WordData { japanese = "買う", english = "buy" },
            new WordData { japanese = "売る", english = "sell" },
            new WordData { japanese = "送る", english = "send" },
            new WordData { japanese = "届く", english = "arrive" },
            new WordData { japanese = "使う", english = "use" },
            new WordData { japanese = "作る", english = "make" },
            new WordData { japanese = "始める", english = "start" },
            new WordData { japanese = "終わる", english = "finish" },
            new WordData { japanese = "新しい", english = "new" },
            new WordData { japanese = "古い", english = "old" },
            new WordData { japanese = "大きい", english = "big" },
            new WordData { japanese = "小さい", english = "small" },
            new WordData { japanese = "高い", english = "expensive" },
            new WordData { japanese = "安い", english = "cheap" },
            new WordData { japanese = "早い", english = "early" },

            // ビジネス基礎 (91-100)
            new WordData { japanese = "仕事", english = "work" },
            new WordData { japanese = "会議", english = "meeting" },
            new WordData { japanese = "予約", english = "reservation" },
            new WordData { japanese = "注文", english = "order" },
            new WordData { japanese = "問題", english = "problem" },
            new WordData { japanese = "質問", english = "question" },
            new WordData { japanese = "答え", english = "answer" },
            new WordData { japanese = "計画", english = "plan" },
            new WordData { japanese = "価格", english = "price" },
            new WordData { japanese = "割引", english = "discount" }
        };
    }
}
