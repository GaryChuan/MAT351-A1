#include <iostream>
#include <fstream>
#include <filesystem>

#include "..\Include\Application.h"

Application::Application(int width, int height, const char* title)
	: mWindow { sf::VideoMode(width, height), title }
	, mObject{ CreateObject(width, height) }
{
}

bool Application::Initialize()
{
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
			else if (event.type == sf::Event::KeyPressed
				&& event.key.code == sf::Keyboard::Left)
			{
				mObject.PrevOrientation();
			}
			else if (event.type == sf::Event::KeyPressed
				&& event.key.code == sf::Keyboard::R)
			{
				mObject.Reset();
			}
		}

		mWindow.clear();
		mWindow.draw(mObject.GetSprite());
		mWindow.display();
	}
}

Object Application::CreateObject(int width, int height) const
{
	const auto currentPath = std::filesystem::current_path();
	const auto size = sf::Vector2f{ width / 2.f, height / 2.f };

	std::filesystem::path path{ currentPath / "Resource" / "path.txt" };
	std::ifstream stream(path);

	std::filesystem::path imagePath{ currentPath / "Resource" / "rletter.png" };

	std::cout << std::filesystem::exists(imagePath) << '\n';

	sf::Texture texture;

	if (stream.good() && texture.loadFromFile(imagePath.string()))
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

		return Object{ angle1, angle2, N, std::move(texture), size};
	}

	return Object{ 0, 180, 2, std::move(texture), size };
}
