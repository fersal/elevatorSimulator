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

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
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

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
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

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public int GetSelectedItem()
        {
            return selectedItem;
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public FloorSelectionDialog()
        {
            InitializeComponent();
            itemsList = new List<int>();
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        private void comboBoxFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedItem = Convert.ToInt32(comboBoxFloors.SelectedItem);
            btnOkay.Focus();
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            selectedItem = Convert.ToInt32(comboBoxFloors.SelectedItem);
            Close();
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        private void comboBoxFloors_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxFloors.ItemsSource = itemsList;
            comboBoxFloors.SelectedIndex = 0;
        }
    }
}
