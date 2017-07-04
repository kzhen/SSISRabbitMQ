using System;
using System.IO;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace BlobSrc
{
    [DtsPipelineComponent(DisplayName = "BLOB Inserter Source", Description = "Inserts files into the data flow as BLOBs")]
    public class BlobSrc : PipelineComponent
    {
        IDTSConnectionManager100 m_ConnMgr;
        int m_FileNameColumnIndex = -1;
        int m_FileBlobColumnIndex = -1;

        public override void ProvideComponentProperties()
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection.New();
            output.Name = "BLOB File Inserter Output";

            IDTSOutputColumn100 column = output.OutputColumnCollection.New();
            column.Name = "FileName";
            column.SetDataTypeProperties(DataType.DT_WSTR, 256, 0, 0, 0);

            column = output.OutputColumnCollection.New();
            column.Name = "FileBLOB";
            column.SetDataTypeProperties(DataType.DT_IMAGE, 0, 0, 0, 0);

            IDTSRuntimeConnection100 conn = ComponentMetaData.RuntimeConnectionCollection.New();
            conn.Name = "FileConnection";
        }

        public override void AcquireConnections(object transaction)
        {
            IDTSRuntimeConnection100 conn = ComponentMetaData.RuntimeConnectionCollection[0];
            m_ConnMgr = conn.ConnectionManager;
        }

        public override void ReleaseConnections()
        {
            m_ConnMgr = null;
        }

        public override void PreExecute()
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

            m_FileNameColumnIndex = (int)BufferManager.FindColumnByLineageID(output.Buffer, output.OutputColumnCollection[0].LineageID);
            m_FileBlobColumnIndex = (int)BufferManager.FindColumnByLineageID(output.Buffer, output.OutputColumnCollection[1].LineageID);
        }

        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            string strFileName = (string)m_ConnMgr.AcquireConnection(null);

            while (strFileName != null)
            {
                buffers[0].AddRow();

                buffers[0].SetString(m_FileNameColumnIndex, strFileName);

                FileInfo fileInfo = new FileInfo(strFileName);
                byte[] fileData = new byte[fileInfo.Length];
                FileStream fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Read(fileData, 0, fileData.Length);

                buffers[0].AddBlobData(m_FileBlobColumnIndex, fileData);

                strFileName = (string)m_ConnMgr.AcquireConnection(null);
            }

            buffers[0].SetEndOfRowset();
        }
    }
}