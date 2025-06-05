using Hospital.Managers;
using Hospital.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class MedicalRecordsDetailViewModel
    {
        private readonly IDocumentManager _documentManager;
        public MedicalRecordJointModel MedicalRecord { get; private set; }
        public ObservableCollection<DocumentModel> Documents { get; private set; }

        public MedicalRecordsDetailViewModel(MedicalRecordJointModel medicalRecord, IDocumentManager documentManager)
        {
            MedicalRecord = medicalRecord;
            _documentManager = documentManager;
            _documentManager.LoadDocuments(MedicalRecord.MedicalRecordId);
            Documents = new ObservableCollection<DocumentModel>(_documentManager.GetDocuments());
        }

        public async Task OnDownloadButtonClicked()
        {
            await _documentManager.DownloadDocuments(MedicalRecord.PatientId);
        }

        public bool GetDownloadButtonIsEnabled()
        {
            return _documentManager.HasDocuments(MedicalRecord.MedicalRecordId);
        }
    }
}
