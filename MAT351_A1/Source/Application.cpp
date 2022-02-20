#include <iostream>
#include <fstream>
#include <filesystem>

#include "..\Include\Application.h"

Application::Application(int width, int height, const char* title)
	: mWindow { sf::VideoMode(width, height), title }
	, mObject{ 10, 30, 24, sf::Vector2f{ width / 2.f, height / 2.f} }
{
}

bool Application::Initialize()
{
	const auto currentPath = std::filesystem::current_path();
	std::filesystem::path path{ currentPath / "Resource" / "path.txt" };
	std::ifstream stream(path);

	if (stream.good())
	{
		float angle1;
		float angle2; 
		int N;

		std::string line;

		stream >> line;
		angle1 = std::stof(line);

		stream >> line;
		angle2 = std::stof(line);

		stream >> line;
		N = std::stoi(line);

		return true;
	}

	std::cerr << " Failed to load " << path << '\n';

	return false;
}


void Application::Run()
{
	while (mWindow.isOpen())
	{
		sf::Event event;

		while (mWindow.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
			{
				mWindow.close();
			}
			else if (event.type == sf::Event::KeyPressed
				  && event.key.code == sf::Keyboard::Right)
			{
				mObject.NextOrientation();
			}
		}

		mWindow.clear();
		mWindow.draw(mObject.GetShape());
		mWindow.display();
	}
}