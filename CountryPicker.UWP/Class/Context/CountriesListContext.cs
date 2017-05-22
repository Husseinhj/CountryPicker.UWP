using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using CountryPicker.UWP.Class.Models;
using CountryPicker.UWP.Data;
using Newtonsoft.Json;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace CountryPicker.UWP.Class.Context
{
    class CountriesListContext 
    {

        //public static IEnumerable<GroupInfoList> GetCountries()
        //{
        //    var json = LoadCountriesJsonFile();

        //    var countriesCollection =
        //        JsonConvert.DeserializeObject<List<CountryModel>>(json);

        //    var col =  new ObservableCollection<CountryModel>(countriesCollection.OrderBy(o => o.Group).ToList());

        //    var enumerable = col.GroupBy(m => m.Group, (key, list) => new GroupInfoList(key,list));

        //    return enumerable;
        //}

        //private static string LoadCountriesJsonFile()
        //{
        //    try
        //    {
        //        return JsonData.Json;
        //    }
        //    catch (Exception exception)
        //    {
        //        Debug.WriteLine(exception);
        //    }
        //    return String.Empty;
        //}

    }
}
