using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
		private const string PLEASE_WAIT = "It could be up to 40 seconds to get to the 8th floor\n		PLEASE WAIT....";

		private Elevator elevator;
		private long travelTime;
		private Passenger passengerOne;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			//elevator starts on bottom floor
			//elevator waits for a Passenger to call from another floor
			//elevator gets to floor, opens door
			//passanger enters elevator, selects destination
			//elevator closes door and travels to destination floor

			var passangerDetailWindow = new PassangerDetailWindow();
			passangerDetailWindow.DetailText.Text = PLEASE_WAIT;
			passangerDetailWindow.Show();

			passengerOne = CreatePassengerFromInputs();
			ThreadStart passangerUsesElevator = new ThreadStart(PassengerUsesElevator);
			Thread passangerThread = new Thread(passangerUsesElevator);
			passangerThread.Start();

			while (passangerThread.IsAlive)
			{
				//Empty loop just to hold Details window until Passanger thread comes back with results.
			}

			passangerDetailWindow.DetailText.Text = "Took " + travelTime + " seconds to travel from floor "
				+ passengerOne.EntryFloor + " to floor " + passengerOne.DestinationFloor;
		}

		private void PassengerUsesElevator()
		{
			elevator = new Elevator();
			travelTime = passengerOne.UseElevator(elevator);
		}

		private Passenger CreatePassengerFromInputs()
		{
			ComboBoxItem passangerTypeComboItem = (ComboBoxItem)PassangerTypeComboBox.SelectedItem;
			string passangerType = passangerTypeComboItem.Content.ToString();

			ComboBoxItem destinationItemSelected = (ComboBoxItem)DestinationFloorComboBox.SelectedItem;
			int passangerDestination = Convert.ToInt16(destinationItemSelected.Content.ToString());

			ComboBoxItem entryItemSelected = (ComboBoxItem)EntryFloorComboBox.SelectedItem;
			int passangerEntry = Convert.ToInt16(entryItemSelected.Content.ToString());

			return new Passenger(passangerEntry, passangerDestination, passangerType);
		}
	}
}
