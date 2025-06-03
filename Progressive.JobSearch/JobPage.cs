using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.JobSearch
{
    internal class JobPage
    {

        private ChromeDriver Driver;
        //private UndetectedChromeDriver Driver;
        string ProgressiveJobPage = "https://careers.progressive.com/search/jobs/?bid=13555";
        string JobTitles = "div.jobs-section__list.non-facet.space-xlarge > div > div > div.columns.xlarge-6 > h4 > a";
        string RemotePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(3) > div.facet-item__heading > button > span";
        string RemoteCheckbox = ".facet-section-inner > div > div.facet-item.padded-v-small.plus.facet-item--expanded > div.facet-item__options > div > div:nth-child(3) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";
        string StatePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(3) > div.facet-item__heading > button > span";
        string StateMore = ".space-medium > div.facet-section-inner > div > div.facet-item.padded-v-small.plus.facet-item--expanded > div.facet-item__options > div.small-text > a";
        string OhioCheckbox = "#facet-item__row--location_state_seo > div:nth-child(25) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";
        // categories change
        string Pages = ".jobs-section__paginate.non-facet > div > a:not([class])";
        int CurrentPage = 1;
        int TotalPages;
        string NextPage = ".jobs-section__paginate.non-facet > div > a.next_page";

        public JobPage()
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--start-maximized");
            //options.AddArgument("--log-level=1");
            //options.AddArgument("--headless=new");
            //options.AddExcludedArgument("enable-automation");
            //options.AddExcludedArgument("enable-logging");
            //options.AddExcludedArgument("useAutomationExtension");
            //options.AddArgument("--disable-blink-features=AutomationControlled");
            //options.AddArguments("--no-sandbox");
            //options.AddArgument("--remote-allow-origins=*");
            //Driver = new ChromeDriver(options);
            options.AddArgument(ProgressiveJobPage);
            //ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            Driver = UndetectedChromeDriver.Create(options: options, driverExecutablePath: "C:\\git\\repos\\Progressive.JobSearch\\Progressive.JobSearch\\bin\\Debug\\chromedriver.exe");
            //Driver.SwitchTo().NewWindow(WindowType.Window);
            string window = Driver.CurrentWindowHandle;
            //Driver = new UndetectedChromeDriver(service, options, TimeSpan.FromSeconds(10));
            //service.Start(); //  Start the service



            //using (var driver = UndetectedChromeDriver.Create(options: options, driverExecutablePath: "C:\\git\\repos\\Progressive.JobSearch\\Progressive.JobSearch\\bin\\Debug\\net8.0\\chrome-win64\\chrome.exe"))
            //{
            //    driver.Url = ProgressiveJobPage;
            //    WaitForList();
            //    SelectRemote();


            //}

        }

        internal void ClickNextPage()
        {
            WaitForElement(NextPage);
            var next = Driver.FindElement(By.CssSelector(NextPage));


            // Cast driver to IJavaScriptExecutor
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;

            // Scroll the element into view
            js.ExecuteScript("arguments[0].scrollIntoView(true);", next);



            next.Click();
            WaitForList();
        }

        internal void NavTo()
        {
            Console.WriteLine(Driver.Url);
            SelectRemote();
            WaitForList();
            SelectOhio();
            WaitForList();
            SetPages();
        }

        internal void SetPages()
        {
            var pages = Driver.FindElements(By.CssSelector(Pages));
            TotalPages = 1 + pages.Count();
            Console.WriteLine($"Total pages: {TotalPages}");
        }

        internal void GetJobs()
        {
            bool done = false;
            while(!done)
            {
                var list = Driver.FindElements(By.CssSelector(JobTitles));
                foreach (var job in list)
                {
                    Console.WriteLine(job.Text);
                }
                if (CurrentPage < TotalPages)
                {
                    CurrentPage++;
                    ClickNextPage();
                }
                else
                {
                    done = true;
                }
            }
        }

        internal void Close()
        {
            Driver.Close();
        }

        internal void WaitForList()
        {
            WebDriverWait w = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            w.Until(condition =>
            {
                //Console.WriteLine("wait for list");
                try
                {
                    var elementToBeDisplayed = Driver.FindElements(By.CssSelector(JobTitles));
                    return elementToBeDisplayed.Count > 0;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        internal void WaitForElement(string selector)
        {
            WebDriverWait w = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            w.Until(condition =>
            {
                try
                {
                    var element = Driver.FindElement(By.CssSelector(selector));
                    //Console.WriteLine("wait for element");
                    return element.Enabled && element.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        internal void WaitForLoader()
        {
            WebDriverWait w = new(Driver, TimeSpan.FromSeconds(60));
            w.Until(condition =>
            {
                try
                {
                    var loader = Driver.FindElement(By.CssSelector("div.preloader.preloader--search"));
                    //Console.WriteLine("wait for loader");
                    if(loader.GetDomAttribute("style") != null)
                    {
                        return loader.GetDomAttribute("style").Contains("display: none;");
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        internal void SelectRemote()
        {
            WaitForElement(RemotePlus);
            Driver.FindElement(By.CssSelector(RemotePlus)).Click();
            WaitForElement(RemoteCheckbox);
            Driver.FindElement(By.CssSelector(RemoteCheckbox)).Click();
            WaitForLoader();


        }

        internal void SelectOhio()
        {
            WaitForElement(StatePlus);
            Driver.FindElement(By.CssSelector(StatePlus)).Click();
            WaitForElement(StateMore);
            Driver.FindElement(By.CssSelector(StateMore)).Click();
            var ohio = Driver.FindElements(By.CssSelector(OhioCheckbox));
            if(ohio.Count > 0)
            {
                WaitForElement(OhioCheckbox);
                Driver.FindElement(By.CssSelector(OhioCheckbox)).Click();
                WaitForLoader();

            }

        }

    }

}
