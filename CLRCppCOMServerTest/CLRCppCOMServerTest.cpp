#include "stdafx.h"

#include "CLRCppCOMServerTest.h"

bool CLRCppCOMServerTest::ExplosiveClass::SelfTest() {
  return true;
}

void CLRCppCOMServerTest::ExplosiveClass::Explode() {
  int* p = (int*)0xFEFEFEFE;
  *p = 1;
}

static int value = 0;

int CLRCppCOMServerTest::ExplosiveClass::AutoInc() {
  return value++;
}