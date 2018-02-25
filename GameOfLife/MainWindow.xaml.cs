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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int numberOfRows;
        int numberOfColumns;
        List<Cell> cells = new List<Cell>();
        Timer mTimer;
        Dictionary<int, string> shapes = new Dictionary<int, string>();
        public MainWindow()
        {
            InitializeComponent();
            DynamicGrid.ShowGridLines = true;
            CreateDynamicWPFGrid(60, 30); //default grid
            
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
            shapes.Add(10, "Exploder");
            shapes.Add(11, "Spaceship");

            shapeComboBox.ItemsSource = shapes.Values;


        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            mTimer = new Timer(1000);
            mTimer.Elapsed += delegate { iteration(cells); };
            mTimer.Start();
        }

        private void CreateDynamicWPFGrid(int numberColumns, int numberRows)
        {
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.RowDefinitions.Clear();
            DynamicGrid.Children.Clear();
            cells.Clear();

            numberOfColumns = numberColumns - 1;
            numberOfRows = numberRows - 1;
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

            for (int j = 0; j < numberColumns; j++)
            {
                for (int k = 0; k < numberRows; k++)
                {
                    cells.Add(new Cell(false, k, j));
                }
            }

            foreach (Cell c in cells)
            {
                Grid.SetRow(c.CellAppearance, c.Row);
                Grid.SetColumn(c.CellAppearance, c.Column);
                c.CellAppearance.MouseDown += delegate { cellClicked(c); };
                DynamicGrid.Children.Add(c.CellAppearance);
            }

        
        }

        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            numberOfRows = Convert.ToInt16(rowTextBox.Text);
            numberOfColumns = Convert.ToInt16(columnTextBox.Text);

            CreateDynamicWPFGrid(numberOfColumns, numberOfRows);
        }

        private static void cellClicked(Cell mCell)
        {
            if (mCell.Alive)
                mCell.changeCellToDead();
            else
                mCell.changeCellToAlive();

        }

        private Cell findCell(int row, int column)
        {

            foreach (Cell c in cells)
            {
                if (c.Row == row && c.Column == column)
                    return c;
            }
            return null;

        }

        private void iteration(List<Cell> mCells)
        {
            foreach (Cell c in mCells)
            {
                int numberOfNeighbours = 0;
                int cellColumn = c.Column;
                int cellRow = c.Row;

                //column and row 
                int leftColumn = cellColumn - 1;
                int topRow = cellRow - 1;
                int rightColumn = cellColumn + 1;
                int bottomRow = cellRow + 1;


                if(topRow >= 0) //if top row exists
                {
                    //check top row middle
                    if (findCell(topRow, cellColumn).Alive) numberOfNeighbours++;

                    //if left column exists, check top left 
                    if (leftColumn >= 0) if (findCell(topRow, leftColumn).Alive) numberOfNeighbours++;

                    //if right column exists, check top right
                    if (rightColumn <= numberOfColumns) if (findCell(topRow, rightColumn).Alive) numberOfNeighbours++;
                }

                if(bottomRow <= numberOfRows)
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

              
               //sort boundarys
                if (c.Alive)
                {
                    if (numberOfNeighbours < 2 || numberOfNeighbours > 3)
                        c.ChangeFlag = true;
                } else
                {
                    if (numberOfNeighbours == 3)
                        c.ChangeFlag = true;
                }
              
                }

            foreach(Cell c in cells)
            {
                if (c.ChangeFlag)
                {
                    if (c.Alive)
                    {
                        if (c.CellAppearance.Dispatcher.CheckAccess())
                            c.changeCellToDead();
                        else
                            this.Dispatcher.Invoke(() => { c.changeCellToDead(); });
                    }
                    else
                    {
                        if (c.CellAppearance.Dispatcher.CheckAccess())
                            c.changeCellToAlive();
                        else
                            this.Dispatcher.Invoke(() => { c.changeCellToAlive(); });
                    }
                    c.ChangeFlag = false;
                }
                
            }
            }

        private void rowTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(rowTextBox.Text != "")
                columnTextBox.Text = Math.Ceiling(Convert.ToDouble(rowTextBox.Text) * 2).ToString();
        }

        private void columnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (columnTextBox.Text != "")
                rowTextBox.Text = Math.Ceiling(Convert.ToDouble(columnTextBox.Text) / 2).ToString();
        }

        private void clearAllGrid()
        {
            foreach (Cell c in cells)
            {
                if (c.Alive)
                    c.changeCellToDead();
            }
                
        }

        private void shapeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clearAllGrid();
            //get middle column
            int middleColumn = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(numberOfColumns / 2)));
            int middleRow = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(numberOfRows / 2)));
            if (shapeComboBox.SelectedIndex == 1)
            {
                //the block
                int leftColumn = middleColumn - 1;
                int bottomRow = middleRow + 1;

                findCell(middleRow, middleColumn).changeCellToAlive();
                findCell(middleRow, leftColumn).changeCellToAlive();
                findCell(bottomRow, leftColumn).changeCellToAlive();
                findCell(bottomRow, middleColumn).changeCellToAlive();
            } else if (shapeComboBox.SelectedIndex == 2)
            {
                //the boat
                int leftColumn = middleColumn - 1;
                int rightColumn = middleColumn + 1;
                int topRow = middleRow - 1;
                int bottomRow = middleRow + 1;

                findCell(topRow, middleColumn).changeCellToAlive();
                findCell(topRow, leftColumn).changeCellToAlive();
                findCell(middleRow, leftColumn).changeCellToAlive();
                findCell(bottomRow, middleColumn).changeCellToAlive();
                findCell(middleRow, rightColumn).changeCellToAlive();
            } else if (shapeComboBox.SelectedIndex == 3)
            {
                int leftColumn = middleColumn - 1;
                int leftLeftColumn = middleColumn - 2;
                int rightColumn = middleColumn + 1;
                int topRow = middleRow - 1;
                int bottomRow = middleRow + 1;
                int bottomBottomRow = middleRow + 2;

                findCell(middleRow, rightColumn).changeCellToAlive();
                findCell(bottomRow, rightColumn).changeCellToAlive();
                findCell(bottomBottomRow, middleColumn).changeCellToAlive();
                findCell(bottomRow, leftColumn).changeCellToAlive();
                findCell(middleRow, leftLeftColumn).changeCellToAlive();
                findCell(topRow, leftColumn).changeCellToAlive();
                findCell(topRow, middleColumn).changeCellToAlive();
            } else if (shapeComboBox.SelectedIndex == 4)
            {
                int leftColumn = middleColumn - 1;
                int leftLeftColumn = middleColumn - 2;
                int rightColumn = middleColumn + 1;
                int topRow = middleRow - 1;
                int bottomRow = middleRow + 1;

                findCell(middleRow, leftLeftColumn).changeCellToAlive();
                findCell(topRow, leftColumn).changeCellToAlive();
                findCell(topRow, middleColumn).changeCellToAlive();
                findCell(middleRow, rightColumn).changeCellToAlive();
                findCell(bottomRow, middleColumn).changeCellToAlive();
                findCell(bottomRow, leftColumn).changeCellToAlive();
            } else if(shapeComboBox.SelectedIndex == 5)
            {
                int leftColumn = middleColumn - 1;
                int rightColumn = middleColumn + 1;

                findCell(middleRow, leftColumn).changeCellToAlive();
                findCell(middleRow, middleColumn).changeCellToAlive();
                findCell(middleRow, rightColumn).changeCellToAlive();
            } else if (shapeComboBox.SelectedIndex == 6)
            {
                int leftColumn = middleColumn - 1;
                int leftLeftColumn = middleColumn - 2;
                int rightColumn = middleColumn + 1;
                int topRow = middleRow - 1;
                int topTopRow = middleRow - 2;
                int bottomRow = middleRow + 1;

                findCell(topTopRow, leftLeftColumn).changeCellToAlive();
                findCell(topTopRow, leftColumn).changeCellToAlive();
                findCell(topRow, leftLeftColumn).changeCellToAlive();
                findCell(middleRow, rightColumn).changeCellToAlive();
                findCell(bottomRow, rightColumn).changeCellToAlive();
                findCell(bottomRow, middleColumn).changeCellToAlive();

            } else if (shapeComboBox.SelectedIndex == 7)
            {
                //toad
                int leftColumn = middleColumn - 1;
                int leftLeftColumn = middleColumn - 2;
                int rightColumn = middleColumn + 1;
                int bottomRow = middleRow + 1;

                findCell(bottomRow, leftLeftColumn).changeCellToAlive();
                findCell(bottomRow, leftColumn).changeCellToAlive();
                findCell(middleRow, leftColumn).changeCellToAlive();
                findCell(middleRow, middleColumn).changeCellToAlive();
                findCell(bottomRow, middleColumn).changeCellToAlive();
                findCell(middleRow, rightColumn).changeCellToAlive();
            } else if (shapeComboBox.SelectedIndex == 8)
            { //pulsar
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
                int bottomRow6 = middleRow + 6;

                findCell(topRow2,rightColumn).changeCellToAlive();
                findCell(topRow3, rightColumn).changeCellToAlive();
                findCell(topRow4,rightColumn).changeCellToAlive();
                findCell(bottomRow2,rightColumn).changeCellToAlive();
                findCell( bottomRow3, rightColumn).changeCellToAlive();
                findCell(bottomRow4, rightColumn).changeCellToAlive();

                findCell(topRow2, leftColumn).changeCellToAlive();
                findCell( topRow3, leftColumn).changeCellToAlive();
                findCell(topRow4, leftColumn).changeCellToAlive();
                findCell( bottomRow2, leftColumn).changeCellToAlive();
                findCell( bottomRow3,leftColumn).changeCellToAlive();
                findCell(bottomRow4,leftColumn).changeCellToAlive();

                findCell(topRow6,leftColumn2).changeCellToAlive();
                findCell(topRow6,leftColumn3).changeCellToAlive();
                findCell(topRow6,leftColumn4).changeCellToAlive();
                findCell(topRow,leftColumn2).changeCellToAlive();
                findCell(topRow,leftColumn3).changeCellToAlive();
                findCell(topRow,leftColumn4).changeCellToAlive();
                findCell(bottomRow,leftColumn2).changeCellToAlive();
                findCell(bottomRow,leftColumn3).changeCellToAlive();
                findCell(bottomRow,leftColumn4).changeCellToAlive();
                findCell(bottomRow6,leftColumn2).changeCellToAlive();
                findCell(bottomRow6,leftColumn3).changeCellToAlive();
                findCell(bottomRow6,leftColumn4).changeCellToAlive();

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
                findCell(bottomRow4, rightColumn6).changeCellToAlive();

               



            }
            
        }
    }



    }





