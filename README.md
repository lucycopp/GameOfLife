# GameOfLife

The Game of Life is zero player game known as an 'cellular automaton'. This means that rules are applied to cells and their neighbours in a grid, causing a variety of outcomes.
It is based on mathematician, John Conways,  Game of Life. The rules cause a large variety of interesting patterns to evolve.
The game is set on a 2D grid, in which each square a cell inhibits. These cells interact with their neighbours, and their state is determined after each iteration.
The following rules are applied to each cell:

- Underpopulation - If an alive cell has less than two neighbours it will die
- Overcrowding - If an alive cell has more than three neighbours it will die
- Survival - If a an alive cell has two or three neighbours it will live
- Creation of Life - When an empty cell has three neighbours, a cell will be created

The neighbours considered in each iteration, are the cells horizontally, vertically and diagonally adjacent to the cell.

# How to Use and Controls

This is the screen you will be presented with when opening the game:  </br>
<img src="https://i.imgur.com/wd05qOR.jpg" width="350"/>


<b>Controls</b>

'Change Grid' Button - Click to access settings in which you can alter the size of the grid<br/>
'Pause' Button - Click the pause button to pause the game at any point </br>
'Play' Button - Click the play button to start the game and begin the iterations </br>
'Stop' Button - Click the stop button to totally stop and restart the game </br>
'Speed' Slider - Alter the speed slider to alter the speed of the game </br>
'Pattern' Combo box - Select a pattern from the drop down box to add the pattern to the grid </br>

<b>Playing the Game</b>

In order to play the game, it is first advised to select a pattern from the pattern drop down box: </br>
<img src="https://i.imgur.com/6I7gZQV.jpg" width="350"/>

Once a pattern has been selected, it should appear on the grid. Then click the play button: </br>
<img src="https://i.imgur.com/efd9zqc.jpg" width="350"/>

The game should now be playing , and iterations should be occuring approximatly every one second. To change the speed, use the speed slider. Slide to the right to speed the iterations up, and slide to the left to slow them down. The minimum speed is iterations every 1.8seconds and the maximum speed is iterations every 0.3seconds: </br>
<img src="https://i.imgur.com/5c9RyTK.jpg" width="350"/>

To pause the game, press the pause button. This will stop the game, and if you press play it will resume from its last point: </br>
<img src="https://i.imgur.com/k0Y4y5X.jpg" width="350"/>

To stop the game, press the stop button. This will stop the game, clear the board and restart the game. </br>
<img src="https://i.imgur.com/0cvyUHX.jpg" width="350"/>

<b>Changing the Grid Size</b>

The game begins with a default grid (30 rows, 60 columns) which is suitable for all patterns and speeds. When changing the size of the grid, it must be greater than 13 rows otherwise not all patterns will fit onto the grid. It is not advised to make the grid too large e.g. 50 rows, 100 columns as this can cause performance issues. Also, the larger the grid, the less likely the game will be able to handle greater speeds. </br>

To change the grid size, click the button labelled 'Change Grid Size': </br>
<img src="https://i.imgur.com/yKHgOKJ.jpg"/>

Once pressed, setting will appear which will allow the rows and columns to be inputted. Please note, as you enter a value into the row text box, double the value will appear column box and vice versa. This occurs as there should be double the amount of columns than rows to keep the square grid aesthetic: </br>
<img src="https://i.imgur.com/Jz2xB0G.jpg" width="350"/>

Once the values have been entered, click the button labelled 'Create Grid'. The current grid will then be replaced by a grid with the new measurements. The settings will also disapear. </br>
<img src="https://i.imgur.com/1KuqpHv.jpg" width="350"/>


# Patterns Included 

The patterns included in the game are common patterns associated with Conways Game of Life. When a pattern has been selected, the pattern is displayed in the middle of the grid. Press play to see how the pattern evolves.

<b> - Custom Pattern </b></br>
This is where a custom pattern can be created. Simply click the cells in which you would like to create life, and once the pattern has been created, click play! 

<b> - The Block </b></br>
This is a still life pattern which does not change as it evolves:</br>
<img src="https://i.imgur.com/lFAXFvZ.jpg" width="350"/> </br></br>
<b> - The Boat </b></br>
This is a still life pattern which does not change as it evolves:</br>
<img src="https://i.imgur.com/3JdtBpS.jpg" width="350"/></br></br>
<b> - The Loaf </b></br>
This is a still life pattern which does not change as it evolves:</br>
<img src="https://i.imgur.com/KgGWqjJ.jpg" width="350"/></br></br>
<b> - The Beehive </b></br>
This is a still life pattern which does not change as it evolves:</br>
<img src="https://i.imgur.com/pyX9Uo4.jpg" width="350"/></br></br>
<b> - The Blinker </b></br>
This is an oscillating pattern which flicks between two patterns as it evolves:</br>
<img src="https://i.imgur.com/SbxUbSi.jpg" width="350"/></br></br>
<b> - The Beacon </b></br>
This is an oscillating pattern which flicks between two patterns as it evolves:</br>
<img src="https://i.imgur.com/Hj5TFCR.jpg" width="350"/></br></br>
<b> - The Toad </b></br>
This is an oscillating pattern which flicks between two patterns as it evolves:</br>
<img src="https://i.imgur.com/Xda00o1.jpg" width="350"/></br></br>
<b> - The Pulsar </b></br>
This is an oscillating pattern which flicks between two patterns as it evolves:</br>
<img src="https://i.imgur.com/jd7Y5Hh.jpg" width="350"/></br></br>
<b> - The Glider </b></br>
This pattern moves across the screen as it evolves:</br>
<img src="https://i.imgur.com/2LV2etj.jpg" width="350"/></br></br>
<b> - The Exploder </b></br>
This pattern presents a series of patterns as it evolves:</br>
<img src="https://i.imgur.com/LAhCHZ8.jpg" width="350"/></br></br>









