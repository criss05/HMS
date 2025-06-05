using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using HMS.Shared.Proxies.Implementations;
using System.Diagnostics;
using Windows.Media.Capture.Core;
using HMS.Shared.Entities;
using HMS.Shared.DTOs;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HMS.DesktopClient.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Window
    {
        public HomePage()
        {
            this.InitializeComponent();
            DepartmentProxy dp = new(App.CurrentUser.Token);
            RootGrid.DataContext = App.CurrentUser;
            Debug.WriteLine("afjdbgfadjgldsjglkadsg");
            test(dp);
        }

        public async void test(DepartmentProxy dp)
        {
            DepartmentDto dto = await dp.GetByIdAsync(1);
            Debug.WriteLine(dto.Name);
        }
    }
}
