using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using System;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Notifications
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            PushNotificationSetup();
            AppCenter.Start("438ad34c-9c3b-46c5-8f42-436713b336dc", typeof(Analytics), typeof(Crashes), typeof(Push));


            //System.Guid? installId = AppCenter.GetInstallIdAsync().Result;

            Guid installId;// = null;
            var idTask = AppCenter.GetInstallIdAsync();
            idTask.ContinueWith((task) =>
            {
                var result = (task as System.Threading.Tasks.Task<Guid?>).Result;
                if (result.HasValue)
                {
                    installId = result.Value;
                }
            });

            //CustomProperties properties = new CustomProperties();
            //properties.Set("usuarioID", 12345);
            //AppCenter.SetCustomProperties(properties);
        }

        private void PushNotificationSetup()
        {
            Push.SetSenderId("1007038591765");

            if (!AppCenter.Configured)
            {
                Push.PushNotificationReceived += (sender, e) =>
                {
                    // Add the notification message and title to the message
                    var summary = $"Push notification received:" +
                                  $"\n\tNotification title: {e.Title}" +
                                  $"\n\tMessage: {e.Message}";

                    // If there is custom data associated with the notification,
                    // print the entries
                    if (e.CustomData != null)
                    {
                        summary += "\n\tCustom data:\n";
                        foreach (var key in e.CustomData.Keys)
                        {
                            summary += $"\t\t{key} : {e.CustomData[key]}\n";
                        }
                    }

                    // Send the notification summary to debug output
                    System.Diagnostics.Debug.WriteLine(summary);
                };
            }
        }
    }
}
