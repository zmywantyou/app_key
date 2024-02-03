
using Android.Bluetooth.LE;
using Android.Content;
using Android.Media.Midi;
using Android.Net.Wifi;
using Android.Nfc;
using Android.OS;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using app_key;
using Java.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;





class WifiSwich : wifi
{
    // private int Leve;
    private int speed;
    private String ssid = null;
    private WifiManager wifiManager;
    private WifiInfo wifiInfo;
    private IList<Android.Net.Wifi.ScanResult> WifiUser;
    private String text;
    private static wifi Wifi;
    // public String ssID;
    private int Leve;

    public WifiSwich(Context context) : base(context)
    {
        wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
        wifiInfo = wifiManager.ConnectionInfo;
        Leve = wifiInfo.Rssi;
        speed = wifiInfo.LinkSpeed;
        ssid = wifiInfo.SSID;
        var ssID = ComperWifi();
        /*   if ( FristChagewifi(ssID, ssid))*/
        // GetWifiUser();
        
        wifi.Get_ssid_pwd(ssID, WifiPassWord(ssID));
        //if (Leve )
    }
    public void show_update_textview3(ref TextView textView3)
    {
        textView3.Text = this.Leve + "dB" + "   " + this.speed + "kb";//wifi栏的wifi信号和速度

    }
    public  void ChangeWifi()
    {
        if (wifi.NOin==1) 
        {
            if(this.Leve>=100) 
            {
                base.close_or_open_wifi();
            }
        }
        
    }

    public  void ChangeWifi()
  {
      if (wifi.NOin==1) 
      {
          if(this.Leve>=100) 
          {
              base.close_or_open_wifi();
          }
      }
      
  }

  private String ComperWifi()
  {




      bool su = this.wifiManager.StartScan();


      //  this.WifiUser.Clear();
      this.WifiUser = this.wifiManager.ScanResults;

      GetWifiUser();

      int leve = -100;
      int leve5G = -100;
      String ssid = su.ToString();
      String ssid5G = su.ToString();
      bool ON= false;

      foreach (var result in this.WifiUser)
      {
          bool is5G = result.Ssid.ToString().EndsWith("5G");
          if (is5G && result.Level > -70)
          {
              if (result.Level > leve5G)
              {
                  leve5G = result.Level;
                  ssid5G = result.Ssid.ToString();
                   ON = true;
              }
          }
          else
          {
              if (result.Level > leve)
              {
                  leve = result.Level;
                  ssid = result.Ssid.ToString();
              }
          }
      }
      if (ON)
          return ssid5G;
      else 
          return ssid;
  }
    public void GetWifiUser()
    {
        String Bed = "zmy";
        String bEd = "zmy_5G";
        String Lving = "LJ";
        String LvinG = "LJ_5G";

        var Wifiuser = this.WifiUser.ToList();
        this.WifiUser.Clear();
        // this.text = Wifiuser[0].Level.ToString();
        foreach (var result in Wifiuser)
        {
            if (
               result.Ssid.Equals(Bed) || result.Ssid.Equals(bEd) ||
                result.Ssid.Equals(Lving) || result.Ssid.Equals(LvinG))
            {
                this.WifiUser.Add(result);
            }
        }
        Wifiuser.Clear();



    }


    private String WifiPassWord(String WifiUser)
    {
        switch (WifiUser)
        {
            case "zmy_5G": return "246810zmy";
            case "zmy": return "246810zmy";
            case "LJ": return "246810luojun";
            case "LJ_5G": return "246810luojun";
        }
        return null;
    }


    /*private  bool FristChagewifi(String SSID1 ,String ssid2)
    {
         if (!MainActivity.UpOn)
        {
           if(ssid2!= SSID1)
            {
                wifi.text2_state = "当前私人WiFi：" + SSID1;
                //if(MainActivity.UpOn)
                // {
                //wifi.b_state = "点击开始连接";
                wifi.Get_ssid_pwd(ComperWifi(), WifiPassWord(ComperWifi()));


                //  }
            } 
            MainActivity.UpOn = true;
            return false;

        }
        else
        {
           // MainActivity.UpOn = false;
            return true;
        }

    }*/

}

/*public class broadcastReceiver : BroadcastReceiver
{
    // private WifiManager wifiManager;
    // IList<Android.Net.Wifi.ScanResult> list;
    public override void OnReceive(Context context, Intent intent)
    {

        if (WifiManager.ScanResultsAvailableAction.Equals(intent.Action))
        {
            var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
            var list = wifiManager.ScanResults;
            //OnScanResultsReceived?.Invoke(new List<Android.Net.Wifi.ScanResult>(list));
        }
        else
        {
            // Log.Debug("\"WiFiScanResults\"", "results size: " + list.Count);
        }

    } }
  *//*  public void gets(WifiManager wifiManager)
    {
        this.wifiManager = wifiManager;
    }*/
  /*  public static IList<Android.Net.Wifi.ScanResult> RIlistScan()
    {
        return list;
    }
}*/



