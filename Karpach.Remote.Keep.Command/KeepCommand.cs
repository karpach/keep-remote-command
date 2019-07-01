using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPSOAuthSharp;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using Karpach.Remote.Keep.Command.Helpers;
using Karpach.Remote.Keep.Command.Models;
using NLog;
using LogLevel = NLog.LogLevel;

namespace Karpach.Remote.Keep.Command
{
    [Export(typeof(IRemoteCommand))]
    public class KeepCommand : CommandBase
    {                
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Lazy<Dictionary<string, string>> _dictionary;
        private string _token = string.Empty;
        private readonly KeepRequestFactory _keepRequestFactoryFactory;

        public KeepCommand():this(null)
        {            
        }

        public KeepCommand(Guid? id) : base(id)
        {
            _keepRequestFactoryFactory = new KeepRequestFactory();
            _dictionary = new Lazy<Dictionary<string, string>>(() =>
            {
                var result = new Dictionary<string,string>();                
                foreach (string filePath in Directory.GetFiles(Path.GetDirectoryName(GetType().Assembly.Location), "*.dict"))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] parts = line?.Split('|');
                            if (parts?.Length == 2 && !result.ContainsKey(parts[0]))
                            {
                                result.Add(parts[0], parts[1]);
                            }
                        }                        
                    }
                }
                return result;
            });
        }
        protected override Type SettingsType => typeof(KeepCommandSettings);
        public override string CommandTitle => ConfiguredValue ? ((KeepCommandSettings)Settings).CommandName : $"Keep Command - {NotConfigured}";
        public override Image TrayIcon => Resources.Icon.ToBitmap();
        public override void RunCommand(params object[] parameters)
        {
            if (!Configured)
            {                
                return;
            }
            int? delay = ((KeepCommandSettings) Settings).ExecutionDelay;
            if (delay.HasValue)
            {
                Thread.Sleep(delay.Value);
            }
            if (parameters != null && parameters.Length == 1)
            {
                string parameter = parameters[0].ToString().ToLower();
                string item = _dictionary.Value.ContainsKey(parameter)
                    ? _dictionary.Value[parameter]
                    : parameters[0].ToString();

                if (item != string.Empty)
                {
                    item = char.ToUpper(item[0]) + item.Substring(1);
                    Run(item);                    
                }
            }
            else
            {
                Run($"{((KeepCommandSettings)Settings).CommandName} {DateTime.Now.ToShortTimeString()}");
            }            
        }

        public override void ShowSettings()
        {
            var dlg = new SampleCommandSettingsForm((KeepCommandSettings)Settings);
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                LibSettings[Id] = dlg.Settings;
                ConfiguredValue = true;
            }
        }

        public override IRemoteCommand Create(Guid id)
        {
            return new KeepCommand(id);
        }

        private async void Run(string item)
        {
            try
            {
                Logger.Log(LogLevel.Error, "Unable to login.\n\n");
                var settings = (KeepCommandSettings)Settings;

                var googleClient = new GPSOAuthClient(settings.GoogleUserName, settings.GooglePassword);
                if (string.IsNullOrEmpty(_token))
                {
                    _token = await GetMasterToken(googleClient);
                }

                Dictionary<string, string> oauthResponse = await GetOauthResponse(googleClient).ConfigureAwait(false);
                if (!oauthResponse.ContainsKey("Auth"))
                {
                    _token = await GetMasterToken(googleClient);
                }
                oauthResponse = await GetOauthResponse(googleClient).ConfigureAwait(false);
                if (!oauthResponse.ContainsKey("Auth"))
                {
                    throw new Exception("Auth token was missing from oauth login response.");
                }

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", oauthResponse["Auth"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.Connection.Add("keep-alive");
                KeepRequest keepRequest = _keepRequestFactoryFactory.Create();
                HttpContent content = GetRequest(keepRequest.ToJson());
                HttpResponseMessage response = await client.PostAsync("https://www.googleapis.com/notes/v1/changes", content).ConfigureAwait(false);
                string json = await GetJson(response).ConfigureAwait(false);
                KeepResponse keepResponse = Serialize.FromJson(json);
                Node[] newNodes = new Node[2];
                string parentNodeId = settings.ListId;
                newNodes[0] = keepResponse.Nodes.Single(n => n.Id == parentNodeId);
                newNodes[1] = new Node
                {
                    Text = item,
                    ParentId = parentNodeId,
                    ParentServerId = newNodes[0].ServerId,

                    Type = "LIST_ITEM",
                    Timestamps = new Timestamps
                    {
                        Created = DateTimeOffset.Now,
                        Updated = DateTimeOffset.Now
                    },
                    Id = new RandomHelper().GetNodeId()
                };
                keepRequest = _keepRequestFactoryFactory.Create(newNodes, keepResponse.ToVersion);
                content = GetRequest(keepRequest.ToJson());
                response = await client.PostAsync("https://www.googleapis.com/notes/v1/changes", content).ConfigureAwait(false);
                json = GetJson(response).ConfigureAwait(false).GetAwaiter().GetResult();
                Logger.Log(LogLevel.Info, json);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Fatal, ex);
            }
        }

        private Task<Dictionary<string, string>> GetOauthResponse(GPSOAuthClient googleClient)
        {
            return googleClient.PerformOAuth(_token, "oauth2:https://www.googleapis.com/auth/memento https://www.googleapis.com/auth/reminders", "com.google.android.keep", "38918a453d07199354f8b19af05ec6562ced5788");
        }

        private async Task<string> GetMasterToken(GPSOAuthClient googleClient)
        {
            var masterLoginResponse = await googleClient.PerformMasterLogin().ConfigureAwait(false);

            if (masterLoginResponse.ContainsKey("Error"))
            {
                throw new Exception($"Google returned an error message: '{masterLoginResponse["Error"]}'");
            }

            if (!masterLoginResponse.ContainsKey("Token"))
            {
                throw new Exception("Token was missing from master login response.");
            }

            return masterLoginResponse["Token"];
        }

        private HttpContent GetRequest(string jsonBody)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            content.Headers.Add("UserAgent", "gkeepapi/0.11.2");
            return content;
        }

        private async Task<string> GetJson(HttpResponseMessage response)
        {
            using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (Stream decompressed = new GZipStream(stream, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(decompressed))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
