# 6. User Interface

<link href="checkers.css" rel="stylesheet"></link>




<br><br><br><br>

## 6.0 Note
For this section, `html` and `css` will be used to illustrate a Checkers board and the various hints and effects anticipated on that board.

HUD elements are drawn using bitmap graphics.




<br><br><br><br>
<div style="break-after:page"></div>

## 6.1 Elements

The following are recognized as part of the user interface:
- The **6.2 Checkers Board**
- The **6.3 Checkers Pieces**
- The **6.4 Menu Flow**
- The **6.5 Heads-Up Display**
- The **6.6 Main Menu**
- The **6.7 Pause Menu**
- The **6.8 Settings Menu**
- The **6.9 Lobby Menu**

Elements will reference diagrams from the  **6.9 Diagrams** section.




<br><br><br><br>
<div style="break-after:page"></div>

## 6.2 Checkers Board




<br><br><br><br>
<div style="break-after:page"></div>

## 6.3 Checkers Pieces
These are the individual tokens that move around on the **Checkers Board**.

They can be:
- **Hovered-Over**
- **Selected**
- **Hinted**
- **Threatened**
- **Moved**

State-changes are communicated through highlighting the **Checkers Pieces** and **Cells** on the **Checkers Board** with different **Colors**.




<br><br><br><br>
<div style="break-after:page"></div>

## 6.4 Menu Flow

This is how the user is anticipated to flow between each menu.

```mermaid
graph TD;
     Start_Screen-->Main_Menu_Screen;
     Main_Menu_Screen-->Exit_Game;
     Main_Menu_Screen-->Main_Menu_Settings;
     Main_Menu_Settings-->Main_Menu_Screen;
     Exit_Game-->ExitApplication
     Main_Menu_Screen-->Lobby_Screen;
     Lobby_Screen-->Create_Lobby;
     Lobby_Screen-->Join_Lobby;
     Create_Lobby-->Main_Scene;
     Join_Lobby-->Main_Scene;
     Main_Scene-->End_Scene;
     Main_Scene-->Pause_Menu;
     Pause_Menu-->Pause_Menu_Settings;
     Pause_Menu_Settings-->Pause_Menu;
     Pause_Menu-->Main_Menu_Screen;
     Pause_Menu-->Main_Scene;
     End_Scene-->Main_Menu_Screen;
```




## 6.5 Heads-Up Display
These are on-screen flat, 2D elements that overlay any other elements. In a 3D game, these are often ammo counters, health meters, and timers.

In *Supreme Checkers*, these are:
- The **Turn Controls**
- The **Game Clock**










<br><br><br><br>
<div style="break-after:page"></div>

## 6.6 Main Menu
The main menu lists 




<br><br><br><br>
<div style="break-after:page"></div>

## 6.7 Pause Menu




<br><br><br><br>
<div style="break-after:page"></div>

## 6.8 Settings
### 6.8.1 Main Menu Settings

### 6.8.2 Pause Menu Settings




<br><br><br><br>
<div style="break-after:page"></div>

## 6.9 Lobby Menus
### 6.9.1 Create Menu

### 6.9.2 Join Menu

## 6.10 Main Scene





<br><br><br><br>
<div style="break-after:page"></div>

## 6.11 Diagrams


<br><br>

### 6.11.1 Blank Checkers Board




<br><br>
<div style="break-after:page"></div>

### 6.11.2 Initial Setup Positions






<br><br>
<div style="break-after:page"></div>

### 6.11.3 Example Highlights




<br><br>
<div style="break-after:page"></div>

### 6.11.4 Hovering Over a Piece




<br><br>
<div style="break-after:page"></div>

### 6.11.5 Selecting a Piece






