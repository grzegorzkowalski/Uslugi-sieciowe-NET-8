using HelloWorld.Interface;

namespace HelloWorld.Models
{
    public class TestInjection : ITestInjection
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string SetName ()
        {
            Name = "TestInjection";
            return Name;
        }
    }
}
