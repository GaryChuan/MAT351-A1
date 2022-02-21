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
	Object CreateObject(int width, int height) const;

private:
	sf::RenderWindow mWindow;
	Object mObject;
};