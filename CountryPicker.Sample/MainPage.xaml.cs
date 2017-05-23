using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

            Loaded +=OnLoaded;
        }

        private  void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var s = new CountryPicker.UWP.Class.PickerDialog("Barbados");
            s.Style = App.Current.Resources["ContentDialogStyle"] as Style;
            
            s.Show();
        }

    }
}
