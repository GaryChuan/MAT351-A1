#include <iostream>
#include "..\Include\Application.h"

int main()
{
    Application app{ 400, 400, "MAT351_A1" };

    app.Initialize();
    app.Run();
}