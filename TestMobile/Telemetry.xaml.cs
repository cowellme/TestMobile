using KbpApi.Models;
using MobileServer.DataBase;
using System;
using Button = Microsoft.Maui.Controls.Button;

namespace TestMobile;

public partial class Telemetry : ContentPage
{
    private static string _valueQre = "";
    private static double _latitude = -1;
    private static double _longitude = -1;

    private static string _codeDot = "";
    private static string _adress = "";
    public bool IsWork { get; set; } = false;

    public Telemetry(string value)
    {
        InitializeComponent();
        _valueQre = value;
        SaveButton.IsEnabled = false;
        if (GetCachedLocation() != null)
        {
            IsWork = true;
        }
        else
        {
            MainPage.IsDetect = false;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        MainPage.IsDetect = false;
        Shell.Current.Navigation.PopToRootAsync();
        return base.OnBackButtonPressed();
    }


    public string? GetCachedLocation()
    {
        try
        {
            Location? location = Geolocation.Default.GetLastKnownLocationAsync().Result;

            if (location == null) return null;

            _latitude = location.Latitude;
            _longitude = location.Longitude;

            var label = 
                $"Широта: {_latitude}" + 
                Environment.NewLine + $"Долгота: {_longitude}" +
                Environment.NewLine + $"Значение: {_valueQre}" +
                Environment.NewLine + $"Адрес: {_adress}";

            LocationLabel.HorizontalTextAlignment = TextAlignment.Start;
            LocationLabel.Text = label;
            var dots = Start(new Item { Longitude = _longitude, Latitude = _latitude });

            if (dots.Count > 0)
            {
                
                foreach (var dot in dots.ToList())
                {
                    var button = new Button { Text = dot.Name, Margin = new Thickness(0,0,0,10)};
                    button.Clicked += ButtonDots_Clicked;
                    StackLayoutDots.Children.Add(button);
                }

            }


            return "OK";
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
        }
        catch (Exception ex)
        {
            // Unable to get location
        }

        return null;
    }

    private void ButtonDots_Clicked(object? sender, EventArgs e)
    {
        var senderType = sender?.GetType();
        
        if (senderType == typeof(Button))
        {
            var button = (Button)sender!;
            if (button == null) throw new ArgumentNullException(nameof(button));
            _adress = button.Text;
            SaveButton.IsEnabled = true; 
            
            var label =
                $"Широта: {_latitude}" +
                Environment.NewLine + $"Долгота: {_longitude}" +
                Environment.NewLine + $"Значение: {_valueQre}" +
                Environment.NewLine + $"Адрес: {_adress}";

            LocationLabel.HorizontalTextAlignment = TextAlignment.Start;
            LocationLabel.Text = label;
        }
    }

    public static List<Dot> Start(Item item)
    {
        var dots = Dot.GetAll();

        foreach (var dot in dots.ToList())
        {
            if (Formul(dot, item)) continue;

            dots.Remove(dot);
        }

        //List<Dot> response = new List<Dot>();

        //for (int i = 0; i < 20; i++)
        //{
        //    var dot = dots[i];
        //    response.Add(dot);
        //}

        return dots;
    }

    private static bool Formul(Dot dot, Item item)
    {
        try
        {
            if (dot.Longitude > item.Longitude - 0.0016 && dot.Longitude < item.Longitude + 0.0016)
                if (dot.Latitude > item.Latitude - 0.00092 && dot.Latitude < item.Latitude + 0.00092)
                    return true;

            return false;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    private void SaveButton_OnClicked(object? sender, EventArgs e)
    {
        if (_valueQre == "") return;

        

        SendRequest();
    }

    private void SendRequest()
    {
        if (_valueQre == "") return;
        if (_latitude == -1) return;
        if (_longitude == -1) return;

        var dots = Dot.GetAll();
        var codeDot = dots.FirstOrDefault(x => x.Name == _adress)?.Code;

        if (codeDot == null) codeDot = "none";

        var item = new Item
        {
            Code = _valueQre,
            Latitude = _latitude,
            Longitude = _longitude,
            CodeDot = codeDot,
            SaveTime = DateTime.Now.ToString("g")

        };

        Database.Add(item);
        MainPage.IsDetect = false;
        Shell.Current.Navigation.PopToRootAsync();
    }

}