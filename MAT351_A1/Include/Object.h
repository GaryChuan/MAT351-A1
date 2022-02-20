#pragma once
#include <SFML/System/Vector2.hpp>
#include <SFML/Graphics.hpp>

class Object
{
public:
	Object(
		float startRot, 
		float endRot, 
		int orientations, 
		const sf::Vector2f& position);

	void NextOrientation();
	void PrevOrientation();

	const sf::RectangleShape& GetShape() const;

private:
	float	mCurrentRotation;
	float	mStartRotation;
	float	mEndRotation;

	int		mCurrentOrientation;
	int		mNumOrientations;

	sf::Transform mTransform;
	sf::RectangleShape mShape;
};