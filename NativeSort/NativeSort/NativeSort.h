#pragma once

#define DLL_EXPORT __declspec(dllexport)

extern "C"
{
    DLL_EXPORT void TestSort(int a[], int length);
}