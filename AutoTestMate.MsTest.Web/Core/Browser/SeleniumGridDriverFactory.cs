﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Helpers;
using AutoTestMate.MsTest.Web.Constants;
using AutoTestMate.MsTest.Web.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace AutoTestMate.MsTest.Web.Core.Browser
{
    [ExcludeFromCodeCoverage]
    public class SeleniumGridDriverFactory : IFactory<IWebDriver>
    {
        protected readonly IFactory<DriverOptions> BrowserOptionsFactory;
		public IConfigurationReader ConfigurationReader { get; set; }

        /// <summary>
        /// Used to create drivers based on the passed in runnsettings, appsetting or datarow stored in the configuration reader
        /// </summary>
        /// <param name="configurationReader">settings to be consumed while creating the driver</param>
        /// <param name="browserOptionsFactory"></param>
        public SeleniumGridDriverFactory(IConfigurationReader configurationReader, IFactory<DriverOptions> browserOptionsFactory)
        {
            ConfigurationReader = configurationReader;
            BrowserOptionsFactory = browserOptionsFactory;
        }

        public virtual IWebDriver Create()
        {
            IWebDriver driver;
            var loginWaitTime = Convert.ToInt64(ConfigurationReader.GetConfigurationValue(Constants.Configuration.LoginWaitTimeKey));

            var browserTypeValue = ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserTypeKey);
            var browserType = !string.IsNullOrWhiteSpace(browserTypeValue) ? BrowserTypeMapper.ConvertBrowserValue(browserTypeValue) : BrowserTypes.InternetExplorer;

			var cfgSeleniumGridUrl = ConfigurationReader.GetConfigurationValue(Constants.Configuration.SeleniumGridUrlKey);
			var seleniumGridUrl = string.IsNullOrEmpty(cfgSeleniumGridUrl) ? Configuration.DefaultSeleniumUrl : cfgSeleniumGridUrl;
			var commandTimeout = TimeSpan.FromMinutes(1);
			var driverOptions = BrowserOptionsFactory.Create();
			driver = new RemoteWebDriver(new Uri(seleniumGridUrl), driverOptions.ToCapabilities(), commandTimeout);
            return driver;
        }

    }
}