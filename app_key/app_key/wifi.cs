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
    public static String b_state="���ʱ����";//��ť��״̬
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
 Disabled�� ��ʾWi-Fi�����ѱ����û�رա��豸��ǰû�������������硣

Disabling�� ��ʾWi-Fi���ڱ����õĹ����С����״̬�ǹ����Եģ�
        ����ϵͳ���ڽ��в����Թر�Wi-Fi���ܡ�

Enabled�� ��ʾWi-Fi���������ã���������ζ���豸�Ѿ��ɹ����ӵ��κ�Wi-Fi���硣
        ��ʱ�豸����ɨ�貢�������ӿ��õ�Wi-Fi���硣

Enabling�� ��ʾWi-Fi���ڱ����õĹ����С�ͬ����һ��������״̬��
        ��ʾϵͳ���ڽ��в����Դ�Wi-Fi���ܡ�

Unknown�� ��ʾ�޷�ȷ��Wi-Fi��״̬����������Ϊϵͳ�ڲ������������δԤ�ڵ����
        �����޷���ȡ׼ȷ��Wi-Fi״̬��Ϣ��
         */

 
        var wifistae = this.wifiManager.WifiState;


        switch (wifistae)
        {
            case WifiState.Disabled:
                this.text1_state = "����״̬�����wifi";
              
                b_state = "���ʱ����";

                break;
            case WifiState.Enabled:
               // var connectionInfo = this.wifiManager.ConnectionInfo;
                var activeNetwork = connectivityManager.ActiveNetwork;
                if (activeNetwork != null)
                {
                    // ���Խ�������ɴ��Լ����ȷ���������ӵ�������
                    var networkCapabilities = connectivityManager.GetNetworkCapabilities(activeNetwork);

                    // �ж������Ƿ����INTERNET����
                   if( networkCapabilities.HasTransport(TransportType.Wifi))
                    {
                       
                      /*  if (MainActivity.UpOn)
                        {
                            NOin = 0;
                            this.text1_state = "����״̬�����ӵ�wifi";
                            text2_state = "�Ƽ�wifi ��"+ssid;
                            b_state = "���ʱ����";
                        }
                        else*/
                        
                            NOin = 1;
                            this.text1_state = "����״̬�����ӵ�wifi";
                            text2_state = "�Ƽ�wifi ��" + ssid;
                            b_state = "���ʱ�ر�";
              
                    }

                    else
                    {
                        NOin = 0;
                        this.text1_state = "����״̬��������wifi";
                        text2_state = "�Ƽ�wifi ��";
                        b_state = "���ʱ����";
                    }
                }
                else
                {
                    NOin = 0;
                    this.text1_state = "����״̬��������wifi";
                   b_state = "���ʱ����";
                    text2_state = "�Ƽ�wifi ��";
                }
                    break;
        
                case WifiState.Unknown:
                NOin = 0;
                this.text1_state = "����״̬��δ֪����";
                b_state = "���ʱ����";
                text2_state = "�Ƽ�wifi ��";

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
        textView2.Text = text2_state;//wifi����wifi����

    }
    public void close_or_open_wifi()
    {
        this.suggestionfun();
        Toast toast = new Toast(this._context);
       
        if (!this.wifiManager.IsWifiEnabled)
        {
            toast.SetText("������WiFi");
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
            toast.SetText("�Ѿ��ܾ�");
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
                          .Build();//�����б�
        List<WifiNetworkSuggestion> suggestionsList = new List<WifiNetworkSuggestion>();//��ȡ�����б�
        //������ϳɹ��ʷ�����
        suggestionsList.Add(suggestion);//��ӽ���
        this.status = this.wifiManager.AddNetworkSuggestions(suggestionsList);
         
    }
    private void cxwifi()
    {


        {
            //�����˺�����
            //NetworkCallback networkCallback = new ConnectivityManager.NetworkCallback();
            this. networkCallback = new MyNetworkCallback(this._context);
            this.builder = new WifiNetworkSpecifier.Builder().
            SetSsid(ssid).
          SetWpa2Passphrase(pwd);
            // this.builder.SetBssid(MacAddress.FromString("192.168.2.1"));
            var buildeR = this.builder.Build();
            //��������
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
         this.wifiManager.Reconnect();//�Ͱ�׿�汾����

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
                //Android11�����Ͽ���ʹ�ã���������б����ԶϿ���ǰ������

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
        this._connectivityManager = connectivityManager;//һ��Ҫ�����������𶥵�
}
    public override void OnAvailable(Network network)//���ӳɹ���Ĳ�����
    {
        Toast toast = new Toast(this._context);
        toast.SetText("ִ�гɹ��������");
        toast.Show();
        this._connectivityManager.UnregisterNetworkCallback(this);
        // ���������ʱִ�еĲ���
    }

    public override void OnLost(Network network)//����ʧ�ܺ�Ĳ���
    {
        Toast toast = new Toast(this._context);
        toast.SetText("����ʧ��");
        toast.Show();
        this._connectivityManager.UnregisterNetworkCallback(this);
        // this. _connectivityManager.UnregisterNetworkCallback(network);
        // �����綪ʧʱִ�еĲ���
    }
}