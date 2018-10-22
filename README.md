"# SFTP" 

Author: Tolu Adepoju
Date: 20181016

This program was written to handle SFTP Tasks via the commandline - specifically for Consumption via SSIS
---------------------------------------------------------------------------------------------------------

The arguments (all required) are:

-host   : "url of the sftp site"
-user   : user name for authentication
-pwd    : password for authentication
-remote : path of/for the remote file
-local  : path of/for the local file
-logger : path for file to use for the logging of events
-action : action to perform. Acceptable values 1.upload 2.download

Usage:

1. To upload a file
-host sftp.my.org -user user@my.org pwd mysecurePassword -remote /GeneralFolder/Uploads/destinationFilename.csv -local "C:\Windows\localFileName.csv" -logger "C:\loggingForFileUploadedToday.txt" -action upload

2. To download a file
 -host sftp.my.org -user user@my.org pwd mysecurePassword -remote /GeneralFolder/Uploads/sourceFilename.sql -local "C:\Windows\destinationFilename.txt" -logger "C:\loggingForFileUploadedToday.xml" -action download

