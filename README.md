# csharp-com-server-example
Example of creation of a COM Server using CSharp

Instructions after compiling the project:

1. Open the Visual Studio Command Prompt
2. Navigate to the bin folder where COMServerTest.dll was built
3. Execute "regsvcs COMServerTest.dll" (this steps registers the COM server in the computer)
4. Execute "regsvcs CLRCppCOMServerTest.dll" (this step registers the C++ COM server)

After the later step, the assembly is registered as a COM server and all types that inherit from System.EnterpriseServices.ServicedComponent will be registered as supported classes to be instantiated within the COM server.
Security doesn't need to be configured for the COM Server because is disabled via attribute:

[assembly: ApplicationAccessControl(false, Authentication = AuthenticationOption.None)]

declared in AssemblyInfo.cs file.

*************************

The minimum things to do to make any .NET class library operate as COM Server are:

1. Added reference to the following assembly: "System.EnterpriseServices"
2. Install nuget package: "System.Runtime.InteropServices"
3. In AssemblyInfo.cs add:
[assembly: ApplicationName("COMServerTest")]
[assembly: ApplicationActivation(ActivationOption.Server)]

4. In AssemblyInfo.cs change ComVisible to "true":
[assembly: ComVisible(true)]

5. Open the properties page of the project, go to signing and check "Sign the assembly"
6. Create or select a signing name key file of your choosing
7. Do this to every class you want to expose to be activated and live within the COM server:
	a. Set as the base class "System.EnterpriseServices.ServicedComponent"
	b. Add attribute [ClassInterface(ClassInterfaceType.None)]
8. Compile your library
9. Follow the steps specified at the top of this document to register your library as a COM Server
10. Link the project to your host application as you would link any project living in the same solution, or link by navigating to the server dll
11. Compile and run your host project

Even though the approach above works (not using an interface to represent the object on the host application) the performance will be significantly slower than if each object exposed in the COM Server declares and implements an interface.
Without digging deeply on what might be going on, I guess that when declaring an interface the calls are serialized via an indexed DispInterface.
The performance difference is significant, in my multi-threaded tests I get close to 1000 calls per second when going thorough an interface, vs. about 600 calls per second when using the object "directly".

In summary, declaring an interface per exposed class is not mandatory but recommended to achieve higher performance

*****

A note on exception management on COM objects. If the machine.config of .NET framework doesn't have the following entry:

    <runtime>
      <legacyCorruptedStateExceptionsPolicy enabled="true"/>
    </runtime>

The COM server will crash upon an access violation exception. The object on the host application will have a stale reference and within the host application a COMException will be thrown if the attempting to execute any kind of operation in this object.

It's important to decide if using or not legacyCorruptStateExceptionPolicy or not. If using it with "true" value the COM Server *may* be left into a bad state after an access violation, but other in-flight calls will continue working, otherwise the server will crash taking every other in-flight call with it.