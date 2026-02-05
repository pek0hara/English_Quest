# Unity English Word Learning App

This project contains the scripts to create a Unity-based English word learning application.
The app displays a 3x3 grid of Japanese words for 10 seconds (Memorize Phase), then switches to randomized English words that the user must rearrange to match the original positions (Solve Phase).

## Setup Instructions

### Automated Setup
To quickly set up the game:
1. Open the project in Unity.
2. Click **Tools** -> **Setup Game** in the menu bar.
3. This will create the necessary assets, scene objects, and configurations automatically.
4. Press Play to run.

### Manual Setup
Follow these steps if you wish to set up the project manually.

### 1. Create a Unity Project
1. Open Unity Hub and create a new **2D** project.
2. Copy the `Assets` folder from this repository into your Unity project's root folder.

### 2. Prepare Prefabs

#### Card Prefab
1. In the Hierarchy, right-click -> UI -> Image. Name it `Card`.
2. Add a **Text** child to `Card` (Right-click `Card` -> UI -> Text). Adjust size to fill the card.
3. Add the following components to the `Card` object:
   - `Canvas Group`
   - `DraggableCard` script
4. Drag the `Text` object to the `Word Text` field of the `DraggableCard` component.
5. Drag the `Card` object from Hierarchy to `Assets/Resources` (or any folder) to create a Prefab.
6. Delete the `Card` from Hierarchy.

#### Slot Prefab
1. In the Hierarchy, right-click -> UI -> Image. Name it `Slot`.
2. Set the color to something distinct (e.g., grey) or make it transparent.
3. Add the `CardSlot` script to the `Slot` object.
4. Drag the `Slot` object to `Assets/Resources` to create a Prefab.
5. Delete the `Slot` from Hierarchy.

### 3. Setup Scene

1. **Canvas**: Ensure there is a Canvas in the scene.
2. **GameManager**: Create an empty GameObject named `GameManager`. Attach the `GameManager` script.
3. **Grid**: Inside the Canvas, create an Empty GameObject named `Grid`.
   - Add a `Grid Layout Group` component.
   - Set `Constraint` to **Fixed Column Count** and set `Constraint Count` to **3**.
   - Adjust `Cell Size` and `Spacing` as needed (e.g., 200x100).
4. **UI Elements**: Create UI Text for Timer and Status, and a Button for "Check Answer".

### 4. Configure GameManager
Select the `GameManager` object and assign the references in the Inspector:
- **Slot Prefab**: Drag the `Slot` prefab.
- **Card Prefab**: Drag the `Card` prefab.
- **Grid Parent**: Drag the `Grid` object from the Hierarchy.
- **Timer Text**: Drag the Timer Text object.
- **Status Text**: Drag the Status Text object.
- **Check Button**: Drag the Check Button object.
- **Word List**: (See below)

### 5. Create Word Data
1. In the Project window, right-click -> Create -> Word List.
2. Select the new `NewWordList` file.
3. In the Inspector, set the Size of `Words` to **9** (or more).
4. Fill in the Japanese and English fields for each element.
5. Drag this `NewWordList` asset to the `Word List` field in the `GameManager`.

### 6. Run
Press Play in the Unity Editor.
- **Memorize Phase**: You will see Japanese words. Wait 10 seconds.
- **Solve Phase**: Words change to English and positions shuffle. Drag and drop cards to rearrange them.
- **Check**: Click the button to verify your answer. Green indicates correct position, Red indicates incorrect.
