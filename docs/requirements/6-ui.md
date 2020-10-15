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
- The **6.4 Heads-Up Display**
- The **6.5 Main Menu**
- The **6.6 Pause Menu**
- The **6.7 Settings Menu**

Elements will reference diagrams from the  **6.8 Diagrams** section.




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

## 6.4 Heads-Up Display
These are on-screen flat, 2D elements that overlay any other elements. In a 3D game, these are often ammo counters, health meters, and timers.

In *Supreme Checkers*, these are:
- The **Turn Controls**
- The **Game Clock**




<br><br><br><br>
<div style="break-after:page"></div>

## 6.8 Diagrams


<br><br>

### Blank Checkers Board

<div style="width:100%;height:800px;">
    <div style="width:80%;display:block;position:relative;margin:auto;float:left;">
        <table class="board" cellpadding="0" cellspacing="0">
            <tr>
                <td ><p class="letter">8</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">7</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">6</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">5</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">4</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">3</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">2</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">1</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td><p class="letter">A</p></td>
                <td><p class="letter">B</p></td>
                <td><p class="letter">C</p></td>
                <td><p class="letter">D</p></td>
                <td><p class="letter">E</p></td>
                <td><p class="letter">F</p></td>
                <td><p class="letter">G</p></td>
                <td><p class="letter">H</p></td>
            </tr>
        </table>
    </div>
</div>




<br><br>
<div style="break-after:page"></div>

### Initial Setup Positions


<div style="width:100%;height:800px;">
    <div style="width:80%;display:block;position:relative;margin:auto;float:left;">
        <table class="board" cellpadding="0" cellspacing="0">
            <tr>
                <td ><p class="letter">8</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
            </tr>
            <tr>
                <td><p class="letter">7</p></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p><p class="black">&#9922;</p></td>
                <td></td>
                <td><p><p class="black">&#9922;</p></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">6</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
            </tr>
            <tr>
                <td><p class="letter">5</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">4</p></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">3</p></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">2</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
            </tr>
            <tr>
                <td><p class="letter">1</p></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td><p class="letter">A</p></td>
                <td><p class="letter">B</p></td>
                <td><p class="letter">C</p></td>
                <td><p class="letter">D</p></td>
                <td><p class="letter">E</p></td>
                <td><p class="letter">F</p></td>
                <td><p class="letter">G</p></td>
                <td><p class="letter">H</p></td>
            </tr>
        </table>
    </div>
</div>





<br><br>
<div style="break-after:page"></div>

### Example Highlights

<div style="width:100%;height:800px;">
    <div style="width:80%;display:block;position:relative;margin:auto;float:left;">
        <table class="board" cellpadding="0" cellspacing="0">
            <tr>
                <td ><p class="letter">8</p></td>
                <td></td>
                <td><p class="black threatened">&#9922;</p></td>
                <td></td>
                <td class="selected"><p class="black selected">&#9922;</p></td>
                <td></td>
                <td><p class="black hovered">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
            </tr>
            <tr>
                <td><p class="letter">7</p></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black selected">&#9922;</p></td>
                <td></td>
                <td class="threatened"><p><p class="black">&#9922;</p></td>
                <td></td>
                <td><p><p class="black">&#9922;</p></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">6</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
                <td></td>
                <td><p class="black">&#9922;</p></td>
            </tr>
            <tr>
                <td><p class="letter">5</p></td>
                <td class="hovered"></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">4</p></td>
                <td></td>
                <td class="selected"></td>
                <td></td>
                <td></td>
                <td class="threatened"></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">3</p></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
            </tr>
            <tr>
                <td><p class="letter">2</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white selected">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
            </tr>
            <tr>
                <td><p class="letter">1</p></td>
                <td><p class="white threatened">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white">&#9922;</p></td>
                <td></td>
                <td><p class="white hovered">&#9922;</p></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td><p class="letter">A</p></td>
                <td><p class="letter">B</p></td>
                <td><p class="letter">C</p></td>
                <td><p class="letter">D</p></td>
                <td><p class="letter">E</p></td>
                <td><p class="letter">F</p></td>
                <td><p class="letter">G</p></td>
                <td><p class="letter">H</p></td>
            </tr>
        </table>
    </div>
</div>


<br><br>
<div style="break-after:page"></div>

### Hovering Over a Piece


<br><br>
<div style="break-after:page"></div>

### Selecting a Piece







