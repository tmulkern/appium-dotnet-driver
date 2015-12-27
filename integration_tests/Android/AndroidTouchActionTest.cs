﻿using NUnit.Framework;
using System;
using Appium.Integration.Tests.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Appium.Integration.Tests.Android
{
	[TestFixture ()]
	public class AndroidTouchActionTest
	{
		private AndroidDriver<AppiumWebElement> driver;

		[TestFixtureSetUp]
		public void BeforeAll(){
			DesiredCapabilities capabilities = Env.isSauce () ? 
				Caps.getAndroid501Caps (Apps.get ("androidApiDemos")) :
				Caps.getAndroid19Caps (Apps.get ("androidApiDemos"));
			if (Env.isSauce ()) {
				capabilities.SetCapability("username", Env.getEnvVar("SAUCE_USERNAME")); 
				capabilities.SetCapability("accessKey", Env.getEnvVar("SAUCE_ACCESS_KEY"));
				capabilities.SetCapability("name", "android - complex");
				capabilities.SetCapability("tags", new string[]{"sample"});
			}
			Uri serverUri = Env.isSauce () ? AppiumServers.sauceURI : AppiumServers.LocalServiceURIAndroid;
            driver = new AndroidDriver<AppiumWebElement>(serverUri, capabilities, Env.INIT_TIMEOUT_SEC);	
			driver.Manage().Timeouts().ImplicitlyWait(Env.IMPLICIT_TIMEOUT_SEC);
            driver.CloseApp();
        }

        [SetUp]
        public void SetUp()
        {
            if (driver != null)
            {
                driver.LaunchApp();
            }
        }

        [TearDown]
        public void TearDowwn()
        {
            if (driver != null)
            {
                driver.CloseApp();
            }
        }

        [TestFixtureTearDown]
		public void AfterAll(){
            if (driver != null)
            {
                driver.Quit();
            }
            if (!Env.isSauce())
            {
                AppiumServers.StopLocalService();
            }
		}

		[Test ()]
		public void TouchActionTestCase ()
		{
			IList<AppiumWebElement> els = driver.FindElementsByClassName ("android.widget.TextView");
			var loc1 = els [7].Location;
            AppiumWebElement target = els[1];
            var loc2 = target.Location;
			driver.Swipe (loc1.X, loc1.Y, loc2.X, loc2.Y, 800); //this action includes almost all touch actions
            Assert.AreNotEqual(loc2.Y, target.Location.Y);
		}

        [Test()]
        public void MultiActionTestCase()
        {
            IList<AppiumWebElement> els = driver.FindElementsByClassName("android.widget.TextView");
            var loc1 = els[7].Location;
            AppiumWebElement target = els[1];
            var loc2 = target.Location;

            TouchAction press = new TouchAction(driver);
            press.Press(loc1.X, loc1.Y);

            TouchAction move = new TouchAction(driver);
            move.MoveTo(loc2.X, loc2.Y);

            MultiAction multiAction = new MultiAction(driver);
            multiAction.Add(press).Add(move).Perform();
            Assert.AreNotEqual(loc2.Y, target.Location.Y);
        }
    }
}
