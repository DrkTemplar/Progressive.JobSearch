using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
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
        string JobTitles = "#search-remote_work-remote-jobs > div.page-container.fadeIn > div > div.template-content.non-menu > div.cs_block.cs_template_content.cs_template_content_jobs_search > div > div > div > div.padded-v-xlarge > div.row > div > div > div.columns.large-8.xlarge-9 > section > div > div.jobs-section__inner > div.jobs-section__list.non-facet.space-xlarge > div > div > div.columns.xlarge-6 > h4 > a";

        public JobPage()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--log-level=1");
            Driver = new ChromeDriver(options);



        }

        internal void NavTo()
        {
            Driver.Url = ProgressiveJobPage;
        }

        internal void GetJobs()
        {

            var list = Driver.FindElements(By.CssSelector(JobTitles));
        }

        internal void Close()
        {
            Driver.Close();
        }
    }

}
