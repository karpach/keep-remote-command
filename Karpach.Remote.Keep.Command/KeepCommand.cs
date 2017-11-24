using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using LogLevel = NLog.LogLevel;

namespace Karpach.Remote.Keep.Command
{
    [Export(typeof(IRemoteCommand))]
    public class KeepCommand : CommandBase
    {
        private readonly Lazy<ChromeDriver> _chromeDriver;
        private const double KeepLoadTimeout = 10;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public KeepCommand():this(null)
        {            
        }

        public KeepCommand(Guid? id) : base(id)
        {
            _chromeDriver = new Lazy<ChromeDriver>(() =>
            {
                var options = new ChromeOptions();
                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                options.AddArguments("--headless");                
                return new ChromeDriver(driverService, options);
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
            _chromeDriver.Value.Navigate().GoToUrl($"https://keep.google.com/#LIST/{((KeepCommandSettings)Settings).ListId}");                        
            var wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));
            try
            {
                ReadOnlyCollection<IWebElement> elements = wait.Until(x => x.FindElements(By.CssSelector("#initialView #identifierId")));
                if (elements.Count == 1)
                {
                    if (!Login(elements[0]))
                    {
                        Logger.Log(LogLevel.Error, "Unable to login:\n" + _chromeDriver.Value.PageSource + "\n\n");
                        return;
                    }
                    wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));
                    wait.Until(ExpectedConditions.UrlContains("https://keep.google.com/"));
                    _chromeDriver.Value.Navigate().GoToUrl($"https://keep.google.com/#LIST/{((KeepCommandSettings)Settings).ListId}");
                }
                wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));
                elements = wait.Until(VisibleElementsByLocatedBy(By.XPath("//div[@aria-label='Remind me']"),1));                
                if (elements.Count != 1)
                {
                    Logger.Log(LogLevel.Error, "Unable to load keep page:\n" + _chromeDriver.Value.PageSource + "\n\n");
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

        private Func<IWebDriver, ReadOnlyCollection<IWebElement>> VisibleElementsByLocatedBy(By locator, int count)
        {
            return (Func<IWebDriver, ReadOnlyCollection<IWebElement>>)(driver =>
            {
                try
                {
                    ReadOnlyCollection<IWebElement> elements = driver.FindElements(locator);                    
                    Func<IWebElement, bool> predicate = element => element.Displayed;                    
                    if (elements.Count(predicate) != count)
                        return null;
                    return new ReadOnlyCollection<IWebElement>(elements.Where<IWebElement>(predicate).ToList());
                }
                catch (StaleElementReferenceException)
                {
                    return (ReadOnlyCollection<IWebElement>)null;
                }
            });
        }

        private bool Login(IWebElement userNameElement)
        {
            var settigs = (KeepCommandSettings)Settings;
            userNameElement.SendKeys(settigs.GoogleUserName + "\n");
            var wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));                                    
            ReadOnlyCollection<IWebElement> elements = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#initialView #password input")));
            if (elements.Count != 1)
            {
                return false;
            }
            elements[0].SendKeys(settigs.GooglePassword + "\n");
            return true;
        }
    }
}
