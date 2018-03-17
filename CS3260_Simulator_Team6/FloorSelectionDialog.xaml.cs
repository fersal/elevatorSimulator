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
using System.Windows.Shapes;

namespace CS3260_Simulator_Team6
{
    /// <summary>
    /// Interaction logic for FloorSelectionDialog.xaml
    /// </summary>


    public partial class FloorSelectionDialog : Window
    {
        private List<int> itemsList;
        private int selectedItem;

        public List<int> ListOfFloorsInComboBox
        {
            get
            {
                return this.itemsList;
            }
            set
            {
                this.comboBoxFloors.Items.Clear();
                this.itemsList.Clear();
                foreach (var floor in value)
                {
                    itemsList.Add(floor);
                }

            }
        }

        public int SelectedFloorIndex
        {
            get
            {
                return (int)this.comboBoxFloors.SelectedItem;
            }
            set
            {
                this.comboBoxFloors.SelectedItem = value;
            }
        }

        public int GetSelectedItem()
        {
            return selectedItem;
        }

        public FloorSelectionDialog()
        {
            InitializeComponent();
            itemsList = new List<int>();
        }

        private void comboBoxFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = Convert.ToInt32(comboBoxFloors.SelectedItem);
            btnOkay.Focus();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = Convert.ToInt32(comboBoxFloors.SelectedItem);
            Close();
        }

        private void comboBoxFloors_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxFloors.ItemsSource = itemsList;
            comboBoxFloors.SelectedIndex = 0;
        }
    }
}
