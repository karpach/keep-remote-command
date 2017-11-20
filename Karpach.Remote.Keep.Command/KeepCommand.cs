using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Karpach.Remote.Keep.Command
{
    [Export(typeof(IRemoteCommand))]
    public class KeepCommand : CommandBase
    {
        private readonly Lazy<ChromeDriver> _chromeDriver;
        private const int KeepLoadTimeout = 10;

        public KeepCommand():this(null)
        {            
        }

        public KeepCommand(Guid? id) : base(id)
        {
            _chromeDriver = new Lazy<ChromeDriver>(() =>
            {
                var options = new ChromeOptions();
                options.AddArguments($"--user-data-dir={((KeepCommandSettings)Settings).ChromeProfileFolder}");
                options.AddArguments("--start-maximized");
                return new ChromeDriver(options);
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
            _chromeDriver.Value.Navigate().GoToUrl($"http://keep.google.com/#LIST/{((KeepCommandSettings)Settings).ListId}");            
            var wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));
            IWebElement element = wait.Until(x => x.FindElement(By.CssSelector("html.gr__keep_google_com")));
            if (element == null)
            {
                return;
            }
            if (parameters != null && parameters.Length == 1)
            {
                _chromeDriver.Value.Keyboard.SendKeys($"{parameters[0]}\n");
            }
            else
            {
                _chromeDriver.Value.Keyboard.SendKeys($"{((KeepCommandSettings)Settings).CommandName} {DateTime.Now.ToShortTimeString()}\n");
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
    }
}
