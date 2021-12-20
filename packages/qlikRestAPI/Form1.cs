using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using Qlik.Sense.RestClient;
using System.Web;
using System.Security;

namespace qlikRestAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Started!");
        }

        private void Export_Application(object sender, EventArgs e)
        {
            var senseServerUrl = "https://dev-gcscanalytics.paypalcorp.com/win/";
            var restClient = new RestClient(senseServerUrl);
            var securePassword = new SecureString();
            foreach (var c in "PayPal123".ToCharArray())
            {
                securePassword.AppendChar(c);
            }
            var certs = RestClient.LoadCertificateFromDirectory("certs/", securePassword);
            restClient.AsDirectConnection(4242, false, certs);
            using (new DebugConsole())
                restClient.Get("/qrs/about");
            /*
            var restClient = new RestClient("https://dev-gcscanalytics.paypalcorp.com/win/");
            restClient.AsNtlmUserViaProxy();
            Console.WriteLine(restClient.Get("/qrs/about"));
            var appId = "1b25daf2-7489-45e7-9283-83cc94748f2a";            
            Console.Write("Exporting... ");
            var rsp = restClient.Post<JObject>($"/qrs/app/{appId}/export/{Guid.NewGuid()}");
            var downloadPath = rsp["downloadPath"].ToString();
            var appName = downloadPath.Split('?').First().Split('/').Last();
            Console.WriteLine("Done.");
            Console.Write("Downloading... ");
            appName = HttpUtility.UrlDecode(appName);
            Console.WriteLine(appName);
            var stream = restClient.GetStream(downloadPath);
            using (var fileStream = File.OpenWrite(appName))
            {
                stream.CopyTo(fileStream);
            }

            Console.WriteLine("Done.");
            Console.WriteLine($"App export complete. Output file: {appName} ({new FileInfo(appName).Length/1000} kilobytes)");
            */
        }

        private void Import_Application(object sender, EventArgs e)
        {
            var restClient = new RestClient("https://dev-gcscanalytics.paypalcorp.com/win/");
            restClient.AsNtlmUserViaProxy();
            var data = File.ReadAllBytes(@"app.qvf");
            const string nameOfApp = "Subramani_R";
            Console.WriteLine(restClient.WithContentType("application/vnd.qlik.sense.app").Post("/qrs/app/upload?keepData=true&name=" + nameOfApp, data));
        }
    }
}
