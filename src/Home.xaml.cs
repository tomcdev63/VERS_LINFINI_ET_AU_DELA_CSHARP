using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text.Json;

//---------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------

namespace VIEAD
{
    public partial class Home : Page
    {

        private const string GLOBAL_DATA_URL = "https://api-adresse.data.gouv.fr/search/?q=postcode=";
        public List<Telescope> telescope = new List<Telescope>();
        public event EventHandler GoToPictures;

        public Home()
        {
            InitializeComponent();
            telescope.Add(new Telescope { Brand = "Celestron", Model = "C500", Carac = "D : 125mm, Lf : 1250mm, R : F/10" });
            telescope.Add(new Telescope { Brand = "Celestron", Model = "C6", Carac = "D : 150mm, Lf : 1500mm, R : F/10" });
            telescope.Add(new Telescope { Brand = "Celestron", Model = "C8", Carac = "D : 203mm, Lf : 2000mm, R : F/10" });
            telescope.Add(new Telescope { Brand = "Celestron", Model = "C9.25", Carac = "D : 235mm, Lf : 2350mm, R : F/10" });
            telescope.Add(new Telescope { Brand = "Celestron", Model = "C11", Carac = "D : 280mm, Lf : 2800mm, R : F/10" });
            telescope.Add(new Telescope { Brand = "Celestron", Model = "C14", Carac = "D : 355mm, Lf : 3910mm, R : F/11" });
            myComboBox.ItemsSource = telescope;

        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------

        private void submitButton(object sender, RoutedEventArgs e)
        {
            GetRequest(GLOBAL_DATA_URL, code_postal.Text);
        }

        async static void GetRequest(string url, string cp)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url + cp))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        var match = Regex.Match(cp, @"([0-9]{5})");

                        if (match.Success)
                        {

                            Root? root = JsonSerializer.Deserialize<Root>(mycontent);
                            MessageBox.Show("Information about the location of the observation : " +
                                            $"\n\n- City : {root.features[2].properties.city} " +
                                            $"\n- Longitude : {root.features[2].geometry.coordinates[0]} " +
                                            $"\n- Latitude : {root.features[2].geometry.coordinates[1]}");
                        }

                        else
                        {
                            MessageBox.Show("You must enter a valid French department number! (5 digits)");
                        }


                    }
                }
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------

        private void pictButton(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Pictures.xaml", UriKind.Relative));
        }

        //---------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------
    }
}

public class Telescope
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Carac { get; set; }

    public string FullName
    {
        get
        {
            return $"{Brand} {Model} {Carac}";
        }
    }
}

//---------------------------------------------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------------------------------------------

public class Feature
{
    public string type { get; set; }
    public Geometry geometry { get; set; }
    public Properties properties { get; set; }
}

public class Geometry
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
}

public class Properties
{
    public string label { get; set; }
    public double score { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public string postcode { get; set; }
    public string citycode { get; set; }
    public double x { get; set; }
    public double y { get; set; }
    public string city { get; set; }
    public string context { get; set; }
    public string type { get; set; }
    public double importance { get; set; }
}

public class Root
{
    public string type { get; set; }
    public string version { get; set; }
    public List<Feature> features { get; set; }
    public string attribution { get; set; }
    public string licence { get; set; }
    public string query { get; set; }
    public int limit { get; set; }
}