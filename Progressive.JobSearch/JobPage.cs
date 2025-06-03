using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumUndetectedChromeDriver;

namespace Progressive.JobSearch
{
    internal class JobPage
    {

        private ChromeDriver Driver;
        string ProgressiveJobPage = "https://careers.progressive.com/search/jobs/?bid=13555";
        string JobTitles = "div.jobs-section__list.non-facet.space-xlarge > div > div > div.columns.xlarge-6 > h4 > a";
        string RemotePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(3) > div.facet-item__heading > button > span";
        string RemoteCheckbox = ".facet-section-inner > div > div.facet-item.padded-v-small.plus.facet-item--expanded > div.facet-item__options > div > div:nth-child(3) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";
        string StatePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(4) > div.facet-item__heading > button > span";
        string StateMore = ".space-medium > div.facet-section-inner > div > div:nth-child(4) > div.facet-item__options > div.small-text > a";
        string OhioCheckbox = "#facet-item__row--location_state_seo > div:nth-child(25) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";
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
            options.AddArgument(ProgressiveJobPage);
            Driver = UndetectedChromeDriver.Create(options: options, driverExecutablePath: "C:\\git\\repos\\Progressive.JobSearch\\Progressive.JobSearch\\bin\\Debug\\chromedriver.exe");
            string window = Driver.CurrentWindowHandle;

        }

        internal void GetJobs()
        {
            bool done = false;
            while (!done)
            {
                var list = Driver.FindElements(By.CssSelector(JobTitles));
                foreach (var job in list)
                {
                    if (job.Text.ToLower().Contains("automation") ||
                        job.Text.ToLower().Contains("qa") ||
                        job.Text.ToLower().Contains("quality") ||
                        job.Text.ToLower().Contains("sdet"))
                    {
                        Console.WriteLine($"*** {job.Text}");
                    }
                    else
                    {
                        Console.WriteLine(job.Text);
                    }
                }
                if (CurrentPage < TotalPages)
                {
                    CurrentPage++;
                    ClickElement(NextPage);
                    WaitForLoader();
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

        private void ScrollToElement(string selector)
        {
            var element = Driver.FindElement(By.CssSelector(selector));
            // Cast driver to IJavaScriptExecutor
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            // Scroll the element into view
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);

        }

        private void ClickElement(string selector)
        {
            WaitForElement(selector);
            ScrollToElement(selector);
            Driver.FindElement(By.CssSelector(selector)).Click();

        }

        private bool ElementExists(string selector)
        {
            WaitForElement(selector);

            if(Driver.FindElements(By.CssSelector(selector)).Count > 0)
            {
                return true;
            };
            return false;
        }

        internal void NavTo()
        {
            Console.WriteLine(Driver.Url);
            Console.WriteLine("Filter for Ohio");
            SelectOhio();
            Console.WriteLine("Filter for Remote");
            SelectRemote();
            TotalPages = 1 + Driver.FindElements(By.CssSelector(Pages)).Count();
            Console.WriteLine($"Total pages: {TotalPages}");
        }

        private void SelectRemote()
        {
            ClickElement(RemotePlus);
            ClickElement(RemoteCheckbox);
            WaitForLoader();
        }

        private void SelectOhio()
        {
            ClickElement(StatePlus);
            ClickElement(StateMore);
            if (ElementExists(OhioCheckbox))
            {
                ClickElement(OhioCheckbox);
                WaitForLoader();
            }
        }

        private void WaitForElement(string selector)
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

        private void WaitForLoader()
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
    }
}
