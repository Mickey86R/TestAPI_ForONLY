namespace TestAPI_ForONLY.Model
{
    public class DeptInfo
    {
        public string Name { get; set; }
        public int PostCount { get; set; }
        public int CustCount { get; set; }

        public DeptInfo(string name, int postCount, int custCount)
        {
            this.Name = name;
            this.PostCount = postCount;
            this.CustCount = custCount;
        }
    }
}
