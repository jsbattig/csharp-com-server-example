#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

#include "SimpleClass.h"

namespace CLRCppCOMServerTest {
  [ClassInterface(ClassInterfaceType::None)]
  public ref class ClassWithNoInterface : System::EnterpriseServices::ServicedComponent {
  public:
    virtual bool SelfTest();
    virtual String^ Echo(SimpleClass obj) {
      return obj.str;
    }
  };
}