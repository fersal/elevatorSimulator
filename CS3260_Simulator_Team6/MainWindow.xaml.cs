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

namespace CS3260_Simulator_Team6
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string ADULT = "Adult";
		private const string CHILD = "Child";

		List<IPeople> passengerList;

		public MainWindow()
		{
			InitializeComponent();
			passengerList = new List<IPeople>();
		}

		private void AddPassangerButton_Click(object sender, RoutedEventArgs e)
		{
			
			ComboBoxItem passangerTypeComboItem = (ComboBoxItem)PassangerTypeComboBox.SelectedItem;
			string passangerType = passangerTypeComboItem.Content.ToString();

			ComboBoxItem destinationItemSelected = (ComboBoxItem)DestinationFloorComboBox.SelectedItem;
			int passangerDestination = Convert.ToInt16(destinationItemSelected.Content.ToString());

			ComboBoxItem entryItemSelected = (ComboBoxItem)EntryFloorComboBox.SelectedItem;
			int passangerEntry = Convert.ToInt16(entryItemSelected.Content.ToString());

			IPeople passenger = new Passanger(passangerEntry, passangerDestination, passangerType);

			passengerList.Add(passenger);
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			var passangerDetailWindow = new PassangerDetailWindow();
			passangerDetailWindow.DetailText.Text = parsePassangerInfo();
			passangerDetailWindow.Show();
		}

		private String parsePassangerInfo()
		{
			StringBuilder stringBuilder = new StringBuilder("Passangers in Elavator: \n");
			foreach (Passanger passanger in passengerList)
			{
				if (passanger.getPassangerType() == ADULT)
				{
					stringBuilder.Append("		- Adult entered Elevator at floor " + passanger.getEntryFloor()
						+ " and will exit at floor " + passanger.getDestinationFloor());
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.Append("		- Child entered Elevator at floor " + passanger.getEntryFloor()
						+ " and will exit at floor " + passanger.getDestinationFloor());
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine();
			}

			return stringBuilder.ToString();
		}
	}
}
