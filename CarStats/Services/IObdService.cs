using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSDHelper.Services {
	public interface IObdService {
        ConnectionStateEnum ConnectionState { get; }
        Task<bool> Connect(string portName);
	    Task<bool> Initialize();
        Task<string> Send(string command);
		Task SaveLog(string fileName);

		Task<ObdResponse> Send(ObdRequest request);
    }
}
