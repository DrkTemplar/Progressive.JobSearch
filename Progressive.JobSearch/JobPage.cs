using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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
        string ProgressiveJobPage = "https://careers.progressive.com/search/jobs/?bid=13555";
        string JobTitles = "div.jobs-section__list.non-facet.space-xlarge > div > div > div.columns.xlarge-6 > h4 > a";
        string RemotePlus = ".space-medium > div.facet-section-inner > div > div:nth-child(3) > div.facet-item__heading > button > span";
        string RemoteCheckbox = ".facet-section-inner > div > div.facet-item.padded-v-small.plus.facet-item--expanded > div.facet-item__options > div > div:nth-child(3) > a > span.facet-item__options-item-type.facet-item__options-item-type--multi";

        public JobPage()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            //options.AddArgument("--log-level=1");
            Driver = new ChromeDriver(options);



        }

        internal void NavTo()
        {
            Driver.Url = ProgressiveJobPage;
            WaitForList();
            SelectRemote();
        }

        internal void GetJobs()
        {

            var list = Driver.FindElements(By.CssSelector(JobTitles));
            Console.WriteLine($"Total jobs found: {list.Count()}");
            foreach (var job in list)
            {
                Console.WriteLine(job.Text);
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
                try
                {
                    var elementToBeDisplayed = Driver.FindElements(By.CssSelector(JobTitles));
                    Console.WriteLine("wait for list");
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
                    Console.WriteLine("wait for element");
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
                    Console.WriteLine("loader");
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
            //WaitForLoader();


        }

    }

}
