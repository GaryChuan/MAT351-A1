#pragma once

#include <cmath>
#include <SFML/System/Vector2.hpp>

namespace Math
{
	constexpr static auto PI = std::integral_constant<float, 3.14159265358979323846f>();

	const static auto LEFT	= sf::Vector2f{ -1, 0 };
	const static auto RIGHT = sf::Vector2f{ 1, 0 };
	const static auto UP	= sf::Vector2f{ 0, 1 };
	const static auto DOWN	= sf::Vector2f{ 0, -1 };


	inline float DegreeToRad(float val)
	{
		return val * PI / 180.f;
	}

	inline float RadToDegree(float val)
	{
		return val * 180.f / PI;
	}

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

	inline float AngleInRad(const sf::Vector2f& lhs, const sf::Vector2f& rhs)
	{
		return std::acosf(
			(lhs.x * rhs.x + lhs.y * rhs.y) /
			(GetMagnitude(lhs) * GetMagnitude(rhs))
		);
	}

	inline float AngleInDeg(const sf::Vector2f& lhs, const sf::Vector2f& rhs)
	{
		return RadToDegree(AngleInRad(lhs, rhs));
	}
}