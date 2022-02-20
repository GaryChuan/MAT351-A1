#pragma once

#include <cmath>
#include <SFML/System/Vector2.hpp>

namespace Math
{
	inline float DotProduct(const sf::Vector2f& lhs, const sf::Vector2f& rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	inline float GetMagnitudeSquared(const sf::Vector2f& vec2)
	{
		return vec2.x * vec2.x + vec2.y * vec2.y;
	}

	inline float GetMagnitude(const sf::Vector2f& vec2)
	{
		return std::sqrtf(GetMagnitudeSquared(vec2));
	}

	inline sf::Vector2f GetNormalizedCopy(const sf::Vector2f& vec2)
	{
		return vec2 / GetMagnitude(vec2);
	}

	inline void Normalize(sf::Vector2f& vec2)
	{
		vec2 /= GetMagnitude(vec2);
	}
}