using HMS.Shared.DTOs;
using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.Equipment
{
    public class EquipmentAllViewModel : INotifyPropertyChanged
    {
        private EquipmentService _equipmentService;
        public ObservableCollection<EquipmentDto> EquipmentList { get; } = new();

        public EquipmentAllViewModel(EquipmentService equipmentService)
        {
            this._equipmentService = equipmentService;
        }

        public async Task LoadAllEquipment()
        {
            try
            {
                EquipmentList.Clear();
                var equipments = await _equipmentService.GetAllAsync();
                foreach (var equipment in equipments)
                {
                    EquipmentList.Add(equipment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load doctors summary: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
