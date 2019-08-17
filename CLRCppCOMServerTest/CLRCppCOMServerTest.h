#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace CLRCppCOMServerTest {
  public interface class IExplosiveClass {
    bool SelfTest();
    void Explode();
    void ThrowCppException();
    void ThrowString();
    int AutoInc();
  };

  [ClassInterface(ClassInterfaceType::None)]
  public ref class ExplosiveClass : System::EnterpriseServices::ServicedComponent, IExplosiveClass {
  public:
    virtual bool SelfTest();
    virtual void Explode();
    virtual int AutoInc();
    virtual void ThrowCppException();
    virtual void ThrowString();
  };
}
