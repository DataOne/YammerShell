      ######################################################################
     ##  __     __                                 _____ _          _ _    ##
    ##   \ \   / /                                / ____| |        | | |    ##
    ##    \ \_/ /_ _ _ __ ___  _ __ ___   ___ _ _| (___ | |__   ___| | |    ##
    ##     \   / _` | '_ ` _ \| '_ ` _ \ / _ \ '__\___ \| '_ \ / _ \ | |    ##
    ##      | | (_| | | | | | | | | | | |  __/ |  ____) | | | |  __/ | |    ##
    ##      |_|\__,_|_| |_| |_|_| |_| |_|\___|_| |_____/|_| |_|\___|_|_|    ##
     ##                                                                    ##
      ######################################################################

# YammerShell

YammerShell is a Windows PowerShell module to access Yammer with PowerShell. It uses Yammer's REST API.  
  => therefore authentication via OAuth is required.

The contained CommandLets cover the most important endpoints provided by the API.


## Importing the module

1. Unpack the zip file.
2. Open Windows PowerShell.
3. Navigate to the directory with the unpacked files with `cd <path>`.
4. Type `Import-Module .\YammerShell.dll`.


## How to get authenticated

* Yammer API needs a registered application to access its endpoints.
* An application has a Bearer Token assigned.
* Using this token YammerShell has the same rights the user that created the app has.

--> The CmdLet `Get-YmToken` helps you to register an application in Yammer.  
   _(**Warning**: to delete an app you have to contact the support!)_

To use the token of a registered app, type `Set-YmToken <token>`.


## How to use YammerShell

After you registered an app and obtained a token,
you can simply start YammerShell using the batch file [_StartYammerShell.bat](doc/_StartYammerShell.bat).

It contains the following line where you have to change
1234 to your token and <path> to the path where YammerShell.dll is located:

```Batchfile
start powershell -NoExit -NoLogo -Command "Import-Module '<path>\YammerShell.dll'; Set-YmToken '1234';`
```

To get a list of all CmdLets type `Get-YmHelp`. Also all available CmdLets are [documented on github](doc/README.md).


## Errors

If you use YammerShell-CmdLets in a script it may happen that an error occurs while performing many actions in a short period.
This is caused by Yammer's API which has some [limitations of requests](https://developer.yammer.com/docs/rest-api-rate-limits).

To fix it just add a small delay (e.g. with the CmdLet `Start-Sleep`) in your script to prevent too many requests.