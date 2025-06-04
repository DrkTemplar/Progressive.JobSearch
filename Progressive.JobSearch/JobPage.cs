using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumUndetectedChromeDriver;

namespace Progressive.JobSearch
{
    internal class JobPage
    {

        private readonly ChromeDriver Driver;
        #region Selectors
        private readonly string ProgressiveJobPage = "https://careers.progressive.com/search/jobs/?bid=13555";
        private readonly string Jobs = "div.jobs-section__list.non-facet.space-xlarge > div > div > div.columns.xlarge-6 > h4 > a";
        private readonly string JobPostedDate = "div.jobs-section__list.non-facet.space-xlarge > div > div > div.columns.xlarge-2 > time";
        private readonly string RemotePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(3) > div.facet-item__heading > button > span";
        private readonly string RemoteCheckbox = ".facet-section-inner > div > div.facet-item.padded-v-small.plus.facet-item--expanded > div.facet-item__options > div > div:nth-child(3) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";
        private readonly string StatePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(4) > div.facet-item__heading > button > span";
        private readonly string StateMore = ".space-medium > div.facet-section-inner > div > div:nth-child(4) > div.facet-item__options > div.small-text > a";
        private readonly string OhioCheckbox = "#facet-item__row--location_state_seo > div:nth-child(25) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";
        private readonly string Pages = ".jobs-section__paginate.non-facet > div > a:not([class])";
        private readonly string NextPage = ".jobs-section__paginate.non-facet > div > a.next_page";
        #endregion
        int CurrentPage = 1;
        int TotalPages = 1;

        public JobPage()
        {
            ChromeOptions options = new();
            //options.AddArgument("--start-maximized");
            //options.AddArgument("--log-level=1");
            //options.AddArgument("--headless=new");
            //options.AddExcludedArgument("enable-automation");
            //options.AddExcludedArgument("enable-logging");
            //options.AddExcludedArgument("useAutomationExtension");
            //options.AddArgument("--disable-blink-features=AutomationControlled");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--remote-allow-origins=*");
            options.AddArgument(ProgressiveJobPage);
            Driver = UndetectedChromeDriver.Create(options: options, driverExecutablePath: "C:\\git\\repos\\Progressive.JobSearch\\Progressive.JobSearch\\bin\\Debug\\chromedriver.exe");
        }

        internal void GetJobs()
        {
            bool done = false;
            while (!done)
            {
                var list = Driver.FindElements(By.CssSelector(Jobs));
                var dates = Driver.FindElements(By.CssSelector(JobPostedDate));
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Text.ToLower().Contains("automation")
                        || list[i].Text.ToLower().Contains("qa")
                        || list[i].Text.ToLower().Contains("quality")
                        || list[i].Text.ToLower().Contains("sdet"))
                    {
                        Console.WriteLine($"*** Title: {list[i].Text}");
                    }
                    else
                    {
                        Console.WriteLine($"Title: {list[i].Text}");
                    }
                    Console.WriteLine($"Posted: {dates[i].Text}");
                }
                done = CurrentPage == TotalPages;
                if (!done)
                {
                    CurrentPage++;
                    if (ClickElement(NextPage))
                    {
                        WaitForLoader();
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
        }

        internal void Close()
        {
            Driver.Close();
        }

        private void ScrollToElement(string selector)
        {
            WaitForElement(selector);
            var element = Driver.FindElement(By.CssSelector(selector));
            Driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        private bool ClickElement(string selector)
        {
            try
            {
                WaitForElement(selector);
                ScrollToElement(selector);
                Driver.FindElement(By.CssSelector(selector)).Click();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        private bool ElementExists(string selector)
        {
            try
            {
                WaitForElement(selector);

                if (Driver.FindElements(By.CssSelector(selector)).Count > 0)
                {
                    return true;
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return false;
        }

        internal bool Filter()
        {
            Console.WriteLine(Driver.Url);
            if (SelectOhio() && SelectRemote())
            {
                TotalPages = 1 + Driver.FindElements(By.CssSelector(Pages)).Count;
                Console.WriteLine($"Total pages: {TotalPages}");
                return true;
            }
            return false;
        }

        private bool SelectRemote()
        {
            Console.WriteLine("Filter for Remote");
            if (ClickElement(RemotePlus) && ClickElement(RemoteCheckbox))
            {
                WaitForLoader();
                return true;
            }
            return false;
        }

        private bool SelectOhio()
        {
            Console.WriteLine("Filter for Ohio");
            if (ClickElement(StatePlus) && ClickElement(StateMore) && ElementExists(OhioCheckbox) && ClickElement(OhioCheckbox))
            {
                WaitForLoader();
                return true;
            }
            return false;
        }

        private void WaitForElement(string selector)
        {
            WebDriverWait w = new(Driver, TimeSpan.FromSeconds(10));
            w.Until(condition =>
            {
                try
                {
                    var element = Driver.FindElement(By.CssSelector(selector));
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
            WebDriverWait w = new(Driver, TimeSpan.FromSeconds(30));
            _ = w.Until(condition =>
            {
                try
                {
                    var loader = Driver.FindElement(By.CssSelector("div.preloader.preloader--search"));
                    string? style = loader.GetDomAttribute("style");
                    return style == null || style.Contains("display: none;");
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
