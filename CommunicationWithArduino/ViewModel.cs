using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWithArduino
{
    internal class ViewModel : INotifyPropertyChanged
    {
        /*
         * ViewModel to connect to an Arduino via serial connection
         */
        SerialPort port;


        //Collection property to save all available comports.
        //Gets displayed in a combox for the user to select one.
        private ObservableCollection<string> _comPorts;
        public ObservableCollection<string> ComPorts
        {
            get { return _comPorts; }
            set
            {
                _comPorts = value;
                OnPropertyChanged(nameof(ComPorts));
            }
        }

        //Take out the selected comport from the combobox
        public string SelectedComPort { get; set; }
        
        //Property used to change the text in the connect/disconnet button
        private string _btnName;
        public string BtnName
        {
            get { return _btnName; }
            set { _btnName = value;
                OnPropertyChanged(nameof(BtnName));
                }
        }

        //Display the temp value from Arduino
        private string _tempC;
        public string TempC
        {
            get { return _tempC; }
            set
            {
                _tempC = value;
                OnPropertyChanged(nameof(TempC));
            }
        }

        //Display the chosen value, mirrored back from the Arduino
        private string _tempCsat;
        public string TempCsat
        {
            get { return _tempCsat; }
            set
            {
                _tempCsat = value;
                OnPropertyChanged(nameof(TempCsat));
            }
        }

        //Text from the Textbox (wpf) to write temp value
        private string _tempInput;
        public string TempInput
        {
            get { return _tempInput; }
            set
            {
                _tempInput = value;
                OnPropertyChanged(nameof(TempInput));
            }
        }
        
        //Following 3 bools are used to see what the system attached to the Arduino is doing
        private bool _vcheck;
        public bool Vcheck
        {
            get { return _vcheck; }
            set
            {
                _vcheck = value;
                OnPropertyChanged(nameof(Vcheck));
            }
        }
        private bool _kcheck;
        public bool Kcheck
        {
            get { return _kcheck; }
            set
            {
                _kcheck = value;
                OnPropertyChanged(nameof(Kcheck));
            }
        }
        private bool _ocheck;
        public bool Ocheck
        {
            get { return _ocheck; }
            set
            {
                _ocheck = value;
                OnPropertyChanged(nameof(Ocheck));
            }
        }


        public ViewModel()
        {
            BtnName = "Connect";    //Set the button to display connect

            ComPorts = new ObservableCollection<string>();

            //On program start get all the available comport (active)
            foreach (string port in SerialPort.GetPortNames())
            {
                ComPorts.Add(port);
            }
        }

        /// <summary>
        /// Method used to convert 0 or 1 to false and true
        /// </summary>
        /// <param name="input">String variable containung either 0 or 1</param>
        /// <returns></returns>
        public bool SetTrueOrFalse(string input)
        {
            if(input == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Handling of the data received on a serial connection
        /// Check on received data if first entry is a 'D'
        /// If yes it is assumed a correct data packet is received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataReceivedOnSerial(object sender, SerialDataReceivedEventArgs e)
        {
            string serialData = port.ReadLine();
            if (serialData[0] == 'D')
            {
                //Data received is , seperated and needs to be split
                string[] sorteretData = serialData.Split(new char[] { ',' });

                /* Received data is:
                 * 'D'
                 * Temperature
                 * Cooling on/off
                 * Heating on/off
                 * Normal  on/off
                 * Mirror wanted temperature
                 */
                TempC = sorteretData[1];
                Kcheck = SetTrueOrFalse(sorteretData[2]);
                Vcheck = SetTrueOrFalse(sorteretData[3]);
                Ocheck = SetTrueOrFalse(sorteretData[4]);
                TempCsat = sorteretData[5];
            }

        }

        /// <summary>
        /// Handling of connect and disconnect of the serial connection
        /// Aktivated by a button on the WPF
        /// </summary>
        public void StartSerialConnection()
        {
            //Check if a serial port exists
            if (port != null)
            {
                if (port.IsOpen)
                {
                    //Disconnects and closes a comport if it exists
                    port.DataReceived -= dataReceivedOnSerial;
                    port.Close();
                    BtnName = "Connect!";
                }
                else
                {
                    //If a comport is is selected, and doesnt exist, create it and open it
                    if (!string.IsNullOrEmpty(SelectedComPort))
                    {
                        port = new SerialPort(SelectedComPort, 9600);
                        port.Open();
                        //Set up receiving data from port
                        port.ReadExisting();
                        port.DataReceived += dataReceivedOnSerial;
                        BtnName = "Disconnect!";
                    }
                }
            }


            if (port == null && !string.IsNullOrEmpty(SelectedComPort))
            {
                port = new SerialPort(SelectedComPort, 9600);
                port.Open();
                port.ReadExisting();
                port.DataReceived += dataReceivedOnSerial;
                BtnName = "Disconnect!";
            }
        }

        /// <summary>
        /// Send the wanted the wanted temperature variable out 
        /// on the serial port, if it is open.
        /// Resets the textboks afterwards.
        /// Activated by a button on the WPF
        /// </summary>
        public void SetTempOnArduino()
        {
            if (port.IsOpen)
            {
                port.Write(TempInput);
            }
            TempInput = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string PropertyNavn)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyNavn));
            }
        }
    }
}
