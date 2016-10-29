using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Windows.Data.Json;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TVScheduler
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    class TVStatus
    {
        public long? dateOfCredit { get; set; }

        public int? amountOfCreditInMinutes { get; set; }
        public int? bonPoints { get; set; }
        public int? activeStandbyState { get; set; }
        public int? playedMediaId { get; set; }
        public int? bonPointsWeek { get; set; }
        public Boolean? relayStatus { get; set; }
        public int? remainingSecond { get; set; }
        public string remainingTime { get; set; }
        public string title { get; set; }


    }



    public sealed partial class MainPage : Page
    {

        TVStatus chan;

        public MainPage()
        {
            this.InitializeComponent();
            chan= new TVStatus();
            chan.title = "Mon Cul is Poulet";
            DataContext = chan;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            getStatus();
            var dialog = new Windows.UI.Popups.MessageDialog(
                          "Aliquam laoreet magna sit amet mauris iaculis ornare. " +
                          "Morbi iaculis augue vel elementum volutpat.",
                          "Lorem Ipsum");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });

            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                // Adding a 3rd command will crash the app when running on Mobile !!!
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Maybe later") { Id = 2 });
            }

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = dialog.ShowAsync();


        }

        private const string URL = "http://192.168.1.32/tvscheduler/credit";

        private const string URLStatus = "http://192.168.1.32/tvscheduler/tvstatus";

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync("?value=1800").Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsStreamAsync().Result;
                Debug.WriteLine("Coucou !" + dataObjects);
            }
            else
            {
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        private void button60mn_Click(object sender, RoutedEventArgs e)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync("?value=3600").Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsStreamAsync().Result;
                Debug.WriteLine("Coucou !" + dataObjects);
            }
            else
            {
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }

        private void buttonOff_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync("?value=-1").Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var dataObjects = response.Content.ReadAsStreamAsync().Result;
                Debug.WriteLine("Coucou !" + dataObjects);
            }
            else
            {
                Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

        }




        private void getStatus()
        {
            string webServiceAddress = URLStatus;
        //    string methodName = "HelloWorld";

        //    string webServiceMethodUri = string.Format("{0}/{1}", webServiceAddress, methodName);

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(webServiceAddress);
            httpWebRequest.Method = "GET";

            httpWebRequest.BeginGetResponse(Response_Completed, httpWebRequest);

            }

        void Response_Completed(IAsyncResult result)
        {
            try
            {
                Debug.WriteLine("Retour de la reponse");
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                Debug.WriteLine("Lecture de l'etat");
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Debug.WriteLine("Reponse digérée");

                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonString = streamReader.ReadToEnd();
                    Debug.WriteLine("Status " + jsonString);
                    JsonObject root = JsonValue.Parse(jsonString).GetObject();
                    int activeStandbyState= (int)root.GetNamedNumber("activeStandbyState");
                    Debug.WriteLine("Active standbystate " + activeStandbyState);
                    chan.activeStandbyState = activeStandbyState;
                    
                 //   chan.dateOfCredit= (long) root.GetNamedNumber("dateOfCredit");
                    /*  for (uint i = 0; i < root.Count; i++)
                   {
                        int activeStandbyState = Int32.Parse(root.GetObjectAt(i).GetNamedString("activeStandbyState"));
                        Debug.WriteLine("Active standbystate " + activeStandbyState);
                        string description1 = root.GetObjectAt(i).GetNamedString("description");
                        string link1 = root.GetObjectAt(i).GetNamedString("link");
                        string cat1 = root.GetObjectAt(i).GetNamedString("cat");
                        string image1 = root.GetObjectAt(i).GetNamedString("image");
                        TVStatus chan = new TVStatus();
                        chan.activeStandbyState = activeStandbyState;
                        
                    }*/
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Erreur " + e.Message + e.StackTrace);

            }
        }
        }
    }




    





