using System.Diagnostics;
using System.Net;
using System.ServiceProcess;
using System.Threading;

namespace FCPSService
{
    public partial class Service : ServiceBase
    {
        public static Timer timer;
        public Service()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            timer = new Timer(new TimerCallback(Execute));
            timer.Change(0, 1000);
        }
        protected override void OnStop()
        {
        }
        public void Execute(object state)
        {
            timer.Change(0, -1);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.Server.Equals("HTTP Appgw"))
                    Process.Start("fcps.exe");
            }
            catch (WebException e)
            {
                if (!(e.Status == WebExceptionStatus.Timeout))
                    eventLog1.WriteEntry(e.ToString());
            }
            timer.Change(0, 10);
        }
    }
}
