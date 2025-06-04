using Progressive.JobSearch;

Console.WriteLine("Start the job search");
JobPage jp = new();
if (jp.Filter())
{
    jp.GetJobs();
}
Console.WriteLine("Complete");
jp.Close();

