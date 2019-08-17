#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace CLRCppCOMServerTest {
  public interface class IExplosiveClass {
    bool SelfTest();
  };

  [ClassInterface(ClassInterfaceType::None)]
  public ref class ExplosiveClass : System::EnterpriseServices::ServicedComponent, IExplosiveClass
	{
  public:
    virtual bool SelfTest();
	};
}
