#!/bin/bash
# =============================================================================
# English Quest - Unity WebGL Build Script
# =============================================================================
# Usage:
#   ./build-webgl.sh                          # 通常ビルド
#   ./build-webgl.sh --development            # 開発ビルド
#   ./build-webgl.sh --unity-path /path/to/Unity  # Unityパス指定
#   ./build-webgl.sh --output ./custom/path   # 出力先指定
# =============================================================================

set -euo pipefail

# デフォルト設定
BUILD_METHOD="WebGLBuilder.Build"
BUILD_OUTPUT="Build/WebGL"
UNITY_PATH=""
LOG_FILE="Build/build.log"
PROJECT_PATH="$(cd "$(dirname "$0")" && pwd)"

# カラー出力
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

log_info() { echo -e "${GREEN}[INFO]${NC} $1"; }
log_warn() { echo -e "${YELLOW}[WARN]${NC} $1"; }
log_error() { echo -e "${RED}[ERROR]${NC} $1"; }

# 引数解析
while [[ $# -gt 0 ]]; do
    case $1 in
        --development|-d)
            BUILD_METHOD="WebGLBuilder.BuildDevelopment"
            log_info "Development build mode enabled"
            shift
            ;;
        --unity-path|-u)
            UNITY_PATH="$2"
            shift 2
            ;;
        --output|-o)
            BUILD_OUTPUT="$2"
            shift 2
            ;;
        --help|-h)
            echo "Usage: $0 [OPTIONS]"
            echo ""
            echo "Options:"
            echo "  -d, --development       Development build with debug info"
            echo "  -u, --unity-path PATH   Path to Unity Editor executable"
            echo "  -o, --output PATH       Output directory (default: Build/WebGL)"
            echo "  -h, --help              Show this help message"
            exit 0
            ;;
        *)
            log_error "Unknown option: $1"
            exit 1
            ;;
    esac
done

# Unity実行ファイルの検出
find_unity() {
    if [[ -n "$UNITY_PATH" ]]; then
        if [[ -x "$UNITY_PATH" ]]; then
            echo "$UNITY_PATH"
            return 0
        else
            log_error "Specified Unity path not found or not executable: $UNITY_PATH"
            return 1
        fi
    fi

    # UNITY_EDITOR_PATH 環境変数
    if [[ -n "${UNITY_EDITOR_PATH:-}" ]] && [[ -x "$UNITY_EDITOR_PATH" ]]; then
        echo "$UNITY_EDITOR_PATH"
        return 0
    fi

    # プラットフォーム別の検索
    local unity_paths=()

    if [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS
        unity_paths=(
            "/Applications/Unity Hub/Unity/*/Unity.app/Contents/MacOS/Unity"
            "/Applications/Unity/Hub/Editor/*/Unity.app/Contents/MacOS/Unity"
            "/Applications/Unity/Unity.app/Contents/MacOS/Unity"
        )
    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux
        unity_paths=(
            "$HOME/Unity/Hub/Editor/*/Editor/Unity"
            "/opt/unity/Editor/Unity"
            "/opt/Unity/Hub/Editor/*/Editor/Unity"
        )
    elif [[ "$OSTYPE" == "msys" ]] || [[ "$OSTYPE" == "cygwin" ]]; then
        # Windows (Git Bash / Cygwin)
        unity_paths=(
            "C:/Program Files/Unity/Hub/Editor/*/Editor/Unity.exe"
            "C:/Program Files/Unity Hub/*/Editor/Unity.exe"
        )
    fi

    for pattern in "${unity_paths[@]}"; do
        # shellcheck disable=SC2086
        local found
        found=$(ls -d $pattern 2>/dev/null | sort -rV | head -1)
        if [[ -n "$found" ]] && [[ -x "$found" ]]; then
            echo "$found"
            return 0
        fi
    done

    # PATH内を検索
    if command -v unity &> /dev/null; then
        command -v unity
        return 0
    fi

    return 1
}

# メイン処理
log_info "========================================="
log_info " English Quest - WebGL Build"
log_info "========================================="
log_info "Project: $PROJECT_PATH"
log_info "Output:  $BUILD_OUTPUT"
log_info "Method:  $BUILD_METHOD"

# Unity検出
UNITY_EXEC=$(find_unity) || {
    log_error "Unity Editor not found!"
    log_error ""
    log_error "Please install Unity Editor or specify the path:"
    log_error "  ./build-webgl.sh --unity-path /path/to/Unity"
    log_error ""
    log_error "Or set the UNITY_EDITOR_PATH environment variable:"
    log_error "  export UNITY_EDITOR_PATH=/path/to/Unity"
    log_error ""
    log_error "Unity Hub: https://unity.com/download"
    exit 1
}

log_info "Unity found: $UNITY_EXEC"

# ビルドディレクトリ作成
mkdir -p "$(dirname "$LOG_FILE")"
mkdir -p "$BUILD_OUTPUT"

# ビルド実行
log_info "Starting WebGL build..."
log_info "Build log: $LOG_FILE"

"$UNITY_EXEC" \
    -batchmode \
    -nographics \
    -projectPath "$PROJECT_PATH" \
    -executeMethod "$BUILD_METHOD" \
    -buildPath "$BUILD_OUTPUT" \
    -buildTarget WebGL \
    -logFile "$LOG_FILE" \
    -quit

BUILD_EXIT_CODE=$?

if [[ $BUILD_EXIT_CODE -eq 0 ]]; then
    log_info "========================================="
    log_info " Build Succeeded!"
    log_info "========================================="
    log_info "Output: $BUILD_OUTPUT"

    if [[ -d "$BUILD_OUTPUT" ]]; then
        log_info "Build size: $(du -sh "$BUILD_OUTPUT" | cut -f1)"
    fi
else
    log_error "========================================="
    log_error " Build Failed! (exit code: $BUILD_EXIT_CODE)"
    log_error "========================================="
    log_error "Check the log for details: $LOG_FILE"

    if [[ -f "$LOG_FILE" ]]; then
        log_error "Last 20 lines of build log:"
        tail -20 "$LOG_FILE"
    fi

    exit $BUILD_EXIT_CODE
fi
