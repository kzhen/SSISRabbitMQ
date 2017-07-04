using Microsoft.SqlServer.Dts.Design;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System;
using System.Windows.Forms;

namespace SSISRabbitMQ.RabbitMQConnectionManager
{
    public class RabbitMQConnectionManagerUI : IDtsConnectionManagerUI
    {
        private IServiceProvider serviceProvider;
        private ConnectionManager connectionManager;

        public void Delete(IWin32Window parentWindow)
        {
        }

        public bool Edit(IWin32Window parentWindow, Connections connections, ConnectionManagerUIArgs connectionUIArg)
        {
            return EditConnection(parentWindow);
        }

        public void Initialize(ConnectionManager connectionManager, IServiceProvider serviceProvider)
        {
            this.connectionManager = connectionManager;
            this.serviceProvider = serviceProvider;
        }

        public bool New(IWin32Window parentWindow, Connections connections, ConnectionManagerUIArgs connectionUIArg)
        {
            IDtsClipboardService clipboardService;

            clipboardService = (IDtsClipboardService)serviceProvider.GetService(typeof(IDtsClipboardService));
            if (clipboardService != null)
            // If connection manager has been copied and pasted, take no action.
            {
                if (clipboardService.IsPasteActive)
                {
                    return true;
                }
            }

            return EditConnection(parentWindow);
        }

        private bool EditConnection(IWin32Window parentWindow)
        {
            RabbitMQConnectionManagerUIForm frm = new RabbitMQConnectionManagerUIForm(connectionManager, serviceProvider);

            var result = frm.ShowDialog();

            if (result == DialogResult.OK)
                return true;

            return false;
        }
    }
}
