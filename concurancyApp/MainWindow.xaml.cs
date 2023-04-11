
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;

namespace CurrencyConverter_Static { 

    public partial class MainWindow : Window
    {
        Root val = new Root();
        public MainWindow()
        {

            InitializeComponent();
            getValue();
            ClearControls();
        }

        //creating a method to get value
        public async void getValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=0696e9ffd11942ddad011044cb7bb146");//API key
            BindCurrency();
        }

        private static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();

            try
            {
                using (var client = new HttpClient()) //HttpClient class provides a base class for
                                                      //send/ receiving the HTTP requests/response from URL
                {
                    client.Timeout = TimeSpan.FromMinutes(1);//The timespan to wait before request times out

                    HttpResponseMessage response = await client.GetAsync(url);
                    //HttpResponseMessage is a way of returning message/data from your action

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)//Check if Api status is OK
                    {
                        //Serialize Http content to a string as an asynchronous operation
                        var rString = await response.Content.ReadAsStringAsync();

                        //JsonConvert.DeserializeObject to deserialize Json to C#
                        var rObject = JsonConvert.DeserializeObject<Root>(rString);

                        //MessageBox.Show("TimeStamp: " + ResponseObject.timestamp, "Information",
                        //    MessageBoxButton.OK, MessageBoxImage.Information);

                        return rObject; //Return Api status
                    }
                }
            }
            catch
            {

                return myRoot;
            }

            return myRoot;
        }

        private void BindCurrency()
        {
            DataTable dt = new DataTable();

            //Add display column in DataTable
            dt.Columns.Add("Text");

            //Add value column in DataTable
            dt.Columns.Add("Value");

            //Add rows in DataTable with text and value
            dt.Rows.Add("--SELECT__", 0);
            dt.Rows.Add("GEL", val.rates.GEL);
            dt.Rows.Add("INR", val.rates.INR);
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("NZD", val.rates.NZD);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("CAD", val.rates.CAD);
            dt.Rows.Add("ISK", val.rates.ISK);

            //The data to currency ComboBox is assigned from dataTable
            cmbFromCurrency.ItemsSource = dt.DefaultView;

            //DisplayMemberPath Property is used to display data in ComboBox
            cmbFromCurrency.DisplayMemberPath = "Text";

            //SelectedValuePath Property is used to set the value in ComboBox
            cmbFromCurrency.SelectedValuePath = "Value";

            //SelectedIndex property is used to bind hint in the ConboBox.  The default value is Select.
            cmbFromCurrency.SelectedIndex = 0;

            //All properties are also set for ToCurrency ComboBox

            //The data to currency ComboBox is assigned from dataTable
            cmbToCurrency.ItemsSource = dt.DefaultView;

            //DisplayMemberPath Property is used to display data in ComboBox
            cmbToCurrency.DisplayMemberPath = "Text";

            //SelectedValuePath Property is used to set the value in ComboBox
            cmbToCurrency.SelectedValuePath = "Value";

            //SelectedIndex property is used to bind hint in the ConboBox.  The default value is Select.
            cmbToCurrency.SelectedIndex = 0;
        }

        //ClearControls used to clear all values
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;

            MyLabel.Content = " ";
            txtCurrency.Focus();
        }

        //Allow only the integer value in the Textbox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Convert button
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Create a value as ConvertedValue to store converted value
            double ConvertedValue;

            //Check if amount Textbox is null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //It should show this message
                MessageBox.Show("Please Enter Currncy",
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                txtCurrency.Focus();
                return;
            }

            //Else if the currency from is not selected  or it is default text SELECT
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                //It should show this message
                MessageBox.Show("Please Select Currency From",
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbFromCurrency.Focus();
                return;
            }

            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                //It should show this message
                MessageBox.Show("Please Select Currency To",
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbToCurrency.Focus();
                return;
            }

            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                ConvertedValue = double.Parse(txtCurrency.Text);
                MyLabel.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
               
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString())
                    * double.Parse(txtCurrency.Text))
                    / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                MyLabel.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        //Clear Button
        private void Clear_Button(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }
    }
}