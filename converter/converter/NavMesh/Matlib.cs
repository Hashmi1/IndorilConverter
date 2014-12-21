/*
Source:  http://rosettacode.org/wiki/Matrix_multiplication#C.23
Modified: Added specific function to scale and rotate a Vec3 point along x-axis

Copyright(c) 2014 Hashmi1

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavMesh
{
    
    public class Matlib
    {
        int n;
        int m;
        double[,] a;

        public static float[] scale(float[] vertex, float val)
        {
            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i] = vertex[i] * val;
            }

            return vertex;
        }

        public static float[] rotateX(float[] vertex, float xR = (float)3.14159265358979323846/2f )
        {            
            Matlib rot = new Matlib(3, 3);
            
            rot[1, 1] = 1.0f;     rot[1, 2] = 0.0f;                 rot[1, 3] = 0.0f;
            rot[2, 1] = 0.0f;     rot[2, 2] = Math.Cos(xR);         rot[2, 3] = -Math.Sin(xR);
            rot[3, 1] = 0.0f;     rot[3, 2] = Math.Sin(xR);         rot[3, 3] = Math.Cos(xR);

            Matlib vertex_mat = new Matlib(3, 1);
            vertex_mat[1,1] = vertex[0];
            vertex_mat[2,1] = vertex[1];
            vertex_mat[3,1] = vertex[2];

            Matlib answer = rot * vertex_mat;

            vertex[0] = (float)answer[1, 1];
            vertex[1] = (float)answer[2, 1];
            vertex[2] = (float)answer[3, 1];

            return vertex;
        }



        private Matlib(int n, int m)
        {
            if (n <= 0 || m <= 0)
                throw new ArgumentException("Matrix dimensions must be positive");
            this.n = n;
            this.m = m;
            a = new double[n, m];
        }

        //indices start from one
        private double this[int i, int j]
        {
            get { return a[i - 1, j - 1]; }
            set { a[i - 1, j - 1] = value; }
        }

        private int N { get { return n; } }
        private int M { get { return m; } }

        public static Matlib operator *(Matlib _a, Matlib b)
        {
            int n = _a.N;
            int m = b.M;
            int l = _a.M;
            if (l != b.N)
                throw new ArgumentException("Illegal matrix dimensions for multiplication. _a.M must be equal b.N");
            Matlib result = new Matlib(_a.N, b.M);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < l; k++)
                        sum += _a.a[i, k] * b.a[k, j];
                    result.a[i, j] = sum;
                }
            return result;
        }
    }
}
