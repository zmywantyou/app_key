using Android.Net.Wifi;
using System;
using Android.Content;
using Android.Net;
using Android.Widget;
using Java.Nio.Channels;
using Android.OS;
using static Android.Net.ConnectivityManager;
using Android.App;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Media.Midi;
using app_key;







public class wifi
{
    private WifiManager wifiManager;
    private WifiNetworkSpecifier.Builder builder;
    private String text1_state;
    public static String b_state="点击时连接";//按钮的状态
    public static String text2_state;
    private static String ssid;
    private static String pwd;
    private  ConnectivityManager connectivityManager;
    public Context _context;
    public static int NOin;
    private NetworkStatus status;
    private MyNetworkCallback networkCallback;
    private wifi Wifi;
    private WifiNetworkSuggestion suggestion;

    public static void Get_ssid_pwd(String SSID ,String PWD)
    {
        ssid= SSID;
        pwd= PWD;
    }
    public wifi(Context context)
    {
        _context = context;
        wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
        connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
    }
    public async void EmaBleD()
    {
        /*
 Disabled： 表示Wi-Fi功能已被禁用或关闭。设备当前没有启用无线网络。

Disabling： 表示Wi-Fi正在被禁用的过程中。这个状态是过渡性的，
        表明系统正在进行操作以关闭Wi-Fi功能。

Enabled： 表示Wi-Fi功能已启用，但并不意味着设备已经成功连接到任何Wi-Fi网络。
        此时设备可以扫描并尝试连接可用的Wi-Fi网络。

Enabling： 表示Wi-Fi正在被启用的过程中。同样是一个过渡性状态，
        表示系统正在进行操作以打开Wi-Fi功能。

Unknown： 表示无法确定Wi-Fi的状态，可能是因为系统内部错误或者其他未预期的情况
        导致无法获取准确的Wi-Fi状态信息。
         */

 
        var wifistae = this.wifiManager.WifiState;


        switch (wifistae)
        {
            case WifiState.Disabled:
                this.text1_state = "连接状态：请打开wifi";
              
                b_state = "点击时连接";

                break;
            case WifiState.Enabled:
               // var connectionInfo = this.wifiManager.ConnectionInfo;
                var activeNetwork = connectivityManager.ActiveNetwork;
                if (activeNetwork != null)
                {
                    // 尝试进行网络可达性检查以确保可以连接到互联网
                    var networkCapabilities = connectivityManager.GetNetworkCapabilities(activeNetwork);

                    // 判断网络是否具有INTERNET能力
                   if( networkCapabilities.HasTransport(TransportType.Wifi))
                    {
                       
                      /*  if (MainActivity.UpOn)
                        {
                            NOin = 0;
                            this.text1_state = "连接状态：连接到wifi";
                            text2_state = "推荐wifi ："+ssid;
                            b_state = "点击时连接";
                        }
                        else*/
                        
                            NOin = 1;
                            this.text1_state = "连接状态：连接到wifi";
                            text2_state = "推荐wifi ：" + ssid;
                            b_state = "点击时关闭";
              
                    }

                    else
                    {
                        NOin = 0;
                        this.text1_state = "连接状态：无连接wifi";
                        text2_state = "推荐wifi ：";
                        b_state = "点击时连接";
                    }
                }
                else
                {
                    NOin = 0;
                    this.text1_state = "连接状态：无连接wifi";
                   b_state = "点击时连接";
                    text2_state = "推荐wifi ：";
                }
                    break;
        
                case WifiState.Unknown:
                NOin = 0;
                this.text1_state = "连接状态：未知错误";
                b_state = "点击时连接";
                text2_state = "推荐wifi ：";

                break;
            }
         
        
      
    }
   public void show_update_button(ref Button button1)
        {
        button1.Text = b_state;
        }
   public void show_update_textview1(ref TextView textView1,ref TextView textView2)
    {
        textView1.Text = text1_state;
        //textView2.Text = this.b_state;
        textView2.Text = text2_state;//wifi栏的wifi名字

    }
    public void close_or_open_wifi()
    {
        this.suggestionfun();
        Toast toast = new Toast(this._context);
       
        if (!this.wifiManager.IsWifiEnabled)
        {
            toast.SetText("请连接WiFi");
            toast.Show();
           
         }
        else if(this.status== NetworkStatus.SuggestionsSuccess) 
        {
            if(NOin==0) 
            {
               
                this.cxwifi();
                
            }
             else
        {
            this.ONcxwifi();
              
            }
        }
        else
        {
            toast.SetText("已经拒绝");
            toast.Show();
            return;
        }
       

     

    }
    public void suggestionfun()
    {

        
        this.suggestion =
                  new WifiNetworkSuggestion.Builder()
                          .SetSsid(ssid)
                          .SetWpa2Passphrase((pwd))
                          .SetIsAppInteractionRequired(true)
                          .Build();//建议列表
        List<WifiNetworkSuggestion> suggestionsList = new List<WifiNetworkSuggestion>();//获取建议列表
        //建议加上成功率翻倍；
        suggestionsList.Add(suggestion);//添加建议
        this.status = this.wifiManager.AddNetworkSuggestions(suggestionsList);
         
    }
    private void cxwifi()
    {


        {
            //密码账号连接
            //NetworkCallback networkCallback = new ConnectivityManager.NetworkCallback();
            this. networkCallback = new MyNetworkCallback(this._context);
            this.builder = new WifiNetworkSpecifier.Builder().
            SetSsid(ssid).
          SetWpa2Passphrase(pwd);
            // this.builder.SetBssid(MacAddress.FromString("192.168.2.1"));
            var buildeR = this.builder.Build();
            //连接请求
            var request = new NetworkRequest.Builder().
            AddTransportType(TransportType.Wifi);
            var RE = request.SetNetworkSpecifier(buildeR).Build();
            this.connectivityManager = (ConnectivityManager)this._context.GetSystemService(Context.ConnectivityService);
            networkCallback.get(connectivityManager, networkCallback);
            connectivityManager.RequestNetwork(RE, networkCallback);
            
            
        }

    }
    /* public void cxwifi()
     {

         String ssid = "zmy_5G";
         String password = "246810zmy";

         WifiConfiguration wifiConfig = new WifiConfiguration();
         wifiConfig.Ssid= String.Format("\"%s\"", ssid);
         wifiConfig.PreSharedKey = String.Format("\"%s\"", password);
         int netId = this.wifiManager.AddNetwork(wifiConfig);
        this.wifiManager.Disconnect();
         this.wifiManager.EnableNetwork(netId, true);
         this.wifiManager.Reconnect();//低安卓版本可用

     }*/
    public void ONcxwifi()
    {


        //this.connectivityManager.UnregisterNetworkCallback(this.networkCallback);

        var currentlyActiveNetwork = this.connectivityManager.ActiveNetwork;
        if (currentlyActiveNetwork != null)
        {
            var networkCapabilities = this.connectivityManager.GetNetworkCapabilities(currentlyActiveNetwork);
            if (networkCapabilities.HasTransport(TransportType.Wifi))
            {
                //Android11及以上可以使用，清除建议列表，可以断开当前的网络

                var networkSuggestions = this.wifiManager.NetworkSuggestions;
                wifiManager.RemoveNetworkSuggestions(networkSuggestions);

            }
        }
    }
    
    }

 class MyNetworkCallback : ConnectivityManager.NetworkCallback
{
   private Context _context;
    private ConnectivityManager _connectivityManager;
    private MyNetworkCallback _networkCallback;
    
    public MyNetworkCallback (Context context)
        {
        _context = context;
        //this._connectivityManager = connectivityManager;
        //this._connectivityManager.UnregisterNetworkCallback(this);
    }
   public void get(ConnectivityManager connectivityManager, MyNetworkCallback networkCallback)
    {
        this._networkCallback = networkCallback;
        this._connectivityManager = connectivityManager;//一定要添加这个否则会别顶调
}
    public override void OnAvailable(Network network)//连接成功后的操作；
    {
        Toast toast = new Toast(this._context);
        toast.SetText("执行成功，待检测");
        toast.Show();
        this._connectivityManager.UnregisterNetworkCallback(this);
        // 当网络可用时执行的操作
    }

    public override void OnLost(Network network)//连接失败后的操作
    {
        Toast toast = new Toast(this._context);
        toast.SetText("连接失败");
        toast.Show();
        this._connectivityManager.UnregisterNetworkCallback(this);
        // this. _connectivityManager.UnregisterNetworkCallback(network);
        // 当网络丢失时执行的操作
    }
}