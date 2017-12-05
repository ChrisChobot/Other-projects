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

#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include <fstream>
#include <conio.h>

class jpgToASCII
{
private:
	sf::Image image;
	int y; //dimensions of image
	int x;
	char selection; //for Y/N choosing
	void SetDimensions();
	static char ConvertToASCII(const int color);
	void Resize(int maxX, int maxY, const int receivedX, const int receivedY);

	//x and y are location of pixel that we are changing
	int LowerResolution(int x, int y); // low-pass filter
	int MedianResolution(int x, int y); // median filter

public:
	jpgToASCII(std::string file);
	~jpgToASCII();
	void ToBlackAndWhite();
	void Resize();
	void Filter();
	void Save();
	sf::Image GetImage();
};
