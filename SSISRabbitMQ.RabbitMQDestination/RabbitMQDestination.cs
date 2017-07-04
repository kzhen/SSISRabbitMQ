using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;


namespace SSISRabbitMQ.RabbitMQDestination
{

    [DtsPipelineComponent(IconResource = "SSISRabbitMQ.RabbitMQDestination.Rabbit.ico",
      DisplayName = "RabbitMQ Destination",
      ComponentType = ComponentType.DestinationAdapter,
      Description = "Connection destination for RabbitMQ",
      UITypeName = "SSISRabbitMQ.RabbitMQDestination.RabbitMQDestinationUI, SSISRabbitMQ.RabbitMQDestination, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d05c62ea16e22819")]
    public class RabbitMQDestination : PipelineComponent
    {
        private IConnection rabbitConnection;
        private IModel publisherChannel;

        private RabbitMQConnectionManager.RabbitMQConnectionManager rabbitMqConnectionManager;

        private string exchangeName;

        Dictionary<int, string> columnNames;

        public override void ProvideComponentProperties()
        {
            // Reset the component
            base.RemoveAllInputsOutputsAndCustomProperties();
            ComponentMetaData.RuntimeConnectionCollection.RemoveAll();

            IDTSInput100 input = ComponentMetaData.InputCollection.New();
            input.Name = "Input";

            IDTSCustomProperty100 exchangeName = ComponentMetaData.CustomPropertyCollection.New();
            exchangeName.Name = "ExchangeName";
            exchangeName.Description = "The name of the RabbitMQ exchange to publish messages to";


            IDTSRuntimeConnection100 connection = ComponentMetaData.RuntimeConnectionCollection.New();
            connection.Name = "RabbitMQ";
            connection.ConnectionManagerID = "RabbitMQ";

        }

        public override void AcquireConnections(object transaction)
        {
            if (ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager != null)
            {
                ConnectionManager connectionManager = Microsoft.SqlServer.Dts.Runtime.DtsConvert.GetWrapper(
                  ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager);

                this.rabbitMqConnectionManager = connectionManager.InnerObject as RabbitMQConnectionManager.RabbitMQConnectionManager;

                if (this.rabbitMqConnectionManager == null)
                    throw new Exception("Couldn't get the RabbitMQ connection manager, ");

                this.exchangeName = ComponentMetaData.CustomPropertyCollection["ExchangeName"].Value;

                rabbitConnection = this.rabbitMqConnectionManager.AcquireConnection(transaction) as IConnection;
            }
        }

        public override void ReleaseConnections()
        {
            if (rabbitMqConnectionManager != null)
            {
                this.rabbitMqConnectionManager.ReleaseConnection(rabbitConnection);
            }
        }

        public override void PreExecute()
        {
            // Get column names
            columnNames = new Dictionary<int, string>();
            int count = 0;
            IDTSInput100 input = ComponentMetaData.InputCollection[0];
            IDTSInputColumnCollection100 inputColumns = input.InputColumnCollection;

            foreach (IDTSInputColumn100 column in inputColumns)
            {
                columnNames[count] = column.Name;
                count++;
            }

            try
            {
                this.publisherChannel = rabbitConnection.CreateModel();
                // Create Queue and Exchange, these operations should be idempotent
                this.publisherChannel.ExchangeDeclare(exchangeName, "direct", true);
                this.publisherChannel.QueueDeclare(exchangeName, true, false, false, null);
                this.publisherChannel.QueueBind(exchangeName, exchangeName, "");
            }
            catch (Exception)
            {
                ReleaseConnections();
                throw;
            }
        }

        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
            if (publisherChannel == null)
            {
                ReleaseConnections();
                throw new Exception("Could not get channel to publish messages to");
            }

            while (buffer.NextRow())
            {
                StringBuilder rowContents = new StringBuilder();
                for (int i = 0; i < buffer.ColumnCount; i++)
                {
                    // Get Column Name
                    string columnName = string.Empty;
                    columnNames.TryGetValue(i, out columnName);
                    
                    // Get Column value
                    var columnValue = buffer[i].ToString();

                    var message = columnName + ": " + columnValue;
                    rowContents.Append(message + ", ");
                }
                this.publisherChannel.BasicPublish(exchangeName, "", null, Encoding.UTF8.GetBytes(rowContents.ToString()));
            }
        }
    }
}
