#include <iostream>

#include "..\Include\Object.h"
#include "..\Include\Math.h"


sf::Vector2f GetOrientation(const float angle)
{
	return {
		std::cosf(angle) * Math::RIGHT.x - std::sinf(angle) * Math::RIGHT.y,
		std::sinf(angle) * Math::RIGHT.x + std::cosf(angle) * Math::RIGHT.y
	};
}

template <typename T>
std::ostream& operator << (std::ostream& os, const sf::Vector2<T> vec2)
{
	return os << "x : " << vec2.x << " y: " << vec2.y << '\n';
}

void setOriginAndReadjust(sf::Transformable& object, const sf::Vector2f& newOrigin)
{
	auto offset = newOrigin - object.getOrigin();
	object.setOrigin(newOrigin);
	object.move(offset);
}

Object::Object(
	float startRot, 
	float endRot, 
	int orientations, 
	sf::Texture&& texture,
	const sf::Vector2f& position)
	: mCurrentRotation		{ startRot }
	, mStartRotation		{ startRot }
	, mEndRotation			{ endRot }
	, mOrientationCount		{ 0 }
	, mNumOrientations		{ orientations }
	, mAnglePerTurn			{ std::abs(startRot - endRot) / mNumOrientations }
	, mTexture				{ std::move(texture) }
	, mSprite				{ }
	, mStartOrientation		{ GetOrientation(Math::DegreeToRad(startRot)) }
	, mEndOrientation		{ GetOrientation(Math::DegreeToRad(endRot)) }
	, mCurrentOrientation	{ mStartOrientation }
{
	auto textureSize = mTexture.getSize();

	mSprite.setScale(0.1f, 0.1f);
	mSprite.setTexture(mTexture);
	mSprite.setOrigin(textureSize.x / 2.f, 
					  textureSize.y / 2.f);

	mSprite.setPosition(position);
	
	auto initialAngle = 2 * Math::PI - Math::AngleInRad(Math::RIGHT, mStartOrientation);

	mSprite.setRotation(Math::RadToDegree(initialAngle));

	std::cout << "Initial Orientation: " << startRot << '\n';
	std::cout << "Final Orientation: " << endRot << '\n';

	CalculateOrientations();
}

void Object::NextOrientation()
{
	if (mOrientationCount < mNumOrientations)
	{
		++mOrientationCount;
	}

	std::cout << "Current Orientation : " << std::abs(mOrientations[mOrientationCount]) << '\n';
	mSprite.setRotation(mOrientations[mOrientationCount]);
}

void Object::PrevOrientation()
{
	if (mOrientationCount > 0)
	{
		--mOrientationCount;
	}

	std::cout << "Current Orientation : " << std::abs(360 - mOrientations[mOrientationCount]) << '\n';
	mSprite.setRotation(mOrientations[mOrientationCount]);
}

void Object::Reset()
{
	mOrientationCount = 0;
	mSprite.setRotation(mOrientations[0]);
}

const sf::Sprite& Object::GetSprite() const
{
	return mSprite;
}

void Object::CalculateOrientations()
{
	const auto dot = Math::DotProduct(mStartOrientation, mEndOrientation);
	const auto denom = std::sqrtf(1 - dot * dot);
	const auto invCos = std::acosf(dot);

	for (int i = 0; i < mNumOrientations + 1; ++i)
	{
		auto t = static_cast<float>(i) / mNumOrientations;
		auto vt = mStartOrientation * static_cast<float>(std::sin((1-t) * invCos) / denom)
				+ mEndOrientation * static_cast<float>(std::sin(t * invCos) / denom);

		mOrientations[i] = 360 - Math::AngleInDeg(Math::RIGHT, vt);
	}
}
