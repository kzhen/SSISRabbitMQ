SSISRabbitMQ
============

Custom SSIS Components for RabbitMQ

This code is based on the blog series on building custom components for SQL Server Integration Services, written by the author:

Introduction
http://kzhendev.wordpress.com/2013/07/31/custom-ssis-components/

Part 1
http://kzhendev.wordpress.com/2013/08/07/part-1-building-a-custom-connection-manager/

Part 2
http://kzhendev.wordpress.com/2013/08/07/part-2-adding-a-custom-ui-to-the-connection-manager/

Part 3
http://kzhendev.wordpress.com/2013/08/13/part-3-building-a-custom-source-component/

In addition to above the custom component for SSISRabbitMQ.RabbitMQDestination was added.

Project Changes
The GitHub project refers to SQL Server 110 in the post build event paths.  However, if you are using SQL Server 2014 this should be 120 (version 12).  In addition the Visual Studio path is set for a 2010 install, the path should be updated to match your current version.  I’ve updated below to version 12.0 which is for VS2013.

Modify post build events for each project to have correct paths.  

•	Right click the required project in solution explorer and select properties.  
•	When the properties screen loads navigate to Build Events and Edit Post-Build event command lines.

Should you have a non 2014 SQL Server/2013  VS modify the paths as required.

•	SSISRabbitMQ.RabbitMQConnectionManager

"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\gacutil.exe" -u $(TargetName)
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\gacutil.exe" -iF $(TargetFileName)
copy $(TargetFileName) "C:\Program Files (x86)\Microsoft SQL Server\120\DTS\Connections\SSISRabbitMQ.RabbitMQConnectionManager.dll" /y

copy "$(TargetDir)RabbitMQ.Client.dll" "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\PublicAssemblies" /y
copy "$(TargetDir)RabbitMQ.Client.dll" "C:\Program Files\Microsoft SQL Server\120\DTS\Binn" /y

•	SSISRabbitMQ.RabbitMQSource

"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\gacutil.exe" -u $(TargetName)
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\gacutil.exe" -iF $(TargetFileName)
copy $(TargetFileName) "C:\Program Files (x86)\Microsoft SQL Server\120\DTS\PipelineComponents\SSISRabbitMQ.RabbitMQSource.dll" /y

copy "$(TargetDir)RabbitMQ.Client.dll" "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\PublicAssemblies" /y
copy "$(TargetDir)RabbitMQ.Client.dll" "C:\Program Files\Microsoft SQL Server\120\DTS\Binn" /y

•	SSISRabbitMQ.RabbitMQDestination

"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\gacutil.exe" -u $(TargetName)
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\gacutil.exe" -iF $(TargetFileName)
copy $(TargetFileName) "C:\Program Files (x86)\Microsoft SQL Server\120\DTS\PipelineComponents\SSISRabbitMQ.RabbitMQDestination.dll" /y

copy "$(TargetDir)RabbitMQ.Client.dll" "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\PublicAssemblies" /y
copy "$(TargetDir)RabbitMQ.Client.dll" "C:\Program Files\Microsoft SQL Server\120\DTS\Binn" /y

Build
You can now build each of the projects independently and they will be deployed to the correct locations.  Once completed close and reopen Visual Studio for the new SSIS task and connection manager to appear.

64bit
Should 64bit be required for server deployment rebuild all of above with x64 solution platform.
