#pragma once
#include <unordered_map>

#include <SFML/System/Vector2.hpp>
#include <SFML/Graphics.hpp>


class Object
{
public:
	Object(
		float startRot, 
		float endRot, 
		int orientations, 
		sf::Texture&& texture,
		const sf::Vector2f& position);

	void NextOrientation();
	void PrevOrientation();

	void Reset();

	//const sf::RectangleShape& GetShape() const;
	const sf::Sprite& GetSprite() const;

private:
	void CalculateOrientations();

private:
	float	mCurrentRotation;
	float	mStartRotation;
	float	mEndRotation;

	int		mOrientationCount;
	int		mNumOrientations;

	float	mAnglePerTurn;

	sf::Vector2f mStartOrientation;
	sf::Vector2f mEndOrientation;
	sf::Vector2f mCurrentOrientation;
	sf::Texture  mTexture;
	sf::Sprite	 mSprite;
	//sf::RectangleShape mShape;

	std::unordered_map<int, float> mOrientations;
};