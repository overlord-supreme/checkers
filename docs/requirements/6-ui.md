# 6. User Interface

<link href="checkers.css" rel="stylesheet"></link>




<br><br><br><br>

## Note
For this section, `html` and `css` will be used to illustrate a Checkers board and the various hints and effects anticipated on that board.




<br><br><br><br>

## Elements

The following are recognized as part of the user interface:
- The **Checker Board**
- The **Checkers Pieces**
- The **Heads-Up Display**


<br><br>

### Checkers Board


<br><br>

### Checkers Pieces
These are the individual tokens that move around on the **Checkers Board**.

They can be:
- **Hovered-Over**
- **Selected**
- **Hinted**
- **Threatened**
- **Moved**

State-changes are communicated through highlighting the **Checkers Pieces** and **Cells** on the **Checkers Board** with different **Colors**.


<br><br>

### Heads-Up Display
These are on-screen flat, 2D elements that overlay any other elements. In a 3D game, these are often ammo counters, health meters, and timers.

In *Supreme Checkers*, these are:
- The **Turn Controls**
- The **Game Clock**




<br><br><br><br>

## Blank Checkers Board

<div class="board_page_insert">
    <div class="board_scaler">
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




<br><br><br><br>

## Initial Setup Positions


<div class="board_page_insert">
    <div class="board_scaler">
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





<br><br><br><br>

## Example Highlights

<div class="board_page_insert">
    <div class="board_scaler">
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




<br><br><br><br>

## Hovering Over a Piece




<br><br><br><br>

## Selecting a Piece







