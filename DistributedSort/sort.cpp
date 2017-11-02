#include "stdafx.h"
#include "sort.h"




int sort::max_index(int* data, int N, int start) 
{
	auto max = data[start], maxi = start;

	for (auto i = start; (start == 0 && i<N) || (start != 0 && i < 2 * N); i++)
	{
		if (data[i] > max)
		{
			max = data[i];
			maxi = i;
		}
	}
	return maxi;
}

int sort::min_index(int* data, int N, int start)
{
	auto min = data[start], mini = start;

	for (auto i = start;(start==0 && i<N) || (start != 0 && i < 2 * N); i++)
	{
		if (data[i] < min)
		{
			min = data[i];
			mini = i;
		}
	}
	return mini;
}

int sort::cmp(const void* ap, const void* bp)
{
	auto a = *static_cast<const int*>(ap);
	auto b = *static_cast<const int*>(bp);

	if (a < b) { return -1; }
	else if (a > b) { return 1; }
	else { return 0; }
}

void sort::init(int N)
{
	srand(N);
	for (auto i = 0; i < N; i++) {
		A[i] = rand() % 100;
	}
}

void sort::d_sort(int* masterNode, const int rank, const int N, const int K)
{
	for (auto i = 0; i < K; i++)
	{
		qsort(A, N, sizeof(int), &cmp); // sort locally worker array

		int partner; // find partner in this phase
		if (i % 2 == 0) // if it's an even phase 
		{
			// if we are an even process 
			if (rank % 2 == 0) { partner = rank + 1; }
			else { partner = rank - 1; }
		}
		else
		{
			// it's an odd phase
			if (rank % 2 == 0) { partner = rank - 1; }
			else { partner = rank + 1; }
		}

		// if the partner is invalid
		if (partner < 0 || partner >= K)
		{
			continue;
		}


#pragma region  	
		if (rank % 2 == 0) //if for avoiding possible deadlocks 
		{
			if (N < K)
			{
				MPI_Send(A, N, MPI_INT, partner, 0, MPI_COMM_WORLD);
				MPI_Recv(masterNode, N, MPI_INT, partner, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
				for (auto j = N, x = 0; j < 2 * N; j++, x++)
				{
					A[j] = masterNode[x];
				}
			}
			else // K < N
			{
				for (auto j = 0; j < N;)
				{
					for (auto x = 0; x < K && j < N; x++, j++)
					{
						MPI_Send(&A[j], 1, MPI_INT, partner, 0, MPI_COMM_WORLD);
						MPI_Recv(&masterNode[x], 1, MPI_INT, partner, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
					}
					for (auto x = N + j - K, y = 0; x <N + j; x++, y++)
					{
						A[x] = masterNode[y];
					}
				}
			}

		}
		else
		{

			if (N < K)
			{
				MPI_Recv(masterNode, N, MPI_INT, partner, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
				MPI_Send(A, N, MPI_INT, partner, 0, MPI_COMM_WORLD);
				for (auto j = N, x = 0; j < 2 * N; j++, x++)
				{
					A[j] = masterNode[x];
				}
			}
			else // K < N
			{
				for (auto j = 0; j < N;)
				{
					for (auto x = 0; x < K && j < N; x++, j++)
					{
						MPI_Recv(&masterNode[x], 1, MPI_INT, partner, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
						MPI_Send(&A[j], 1, MPI_INT, partner, 0, MPI_COMM_WORLD);						
					}
					for (auto x = N + j - K, y = 0; x <N + j; x++, y++)
					{
						A[x] = masterNode[y];
					}
				}
			}			
		}
#pragma endregion sending and receiving	


#pragma region 		
		int mini, maxi;
		
		if (rank < partner) //keep smaller keys 
		{
			for (auto j=0;;++j)
			{
				// find the smallest one in imported array
				mini = min_index(A, N, N);

				// find the largest one in local array
				maxi = N - j+1;

				//swap if imported is smaller
				if (A[mini] < A[maxi])
				{
					auto temp = A[mini];
					A[mini] = A[maxi];
					A[maxi] = temp;
				}
				else { break; }// we have all the smallest in our array		
			}
		}
		else //keep larger keys
		{
			for (auto j = 0;; ++j)
			{
				// find the largest one in imported array
				maxi = max_index(A, N, N);

				// find the smallest one in local array
				mini = j;

				//swap if imported is larger
				if (A[maxi] > A[mini])
				{
					auto temp = A[maxi];
					A[maxi] = A[mini];
					A[mini] = temp;
				}
				else { break; }// we have all the largest in our array		
			}
		}
#pragma endregion  comparing data (lower rank have smaller numbers)
		
		//clean (not really needed)
		for (auto j = N; j<2 * N; j++)
		{
			A[j] = NULL;
		}
	} //end for
	qsort(A, N, sizeof(int), &cmp);
}


void sort::print(int N, int rank) const
{
	cout << "Worker " << rank << ": ";
	for (auto i = 0; i < N; i++)
	{
		cout << A[i] << ' ';
	}
	cout << '\n';
}

sort::sort(int const N)
{
	A = new int[2 * N];
	for (auto i = 0; i < 2 * N; i++) { A[i] = NULL; }
	init(N);
}

sort::~sort()
{
	delete[] A;
}
