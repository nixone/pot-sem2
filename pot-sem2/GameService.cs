using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        GameState GetCurrentState();
    }
}
