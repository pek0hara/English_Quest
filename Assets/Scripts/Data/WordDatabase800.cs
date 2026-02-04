using System.Collections.Generic;

/// <summary>
/// TOEIC 800点レベル - 上級単語（100語）
/// 専門ビジネス・抽象概念・高度な表現
/// </summary>
public static class WordDatabase800
{
    public static List<WordData> GetWords()
    {
        return new List<WordData>
        {
            // 経営・財務上級 (1-20)
            new WordData { japanese = "買収", english = "acquisition" },
            new WordData { japanese = "合併", english = "merger" },
            new WordData { japanese = "子会社", english = "subsidiary" },
            new WordData { japanese = "持株会社", english = "holding company" },
            new WordData { japanese = "株主", english = "shareholder" },
            new WordData { japanese = "利害関係者", english = "stakeholder" },
            new WordData { japanese = "監査", english = "audit" },
            new WordData { japanese = "破産", english = "bankruptcy" },
            new WordData { japanese = "債権者", english = "creditor" },
            new WordData { japanese = "債務者", english = "debtor" },
            new WordData { japanese = "担保", english = "collateral" },
            new WordData { japanese = "償還", english = "redemption" },
            new WordData { japanese = "減価償却", english = "depreciation" },
            new WordData { japanese = "資本金", english = "capital" },
            new WordData { japanese = "流動性", english = "liquidity" },
            new WordData { japanese = "収益性", english = "profitability" },
            new WordData { japanese = "透明性", english = "transparency" },
            new WordData { japanese = "説明責任", english = "accountability" },
            new WordData { japanese = "企業統治", english = "governance" },
            new WordData { japanese = "内部統制", english = "internal control" },

            // 法務・契約 (21-40)
            new WordData { japanese = "訴訟", english = "litigation" },
            new WordData { japanese = "和解", english = "settlement" },
            new WordData { japanese = "仲裁", english = "arbitration" },
            new WordData { japanese = "調停", english = "mediation" },
            new WordData { japanese = "知的財産", english = "intellectual property" },
            new WordData { japanese = "特許", english = "patent" },
            new WordData { japanese = "商標", english = "trademark" },
            new WordData { japanese = "著作権", english = "copyright" },
            new WordData { japanese = "守秘義務", english = "confidentiality" },
            new WordData { japanese = "免責条項", english = "disclaimer" },
            new WordData { japanese = "賠償", english = "compensation" },
            new WordData { japanese = "違反", english = "violation" },
            new WordData { japanese = "制裁", english = "sanction" },
            new WordData { japanese = "罰金", english = "penalty" },
            new WordData { japanese = "規制緩和", english = "deregulation" },
            new WordData { japanese = "独占禁止", english = "antitrust" },
            new WordData { japanese = "義務", english = "obligation" },
            new WordData { japanese = "権利放棄", english = "waiver" },
            new WordData { japanese = "履行", english = "fulfillment" },
            new WordData { japanese = "拘束力", english = "binding" },

            // 戦略・経営手法 (41-60)
            new WordData { japanese = "多角化", english = "diversification" },
            new WordData { japanese = "統合", english = "integration" },
            new WordData { japanese = "外部委託", english = "outsourcing" },
            new WordData { japanese = "再編", english = "restructuring" },
            new WordData { japanese = "合理化", english = "rationalization" },
            new WordData { japanese = "最適化", english = "optimization" },
            new WordData { japanese = "標準化", english = "standardization" },
            new WordData { japanese = "差別化", english = "differentiation" },
            new WordData { japanese = "相乗効果", english = "synergy" },
            new WordData { japanese = "持続可能性", english = "sustainability" },
            new WordData { japanese = "革新", english = "innovation" },
            new WordData { japanese = "破壊的", english = "disruptive" },
            new WordData { japanese = "拡張性", english = "scalability" },
            new WordData { japanese = "柔軟性", english = "flexibility" },
            new WordData { japanese = "適応性", english = "adaptability" },
            new WordData { japanese = "回復力", english = "resilience" },
            new WordData { japanese = "即応性", english = "agility" },
            new WordData { japanese = "基準", english = "benchmark" },
            new WordData { japanese = "指標", english = "indicator" },
            new WordData { japanese = "測定基準", english = "metrics" },

            // 専門・技術用語 (61-80)
            new WordData { japanese = "インフラ", english = "infrastructure" },
            new WordData { japanese = "帯域幅", english = "bandwidth" },
            new WordData { japanese = "暗号化", english = "encryption" },
            new WordData { japanese = "認証", english = "authentication" },
            new WordData { japanese = "冗長性", english = "redundancy" },
            new WordData { japanese = "互換性", english = "compatibility" },
            new WordData { japanese = "試作品", english = "prototype" },
            new WordData { japanese = "実現可能性", english = "feasibility" },
            new WordData { japanese = "仕様", english = "specification" },
            new WordData { japanese = "統合", english = "integration" },
            new WordData { japanese = "移行", english = "migration" },
            new WordData { japanese = "展開", english = "deployment" },
            new WordData { japanese = "保守性", english = "maintainability" },
            new WordData { japanese = "相互運用性", english = "interoperability" },
            new WordData { japanese = "自動化", english = "automation" },
            new WordData { japanese = "仮想化", english = "virtualization" },
            new WordData { japanese = "処理能力", english = "throughput" },
            new WordData { japanese = "待ち時間", english = "latency" },
            new WordData { japanese = "障害", english = "malfunction" },
            new WordData { japanese = "復旧", english = "recovery" },

            // 抽象概念・高度表現 (81-100)
            new WordData { japanese = "包括的", english = "comprehensive" },
            new WordData { japanese = "暫定的", english = "provisional" },
            new WordData { japanese = "相互の", english = "mutual" },
            new WordData { japanese = "累積的", english = "cumulative" },
            new WordData { japanese = "付随的", english = "incidental" },
            new WordData { japanese = "不可欠な", english = "indispensable" },
            new WordData { japanese = "固有の", english = "inherent" },
            new WordData { japanese = "潜在的", english = "potential" },
            new WordData { japanese = "明示的", english = "explicit" },
            new WordData { japanese = "暗黙の", english = "implicit" },
            new WordData { japanese = "一貫した", english = "consistent" },
            new WordData { japanese = "断続的", english = "intermittent" },
            new WordData { japanese = "同時の", english = "simultaneous" },
            new WordData { japanese = "継続的", english = "continuous" },
            new WordData { japanese = "段階的", english = "gradual" },
            new WordData { japanese = "即時の", english = "immediate" },
            new WordData { japanese = "恒久的", english = "permanent" },
            new WordData { japanese = "一時的", english = "temporary" },
            new WordData { japanese = "相対的", english = "relative" },
            new WordData { japanese = "絶対的", english = "absolute" }
        };
    }
}
