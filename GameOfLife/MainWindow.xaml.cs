using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Timers;
using System.ComponentModel;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int numberOfRows;
        int numberOfColumns;
        List<Cell> cells = new List<Cell>(); //each square on grid represents  cell
        Timer mTimer; //timer necessary for automatic iterations
        Dictionary<int, string> shapes = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();
            DynamicGrid.ShowGridLines = true;
            CreateDynamicWPFGrid(60, 30); //default grid
            mTimer = new Timer(1000); //timers default speed is 1 second
            mTimer.Elapsed += delegate { iteration(cells); }; //iteration method called when 1 second elapsed

            shapes.Add(0, "Custom");
            shapes.Add(1, "The Block - Still Life");
            shapes.Add(2, "The Boat - Still Life");
            shapes.Add(3, "The Loaf - Still Life");
            shapes.Add(4, "The Beehive - Still Life");
            shapes.Add(5, "The Blinker - Oscillator");
            shapes.Add(6, "The Beacon - Oscillator");
            shapes.Add(7, "The Toad - Oscillator");
            shapes.Add(8, "The Pulsar - Oscillator");
            shapes.Add(9, "Glider");
            shapes.Add(10, "Exploder"); //add shape names to dictionary - combo box displays contents of dictionary

            shapeComboBox.ItemsSource = shapes.Values; //bind contents of combo box to dictionary values
            pauseButton.IsEnabled = false;
            stopButton.IsEnabled = false;
            speedSlider.IsEnabled = false; //functionality of these will not be necessary whilst no game is playing

         


        }
        /// <summary>
        /// Method called when play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                changeGridSizeButton.IsEnabled = false;
                pauseButton.IsEnabled = true;
                stopButton.IsEnabled = true;
                playButton.IsEnabled = false; //already playing so don't need play button functionality
                speedSlider.IsEnabled = true;  //stop, pause and speed slider now enabled as functionality may be necessary

                changeGridSizeButton.Visibility = Visibility.Visible;
                rowLabel.Visibility = Visibility.Hidden;
                columnLabel.Visibility = Visibility.Hidden;
                rowTextBox.Visibility = Visibility.Hidden;
                columnTextBox.Visibility = Visibility.Hidden;
                gridButton.Visibility = Visibility.Hidden; //make unable to change grid size at this point

                mTimer.Start(); //begin timer for automatic iteration
            }
            catch
            {
                MessageBox.Show("Unable to play game");
            }
        }
        /// <summary>
        /// Method to create the grid
        /// </summary>
        /// <param name="numberColumns"></param>
        /// <param name="numberRows"></param>
        private void CreateDynamicWPFGrid(int numberColumns, int numberRows)
        {
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.Children.Clear();
            cells.Clear(); //clear in case a grid has previously been created 

            numberOfColumns = numberColumns - 1;
            numberOfRows = numberRows - 1;  //grid row and column start at 0 so -1 for actual amount

            // Create Columns
            for (int i = 0; i < numberColumns; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            } 
            // Create Rows
            for (int i = 0; i < numberRows; i++)
            {
                DynamicGrid.RowDefinitions.Add(new RowDefinition());
            }
            //initialise cells and add to list
            for (int j = 0; j < numberColumns; j++)
            {
                for (int k = 0; k < numberRows; k++)
                {
                    cells.Add(new Cell(false, k, j)); //cells default set to dead
                }
            }

            foreach (Cell c in cells)
            {
                Grid.SetRow(c.CellAppearance, c.Row); 
                Grid.SetColumn(c.CellAppearance, c.Column); //create label for each cell
                c.CellAppearance.MouseDown += delegate { cellClicked(c); }; //call method when cell clicked
                DynamicGrid.Children.Add(c.CellAppearance); //add labels to represent cells to grid
            }

        
        }

        /// <summary>
        /// Method when button to create grid is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                numberOfRows = Convert.ToInt16(rowTextBox.Text);
                numberOfColumns = Convert.ToInt16(columnTextBox.Text); //get inputted rows and columns from UI

                if (numberOfRows < 12)
                {
                    MessageBox.Show("Please make grid greater than 13 rows");
                }
                else
                {
                    changeGridSizeButton.Visibility = Visibility.Visible;
                    rowLabel.Visibility = Visibility.Hidden;
                    columnLabel.Visibility = Visibility.Hidden;
                    rowTextBox.Visibility = Visibility.Hidden;
                    columnTextBox.Visibility = Visibility.Hidden;
                    gridButton.Visibility = Visibility.Hidden; //hide text boxes and display create grid button

                    CreateDynamicWPFGrid(numberOfColumns, numberOfRows); //create grid from user input
                }
            }
            catch { MessageBox.Show("Only numeric values should be entered"); } //in case other type of value has been inputted
            
        }

        /// <summary>
        /// Method called when cell label in grid is called
        /// </summary>
        /// <param name="mCell"></param>
        private static void cellClicked(Cell mCell)
        {
            if (mCell.Alive)
                mCell.changeCellToDead();
            else
                mCell.changeCellToAlive();
            //change cell to opposite status
        }

        /// <summary>
        /// Method to find particular cell in list of cells
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private Cell findCell(int row, int column)
        {
           try { 
                foreach (Cell c in cells) //loop through all cells in list
                {
                    if (c.Row == row && c.Column == column) //if cell matches row and column
                        return c;
                }
                return null;
            }
            catch {  return null; } //sometimes needs catching when stop button has been clicked in middle of iteration
           
        }

        /// <summary>
        /// Method for the functionality needed in each iteration
        /// </summary>
        /// <param name="mCells"></param>
        private void iteration(List<Cell> mCells)
        {
            try
            {
                foreach (Cell c in mCells) //loop through all cells in list
                {
                    int numberOfNeighbours = 0; //set number of neighbours to 0 after each iteration
                    int cellColumn = c.Column;
                    int cellRow = c.Row; //get cells row and column

                    int leftColumn = cellColumn - 1;
                    int topRow = cellRow - 1;
                    int rightColumn = cellColumn + 1;
                    int bottomRow = cellRow + 1; //get the cells surrounding rows and columns


                    if (topRow >= 0) //if top row exists
                    {
                        //check top row middle
                        try { if (findCell(topRow, cellColumn).Alive) numberOfNeighbours++; } catch { } //needed for when stop

                        //if left column exists, check top left 
                        if (leftColumn >= 0) if (findCell(topRow, leftColumn).Alive) numberOfNeighbours++;

                        //if right column exists, check top right
                        if (rightColumn <= numberOfColumns) if (findCell(topRow, rightColumn).Alive) numberOfNeighbours++;
                    }

                    if (bottomRow <= numberOfRows)
                    {
                        //check bottom row middle
                        if (findCell(bottomRow, cellColumn).Alive) numberOfNeighbours++;

                        //if left column exists, check bottom left
                        if (leftColumn >= 0) if (findCell(bottomRow, leftColumn).Alive) numberOfNeighbours++;

                        //if right column exists, check bottom right
                        if (rightColumn <= numberOfColumns) if (findCell(bottomRow, rightColumn).Alive) numberOfNeighbours++;
                    }

                    //if left column exists check left middle
                    if (leftColumn >= 0) if (findCell(cellRow, leftColumn).Alive) numberOfNeighbours++;
                    //if right column exisst check right middle
                    if (rightColumn <= numberOfColumns) if (findCell(cellRow, rightColumn).Alive) numberOfNeighbours++;


                    if (c.Alive)
                    { //if cell alive, check for underpopulation and overcrowding
                        if (numberOfNeighbours < 2 || numberOfNeighbours > 3)
                            c.ChangeFlag = true; //cells status needs to change
                    }
                    else //if cell is dead and has 3 neighbours
                    {
                        if (numberOfNeighbours == 3)
                            c.ChangeFlag = true; //cells status needs to change
                    }

                }

                foreach (Cell c in cells)
                { //loop through all cells in list
                    if (c.ChangeFlag)
                    { //if cells status needs to change
                        if (c.Alive)
                        {
                            if (c.CellAppearance.Dispatcher.CheckAccess()) //if have access to UI
                                c.changeCellToDead(); //change cell status to dead
                            else
                                this.Dispatcher.Invoke(() => { c.changeCellToDead(); }); //get access to UI and cell status changed to dead
                        } //UI access necessary as changing colour of cells label
                        else
                        {
                            if (c.CellAppearance.Dispatcher.CheckAccess()) //if access to UI
                                c.changeCellToAlive();//change cell status to alive
                            else
                                this.Dispatcher.Invoke(() => { c.changeCellToAlive(); }); //get access to UI and change cell status to alive
                        }
                        c.ChangeFlag = false; //reset flag
                    }

                }
            } catch { } //prevent crashing when stop button has been pressed
            }
        

        /// <summary>
        /// Method called when text has been altered in the text box provided to enter the number of rows in a grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (rowTextBox.Text != "")
                    columnTextBox.Text = Math.Ceiling(Convert.ToDouble(rowTextBox.Text) * 2).ToString(); //column needs to be double amount of rows to keep square grid aesthetic
            }
            catch { MessageBox.Show("Please only enter numeric value into row text box"); rowTextBox.Text = ""; }
        }

        /// <summary>
        /// Method called when text has been altered in the text box provided to enter the number of columns in a grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void columnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (columnTextBox.Text != "")
                    rowTextBox.Text = Math.Ceiling(Convert.ToDouble(columnTextBox.Text) / 2).ToString(); //rows must be half the amount of columns to keep square grid aesthetic
            }
            catch { MessageBox.Show("Please only enter numeric values into column text box"); columnTextBox.Text = ""; }
        }

        /// <summary>
        /// Method called to clear the grid of any cells that are alive
        /// </summary>
        private void clearAllGrid()
        {
            foreach (Cell c in cells)
            {
                if (c.Alive)
                    c.changeCellToDead(); //make all cells dead
            }
                
        }

        /// <summary>
        /// Method called when a selection has been made of the pattern of alive cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shapeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearAllGrid(); //clear the grid of any alive cells
            
            int middleColumn = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(numberOfColumns / 2)));
            int middleRow = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(numberOfRows / 2))); //get the middle row and column (no decimals)
            switch (shapeComboBox.SelectedIndex)  //get selected index from combo box
            { 
                case 1:
                    try { createBlockShape(middleColumn, middleRow); } catch { } //create block shape
                    break;
                    //in try catch so any issues won't break whole program
                case 2:
                    try { createBoatShape(middleColumn, middleRow); } catch { }//create boat shape
                    break;
                case 3:
                    try { createLoafShape(middleColumn, middleRow); } catch { }//create loaf shape
                    break;
                case 4:
                    try { createBeehiveShape(middleColumn, middleRow); } catch { }//create beehive shape
                    break;
                case 5:
                    try { createBlinkerShape(middleColumn, middleRow); } catch { }//create blinker shape
                    break;
                case 6:
                    try { createBeaconShape(middleColumn, middleRow); } catch { }//create beacon shape
                    break;
                case 7:
                    try { createToadShape(middleColumn, middleRow); } catch { }//create toad shape
                    break;
                case 8:
                    try { createPulsarShape(middleColumn, middleRow); } catch { }//create pulsar shape
                    break;
                case 9:
                    try { createGliderShape(middleColumn, middleRow); } catch { }//create glider shape
                    break;
                case 10:
                    try { createExploderShape(middleColumn, middleRow); } catch { }//create explorer shape
                    break;
                default:
                    break;
            }

        }

        #region Methods to create shapes

        /// <summary>
        /// Method to create exploder shape
        /// </summary>
        /// <param name="middleColumn"></param>
        /// <param name="middleRow"></param>
        private void createExploderShape(int middleColumn, int middleRow)
        {
            //exploder
            int topRow = middleRow - 1;
            int topRow2 = middleRow - 2;
            int bottomRow = middleRow + 1;
            int bottomRow2 = middleRow + 2; 

            int leftColumn2 = middleColumn - 2;
            int rightColumn2 = middleColumn + 2; //get rows and columns needed to make shape

            findCell(topRow2, leftColumn2).changeCellToAlive();
            findCell(topRow, leftColumn2).changeCellToAlive();
            findCell(middleRow, leftColumn2).changeCellToAlive();
            findCell(bottomRow, leftColumn2).changeCellToAlive();
            findCell(bottomRow2, leftColumn2).changeCellToAlive();


            findCell(topRow2, rightColumn2).changeCellToAlive();
            findCell(topRow, rightColumn2).changeCellToAlive();
            findCell(middleRow, rightColumn2).changeCellToAlive();
            findCell(bottomRow, rightColumn2).changeCellToAlive();
            findCell(bottomRow2, rightColumn2).changeCellToAlive();

            findCell(topRow2, middleColumn).changeCellToAlive();
            findCell(bottomRow2, middleColumn).changeCellToAlive(); //change neccessary cells to alive
        }

        private void createGliderShape(int middleColumn, int middleRow)
        {
            //glider
            int topRow = middleRow - 1;
            int bottomRow = middleRow + 1;
            int leftColumn = middleColumn - 1;
            int rightColumn = middleColumn + 1; //get rows and columns necessary to make shape

            findCell(topRow, middleColumn).changeCellToAlive();
            findCell(middleRow, rightColumn).changeCellToAlive();
            findCell(bottomRow, rightColumn).changeCellToAlive();
            findCell(bottomRow, middleColumn).changeCellToAlive();
            findCell(bottomRow, leftColumn).changeCellToAlive(); //change neccessary cells to alive
        }

        private void createPulsarShape(int middleColumn, int middleRow)
        {
            //pulsar
            int rightColumn = middleColumn + 1;
            int rightColumn2 = middleColumn + 2;
            int rightColumn3 = middleColumn + 3;
            int rightColumn4 = middleColumn + 4;
            int rightColumn6 = middleColumn + 6; 

            int leftColumn = middleColumn - 1;
            int leftColumn2 = middleColumn - 2;
            int leftColumn3 = middleColumn - 3;
            int leftColumn4 = middleColumn - 4;
            int leftColumn6 = middleColumn - 6;

            int topRow = middleRow - 1;
            int topRow2 = middleRow - 2;
            int topRow3 = middleRow - 3;
            int topRow4 = middleRow - 4;
            int topRow5 = middleRow - 5;
            int topRow6 = middleRow - 6;

            int bottomRow = middleRow + 1;
            int bottomRow2 = middleRow + 2;
            int bottomRow3 = middleRow + 3;
            int bottomRow4 = middleRow + 4;
            int bottomRow5 = middleRow + 5;
            int bottomRow6 = middleRow + 6; //get rows and columns necessary to make shape

            findCell(topRow2, rightColumn).changeCellToAlive();
            findCell(topRow3, rightColumn).changeCellToAlive();
            findCell(topRow4, rightColumn).changeCellToAlive();
            findCell(bottomRow2, rightColumn).changeCellToAlive();
            findCell(bottomRow3, rightColumn).changeCellToAlive();
            findCell(bottomRow4, rightColumn).changeCellToAlive();

            findCell(topRow2, leftColumn).changeCellToAlive();
            findCell(topRow3, leftColumn).changeCellToAlive();
            findCell(topRow4, leftColumn).changeCellToAlive();
            findCell(bottomRow2, leftColumn).changeCellToAlive();
            findCell(bottomRow3, leftColumn).changeCellToAlive();
            findCell(bottomRow4, leftColumn).changeCellToAlive();

            findCell(topRow6, leftColumn2).changeCellToAlive();
            findCell(topRow6, leftColumn3).changeCellToAlive();
            findCell(topRow6, leftColumn4).changeCellToAlive();
            findCell(topRow, leftColumn2).changeCellToAlive();
            findCell(topRow, leftColumn3).changeCellToAlive();
            findCell(topRow, leftColumn4).changeCellToAlive();
            findCell(bottomRow, leftColumn2).changeCellToAlive();
            findCell(bottomRow, leftColumn3).changeCellToAlive();
            findCell(bottomRow, leftColumn4).changeCellToAlive();
            findCell(bottomRow6, leftColumn2).changeCellToAlive();
            findCell(bottomRow6, leftColumn3).changeCellToAlive();
            findCell(bottomRow6, leftColumn4).changeCellToAlive();

            findCell(topRow6, rightColumn2).changeCellToAlive();
            findCell(topRow6, rightColumn3).changeCellToAlive();
            findCell(topRow6, rightColumn4).changeCellToAlive();
            findCell(topRow, rightColumn2).changeCellToAlive();
            findCell(topRow2, leftColumn6).changeCellToAlive();
            findCell(topRow3, leftColumn6).changeCellToAlive();
            findCell(topRow4, leftColumn6).changeCellToAlive();
            findCell(bottomRow2, leftColumn6).changeCellToAlive();
            findCell(bottomRow3, leftColumn6).changeCellToAlive();
            findCell(bottomRow4, leftColumn6).changeCellToAlive();
            findCell(topRow, rightColumn3).changeCellToAlive();
            findCell(topRow, rightColumn4).changeCellToAlive();
            findCell(bottomRow, rightColumn2).changeCellToAlive();
            findCell(bottomRow, rightColumn3).changeCellToAlive();
            findCell(bottomRow, rightColumn4).changeCellToAlive();
            findCell(bottomRow6, rightColumn2).changeCellToAlive();
            findCell(bottomRow6, rightColumn3).changeCellToAlive();
            findCell(bottomRow6, rightColumn4).changeCellToAlive();

            findCell(topRow2, rightColumn6).changeCellToAlive();
            findCell(topRow3, rightColumn6).changeCellToAlive();
            findCell(topRow4, rightColumn6).changeCellToAlive();
            findCell(bottomRow2, rightColumn6).changeCellToAlive();
            findCell(bottomRow3, rightColumn6).changeCellToAlive();
            findCell(bottomRow4, rightColumn6).changeCellToAlive(); //change necessary cells to alive
        }

        private void createToadShape(int middleColumn, int middleRow)
        {
            //toad
            int leftColumn = middleColumn - 1;
            int leftLeftColumn = middleColumn - 2;
            int rightColumn = middleColumn + 1;
            int bottomRow = middleRow + 1; //get rows and columns necessary to make shape

            findCell(bottomRow, leftLeftColumn).changeCellToAlive();
            findCell(bottomRow, leftColumn).changeCellToAlive();
            findCell(middleRow, leftColumn).changeCellToAlive();
            findCell(middleRow, middleColumn).changeCellToAlive();
            findCell(bottomRow, middleColumn).changeCellToAlive();
            findCell(middleRow, rightColumn).changeCellToAlive(); //change necessary cells to alive 
        }

        private void createBeaconShape(int middleColumn, int middleRow)
        {
            //beacon
            int leftColumn = middleColumn - 1;
            int leftLeftColumn = middleColumn - 2;
            int rightColumn = middleColumn + 1;
            int topRow = middleRow - 1;
            int topTopRow = middleRow - 2;
            int bottomRow = middleRow + 1; //get rows and columns necessary to make shape

            findCell(topTopRow, leftLeftColumn).changeCellToAlive();
            findCell(topTopRow, leftColumn).changeCellToAlive();
            findCell(topRow, leftLeftColumn).changeCellToAlive();
            findCell(middleRow, rightColumn).changeCellToAlive();
            findCell(bottomRow, rightColumn).changeCellToAlive();
            findCell(bottomRow, middleColumn).changeCellToAlive(); //change necessary cells to alive
        }

        private void createBlinkerShape(int middleColumn, int middleRow)
        {
            //the blinker
            int leftColumn = middleColumn - 1;
            int rightColumn = middleColumn + 1; //get rows and columns necessary to make shape

            findCell(middleRow, leftColumn).changeCellToAlive();
            findCell(middleRow, middleColumn).changeCellToAlive();
            findCell(middleRow, rightColumn).changeCellToAlive(); //change necessary cells to alive
        }

        private void createBeehiveShape(int middleColumn, int middleRow)
        {
            //the beehive
            int leftColumn = middleColumn - 1;
            int leftLeftColumn = middleColumn - 2;
            int rightColumn = middleColumn + 1;
            int topRow = middleRow - 1;
            int bottomRow = middleRow + 1; //get rows and columns necessary to make shape

            findCell(middleRow, leftLeftColumn).changeCellToAlive();
            findCell(topRow, leftColumn).changeCellToAlive();
            findCell(topRow, middleColumn).changeCellToAlive();
            findCell(middleRow, rightColumn).changeCellToAlive();
            findCell(bottomRow, middleColumn).changeCellToAlive();
            findCell(bottomRow, leftColumn).changeCellToAlive(); //change necessary cells to alive
        }

        private void createLoafShape(int middleColumn, int middleRow)
        {
            //the loaf
            int leftColumn = middleColumn - 1;
            int leftLeftColumn = middleColumn - 2;
            int rightColumn = middleColumn + 1;
            int topRow = middleRow - 1;
            int bottomRow = middleRow + 1;
            int bottomBottomRow = middleRow + 2; //get rows and columns necessary to make shape

            findCell(middleRow, rightColumn).changeCellToAlive();
            findCell(bottomRow, rightColumn).changeCellToAlive();
            findCell(bottomBottomRow, middleColumn).changeCellToAlive();
            findCell(bottomRow, leftColumn).changeCellToAlive();
            findCell(middleRow, leftLeftColumn).changeCellToAlive();
            findCell(topRow, leftColumn).changeCellToAlive();
            findCell(topRow, middleColumn).changeCellToAlive(); //change necessary cells to alive
        }

        private void createBoatShape(int middleColumn, int middleRow)
        {
            //the boat
            int leftColumn = middleColumn - 1;
            int rightColumn = middleColumn + 1;
            int topRow = middleRow - 1;
            int bottomRow = middleRow + 1; //get rows and columns necessary to make shape

            findCell(topRow, middleColumn).changeCellToAlive();
            findCell(topRow, leftColumn).changeCellToAlive();
            findCell(middleRow, leftColumn).changeCellToAlive();
            findCell(bottomRow, middleColumn).changeCellToAlive();
            findCell(middleRow, rightColumn).changeCellToAlive(); //change necessary cells to alive
        }

        private void createBlockShape(int middleColumn, int middleRow)
        {
            //the block
            int leftColumn = middleColumn - 1;
            int bottomRow = middleRow + 1; //get rows and columns necessary to make shape

            findCell(middleRow, middleColumn).changeCellToAlive();
            findCell(middleRow, leftColumn).changeCellToAlive();
            findCell(bottomRow, leftColumn).changeCellToAlive();
            findCell(bottomRow, middleColumn).changeCellToAlive(); //change necessary cells to alive
        }
        #endregion

        /// <summary>
        /// Method called when button to display grid size options is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeGridSizeButton_Click(object sender, RoutedEventArgs e)
        {
            changeGridSizeButton.Visibility = Visibility.Hidden; //hide button
            rowLabel.Visibility = Visibility.Visible;
            columnLabel.Visibility = Visibility.Visible;
            rowTextBox.Visibility = Visibility.Visible;
            columnTextBox.Visibility = Visibility.Visible;
            gridButton.Visibility = Visibility.Visible; //display options to create grid


        }

        /// <summary>
        /// Method called when pause button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pauseButton.IsEnabled = false;
                playButton.IsEnabled = true;
                speedSlider.IsEnabled = false; //disable functionality not needed and enable play button
                mTimer.Stop(); //stop the timer
            }
            catch
            {
                MessageBox.Show("Unable to pause game"); //catch if unable to pause
            }
        }

        /// <summary>
        /// Method called when stop button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try { mTimer.Stop(); } catch { }; //timer may have been stopped by pause button
                CreateDynamicWPFGrid(numberOfColumns + 1, numberOfRows + 1); //re create grid from fresh
                changeGridSizeButton.IsEnabled = true; //enable functionality to change grid size
                stopButton.IsEnabled = false;
                pauseButton.IsEnabled = false;
                speedSlider.IsEnabled = false; //disable functions no longer needed
                playButton.IsEnabled = true; //enable play button
                shapeComboBox.Text = ""; //no pattern
            }
            catch
            {
                MessageBox.Show("Unable to stop game"); //catch if unable to stop
            }
        }

        /// <summary>
        /// Method called when slider is altered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = Math.Abs(speedSlider.Value); //get positive value
            try { mTimer.Interval = value; } catch { } //only if timer has been initialised, set interval to value of slider
        }
    }



    }





