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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DynamicGrid.ShowGridLines = true;
            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {

        }

         private void CreateDynamicWPFGrid(int numberColumns, int numberRows)
      
         {
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.RowDefinitions.Clear();

             // Create Columns
            for (int i = 0; i < numberColumns; i++) {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
             }
             // Create Rows
             for (int i = 0; i < numberRows; i++) {
                 DynamicGrid.RowDefinitions.Add(new RowDefinition());
             }
         }

        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            int numberOfRows = Convert.ToInt16(rowTextBox.Text);
            int numberOfColumns = Convert.ToInt16(columnTextBox.Text);

            CreateDynamicWPFGrid(numberOfColumns, numberOfRows);
        }
    }
}
