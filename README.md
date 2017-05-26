# CountryPicker.UWP
Country picker control for Windows Universal App
Support target UWP >= 10.0 Build 10240 

To install CountryPicker for UWP, run the following command in the Package Manager Console

![Install-Package CountryPicker.UWP](http://uupload.ir/files/8xms_nuget_countrypicker.png)

Release Notes
v1.3.0
+New properties for customize picker 
+Add back button and event 
-fix bugs and improve preformance 

Sample code :

```
var countryPickerDialog = new UWP.Class.PickerDialog()
{
    CountryName = BtnCountryPicker.Content.ToString(),
    SearchBoxPlaceHolder = "Search",
    Header = "Choose country",
    SearchBoxFlowDirection = FlowDirection.LeftToRight
};

countryPickerDialog.SelectedCountry += CountyPickerDialogOnSelectedCountry;
countryPickerDialog.Show();

// Event when country was selected by user
private void CountyPickerDialogOnSelectedCountry(object sender, CountryModel selected)
{
     LblCountryCode.Text = String.Format("+{0}", selected.Code);

     BtnCountryPicker.Content = selected.Name;

     ImgFlag.Source = new BitmapImage(new Uri(selected.Flag, UriKind.RelativeOrAbsolute));
}
                
```


Desktop version:

![enter image description here](http://uupload.ir/files/em9r_desktopversion_country_picker.png)


Mobile version:

![enter image description here](http://uupload.ir/files/mp56_mobileversioncountrypicker.png)
