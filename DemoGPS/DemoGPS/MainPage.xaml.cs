using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace DemoGPS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            btnGetLoc.Clicked += BtnGetLoc_Clickied;
        }

        private async void BtnGetLoc_Clickied(object sender, EventArgs e)
        {
            await RetriveLocation();
        }

        public async Task RetriveLocation()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    var position = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromMilliseconds(10000));
                    txtLat.Text = "Latitude: " + position.Latitude;
                    txtLong.Text = "Longitude: " + position.Longitude;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {

                txtLat.Text = "Error: " + ex;
                txtLong.Text = "Error: " + ex;
            }
        }
    }
}
