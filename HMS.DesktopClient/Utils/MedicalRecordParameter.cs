using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.Utils
{
    public class MedicalRecordPageParameter
    {
        public int UserId { get; set; }
        public string UserType { get; set; } = null!; // "Patient" or "Doctor"
    }
}
