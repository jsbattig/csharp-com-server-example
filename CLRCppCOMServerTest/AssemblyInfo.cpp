#include "stdafx.h"

using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::CompilerServices;
using namespace System::Runtime::InteropServices;
using namespace System::Security::Permissions;
using namespace System::EnterpriseServices;

[assembly:AssemblyTitleAttribute(L"CLRCppCOMServerTest")];
[assembly:AssemblyDescriptionAttribute(L"")];
[assembly:AssemblyConfigurationAttribute(L"")];
[assembly:AssemblyCompanyAttribute(L"")];
[assembly:AssemblyProductAttribute(L"CLRCppCOMServerTest")];
[assembly:AssemblyCopyrightAttribute(L"Copyright (c)  2019")];
[assembly:AssemblyTrademarkAttribute(L"")];
[assembly:AssemblyCultureAttribute(L"")];

[assembly:AssemblyVersionAttribute("1.0.*")];

[assembly:ComVisible(true)];

[assembly:CLSCompliantAttribute(true)];

[assembly:ApplicationName(L"CPPCOMServerTest")];
[assembly:ApplicationActivation(ActivationOption::Server)];
[assembly:ApplicationAccessControl(false, Authentication = AuthenticationOption::None)];
[assembly:AssemblyKeyFileAttribute("CLRCppCOMServerTest.snk")];