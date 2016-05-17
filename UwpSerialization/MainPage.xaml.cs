using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpSerialization
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
        #region Read me
        //There are 4 parts to understand data storing
        //- The object graph
        //- The serialization classes / methods
        //- The stream
        //- The file in the Local Folder
        #endregion

        #region Buttons
        private async void writeButton_Click(object sender, RoutedEventArgs e)
        {
            await writeXMLAsync();
            //await writeJsonAsync();
        }
        private async void readButton_Click(object sender, RoutedEventArgs e)
        {
            await readXMLAsync();
            //await readJsonAsync();
            //await deserializeJsonAsync();
        }
        #endregion

        #region ModelData methods "Object Graphs"
        private List<Car> buildObjectGraph()
        {
            var myCars = new List<Car>();

            myCars.Add(new Car() { Id = 1, Make = "Oldsmobile", Model = "Cutlas Supreme", Year = 1985 });
            myCars.Add(new Car() { Id = 2, Make = "Geo", Model = "Prism", Year = 1993 });
            myCars.Add(new Car() { Id = 3, Make = "Ford", Model = "Escape", Year = 2005 });

            return myCars;
        }
        #endregion

        #region Instance Fields
        //Used by XML helper methods
        private const string XMLFILENAME = "data.xml";

        //used by JSON helper methods
        private const string JSONFILENAME = "data.json";
        #endregion

        #region XML helper methods
        private async Task writeXMLAsync()
        {
            var myCars = buildObjectGraph();

            var serializer = new DataContractSerializer(typeof(List<Car>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                              XMLFILENAME,
                              CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, myCars);
            }

            resultTextBlock.Text = "Write succeeded";
        }

        private async Task readXMLAsync()
        {
            string content = String.Empty;

            var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(XMLFILENAME);
            using (StreamReader reader = new StreamReader(myStream))
            {
                content = await reader.ReadToEndAsync();
            }

            resultTextBlock.Text = content;
        }
        #endregion

        #region JSON helper methods
        private async Task writeJsonAsync()
        {
            // Notice that the write is ALMOST identical ... except for the serializer.

            var myCars = buildObjectGraph();

            var serializer = new DataContractJsonSerializer(typeof(List<Car>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                          JSONFILENAME,
                          CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, myCars);
            }

            resultTextBlock.Text = "Write succeeded";
        }

        private async Task readJsonAsync()
        {
            // Notice that the write **IS** identical ... except for the serializer.

            string content = String.Empty;

            var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);
            using (StreamReader reader = new StreamReader(myStream))
            {
                content = await reader.ReadToEndAsync();
            }

            resultTextBlock.Text = content;
        }
        #endregion

        #region object graph JSON
        private async Task deserializeJsonAsync()
        {
            string content = String.Empty;

            List<Car> myCars;
            var jsonSerializer = new DataContractJsonSerializer(typeof(List<Car>));

            var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);

            myCars = (List<Car>)jsonSerializer.ReadObject(myStream);

            foreach (var car in myCars)
            {
                content += String.Format("ID: {0}, Make: {1}, Model: {2} ... ", car.Id, car.Make, car.Model);
            }

            resultTextBlock.Text = content;
        }
        #endregion
    }
}
