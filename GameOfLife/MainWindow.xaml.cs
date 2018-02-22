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
        public MainWindow()
        {
            InitializeComponent();
            DynamicGrid.ShowGridLines = true;

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {

            

        }

        private void CreateDynamicWPFGrid(int numberColumns, int numberRows)
        {
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.RowDefinitions.Clear();

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
                    cells.Add(new Cell(true, k, j));
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
            //dynamic grid children add?
            else
                mCell.changeCellToAlive();

        }

    }

}

