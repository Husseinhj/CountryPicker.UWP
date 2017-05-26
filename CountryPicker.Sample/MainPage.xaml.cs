using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using CountryPicker.UWP.Class.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CountryPicker.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void BtnCountryPicker_OnClick(object sender, RoutedEventArgs e)
        {
            if (BtnCountryPicker.Content != null)
            {
                var countyPickerDialog = new UWP.Class.PickerDialog()
                {
                    CountryName = BtnCountryPicker.Content.ToString(),
                    Style = Application.Current.Resources["ContentDialogStyle"] as Style,
                    SearchBoxPlaceHolder = "Search",
                    Header = "Choose country"
                };
                countyPickerDialog.SelectedCountry += CountyPickerDialogOnSelectedCountry;

                await countyPickerDialog.ShowAsync();
            }
        }

        private void CountyPickerDialogOnSelectedCountry(object sender, CountryModel selected)
        {
            LblCountryCode.Text = String.Format("+{0}",selected.Code);

            BtnCountryPicker.Content = selected.Name;

            ImgFlag.Source = new BitmapImage(new Uri(selected.Flag,UriKind.RelativeOrAbsolute));
        }
    }
}
