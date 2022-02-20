#include "..\Include\Object.h"

Object::Object(
	float startRot, 
	float endRot, 
	int orientations, 
	const sf::Vector2f& position)
	: mCurrentRotation		{ startRot }
	, mStartRotation		{ startRot }
	, mEndRotation			{ endRot }
	, mCurrentOrientation	{ 0 }
	, mNumOrientations		{ orientations }
	, mShape				{ sf::Vector2{ 100.f, 100.f } }
{
	mShape.setFillColor(sf::Color::Green);
	mShape.setOrigin(50.f, 50.f);
	mShape.setPosition(position);
}

void Object::NextOrientation()
{
	mShape.setRotation(mShape.getRotation() + 10);
	// Update Current Rotation
}

void Object::PrevOrientation()
{
	// Update Current Rotation
}

const sf::RectangleShape& Object::GetShape() const
{
	return mShape;
}