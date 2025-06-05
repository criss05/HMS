using HMS.Shared.DTOs.Patient;
using HMS.Shared.Proxies.Implementations;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class LoginService
    {
        private readonly PatientProxy _patientProxy;

        public LoginService(PatientProxy patientProxy)
        {
            _patientProxy = patientProxy;
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            return await _patientProxy.GetByIdAsync(id);
        }
    }
}
