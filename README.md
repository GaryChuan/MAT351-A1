# MAT351-A1

This repository was created to accomplish assignment 1 for MAT 351:Quaternions, Interpolation, and Animation.

The assignment involves the completion of the following two tasks:

1. Create a program that reads an input file and outputs, using a visualization using an asymetrical object (such as the letter R) which displays the N + 1 orientations of the object, starting from the rotation of θ1 counterclockwise to the rotation of θ2 counterclockwise, with each intermediate orientation after a uniform amount of time (assuming uniform angular velocity) sequentially. The input file will contain a single line with three real numbers θ1 θ2 N seperated by single spaces with 0 <= θ1, θ2 < 360, where 10 * θi is an integer and N is an integer, 2 <= N <= 24.

2. Create a program that reads an input file and outputs, as a sequence of orientations x y z θ, the N + 1 orientations of the object, starting from the rotation of θ1 counterclockwise when fixing the vector <x1, x1, z1> to the rotation of θ2 counterclockwise when fixing the vector <x2,y2,z2>, with each intermediate orientation after a uniform amount of time (assuming uniform angular velocity) sequentially. This may be outputted to a file with one orientation per line sequentially. The input file will consists of three lines, each of the first two lines with four real numbers, xi yi zi θi on the ith line, separated by single spaces and ending in a carriage return, with -10 <= xi, yi, zi <= 10 where 10xi, 10yi, 10zi are integers, and 0 <= θ < 360, where 10θ is an integer with the third line containig an integer N, 2 <= N <= 24.

A visualization using an asymmetrical object which displays all the intermediate positions and orientations for the problem, for time t, 0 ≤ t ≤ N, will be awarded five additional points
