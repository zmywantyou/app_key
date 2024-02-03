using Android;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Net.Wifi.Aware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Lang;
using System;
using Android.Content.PM;
using Android.Media.Midi;
using System.Runtime.Remoting.Contexts;


namespace app_key
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
         Button button1;
        TextView textview1;
        TextView textview2;
        TextView textview3;
        Handler handler=new Handler();
        Runnable updateTask;
        Handler handler2 = new Handler();
        Runnable updateTask2;
        WifiSwich wifiSwich ;
        private const int REQUEST_CODE_ACCESS_FINE_LOCATION = 100;
        public static bool UpOn=false;
        wifi Wifi;
        //broadcastReceiver mBroadcastReceiver;
    

        //broadcastReceiver mBroadcastReceiver = new broadcastReceiver();
        protected override void OnCreate(Bundle savedInstanceState)
        {
  
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
           /* if (ContextCompat.CheckSelfPermission(this,Manifest.Permission.AccessWifiState) !=.Granted ||
           ContextCompat.CheckSelfPermission(this, Manifest.Permission.ChangeWifiState) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessWifiState, Manifest.Permission.ChangeWifiState }, 100);
            }
            else
            {
                StartScanAndFetchResults();
            }*/
            SetContentView(Resource.Layout.activity_main);
            this.button1=FindViewById<Button>(Resource.Id.button1);
            this.textview1 = FindViewById<TextView>(Resource.Id.textView1);
            this.textview2 = FindViewById<TextView>(Resource.Id.textView2);
            this.textview3 = FindViewById<TextView>(Resource.Id.textView3);
            
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Permission.Granted || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessWifiState) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.ChangeWifiState }, REQUEST_CODE_ACCESS_FINE_LOCATION);
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.ChangeWifiState }, 1); //REQUEST_CODE_ACCESS_FINE_LOCATION//REQUEST_CODE_ACCESS_FINE_LOCATION
            }
            
               
               // this.Wifi = new wifi(this);
               
           
            this.updateTask = new Runnable(() =>
                {
                    this.wifiSwich = new WifiSwich(this);
                    this.wifiSwich.EmaBleD(); 
                    /*this.wifiSwich.show_update_textview1(ref this.textview1, ref this.textview2);
                    this.wifiSwich.show_update_button(ref this.button1);
                    this.wifiSwich.show_update_textview3(ref this.textview3);*/
                    this.handler.PostDelayed(this.updateTask, 1);
                   
                });
                this.handler.Post(updateTask);
            this.updateTask2 = new Runnable(() =>
            {
                //this.wifiSwich = new WifiSwich(this);
                //this.wifiSwich.EmaBleD();
                this.wifiSwich.show_update_textview1(ref this.textview1, ref this.textview2);
                this.wifiSwich.show_update_button(ref this.button1);
                this.wifiSwich.show_update_textview3(ref this.textview3);
                this.wifiSwich.ChangeWifi();
                this.handler.PostDelayed(this.updateTask2, 100);

            });
            this.handler2.Post(updateTask2);

            this.button1.Click += OnButton1Click;
                //OnScanResultsReceived ? += HandleScanResults;
            

           
           
            

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void OnButton1Click(object sender, EventArgs e)
        {
            
            this.wifiSwich.close_or_open_wifi();
            
            
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            handler.RemoveCallbacks(updateTask);
        }

    }
}