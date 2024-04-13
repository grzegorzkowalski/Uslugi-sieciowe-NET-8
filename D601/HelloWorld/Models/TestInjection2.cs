using HelloWorld.Interface;

namespace HelloWorld.Models
{
    public class TestInjection2 : ITestInjection
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string SetName()
        {
            Name = "Test";
            return Name;
        }
    }
}
