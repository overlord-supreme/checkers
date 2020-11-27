

# Supreme Checkers

## Overview
- A networked Unity-2D Checkers game for Drexel's **`SE-181:`** *Intro to Software Engineering*.
- **[Click Here to Get Started](https://github.com/overlord-supreme/checkers/wiki/getting-started)**
- In general, check the **[Wiki](FIXME)** if you have a question, or refer to one of the developers on **[Discord](FIXME)**

## File Structure
- This is a conglomerate of separate concerns:
  - Documentation
  - Third Party Libraries
  - Learning
- As such, the file structure reflects this
  - The **Game Files** are stored in [**`Unity`**](FIXME/gen)
  - The **Document Source Code** is stored in [**`docs`**](FIXME/docs)
  - The **Rendered Documents** are in [**`gen`**](FIXME/gen)
  - Within [**`Unity/Assets`**](FIXME/Unity/Assets/), there are **two** folders
    - The **Checkers Game** in [**`Checkers`**](FIXME/Unity/Assets/Checkers/)
    - The **Networking Tutorial** in [**`RW`**](FIXME/Unity/Assets/RW/)
    - The Network Tutorial, out of a lack of time, is also where the **Networking Libraries** are located:
      - The **Core Networking** in [**`Unity/Assets/RW/Photon/PhotonUnityNetworking`**](FIXME/Unity/Assets/RW/Photon/PhotonUnityNetworking/) (*BIG* folder, lots of useful scripts)
      - The **Realtime Networking** in [**`Unity/Assets/RW/Photon/Realtime`**](FIXME/Unity/Assets/RW/Photon/Realtime)
      - The **** in [**`Unity/Assets/RW/Photon/`**](FIXME/Unity/Assets/RW/Photon/)
- There are some miscellanious folders that need to be cleaned up:
  - **[`Unity/Assets/Photon`](FIXME/Unity/Assets/Photon/):** Empty, nothing important in here
  - **[`Unity/Assets/StreamingAssets`](FIXME/Unity/Assets/StreamingAssets):** Again, nothing important
  - **Most of [`Unity/Assets/RW`](FIXME/Unity/Assets/RW):** There are many files in here belonging to the *Tutorial*, that are not needed for the *Checkers* game

## Testing
- Unity has a built-in testing framework that uses "Assembly Definition Files" to *"see"* other scripts
- These are `.json` files that must be added to whatever directories with scripts in them that you want to be able to test
- An `.asmdef` file exists in the major script locations:
  - [`Unity/Assets/Checkers/Board/Board.asmdef`](FIXME/Unity/Assets/Checkers/Board/Board.asmdef)
  - [`Unity/Assets/Checkers/Board/Board.asmdef`](FIXME/Unity/Assets/Checkers/Board/Board.asmdef)
  - [`Unity/Assets/Checkers/Board/Board.asmdef`](FIXME/Unity/Assets/Checkers/Board/Board.asmdef)
  - [`Unity/Assets/RW/Scripts/RW.asmdef`](FIXME/Unity/Assets/RW/Scripts/RW.asmdef)
- We assume that the included *Photon* libraries work, and so no `.asmdef` files have been created for them
- For more on testing inside *Unity*, [visit these pages on the wiki](FIXME)
- There are **two** kinds of tests:
  - **`EditMode`**
    - These are tests that run in the *editor*, and *not* during *play*
    - Similar to **Unit Tests**, these tests **cannot** access the *Scene*, but run faster and at any time
    - Better to test the individual methods of a class
  - **`PlayMode`**
    - These are tests that run *in game*, and *not* *in editor*
    - Similar to **Integration Tests**, these tests can access the **Scene** and talk to other **GameObjects**
    - Better to test the behavior of multiple GameObjects interacting with each other
- The Tests should be located in [`Unity/Assets/Checkers/Tests/`](FIXME/Unity/Assets/Checkers/Tests/)
  - There are **two** subfolders, labelled [`PlayMode`](FIXME/Unity/Assets/Checkers/Tests/PlayMode) and [`EditMode`](FIXME/Unity/Assets/Checkers/Tests/EditMode/)
  - There is an example test in each folder

## Coding
- Each of the scripts has some documentation
- Please add your notes to them as you write them out, what issues you are having, etc

## Issues
- If you have the time, please add any persistent issues to the [*Github Issues Tracker*](FIXME)
- Otherwise, note the issue in the script location of the problem, eg on a **Method** or on a **Class** as a *comment**




