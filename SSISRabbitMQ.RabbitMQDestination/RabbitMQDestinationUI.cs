using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DTSRuntime = Microsoft.SqlServer.Dts.Runtime;

namespace SSISRabbitMQ.RabbitMQDestination
{
    public class RabbitMQDestinationUI : IDtsComponentUI
    {
        private IServiceProvider serviceProvider;
        private IDTSComponentMetaData100 metaData;
        private IDtsConnectionService connectionService;

        public void Delete(IWin32Window parentWindow)
        {
        }

        public bool Edit(IWin32Window parentWindow, DTSRuntime.Variables variables, DTSRuntime.Connections connections)
        {
            ShowForm(parentWindow);

            return true;
        }

        public void Help(IWin32Window parentWindow)
        {
        }

        private DialogResult ShowForm(IWin32Window window)
        {
            RabbitMQDestinationUIForm form = new RabbitMQDestinationUIForm(metaData, serviceProvider);

            return form.ShowDialog(window);
        }

        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.metaData = dtsComponentMetadata;

            this.connectionService = (IDtsConnectionService)serviceProvider.GetService(typeof(IDtsConnectionService));
        }

        public void New(IWin32Window parentWindow)
        {
            ShowForm(parentWindow);
        }
    }
}
