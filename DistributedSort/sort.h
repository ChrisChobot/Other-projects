#include <cstdlib>
#include <mpi.h>
#include <conio.h>
#include <iostream>
using namespace std;
#pragma once
class sort
{
private:
	int* A;
	static int max_index(int* data, int N, int start);// find the index of the largest item in an array 
	static int min_index(int* data, int N, int start);// find the index of the smallest item in an array 
	static int cmp(const void* ap, const void* bp);//comparison function for qsort
	void init(int N);//data initialization
public:	
	void d_sort(int* masterNode, int rank, int N, int K); //main sorting function	
	void print(int N, int rank) const;
	sort(int const N);
	~sort();
};

