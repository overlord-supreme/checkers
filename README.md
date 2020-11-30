



# Supreme Checkers




## Overview
- A networked Unity-2D Checkers game for Drexel's **`SE-181:`** *Intro to Software Engineering*.
- **[Click Here to Get Started](https://github.com/overlord-supreme/checkers/wiki/getting-started)**
- In general, check the **[Wiki](https://github.com/overlord-supreme/checkers/wiki)** if you have a question, or refer to one of the developers on **[Discord]** (contact `peter201943#8017` for access)




## File Structure
- This is a conglomerate of separate concerns:
  - Documentation
  - Third Party Libraries
  - Learning
- As such, the **root** file structure reflects this:
  - The **Game Files** are stored in [**`Unity`**](https://github.com/overlord-supreme/checkers/tree/master/Unity)
  - The **Document Source Code** is stored in [**`docs`**](https://github.com/overlord-supreme/checkers/tree/master/docs)
  - The **Rendered Documents** are in [**`gen`**](https://github.com/overlord-supreme/checkers/tree/master/gen)
  - The **Code Coverage** is in [`**`CodeReport`**](https://github.com/overlord-supreme/checkers/tree/master/CodeReport)
  - Within [**`Unity/Assets`**](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets), there are **two** folders
    - The **Checkers Game** in [**`Checkers`**](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers)
    - The **Networking Tutorial** in [**`RW`**](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/RW)
    - The Network Tutorial, out of a lack of time, is also where the **Networking Libraries** are located:
      - The **Core Networking** in [**`Unity/Assets/RW/Photon/PhotonUnityNetworking`**](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/RW/Photon/PhotonUnityNetworking) (*BIG* folder, lots of useful scripts)
      - The **Realtime Networking** in [**`Unity/Assets/RW/Photon/PhotonRealtime`**](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/RW/Photon/PhotonRealtime)
- There are some miscellanious folders that need to be cleaned up:
  - **[`Unity/Assets/Photon`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Photon):** Empty, nothing important in here
  - **[`Unity/Assets/StreamingAssets`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/StreamingAssets):** Again, nothing important
  - **Most of [`Unity/Assets/RW`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/RW):** There are many files in here belonging to the *Tutorial*, that are not needed for the *Checkers* game




## Game File Structure
- Each of the *Elements* of the game (*Board*, *Piece*, *Player*, *Game*, *Tests*) gets its own folder, where a *script* and/or *scene*/*prefab* is stored
- A better understanding of each class can be had by visiting the comments in the source code
- The *Elements* are:
  - **[`Board`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Board)**
    - A *Prefab* and a *Script*
    - The *Prefab* contains an `8x8` 3D Grid of Cubes with `Tile` components attached to make the "board"
    - The *Script* Handles almost everything, from *Cell Highlighting*, to *Networking*, to *Turn Control*, and so on
    - Potentially a *GodScript*, is a future issue to consider
    - Also contains the *non-GameObject* **`Space`** class, which has various stats for a cell/tile/grid/square/space on the board
  - **[`Game`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Game)**
    - Just a *Scene* with an instance of **`Board`** and many **`Pieces`**
    - Is the **"Main Scene"** that gets loaded *after* the **Launcher**
  - **[`Piece`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Piece)**
    - Nothing?
  - **[`Player`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Player)**
    - Mostly stats, such as whether the player is the *"Current"* Player
  - **[`Tests`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Tests)**
    - The *Unit*, *Integration*, and other Tests required by the course
  - **[`Launcher`](https://github.com/overlord-supreme/checkers/blob/master/Unity/Assets/Checkers/Launcher.unity)**
    - Taken from the *Tutorial*, is a simple matchmaking menu
    - This should be the **"First Scene"** that gets loaded on opening the app




## Testing
- Unity has a built-in testing framework that uses "Assembly Definition Files" to *"see"* other scripts
- These are `.json` files that must be added to whatever directories with scripts in them that you want to be able to test
- An `.asmdef` file exists in the major script locations:
  - [`Unity/Assets/Checkers/Board/Board.asmdef`](https://github.com/overlord-supreme/checkers/blob/master/Unity/Assets/Checkers/Board/Board.asmdef)
  - [`Unity/Assets/Checkers/Piece/Piece.asmdef`](https://github.com/overlord-supreme/checkers/blob/master/Unity/Assets/Checkers/Piece/Piece.asmdef)
  - [`Unity/Assets/Checkers/Player/Player.asmdef`](https://github.com/overlord-supreme/checkers/blob/master/Unity/Assets/Checkers/Player/Player.asmdef)
  - [`Unity/Assets/RW/Scripts/RW.asmdef`](https://github.com/overlord-supreme/checkers/blob/master/Unity/Assets/RW/Scripts/RW.asmdef)
- We assume that the included *Photon* libraries work, and so no `.asmdef` files have been created for them
- For more on testing inside *Unity*, [visit these pages on the wiki](https://github.com/overlord-supreme/checkers/wiki/unit-test-games)
- There are **two** kinds of tests:
  - **`EditMode`**
    - These are tests that run in the *editor*, and *not* during *play*
    - Similar to **Unit Tests**, these tests **cannot** access the *Scene*, but run faster and at any time
    - Better to test the individual methods of a class
  - **`PlayMode`**
    - These are tests that run *in game*, and *not* *in editor*
    - Similar to **Integration Tests**, these tests can access the **Scene** and talk to other **GameObjects**
    - Better to test the behavior of multiple GameObjects interacting with each other
- The Tests should be located in [`Unity/Assets/Checkers/Tests/`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Tests)
  - There are **two** subfolders, labelled [`PlayMode`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Tests/PlayMode) and [`EditMode`](https://github.com/overlord-supreme/checkers/tree/master/Unity/Assets/Checkers/Tests/EditMode)
  - There is an example test in each folder
- For Code Coverage/Static Analysis, we used [**Roslyn**](https://github.com/dotnet/roslyn) with [**Visual Studio**](https://visualstudio.microsoft.com/)




## Coding
- Each of the scripts has some documentation
- Please add your notes to them as you write them out, what issues you are having, etc




## Issues
- If you have the time, please add any persistent issues to the [*Github Issues Tracker*](https://github.com/overlord-supreme/checkers/issues)
- Otherwise, note the issue in the script location of the problem, eg on a **Method** or on a **Class** as a *comment**




## Branches
- (last updated 2020-11-28T11:55:00-05)
- **`master`:** The current branch




## Releases

### [`Version 1.0.0`](https://github.com/overlord-supreme/checkers/commit/1e4f3fb41d439980f801182d9a7f9da6739acc14)
Logic and networking code for game reached completion point.

### [`Version 1.0.1`](https://github.com/overlord-supreme/checkers/commit/0143b7c58fa4d270b07c0e51a4a4e0453dde3562)
Functionality for the sound and credits were added.

### [`Version 1.0.2`](https://github.com/overlord-supreme/checkers/commit/9c29ad27663d046658a8a1547e3f86d3ae9bd97a)
Code and sounds credits added to project. As well the functionality for implementing sounds were added.

### [`Version 1.0.3`](https://github.com/overlord-supreme/checkers/commit/f294755c8f1cfeef8f70d538bfe62dc606695989)
Piece path highlighting was added to the game.

### [`Version 1.0.4`](https://github.com/overlord-supreme/checkers/commit/6751a233b74b06ce2213f8a09c42f8e2342aa376)
Process of adding in unit tests was began.

### [`Version 1.0.5`](https://github.com/overlord-supreme/checkers/commit/acab0972bc7b98b9b97c5539584c0c0296571b47)
Added in the rest of the unit tests to the project.

### [`Version 1.0.6`](https://github.com/overlord-supreme/checkers/commit/1925bbdd4dc5a7e8a4b29b03624f0e404dc23c11)
Fixed logic issues with jumps.





## Future Work
- We need to add a **Code Reports** folder that details whether our **[Test Cases](https://docs.google.com/spreadsheets/d/1UExj35ewBux2ftpgoRd4cAy85XGLS10_uxYvNaTXQ-8/edit#gid=0)** document is passing *for each test**
- Delete `test-cases` from `docs`
- Add a `.pdf` render of the *test cases* sheet to `gen`
- Add *Feature Requests* as *Issues*
- finish linking `.asmdef`'s to test `.asmdef`'s
- Add *Overview* to Wiki
- combine meeting notes and readme for better overview
- update readme for accuracy
