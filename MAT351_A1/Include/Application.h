#pragma once
#include <SFML/Graphics.hpp>
#include "Object.h"

class Application
{
public:
	Application(int width, int height, const char* title = "Example");

	bool Initialize();
	void Run();

private:
	sf::RenderWindow mWindow;
	Object mObject;
};