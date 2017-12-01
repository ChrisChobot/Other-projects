/*
Copyright 2017 Krzysztof Chobot

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/

#include "stdafx.h"
#include <SFML/Graphics.hpp>
#include <iostream>
#include <fstream>
#include <conio.h>
using namespace std;


int LowerResolution(sf::Image image,int x, int y, int maxX, int maxY) // low-pass filter
{ 
	//const auto masc{ 1 };
	auto divideBy{ 9 };
	
	auto result{ 0 };

	for (auto i = y - 1; i <= y + 1; i++)
	{
		for (auto j = x - 1; j <= x + 1; j++)
		{
			if (j >= 0 && j < maxX && i >= 0 && i < maxY)
			{
				result += static_cast<int>(image.getPixel(j, i).r);// *masc;
			}
			else { divideBy--; }
		}
	}

	result /= divideBy;
	return result;
}

int MedianResolution(sf::Image image, int x, int y, int maxX, int maxY) // median filter
{
	auto result{ 0 };
	vector<int> toSort;
	for (auto i=y-1;i<=y+1;i++)
	{
		for (auto j = x - 1; j <= x + 1; j++)
		{
			if (j >= 0 && j < maxX && i >= 0 && i < maxY)
			{
				toSort.push_back(static_cast<int>(image.getPixel(j, i).r));
			}
		}
	}

	sort(toSort.begin(),toSort.end());
	auto vsize{ toSort.size() };
	result = toSort[vsize / 2 + (vsize % 2 != 0 && vsize > 1)];

	return result;
}

char ConvertToASCII(const int color)
{
	if (color < 25)
	{
		return '@';
	}
	else if (color < 50)
	{
		return '#';
	}
	else if (color < 75)
	{
		return '$';
	}
	else if (color < 100)
	{
		return '%';
	}
	else if (color < 125)
	{
		return '=';
	}
	else if (color < 150)
	{
		return '+';
	}
	else if (color < 175)
	{
		return '|';
	}
	else if (color < 200)
	{
		return ':';
	}
	else if (color < 225)
	{
		return '.';
	}
	else
	{
		return ' ';
	}

}
	

void resize(sf::Image image, int maxX, int maxY)
{
	auto divideBy{ 4 };
	auto result{ 0 }; 
	const auto startY = maxY; // needed for deleting

	int **grid = new int*[maxY];
	for (int i = 0; i < maxY; i++)
	{
		grid[i] = new int[maxX];
	}

	for (auto y = 0; y < maxY; y++)
	{
		for (auto x = 0; x < maxX; x++) // dimensions of image
		{
			grid[y][x] = 0;
		}
	}

	for (auto y = 0; y < maxY; y++)
	{
		for (auto x = 0; x < maxX; x++) // dimensions of image
		{
			grid[y][x] = image.getPixel(x, y).r;
		}
	}


	while (maxX >= 175 && maxY >= 175)
	{
		for(auto y=0;y<maxY;y+=2)
		{
			for (auto x = 0; x<maxX; x += 2) // dimensions of image
			{
				for (auto i = y; i <= y + 1; i++)
				{
					for (auto j = x; j <= x + 1; j++) //getting 2x2 boxes of pixels
					{
						if (j < maxX && i < maxY)
						{
							result += grid[y][x];
							
						}
						else { divideBy--; }
					}
				}
				if (divideBy!=0) { result /= divideBy; }
				grid [y / 2][x / 2] = result;
				result = 0;
			}
		}

		//cout << "again.";
		maxX /= 2;
		maxY /= 2;
	}


	sf::Image newImage;
	newImage.create(maxX, maxY, sf::Color(0, 0, 0));

	for (auto y = 0; y < maxY; y++)
	{
		for (auto x = 0; x < maxX; x++) // dimensions of image
		{
			newImage.setPixel(x, y, sf::Color(grid[y][x], grid[y][x], grid[y][x]));
		}
	}

	newImage.saveToFile("tmp.jpg");


	for(auto i=0;i<startY;i++)
	{
		delete[] grid[i];
	}
	delete[] grid;
}

int main(int /*argc*/, char ** argv)
{
	
#pragma region //inicjalization
	string file;
	cout << "enter path to the file (C:\\photo\\photo.jpg)\n";
	cin >> file;
	char selection{ NULL };
	sf::Image image;
	
	while (true){
		if (image.loadFromFile(file))
		{
			cout << "correctly loaded file\n"; break;
		}
		else
		{
			cout << "enter correct path to the file\n";
			cin >> file;
		}
	}

	 auto y = static_cast<int>(image.getSize().y); //static cast for disable warning C4018
	 auto x = static_cast<int>(image.getSize().x);

#pragma endregion

#pragma region //converting to black and white

	auto mixedColours = 0;
	for (auto i = 0; i < y; i++)
	{
		for (auto j = 0; j < x; j++)
		{
			mixedColours = (image.getPixel(j, i).r + image.getPixel(j, i).g + image.getPixel(j, i).b) / 3;
			image.setPixel(j, i, sf::Color (mixedColours, mixedColours, mixedColours));
		}
	}

#pragma endregion

	
#pragma region //resize

	cout << "do you want to resize your image (highly recommended for bigger ones)\n"
		"Y/N\n";
	cin >> selection;
	if(selection == 'Y' || selection == 'y')
	{
		resize(image, x, y);
		image.loadFromFile("tmp.jpg");
		y = static_cast<int>(image.getSize().y); //static cast for disable warning C4018
		x = static_cast<int>(image.getSize().x);

		remove("tmp.jpg");
		cout << "your image has ben successfully resized to: " << x << "x" << y << "\n\n";
	}

#pragma endregion

#pragma region //lowering quality
	sf::Image newimage(image);

	
	cout << "Which filter do you want to use? (Be aware that might take a while)\n"
	"L - low-pass filter\n"
	"M - median filter\n"
	"C - no filter (clean)\n";
	cin >> selection;

	switch (selection)
	{
	case 'L':
	case 'l':
	default:
		cout << "you have choosed low-pass filter\ncomputing data...\n";
		for (auto i = 0; i < y; i++)
		{
			for (auto j = 0; j < x; j++)
			{
				mixedColours = LowerResolution(image, j, i, x-1, y-1);
				newimage.setPixel(j, i, sf::Color(mixedColours, mixedColours, mixedColours));
			}
		}
		cout << "filtering ended successfully\n";
		break;
	case 'M':
	case 'm':
		cout << "you have choosed median filter\ncomputing data...\n";
		for (auto i = 0; i < y; i++)
		{
			for (auto j = 0; j < x; j++)
			{
				mixedColours = MedianResolution(image, j, i, x, y);
				newimage.setPixel(j, i, sf::Color(mixedColours, mixedColours, mixedColours));
			}
		}
		cout << "filtering ended successfully\n";
		break;
	case 'C': 
	case 'c': break;
	}
	
#pragma endregion

#pragma region //save result
	string resultPath;
	string resultASCII;

	for (auto i = 0; i < y; i++)
	{
		for (auto j = 0; j < x; j++)
		{
			resultASCII += ConvertToASCII(static_cast<int>(newimage.getPixel(j, i).r));
		}
		resultASCII += '\n';
	}

	while (true) {
		cout << "input name for ASCII file\n";
		cin >> resultPath;
		resultPath += ".txt";
		ofstream outputFile(resultPath);
		if (outputFile) { outputFile << resultASCII; break; }
		else { cout << "wrong file name\n"; }
	}
	cout << "file with ASCII characters have been saved\n";
#pragma endregion


#pragma region //for debugging
	//sf::Texture texture;
	//texture.loadFromImage(image);
	//sf::Sprite sprite;
	//sprite.setTexture(texture);
	//sf::RenderWindow window({ 1024,640 },"DEBUG");
	//window.setFramerateLimit(30);
	//while (window.isOpen())
	//{
	//	sf::Event event;
	//	
	//	while (window.pollEvent(event))
	//	{
	//		if (event.type == sf::Event::Closed)
	//			window.close();
	//	}
	//	window.clear();
	//	window.draw(sprite);
	//	window.display();
	//}
#pragma endregion

	cout << "Press Enter to exit";
	_getch();
    return 0;
}
