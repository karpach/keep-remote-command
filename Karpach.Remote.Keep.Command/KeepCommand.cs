using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Karpach.Remote.Commands.Base;
using Karpach.Remote.Commands.Interfaces;
using Karpach.Remote.Keep.Command.Helpers;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Keys = OpenQA.Selenium.Keys;
using LogLevel = NLog.LogLevel;

namespace Karpach.Remote.Keep.Command
{
    [Export(typeof(IRemoteCommand))]
    public class KeepCommand : CommandBase
    {
        private readonly Lazy<ChromeDriver> _chromeDriver;
        private const double KeepLoadTimeout = 10;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Lazy<Dictionary<string, string>> _dictionary;

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
                if (((KeepCommandSettings) Settings).Headless)
                {
                    options.AddArguments("--headless");
                    options.AddArguments("--disable-gpu");
                }

                string chromeProfileFolder = ((KeepCommandSettings) Settings).ChromeProfileFolder.NormalizeChromeProfileFolder();
                if (!string.IsNullOrEmpty(chromeProfileFolder))
                {
                    options.AddArguments($"--user-data-dir={chromeProfileFolder}");
                }                
                return new ChromeDriver(driverService, options);
            });

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
            try
            {
                string keepUrl = $"https://keep.google.com/#LIST/{((KeepCommandSettings)Settings).ListId}";
                if (!string.Equals(_chromeDriver.Value.Url, keepUrl, StringComparison.InvariantCultureIgnoreCase))
                {                
                    string photosUrl = "https://photos.google.com";
                    WebDriverWait wait;
                    ReadOnlyCollection<IWebElement> elements;
                    _chromeDriver.Value.Navigate().GoToUrl($"{photosUrl}/login");
                    if (!_chromeDriver.Value.Url.Contains(photosUrl))
                    {                    
                        wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));
                        elements = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#initialView #identifierId")));
                        if (elements.Count == 1)
                        {
                            if (!Login(elements[0]))
                            {
                                Logger.Log(LogLevel.Error, "Unable to login:\n" + _chromeDriver.Value.PageSource + "\n\n");
                                return;
                            }
                            wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout * 5));
                            wait.Until(ExpectedConditions.UrlContains(photosUrl));                        
                        }
                        else
                        {
                            Logger.Log(LogLevel.Error, "Unable to load login page:\n" + _chromeDriver.Value.PageSource + "\n\n");
                            return;
                        }
                    }                
                    _chromeDriver.Value.Navigate().GoToUrl(keepUrl);
                    wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));                
                    elements = wait.Until(VisibleElementsByLocatedBy(By.XPath("//div[@aria-label='Remind me']"),1));                
                    if (elements.Count != 1)
                    {
                        Logger.Log(LogLevel.Error, "Unable to load keep page:\n" + _chromeDriver.Value.PageSource + "\n\n");
                        return;
                    }
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
                        _chromeDriver.Value.Keyboard.SendKeys($"{item}\n");
                    }                    
                }
                else
                {
                    _chromeDriver.Value.Keyboard.SendKeys($"{((KeepCommandSettings)Settings).CommandName} {DateTime.Now.ToShortTimeString()}\n");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Fatal, ex);
                Logger.Log(LogLevel.Error, "Page source:\n" + _chromeDriver.Value.PageSource + "\n\n");
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

        public override void Dispose()
        {
            base.Dispose();
            if (_chromeDriver.IsValueCreated)
            {                
                _chromeDriver.Value.Close();
                _chromeDriver.Value.Dispose();
            }            
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
            userNameElement.SendKeys(settigs.GoogleUserName);
            Thread.Sleep(200);
            userNameElement.SendKeys(Keys.Enter);            
            var wait = new WebDriverWait(_chromeDriver.Value, TimeSpan.FromSeconds(KeepLoadTimeout));
            ReadOnlyCollection<IWebElement> elements = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#initialView #password input")));
            if (elements.Count != 1)
            {
                return false;
            }
            elements[0].SendKeys(settigs.GooglePassword);
            Thread.Sleep(200);
            elements[0].SendKeys(Keys.Enter);
            
            return true;
        }
    }
}
