#pragma once

using namespace System;

namespace CLRCppCOMServerTest {
  public ref class SimpleClass {
  public:
    String^ str;
    SimpleClass();
    bool SelfTest() {
      return true;
    }
  };
}

