using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace MemLeakTest;

public class Tests
{
    private IOSDriver _driver;

    [SetUp]
    public void SetUp()
    {
        var serverUri = new Uri(Environment.GetEnvironmentVariable("APPIUM_HOST") ?? "http://127.0.0.1:4723/");
        var driverOptions = new AppiumOptions
                            {
                                AutomationName = AutomationName.iOSXcuiTest,
                                App = "<APPLOCATION<",
                                PlatformName = "iOS",
                                DeviceName = "<IPHONENAME>",
                                PlatformVersion = "17.6"
                            };

        driverOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, "<UDID>");
        driverOptions.AddAdditionalAppiumOption("appium:xcodeOrgId", "<ENTER>");
        driverOptions.AddAdditionalAppiumOption("appium:xcodeSigningId", "<ENTER>");
        driverOptions.AddAdditionalAppiumOption("appium:DEVELOPMENT_TEAM", "<ENTER>");

        _driver = new IOSDriver(serverUri, driverOptions, TimeSpan.FromSeconds(180));
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Dispose();
    }

    private void NavigateToMainPage()
    {
        var backButtonQuery = new ByAccessibilityId("BackButton");
        var backButton = _driver.FindElement(backButtonQuery);
        Assert.That(backButton, Is.Not.Null);
        backButton.Click();
    }

    private void NavigateToSubPage()
    {
        var mainButtonQuery = new ByAccessibilityId("MainPageButton");
        var backButton = _driver.FindElement(mainButtonQuery);
        Assert.That(backButton, Is.Not.Null);
        backButton.Click();
    }

    [Test]
    public void FindByAccessibilityIdTest()
    {
        var counter = 0;

        try
        {
            const int max = 100;
            for (var i = 0; i < max; i++)
            {
                NavigateToSubPage();
                Thread.Sleep(1000);
                NavigateToMainPage();
                Thread.Sleep(1000);
                counter++;
            }
        }
        catch (Exception e)
        {
            Assert.Fail($"Failed after {counter}");
            Console.WriteLine(e.Message);
        }
    }
}