using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        string GetHelloMessage(string name);
    }
    
    public class SampleService : ISampleService
    {
        public SampleService()
        {
            Console.WriteLine("Sample service created!");
        }

        public string GetHelloMessage(string name)
        {
            return "Hello " + name;
        }
    }
}
